namespace minprofil.Data;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = "";

    public string Password { get; set; } = "";

    public string DisplayName { get; set; } = "";

    // Fritextpresentation som visas på profilsidan och för andra användare.
    public string Presentation { get; set; } = "";

    public bool IsAdmin { get; set; }
}
