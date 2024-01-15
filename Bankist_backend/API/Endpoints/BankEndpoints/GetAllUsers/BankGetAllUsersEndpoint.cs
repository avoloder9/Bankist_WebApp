using API.Data;
using API.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints.BankEndpoints.GetAllUsers
{
    [ApiController]
    [Route("[controller]")]
    public class BankGetAllUsersEndpoint : MyBaseEndpoint<BankGetAllUsersRequest, BankGetAllUsersResponse>
    {
        private readonly ApplicationDbContext _dbContext;
        public BankGetAllUsersEndpoint(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public override async Task<BankGetAllUsersResponse> Procces([FromQuery] BankGetAllUsersRequest request, CancellationToken cancellationToken)
        {
            var bankUsers = await _dbContext.BankUserCard
                   .Include(buc => buc.user)
                   .Include(buc => buc.card)
                   .Where(buc => buc.bankId == request.bankId)
                   .ToListAsync(cancellationToken);

            var response = bankUsers.Select(buc => new BankGetAllUsersResponseBank
            {
                FirstName = buc.user.firstName,
                LastName = buc.user.lastName,
                Email = buc.user.email,
                Phone = buc.user.phone,
                BirthDate = buc.user.birthDate,
                RegistrationDate = buc.user.registrationDate,
                CardNumber = buc.card.cardNumber,
                Amount = buc.card.amount,
                ExpirationDate = buc.card.expirationDate
            }).ToList();

            return new BankGetAllUsersResponse
            {
                Banks = response
            };
        }

    }
}