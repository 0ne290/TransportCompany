using System.Security.Cryptography;
using System.Text;
using Domain.Interfaces;

namespace Domain.Entities;

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

    public string DynamicPartOfSalt { get; } = null!;
    
    public string Login { get; set; } = null!;

    public string Password
    {
        get => _password;
        set
        {
            var sha256OfSaltyNewPassword = SHA512.HashData(GetSaltedBytes(value));
            
            var stringBuilder = new StringBuilder();
            
            foreach (var b in sha256OfSaltyNewPassword)
                stringBuilder.Append(b.ToString("x2"));

            _password = stringBuilder.ToString();
        }
    }

    private byte[] GetSaltedBytes(string source)
    {
        var saltedBytes = Encoding.UTF8.GetBytes(source + StaticPartOfSalt + Login + source + DynamicPartOfSalt + Login);
    
        foreach (var b in saltedBytes)
        {
            if (b < 127)
                b += 43;
            else
                b -= 21;
        }

        return saltedBytes;
    }

    public string Name { get; set; } = null!;

    public string Contact { get; set; } = null!;

    public string? DefaultAddress { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    private string _password;
}
