using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using ActorCQRS.Actor1.Interfaces;

namespace ActorCQRS.Actor1
{
    /// <remarks>
    /// This class represents an actor.
    /// Every ActorID maps to an instance of this class.
    /// The StatePersistence attribute determines persistence and replication of actor state:
    ///  - Persisted: State is written to disk and replicated.
    ///  - Volatile: State is kept in memory only and replicated.
    ///  - None: State is kept in memory only and not replicated.
    /// </remarks>
    [StatePersistence(StatePersistence.Persisted)]
    internal class Actor1 : Actor, ICommandMeasurement, IQueryMeasurement
    {
        const string ActorStateKey = "Measurements";


        public Actor1(ActorService actorService, ActorId actorId) 
            : base(actorService, actorId)
        {
        }

        public async Task AddMasurementAsync(Measurement measurement)
        {
            List<Measurement> measurements = await StateManager.GetOrAddStateAsync(ActorStateKey, new List<Measurement>());

            measurements.Add(measurement);

            await StateManager.SetStateAsync(ActorStateKey, measurements);
        }

        public async Task<List<Measurement>> GetMeasurementsAsync()
        {
            List<Measurement> measurements = await StateManager.GetOrAddStateAsync(ActorStateKey, new List<Measurement>());

            return measurements;
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// An actor is activated the first time any of its methods are invoked.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            ActorEventSource.Current.ActorMessage(this, "Actor activated.");

            // The StateManager is this actor's private state store.
            // Data stored in the StateManager will be replicated for high-availability for actors that use volatile or persisted state storage.
            // Any serializable object can be saved in the StateManager.
            // For more information, see https://aka.ms/servicefabricactorsstateserialization

            return this.StateManager.TryAddStateAsync("count", 0);
        }
    }
}
