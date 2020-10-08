using Constants;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.Azure;
using Microsoft.Azure.Commerce.UsageAggregates;
using System;
using Microsoft.Azure.Commerce.UsageAggregates.Models;

namespace Resources
{
    class USageFetcher {

        private IMongoClient mClient {get;set;}
        private string subscription {get;set;}

        private string resourceGroup {get;set;}

        private IUsageAggregationManagementClient cli = null;

        public USageFetcher(IMongoClient cli, string subscriptionId, string resourceGroup, TokenCredential credentials) {
            this.mClient = cli;
            this.resourceGroup = resourceGroup;
            this.subscription = subscription;

            SubscriptionCloudCredentials oldAuthCredentials = null;            
            this.cli = new Microsoft.Azure.Commerce.UsageAggregates.UsageAggregationManagementClient(oldAuthCredentials);
        }
        
        public async Task GetAllVirtualMachines(CancellationToken token) {
            var db = mClient.GetDatabase(StorageConstants.DataBaseName);
            var coll = db.GetCollection<UsageAggregation>("vms_scale_set");
            UsageAggregationGetResponse response = null;

            do {
                var continuation = response == null ? null : response.ContinuationToken;
                response = await cli.UsageAggregates.GetAsync(DateTime.Now, DateTime.Now,  AggregationGranularity.Hourly, true, continuation, token);           
                await coll.InsertManyAsync(response.UsageAggregations, null, token);

            } while(!String.IsNullOrWhiteSpace(response.NextLink));    

            
        }
    }
}
