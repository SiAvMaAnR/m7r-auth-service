using Auth.Domain.Shared.Constants.Common;
using Auth.WebApi.Middlewares;

namespace Auth.WebApi.ApiBuilder.ApplicationBuilder;

public static partial class ApplicationBuilderExtension
{
    public static void CommonConfiguration(this WebApplication webApplication)
    {
        webApplication.UseMiddleware<TimingMiddleware>();
        webApplication.UseCors(CorsPolicyName.Default);
        webApplication.UseHttpsRedirection();
        webApplication.UseRouting();
        webApplication.MapControllers();
    }
}
