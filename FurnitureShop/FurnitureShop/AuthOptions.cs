using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FurnitureShop
{
    public class AuthOptions
    {
        public const string ISSUER = "AuthServer";
        public const string AUDIENCE = "AuthClient";
        const string KEY = "42_is_an_answer_to_all_of_your_questions!!!";
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
        }
    }
}
