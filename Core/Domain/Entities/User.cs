using System.Security.Cryptography;
using System.Text;

namespace Domain.Entities;

public class User
{
    public User()
    {
        DynamicPartOfSalt = RandomNumberGenerator.GetHexString(128);
    }

    // Constructor for EF
    private User(string dynamicPartOfSalt, string password)
    {
        DynamicPartOfSalt = dynamicPartOfSalt;
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

    private byte[] GetSaltedBytes(string source) => Encoding.UTF8
        .GetBytes(source + StaticPartOfSalt + Login + source + DynamicPartOfSalt + Login).Select(b =>
        {
            if (b < 127)
                return (byte)(b + 43);
            return (byte)(b - 21);
        }).ToArray();

    public string Name { get; set; } = null!;

    public string Contact { get; set; } = null!;

    public string? DefaultAddress { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    private string _password = null!;

    private const string StaticPartOfSalt = "6d9ace9d25bca79be42c971f85a543b22dcee800101d9b39b9213741a5cdcf147b853dc142fa761f66b6cffb50e1a3c5183ae78013124fa58ff41a6edfc6e969";
}
