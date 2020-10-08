using Constants;
using MongoDB.Driver;
using Azure.ResourceManager.Compute.Models;
using Azure.ResourceManager.Compute;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;

namespace Resources
{
    class VMFetcher {

        private IMongoClient mClient {get;set;}
        private string subscription {get;set;}

        private string resourceGroup {get;set;}

        private  ComputeManagementClient cli = null;

        public VMFetcher(IMongoClient cli, string subscriptionId, string resourceGroup, TokenCredential credentials) {
            this.mClient = cli;
            this.resourceGroup = resourceGroup;
            this.subscription = subscription;
            this.cli = new ComputeManagementClient(subscriptionId,  credentials);
        }
        
        public async Task GetAllVirtualMachines(CancellationToken token) {
            var db = mClient.GetDatabase(StorageConstants.DataBaseName);
            var coll = db.GetCollection<VirtualMachineScaleSet>("vms_scale_set");
            var something = cli.VirtualMachineScaleSets.List(resourceGroup, token);
        
            await coll.InsertManyAsync(something, null, token);
        }
    }
}
