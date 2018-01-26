namespace HMS.Catalog.API.IntegrationEvents.EventHandling
{
    using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
    using System.Threading.Tasks;
    using Infrastructure;
    using HMS.Catalog.API.IntegrationEvents.Events;

    public class OrderStatusChangedToPaidIntegrationEventHandler : 
        IIntegrationEventHandler<OrderStatusChangedToPaidIntegrationEvent>
    {
        private readonly CatalogContext _catalogContext;

        public OrderStatusChangedToPaidIntegrationEventHandler(CatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public async Task Handle(OrderStatusChangedToPaidIntegrationEvent command)
        {
            //we're not blocking stock/inventory
            foreach (var orderStockItem in command.OrderStockItems)
            {
                var catalogItem = _catalogContext.Products.Find(orderStockItem.ProductId);

                catalogItem.RemoveStock(orderStockItem.Units);
            }

            await _catalogContext.SaveChangesAsync();
        }
    }
}