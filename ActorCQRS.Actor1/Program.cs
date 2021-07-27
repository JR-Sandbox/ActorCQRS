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
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using System.Linq;
using Microsoft.ServiceFabric.Actors.Remoting;

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
            //var v = base.CreateServiceReplicaListeners();

            //var t = new FabricTransportServiceRemotingListener(null, this, new FabricTransportRemotingListenerSettings
            //{
            //    EndpointResourceName = "QueryListenerEndpoint"
            //});


            try
            {
                var results = new []
                {
                    new ServiceReplicaListener((context) => new FabricTransportServiceRemotingListener(context, this, new FabricTransportRemotingListenerSettings { EndpointResourceName = "QueryListenerEndpoint" }), "QueryListener", true),
                    new ServiceReplicaListener((context) => new FabricTransportServiceRemotingListener(context, this, new FabricTransportRemotingListenerSettings { EndpointResourceName = "CommandListenerEndpoint" }), "CommandListener", true)


                    //new ServiceReplicaListener((context) => new FabricTransportServiceRemotingListener(context, this, new FabricTransportRemotingListenerSettings { EndpointResourceName = "QueryListenerEndpoint"), "QueryListener", true)
                };

                return results;



                // works, but appears to be V1
                //var results = new[]
                //{
                //    new ServiceReplicaListener((context) => new FabricTransportServiceRemotingListener(context, this), "QueryListener", true),
                //    new ServiceReplicaListener((context) => new FabricTransportServiceRemotingListener(context, this), "CommandListener", false),
                //};

                //return results;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
