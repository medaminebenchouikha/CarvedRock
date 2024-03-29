﻿using CarvedRock.Api.ApiModels;

namespace CarvedRock.Api.Interfaces
{
    public interface IQuickOrderLogic
    {
        Task<Guid> PlaceQuickOrder(QuickOrder order, int customerId);
    }
}
