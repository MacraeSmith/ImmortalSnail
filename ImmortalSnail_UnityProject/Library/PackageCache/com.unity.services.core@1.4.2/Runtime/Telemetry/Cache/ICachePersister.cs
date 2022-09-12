<<<<<<< Updated upstream
namespace Unity.Services.Core.Telemetry.Internal
{
    interface ICachePersister<TPayload>
        where TPayload : ITelemetryPayload
    {
        bool CanPersist { get; }

        void Persist(CachedPayload<TPayload> cache);

        bool TryFetch(out CachedPayload<TPayload> persistedCache);

        void Delete();
    }
}
=======
namespace Unity.Services.Core.Telemetry.Internal
{
    interface ICachePersister<TPayload>
        where TPayload : ITelemetryPayload
    {
        bool CanPersist { get; }

        void Persist(CachedPayload<TPayload> cache);

        bool TryFetch(out CachedPayload<TPayload> persistedCache);

        void Delete();
    }
}
>>>>>>> Stashed changes
