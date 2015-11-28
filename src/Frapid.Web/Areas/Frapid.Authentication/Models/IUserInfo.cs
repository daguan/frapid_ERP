namespace Frapid.Authentication.Models
{
    public interface IUserInfo
    {
        string Name { get; set; }
        string Email { get; set; }
    }
}