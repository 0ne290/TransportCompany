using System.Security.Cryptography;
using System.Text;
using Domain.Interfaces;

namespace Dal;

public partial class User : IEntity
{
    public User()
    {
        PartOfSalt = RandomNumberGenerator.GetHexString(128);
    }

    // Constructor for EF
    private User(string partOfSalt, string password)
    {
        PartOfSalt = PartOfSalt;
        _password = password;
    }

    public string PartOfSalt { get; } = null!;
    
    public string Login { get; set; } = null!;

    public string Password
    {
        get => _password;
        set
        {
            var newPasswordInBytes = Encoding.UTF8.GetBytes(value);
            var saltyNewPasswordInBytes = CalculateSaltedBytes(newPasswordInBytes);
            var sha256OfSaltyNewPassword = SHA512.HashData(saltyNewPasswordInBytes);
            
            var stringBuilder = new StringBuilder();
            
            foreach (var b in sha256OfNewPassword)
                stringBuilder.Append(b.ToString("x2"));

            _password = stringBuilder.ToString();
        }
    }

    private byte[] CalculateSaltedBytes(byte[] source)
    {
        var salt = CalculateSalt();
        
        var saltedBytes = new byte[source.Length + salt.Length];
        source.CopyTo(saltedBytes, 0);
        salt.CopyTo(saltedBytes, source.Length);
    
        foreach (var b in saltedBytes)
        {
            if (b < 127)
                b += 43;
            else
                b -= 21;
        }

        return saltedBytes;
    }

    private byte[] CalculateSalt() => Encoding.UTF8.GetBytes(PartOfSalt + Login);

    public string Name { get; set; } = null!;

    public string Contact { get; set; } = null!;

    public string? DefaultAddress { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    private string _password;
}
