public interface IAuthProvider
{
    Task<string?> LoginAsync(object request);
     Task<string?> RegisterAsync(object request);
}