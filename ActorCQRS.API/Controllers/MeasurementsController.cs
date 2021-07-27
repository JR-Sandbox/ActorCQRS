using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActorCQRS.Actor1.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;

namespace ActorCQRS.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MeasurementsController : ControllerBase
    {
        private Guid _plantId = Guid.Parse("4e1bb5e4-7b97-4909-9247-f2bfe8d2622c");

        [HttpPost]
        public async Task<IActionResult> PostMeasurement([FromBody] Measurement measurement)
        {
            var serviceUri = new Uri("fabric:/ActorCQRS/Actor1ActorService");
            var actorId = new ActorId(_plantId);
            var proxy = ActorProxy.Create<IActor1>(actorId, serviceUri, "V2_1Listener");

            await proxy.AddMasurementAsync(measurement);

            return StatusCode(201);
        }

        [HttpGet]
        public async Task<IActionResult> GetMeasurements()
        {
            var serviceUri = new Uri("fabric:/ActorCQRS/Actor1ActorService");
            var actorId = new ActorId(_plantId);
            var proxy = ActorProxy.Create<IActor1>(actorId, serviceUri, "QueryListenerName");

            //var getter = ActorProxy.Create<IActor1>(actorId, null, "Actor1ActorService", "QueryListenerName");

            var result = await proxy.GetMeasurementsAsync();

            return Ok(result);


            //var serviceUri = new Uri("fabric:/ActorCQRS/Actor1ActorService");
            //var actorId = new ActorId(_plantId);
            //var proxy = ActorProxy.Create<IActor1>(actorId, serviceUri, "V2_1Listener");

            //// specify listener name
            //IActor1 getter = ActorProxy.Create<IActor1>(actorId, null, "Actor1ActorService", "ListenerName");

            //var result = await proxy.GetMeasurementsAsync();

            //return Ok(result);


        }

    }
}
