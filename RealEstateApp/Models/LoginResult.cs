namespace RealEstateApp.Models;

public class LoginResult : BaseModel
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public bool Succeed { get; set; }
}