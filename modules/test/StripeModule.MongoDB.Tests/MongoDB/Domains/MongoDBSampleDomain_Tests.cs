using StripeModule.Samples;
using Xunit;

namespace StripeModule.MongoDB.Domains;

[Collection(MongoTestCollection.Name)]
public class MongoDBSampleDomain_Tests : SampleManager_Tests<StripeModuleMongoDbTestModule>
{

}
