namespace API.Endpoints.AuthEndpoints.Login
{
    public class AuthLoginVM
    {
        public string username { get; set; }
        public string password { get; set; }
        public string SignalRConnectionID { get;  set; }
    }
}
