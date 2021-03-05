using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Volo.Abp;

namespace AbpStringEncrypt
{
    public class AbpStringEncryptHostedService : IHostedService
    {
        private readonly IAbpApplicationWithExternalServiceProvider _application;
        private readonly IServiceProvider _serviceProvider;
        private readonly StringEncryptionService _stringEncryptionService;

        public AbpStringEncryptHostedService(
            IAbpApplicationWithExternalServiceProvider application,
            IServiceProvider serviceProvider,
            StringEncryptionService stringEncryptionService)
        {
            _application = application;
            _serviceProvider = serviceProvider;
            _stringEncryptionService = stringEncryptionService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _application.Initialize(_serviceProvider);
            
            _stringEncryptionService.Run();

            _application.Shutdown();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _application.Shutdown();

            return Task.CompletedTask;
        }
    }
}
