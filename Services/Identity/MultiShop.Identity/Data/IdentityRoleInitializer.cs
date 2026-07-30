using Microsoft.AspNetCore.Identity;

namespace MultiShop.Identity.Data
{
    public sealed class IdentityRoleInitializer : IHostedService
    {
        private static readonly string[] RoleNames =
        [
            "User",
            "Manager",
            "Admin"
        ];

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public IdentityRoleInitializer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var roleName in RoleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName))
                {
                    continue;
                }

                var result = await roleManager.CreateAsync(new IdentityRole(roleName));

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(error => error.Description));
                    throw new InvalidOperationException(
                        $"{roleName} rolü oluşturulamadı: {errors}");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
