using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.DataAccess
{
    public abstract class BaseRepository<T> where T : class
    {
        private string _databaseName = null;

        private DocumentClient _documentClient = null;
        private DocumentCollection _documentCollection = null;

        protected DocumentClient Context
        {
            get
            {
                return _documentClient;
            }
        }

        public BaseRepository(string endpointUri = null, string primaryKey = null, string databaseName = null)
        {
            endpointUri = endpointUri ?? ConfigurationManager.AppSettings["CosmosEndpointUrl"];
            primaryKey = primaryKey ?? ConfigurationManager.AppSettings["CosmosPrimaryKey"];

            _databaseName = databaseName ?? ConfigurationManager.AppSettings["CosmosDatabaseName"];

            _documentClient = new DocumentClient(new Uri(endpointUri), primaryKey, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            this.CreateDatabase();
            this.CreateCollection();
        }

        protected ResourceResponse<Database> CreateDatabase()
        {
            var response = _documentClient.CreateDatabaseIfNotExistsAsync(new Microsoft.Azure.Documents.Database()
            {
                Id = _databaseName
            }).Result;

            return response;
        }

        protected void CreateCollection()
        {
            var name = typeof(T).Name;
            var databaseUri = UriFactory.CreateDatabaseUri(_databaseName);

            _documentCollection = new DocumentCollection();
            _documentCollection.Id = name;

            _documentClient.CreateDocumentCollectionAsync(databaseUri, _documentCollection);
        }

        protected T GetFirstOrDefault(Func<T, bool> predicate)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(_databaseName, _documentCollection.Id);

            var item = _documentClient.CreateDocumentQuery<T>(uri)
                    .Where(predicate)
                    .AsEnumerable()
                    .FirstOrDefault();

            return item;
        }

        protected List<T> GetAll(Func<T, bool> predicate = null)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(_databaseName, _documentCollection.Id);

            var query = _documentClient.CreateDocumentQuery<T>(uri);

            if (predicate != null)
                return query.Where(predicate)
                    .AsEnumerable()
                    .ToList();

            return query.AsEnumerable()
                    .ToList();
        }

        protected bool Upsert(T entity)
        {
            var uri = UriFactory.CreateDocumentCollectionUri(_databaseName, _documentCollection.Id);

            var result = _documentClient.UpsertDocumentAsync(uri, entity).Result;

            var success = result.StatusCode == System.Net.HttpStatusCode.Created;

            return success;
        }

        protected bool Delete(T entity)
        {
            var id = this.GetIdPropertyValue(entity);

            if (string.IsNullOrEmpty(id))
                throw new InvalidOperationException();

            var success = this.Delete(id);

            return success;
        }

        protected bool Delete(string id)
        {
            var uri = UriFactory.CreateDocumentUri(_databaseName, _documentCollection.Id, id);

            var result = _documentClient.DeleteDocumentAsync(uri).Result;

            var success = result.StatusCode == System.Net.HttpStatusCode.Created;

            return success;
        }

        private string GetIdPropertyValue(T entity)
        {
            var properties = typeof(T).GetProperties();

            var withJsonAttribute = properties.FirstOrDefault(p =>
            {
                var attribute = Attribute.GetCustomAttribute(p, typeof(JsonPropertyAttribute));
                return attribute != null && ((JsonPropertyAttribute)attribute).PropertyName == "id";
            });

            if (withJsonAttribute != null)
                return Convert.ToString(withJsonAttribute.GetValue(entity));

            var withName = properties.FirstOrDefault(p => string.Compare(p.Name, "Id", true) == 0);

            if (withName != null)
                return Convert.ToString(withName.GetValue(entity));

            return null;
        }
    }
}
