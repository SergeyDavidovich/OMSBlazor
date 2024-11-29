using StripeModule.MongoDB;
using StripeModule.Samples;
using Xunit;

namespace StripeModule.MongoDb.Applications;

[Collection(MongoTestCollection.Name)]
public class MongoDBSampleAppService_Tests : SampleAppService_Tests<StripeModuleMongoDbTestModule>
{

}
