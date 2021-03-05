using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Encryption;

namespace AbpStringEncrypt
{

    [DependsOn(
        typeof(AbpAutofacModule)
    )]
    public class AbpStringEncryptModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostEnvironment = context.Services.GetSingletonInstance<IHostEnvironment>();

            Configure<AbpStringEncryptionOptions>(opt =>
            {
                opt.DefaultPassPhrase = configuration.GetValue("StringEncryption:DefaultPassPhrase", "gsKnGZ041HLL4IM8");
                opt.InitVectorBytes = Encoding.ASCII.GetBytes(configuration.GetValue("StringEncryption:InitVectorBytes", "jkE49230Tf093b42"));
                opt.DefaultSalt = Encoding.ASCII.GetBytes(configuration.GetValue("StringEncryption:DefaultSalt", "hgt!16kl"));
            });

            context.Services.AddTransient<IStringEncryptionService, Volo.Abp.Security.Encryption.StringEncryptionService>();

            context.Services.AddHostedService<AbpStringEncryptHostedService>();
        }
    }
}
