using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.UserEndpoints.CheckExists
{
    [ApiController]
    [Route("[controller]")]
    public class UserCheckExists : ControllerBase
    {
        private readonly IUserService _userService;

        public UserCheckExists(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult CheckUserExists([FromBody] UserCheckRequest request)
        {
            bool userExists = _userService.UserExists(request.userName, request.email);

            return Ok(new { Exists = userExists });
        }
    }
}
