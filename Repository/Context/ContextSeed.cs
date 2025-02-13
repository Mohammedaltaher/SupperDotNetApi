//using Domain.Entities.Lookups;
//using MongoDB.Driver;

//namespace Repository.Context;

//public class ContextSeed
//{
//    public static void SeedStepScreen(IMongoCollection<StepScreen> CustomerTypeCollection)
//    {
//        if (!CustomerTypeCollection.Find(x => true).Any())
//        {
//            List<StepScreen> customerTypes = StepScreen.GetScreenSteps();

//            CustomerTypeCollection.InsertMany(customerTypes);
//        }
//    }
//}



