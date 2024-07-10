
namespace RevitPlugIn.Dtos
{
    public class UserLoggedDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        
        public UserRole Role { get; set; }

    }
}
