using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using ReviewRumble.Models;
using System.Text;

namespace ReviewRumble.utils;

public static class JwtManager
{
	private static JwtOption JwtOption;

    static JwtManager()
    {
	    JwtOption = new JwtOption
	    {
		    Issuer = "https://localhost:44377",
		    Audience = "https://localhost:44377",
		    Secret = "uesrswfkwhek'ssecret1234567890qwertyuiop0876543@/.';;lou][o"
	    };
    }

    public static async Task<string> GenerateTokenAsync(GitUser user)
    {
	    var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOption.Secret));
		var tokenHandler = new JwtSecurityTokenHandler();

        var now = DateTime.UtcNow;
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            ]),

            Expires = now.AddMinutes(Convert.ToInt32(JwtOption.ExpiryInMinutes)),
            Issuer = JwtOption.Issuer,
            Audience = JwtOption.Audience,
            SigningCredentials = new SigningCredentials(symmetricKey,
                SecurityAlgorithms.HmacSha256Signature)
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);

        return token;
    }

    public static async Task<int?> ValidateTokenAsync(string? token)
    {
        if(token == null) return null;

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
	        tokenHandler.ValidateToken(token, GetTokenValidationParameters(), out SecurityToken validatedToken);
	        var jwt = (JwtSecurityToken)validatedToken;
	        return int.Parse(jwt.Claims.First(c => c.Type == "Id").Value);
        }
        catch
        {
	        return null;
        }
	}

    public static TokenValidationParameters GetTokenValidationParameters()
    {
		var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOption.Secret));
		return new TokenValidationParameters
	    {
		    IssuerSigningKey = symmetricKey,
		    ValidateIssuerSigningKey = true,
            ValidAudience = JwtOption.Audience,
            ValidIssuer = JwtOption.Issuer,
		    ValidateLifetime = true,
	    };
    }
}