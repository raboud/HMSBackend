using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.BuildingBlocks.EventBus.Abstractions;
using Microsoft.BuildingBlocks.EventBus.Events;
using Microsoft.BuildingBlocks.IntegrationEventLogEF.Services;
using Microsoft.BuildingBlocks.IntegrationEventLogEF.Utilities;
using Ordering.Infrastructure;
using System;
using System.Data.Common;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.BuildingBlocks.IntegrationEventLogEF;

namespace Ordering.API.Application.IntegrationEvents
{

	public class OrderingIntegrationEventService : IOrderingIntegrationEventService
	{
		private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
		private readonly IEventBus _eventBus;
		private readonly OrderingContext _orderingContext;
		private readonly IntegrationEventLogContext _eventLogContext;
		private readonly IIntegrationEventLogService _eventLogService;
		private readonly ILogger<OrderingIntegrationEventService> _logger;

		public OrderingIntegrationEventService(IEventBus eventBus,
			OrderingContext orderingContext,
			IntegrationEventLogContext eventLogContext,
			Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory,
			ILogger<OrderingIntegrationEventService> logger)
		{
			_orderingContext = orderingContext ?? throw new ArgumentNullException(nameof(orderingContext));
			_eventLogContext = eventLogContext ?? throw new ArgumentNullException(nameof(eventLogContext));
			_integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
			_eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
			_eventLogService = _integrationEventLogServiceFactory(_orderingContext.Database.GetDbConnection());
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task PublishEventsThroughEventBusAsync()
		{
			var pendindLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync();

			foreach (var logEvt in pendindLogEvents)
			{
				_logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", logEvt.EventId, Program.AppName, logEvt.IntegrationEvent);

				try
				{
					await _eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
					_eventBus.Publish(logEvt.IntegrationEvent);
					await _eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId} from {AppName}", logEvt.EventId, Program.AppName);

					await _eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
				}
			}
		}

		public async Task AddAndSaveEventAsync(IntegrationEvent evt)
		{
			_logger.LogInformation("----- Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);

			await _eventLogService.SaveEventAsync(evt, _orderingContext.GetCurrentTransaction.GetDbTransaction());
		}
	}
}
