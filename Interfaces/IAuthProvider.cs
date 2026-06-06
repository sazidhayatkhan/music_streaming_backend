public interface IAuthProvider
{
    Task<string?> LoginAsync(object request);
}