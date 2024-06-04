using API.Data.Models;
using API.Data;
using API.Endpoints.TransactionEndpoints.Execute;
using API.Helper.Services;
using API.Helper;
using API.SignalR;
using API.ViewModels;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoanController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly MyAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly IMemoryCache _cache;
        private readonly LoyaltyService _loyaltyService;
        public LoanController(ApplicationDbContext dbContext, MyAuthService authService, IHttpContextAccessor httpContextAccessor, IHubContext<SignalRHub> hubContext, IMemoryCache cache)
        {
            _dbContext = dbContext;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
            _cache = cache;
            _loyaltyService = new LoyaltyService(_dbContext, _hubContext);
        }

        [HttpPost("request-loan")]
        public async Task<ActionResult<Success>> RequestLoan([FromBody] LoanRequestVM request)
        {
            if (!_authService.IsLogin())
            {
                return Unauthorized();
            }

            var userCard = _dbContext.BankUserCard.FirstOrDefault(buc => buc.cardId == request.userCard);

            if (userCard == null) {
                return BadRequest(new { message = "Card not found!" });
            }

            var loanType = _dbContext.LoanType.FirstOrDefault(t => t.loanTypeId == request.loanTypeId);

            if (loanType == null)
            {
                return BadRequest(new { message = "Loan type invalid!" });
            }

            if (request.amount > loanType.maxLoanAmount || request.amount < 1000)
            {
                return BadRequest(new { message = "Loan amount too high!" });
            }

            if (request.rates > loanType.maximumRepaymentMonths || request.amount < 1)
            {
                return BadRequest(new { message = "Too many rates selected!" });
            }

            var newLoan = new Loan
            {
                amount = request.amount,
                interest = request.amount * 0.10f,
                rate = request.amount / request.rates,
                issueDate = DateTime.UtcNow,
                dueDate = DateTime.UtcNow.AddMonths(request.rates),
                totalAmount = request.amount + request.amount * 0.10f,
                totalAmountPayed = 0,
                remainingAmount = request.amount + request.amount * 0.10f,
                status = "PENDING",
                cardId = userCard.cardId,
                loanType = loanType,
            };

            _dbContext.Loan.Add(newLoan);
            _dbContext.SaveChanges();

            return Ok(new { message = "Loan request successful!" });
        }

        [HttpGet("get-loans")]
        public async Task<ActionResult> GetLoansByBankId([FromQuery] string bankId)
        {
            if (!_authService.IsLogin())
            {
                return Unauthorized();
            }

            int numericBankId;
            if (int.TryParse(bankId, out numericBankId))
            {
                var cards = await _dbContext.BankUserCard.Where(buc => buc.bankId == numericBankId).Select(buc => buc.cardId).ToListAsync();

                if (cards == null)
                {
                    return NotFound(new { message = "No cards found" });
                }

                var loans = await _dbContext.Loan.Where(loan => cards.Contains(loan.cardId)).Include(loan => loan.card).ToListAsync();

                return Ok(loans);
            }
            else
            {
                return BadRequest(new { message = "Invalid bankId: must be a numeric value." });
            }
        }

        [HttpGet("get-loan-types")]
        public ActionResult GetLoanTypes()
        {
            if (!_authService.IsLogin())
            {
                return Unauthorized();
            }

            var loanTypes = _dbContext.LoanType.ToList();

            if (loanTypes == null)
            {
                return NotFound(new { message = "No types found" });
            }

            return Ok(loanTypes);
        }

        [HttpGet("get-user-loans")]
        public async Task<ActionResult> GetUserLoans([FromQuery] string cardNumber)
        {
            if (!_authService.IsLogin())
            {
                return Unauthorized();
            }


            int cardId;
            if (int.TryParse(cardNumber, out cardId))
            {
                var card = await _dbContext.BankUserCard.FirstOrDefaultAsync(buc => buc.cardId == cardId);

                if (card == null)
                {
                    return NotFound(new { message = "Card not found" });
                }

                var loans = await _dbContext.Loan.Where(loan => loan.cardId == cardId).Include(loan => loan.loanType).ToListAsync();

                return Ok(loans);
            }

            return BadRequest(new { message = "Invalid card number" });
        }

        [HttpPut("approve-loan")]
        public ActionResult ApproveLoan(int loanId)
        {
            if (!_authService.IsLogin())
            {
                return Unauthorized();
            }

            Account account = _authService.GetAuthInfo().account!;
            if (!(account.isBank))
            {
                return Unauthorized(new { message = "Access denied" });
            }

            var loan = _dbContext.Loan.FirstOrDefault(loan => loan.loanId == loanId);

            if (loan == null)
            {
                return NotFound(new { message = "Loan not found" });
            }

            if (loan.status != "PENDING")
            {
                return BadRequest(new { message = "Loan already approved!" });
            }

            loan.status = "APPROVED";
            _dbContext.SaveChanges();

            return Ok(loan);
        }
        [HttpPut("reject-loan")]
        public ActionResult RejectLoan(int loanId)
        {
            if (!_authService.IsLogin())
            {
                return Unauthorized();
            }

            Account account = _authService.GetAuthInfo().account!;
            if (!(account.isBank))
            {
                return Unauthorized(new { message = "Access denied" });
            }

            var loan = _dbContext.Loan.FirstOrDefault(loan => loan.loanId == loanId);

            if (loan == null)
            {
                return NotFound(new { message = "Loan not found" });
            }

            if (loan.status != "PENDING")
            {
                return BadRequest(new { message = "Cannot reject approved loan!" });
            }

            loan.status = "REJECTED";
            _dbContext.SaveChanges();

            return Ok(loan);
        }
    }
}
