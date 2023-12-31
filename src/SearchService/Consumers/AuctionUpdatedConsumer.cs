﻿using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
{
  private readonly IMapper _mapper;

  public AuctionUpdatedConsumer(IMapper mapper) {
    this._mapper = mapper;
  }

  public async Task Consume(ConsumeContext<AuctionUpdated> context)
  {
    Console.WriteLine("--> Consuming auction updated: " + context.Message.Id);

    var item = _mapper.Map<Item>(context.Message);

    var result = await DB.Update<Item>()
                  .Match(x => x.ID == context.Message.Id)
                  .ModifyOnly(x => new {
                    x.Color,
                    x.Make,
                    x.Model,
                    x.Year,
                    x.Mileage
                  }, item)
                  .ExecuteAsync();

    if (!result.IsAcknowledged){
      throw new MessageException(typeof(AuctionUpdated), "Problem updating DB");
    }
  }
}
