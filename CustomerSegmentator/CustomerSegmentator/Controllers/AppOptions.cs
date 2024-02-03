public class AppOptions
{
    public string[] Segments { get; set; }
    public string[] Tariffs { get; set; }
    public string[] PaymentTypes { get; set; }
    public User[] Users { get; set; }

    public string Salt { get; set; }
}

public class User
{
    public string Role { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
}
