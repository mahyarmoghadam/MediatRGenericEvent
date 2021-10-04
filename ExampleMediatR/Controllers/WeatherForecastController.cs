using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExampleMediatR.Controllers
{
    public class Entity
    {
    }

    public class Product : Entity
    {
        public int Id { get; set; }
    }

    public class Order : Entity
    {
        public int Id { get; set; }
    }


    public class Test : INotification
    {
        public int Id { get; set; }
    }

    public abstract class EntityEvent : INotification
    {
    }

    public abstract class EntityEvent<T> : EntityEvent
    {
    }


    public class CreatedEntityEvent<TEntity> : EntityEvent<TEntity> where TEntity : Entity
    {
        public CreatedEntityEvent(TEntity item)
        {
            Item = item;
        }

        public TEntity Item { get; set; }

        public override string ToString() => $"{nameof(TEntity)} with id created.";
    }


    public class CreateHandler<T> : INotificationHandler<CreatedEntityEvent<T>> where T : Entity
    {
        public Task Handle(CreatedEntityEvent<T> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public class TestHandler : INotificationHandler<Test>
    {
        public Task Handle(Test notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }


    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMediator _mediator;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _mediator.Publish(new CreatedEntityEvent<Product>(new Product() {Id = 1}));
            await _mediator.Publish(new Test() {Id = 1});
            return Ok();
        }
    }
}