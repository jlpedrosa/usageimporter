using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Azure.Identity;
using Azure.Core;
using Resources;
using Azure.ResourceManager.Network.Models;
using Azure.ResourceManager.Network;

namespace usageimporter
{
    class Program
    {
        static async Task Main(string[] args)
        {
             var subscriptionId = Environment.GetEnvironmentVariable("AZURE_SUBSCRIPTION_ID");
            var resourceGroup = Environment.GetEnvironmentVariable("AZURE_RESOURCE_GROUP");

            IMongoClient cli = new MongoClient();
            var creds = GetCredentials();

            var token = new CancellationToken();

            VMFetcher vmFetch = new VMFetcher(cli, subscriptionId, resourceGroup,  creds);      
            NetworkFetcher netFetcher = new NetworkFetcher(cli, subscriptionId, resourceGroup,  creds);
            //await vmFetch.GetAllVirtualMachines(token);
            //await netFetcher.GetAllNetworks(token);

            var genericfetecher = new GenericResouceFetcher<VirtualNetwork, NetworkManagementClient>(cli, subscriptionId, resourceGroup, creds);
            genericfetecher.GetAll(token);
        }

        static TokenCredential GetCredentials() {     
             Console.WriteLine("Hello World!");       
            
           return new InteractiveBrowserCredential();
        }
    }    
}