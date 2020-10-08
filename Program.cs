using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Azure.Identity;
using Azure.Core;
using Resources;

namespace usageimporter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");       
            var subscriptionId = Environment.GetEnvironmentVariable("AZURE_SUBSCRIPTION_ID");
            var resourceGroup = Environment.GetEnvironmentVariable("AZURE_RESOURCE_GROUP");

            resourceGroup = "MC_jopedros-nop-devel_jopedros-nop-devel_northeurope";            
            subscriptionId = "0dea505e-f72c-4939-91fb-c5d318d31cbd"; 

            IMongoClient cli = new MongoClient();
            var creds = GetCredentials();

            var token = new CancellationToken();

            VMFetcher vmFetch = new VMFetcher(cli, subscriptionId, resourceGroup,  creds);      
            NetworkFetcher netFetcher = new NetworkFetcher(cli, subscriptionId, resourceGroup,  creds);
            //await vmFetch.GetAllVirtualMachines(token);
            await netFetcher.GetAllVirtualMachines(token);
        }

        static TokenCredential GetCredentials() {           
           return new InteractiveBrowserCredential();
        }
    }    
}