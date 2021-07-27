using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;

namespace ActorCQRS.Actor1
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                // This line registers an Actor Service to host your actor class with the Service Fabric runtime.
                // The contents of your ServiceManifest.xml and ApplicationManifest.xml files
                // are automatically populated when you build this project.
                // For more information, see https://aka.ms/servicefabricactorsplatform

                ActorRuntime.RegisterActorAsync<Actor1>(
                    (context, actorType) => new MyActorService(context, actorType)).GetAwaiter().GetResult();


                //ActorRuntime.RegisterActorAsync<Actor1> (
                //   (context, actorType) => new ActorService(context, actorType)).GetAwaiter().GetResult();

                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ActorEventSource.Current.ActorHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }


    class MyActorService : ActorService
    {
        public MyActorService(StatefulServiceContext context, ActorTypeInformation typeInfo)
            : base(context, typeInfo)
        { }

        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            ///////////// check out the last parameter for listening on secondary...
            return new[]
            {
                new ServiceReplicaListener(null, "QueryListenerName", true),
                new ServiceReplicaListener(null, "CommandListenerName", true),
            };
        }
    }
}
