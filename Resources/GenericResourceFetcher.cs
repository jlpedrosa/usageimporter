
using Constants;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using System;

namespace Resources
{
    class GenericResouceFetcher<T, TFactory> {

        private IMongoClient mClient {get;set;}
        
        private string subscription {get;set;}

        private string resourceGroup {get;set;}

        private  TFactory cli {get;set;}

        public GenericResouceFetcher(IMongoClient cli, string subscriptionId, string resourceGroup, TokenCredential credentials) {
            this.mClient = cli;
            this.resourceGroup = resourceGroup;
            this.subscription = subscription;
            this.cli = (TFactory) Activator.CreateInstance(typeof(TFactory), subscription, credentials);            
        }
        
        public async Task GetAll(CancellationToken token) {
            var db = mClient.GetDatabase(StorageConstants.DataBaseName);
            var coll = db.GetCollection<T>(nameof(T));
            var something = GetItems(token);
            await coll.InsertManyAsync(something, null, token);
        }

        public Azure.Pageable<T> GetItems(CancellationToken token){
            var factoryType = typeof(TFactory);
            var objecttType = typeof(T);
            var objectTypeName = objecttType.Name;
            var property = factoryType.GetProperty(objectTypeName+"s");
            var typeOfPropery = property.GetType();
            var methodToInvoke = typeOfPropery.GetMethod("List");
            var args = new object[]{
                    resourceGroup
            };

            var items = (Azure.Pageable<T>)  methodToInvoke.Invoke(cli, args);
            return items;
        }
    }
}
