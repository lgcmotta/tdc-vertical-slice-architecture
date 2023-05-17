namespace DesafioWarren.Application.Services.Identity
{
    public interface IIdentityService
    {
        string GetRequestPath();
        
        string GetUserDisplayName();
    }
}