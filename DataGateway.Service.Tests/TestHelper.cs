using System;
using System.IO;
using Azure.DataGateway.Service.configurations;
using Azure.DataGateway.Service.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Azure.DataGateway.Service.Tests
{
    class TestHelper
    {
        public static readonly string DB_NAME = "myDB";
        public static readonly string COL_NAME = "myCol";
        public static readonly string QUERY_NAME = "myQuery";
        public static readonly string MUTATION_NAME = "addPost";
        public static string GraphQLTestSchema = @"
                                        type Query {
                                          myQuery: MyPojo
                                        }

                                        type MyPojo {
                                            myProp : String
                                            id : String
                                        }

                                        type Mutation {
                                            addPost(
                                                myProp: String!
                                                id : String!
                                            ): MyPojo
                                        }";

        public static string SampleQuery = "{\"query\": \"{myQuery { myProp    }}\" } ";

        public static string SampleMutation = "{\"query\": \"mutation addPost {addPost(myProp : \\\"myValueBM \\\"id : \\\"myIdBM \\\") { myProp}}\"}";

        public static GraphQLQueryResolver SampleQueryResolver()
        {
            string raw =
                "{\r\n    \"id\" : \"myQuery\",\r\n    \"databaseName\": \"" + DB_NAME + "\",\r\n    \"containerName\": \"" + COL_NAME + "\",\r\n    \"parametrizedQuery\": \"SELECT * FROM r\"\r\n}";

            return JsonConvert.DeserializeObject<GraphQLQueryResolver>(raw);
        }

        public static MutationResolver SampleMutationResolver()
        {
            string raw =
                "{   \"id\": \"addPost\",\r\n    \"databaseName\": \"" + DB_NAME + "\",\r\n    \"containerName\": \"" + COL_NAME + "\",\r\n    \"operationType\": \"UPSERT\"\r\n}";
            return JsonConvert.DeserializeObject<MutationResolver>(raw);
        }

        private static Lazy<IOptions<DataGatewayConfig>> _dataGatewayConfig = new(() => TestHelper.LoadConfig());

        private static IOptions<DataGatewayConfig> LoadConfig()
        {
            DataGatewayConfig datagatewayConfig = new();
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json")
                .Build();

            config.Bind(nameof(DataGatewayConfig), datagatewayConfig);

            return Options.Create(datagatewayConfig);
        }

        public static IOptions<DataGatewayConfig> DataGatewayConfig
        {
            get { return _dataGatewayConfig.Value; }
        }

        public static object GetItem(string id)
        {
            return new
            {
                id = id,
                myProp = "a value",
                myIntProp = 4,
                myBooleanProp = true,
                anotherPojo = new
                {
                    anotherProp = "myname",
                    anotherIntProp = 55,
                    person = new
                    {
                        firstName = "A Person",
                        lastName = "the last name",
                        zipCode = 784298
                    }
                }
            };
        }
    }
}