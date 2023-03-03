namespace BEAPICapstoneProjectFLS.ViewModel
{
    public class ApiResponseToken
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
