using System;

namespace dotnetsheff.Api.FunctionalTests
{
    public class AzureFunctionsFixture : IDisposable
    {
        public static int Port { get; } = 7072;

        private readonly AzureFunctionsStartup _azureFunctionsStartup;

        public AzureFunctionsFixture()
        {
            _azureFunctionsStartup = new AzureFunctionsStartup(Port, MeetupSettings.MeetupApiBaseUri, MeetupSettings.MeetupApiKey);
            _azureFunctionsStartup.Start();
        }

        public void Dispose()
        {
            _azureFunctionsStartup.Stop();
        }
    }
}