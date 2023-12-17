using API.Data;
using API.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints.BankEndpoints.GetAll
{

    [ApiController]
    [Route("[controller]")]
    public class BankGetAllEndpoint : MyBaseEndpoint<BankGetAllRequest, BankGetAllResponse>
    {
        private readonly ApplicationDbContext _dbContext;

        public BankGetAllEndpoint(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public override async Task<BankGetAllResponse> Procces([FromQuery] BankGetAllRequest request, CancellationToken cancellationToken)
        {
            var bank = await _dbContext.Bank.OrderByDescending(x => x.id).Select(x => new BankGetAllResponseBank()
            {
                bankId = x.id,
                bankName = x.username,
                password = x.password,
                totalCapital = x.totalCapital,
                numberOfUsers = x.numberOfUsers
            }).ToListAsync(cancellationToken: cancellationToken);

            return new BankGetAllResponse
            {
                Banks = bank
            };
        }
    }
}
