public class AuthFactory
{
    private readonly IServiceProvider _provider;

    public AuthFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    public IAuthProvider Create(string type)
    {
        return type.ToLower() switch
        {
            "email" => _provider.GetRequiredService<EmailAuthProvider>(),
            "google" => _provider.GetRequiredService<GoogleAuthProvider>(),
            _ => throw new Exception("Invalid auth type")
        };
    }
}