using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Encryption;

namespace AbpStringEncrypt
{
    public class StringEncryptionService : ITransientDependency
    {
        protected readonly IServiceProvider ServiceProvider;
        protected readonly IStringEncryptionService StringEncryption;
        protected readonly AbpStringEncryptionOptions StringEncryptionOptions;
        protected readonly AppOptions AppOptions;


        public StringEncryptionService(
            IServiceProvider serviceProvider,
            IStringEncryptionService stringEncryption,
            IOptions<AbpStringEncryptionOptions> stringEncryptionOptions,
            AppOptions appOptions)
        {
            ServiceProvider = serviceProvider;
            StringEncryption = stringEncryption;
            StringEncryptionOptions = stringEncryptionOptions.Value;
            AppOptions = appOptions;
        }

        public void Run()
        {
            Mode runMode = AppOptions.DefaultMode;

            if (AppOptions.Mode == Mode.Unset)
            {
                var configuration = ServiceProvider.GetRequiredService<IConfiguration>();

                var defaultMode = configuration.GetSection("DefaultMode");
                if (defaultMode.Exists())
                {
                    if (defaultMode.Value.Equals("Encrypt", StringComparison.OrdinalIgnoreCase))
                        AppOptions.Mode = Mode.Encrypt;
                    else if (defaultMode.Value.Equals("Decrypt", StringComparison.OrdinalIgnoreCase))
                        AppOptions.Mode = Mode.Decrypt;
                }
            }

            string inputString;
            do
            {
                Console.Write("Enter string to {0}: ", Enum.GetName(runMode).ToLower());
                inputString = Console.ReadLine();

            } while (inputString.IsNullOrEmpty());

            try
            {
                var result = runMode == Mode.Decrypt 
                    ? StringEncryption.Decrypt(inputString)
                    : StringEncryption.Encrypt(inputString);

                Console.WriteLine("\nRESULT:");
                Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Failed to {0}: ", Enum.GetName(runMode).ToLower());
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
        }
    }
}
