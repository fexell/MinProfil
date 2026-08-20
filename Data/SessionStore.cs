namespace minprofil.Data;

using System.Collections.Concurrent;
using System.Security.Cryptography;

// Håller reda på inloggade sessioner i minnet. En token i användarens cookie
// pekar ut vilken användare sessionen tillhör.
public class SessionStore
{
    private static readonly TimeSpan SessionLifetime = TimeSpan.FromMinutes(30);

    private sealed class Session
    {
        public required int UserId { get; init; }
        public required DateTimeOffset Expires { get; set; }
    }

    private readonly ConcurrentDictionary<string, Session> _sessions = new();

    public string Create(int userId)
    {
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        _sessions[token] = new Session
        {
            UserId = userId,
            Expires = DateTimeOffset.UtcNow.Add(SessionLifetime),
        };
        return token;
    }

    public int? GetUserId(string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        if (!_sessions.TryGetValue(token, out var session))
        {
            return null;
        }

        if (session.Expires < DateTimeOffset.UtcNow)
        {
            _sessions.TryRemove(token, out _);
            return null;
        }

        session.Expires = DateTimeOffset.UtcNow.Add(SessionLifetime);
        return session.UserId;
    }

    public void Invalidate(string? token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            _sessions.TryRemove(token, out _);
        }
    }
}
