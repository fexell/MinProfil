namespace minprofil.Data;

// Läser ut den inloggade användaren utifrån sessionscookien.
public class CurrentUser
{
    public const string CookieName = "session";

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SessionStore _sessions;
    private readonly AppDatabase _database;

    public CurrentUser(IHttpContextAccessor httpContextAccessor, SessionStore sessions, AppDatabase database)
    {
        _httpContextAccessor = httpContextAccessor;
        _sessions = sessions;
        _database = database;
    }

    public string? Token =>
        _httpContextAccessor.HttpContext?.Request.Cookies[CookieName];

    public User? Get()
    {
        var userId = _sessions.GetUserId(Token);
        return userId is null ? null : _database.GetById(userId.Value);
    }
}
