using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

public class HotelUserManager : UserManager<User>
{
	public HotelUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor,
		IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators,
		IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer,
		IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
	{
	}

    public async override Task<IdentityResult> CreateAsync(User user)
    {
        await base.CreateAsync(user);
		
		return await AddToRoleAsync(user, "Guest");
    }
}