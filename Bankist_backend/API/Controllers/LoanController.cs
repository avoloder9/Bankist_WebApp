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
    }
}
