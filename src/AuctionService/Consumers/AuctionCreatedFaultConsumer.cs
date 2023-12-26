using Contracts;
using MassTransit;

namespace AuctionService;

public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
{
  public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
  {
    Console.WriteLine("--> Consuming faulty creation");
    var exception = context.Message.Exceptions.First();

    if (exception.ExceptionType == typeof(ArgumentException).ToString()) {
      context.Message.Message.Model = "FooBar";
      await context.Publish(context.Message.Message);
    } else {
      Console.WriteLine("Type of exception is" + exception.ExceptionType);
    }
  }
}
