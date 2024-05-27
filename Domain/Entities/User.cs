using System.Security.Cryptography;
using System.Text;
using Domain.Interfaces;

namespace Dal;

public partial class User : IEntity
{
    public string Salt { get; set; } = null!;
    
    public string Login { get; set; } = null!;

    public string Password
    {
        get => _password;
        set
        {
            var newPasswordInBytes = Encoding.UTF8.GetBytes(value);
            var sha256OfNewPassword = SHA512.HashData(newPasswordInBytes);
            
            var stringBuilder = new StringBuilder();
            
            foreach (var b in sha256OfNewPassword)
                stringBuilder.Append(b.ToString("x2"));

            _password = stringBuilder.ToString();
        }
    }

    public string Name { get; set; } = null!;

    public string Contact { get; set; } = null!;

    public string? DefaultAddress { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    private string _password;
}
