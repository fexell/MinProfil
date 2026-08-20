namespace minprofil.Data;

using System.Collections.Concurrent;
using System.Security.Cryptography;

// Håller reda på inloggade sessioner i minnet. En token i användarens cookie
// pekar ut vilken användare sessionen tillhör.
public class SessionStore
{
    private readonly ConcurrentDictionary<string, int> _sessions = new();

    public string Create(int userId)
    {
        var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        _sessions[token] = userId;
        return token;
    }

    public int? GetUserId(string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        return _sessions.TryGetValue(token, out var userId) ? userId : null;
    }

    public void Invalidate(string? token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            _sessions.TryRemove(token, out _);
        }
    }
}
