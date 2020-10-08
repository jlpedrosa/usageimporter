
using Constants;
using MongoDB.Driver;
using Azure.ResourceManager.Network.Models;
using Azure.ResourceManager.Network;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;

namespace Resources
{
    class NetworkFetcher {

        private IMongoClient mClient {get;set;}
        private string subscription {get;set;}

        private string resourceGroup {get;set;}

        private  NetworkManagementClient cli = null;

        public NetworkFetcher(IMongoClient cli, string subscriptionId, string resourceGroup, TokenCredential credentials) {
            this.mClient = cli;
            this.resourceGroup = resourceGroup;
            this.subscription = subscription;
            this.cli = new NetworkManagementClient(subscriptionId,  credentials);
        }
        
        public async Task GetAllNetworks(CancellationToken token) {
            
            var db = mClient.GetDatabase(StorageConstants.DataBaseName);
            var coll = db.GetCollection<VirtualNetwork>("vnets");
            var something = cli.VirtualNetworks.List(resourceGroup, token);        
            await coll.InsertManyAsync(something, null, token);
        }
    }
}
