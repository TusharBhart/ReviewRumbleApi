using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ReviewRumble.Models;

namespace ReviewRumble.Extension
{
	public static class WebApplicationExtension
	{
		public static void AddJwtTokenServices(this IServiceCollection services, IConfiguration configuration)
		{
			var jwtConfig = new JwtOption();
			configuration.Bind(JwtOption.SectionName, jwtConfig);

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					IssuerSigningKey =
						new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret)),
					ValidIssuer = jwtConfig.Issuer,
					ValidAudience = jwtConfig.Audience,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidateLifetime = true
				};
			});
		}
	}
}
