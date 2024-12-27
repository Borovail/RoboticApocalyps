using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Systems;
using Unity.Physics;
using Unity.Physics.Extensions;
using UnityEngine;
using UnityEngine.LightTransport;

namespace Assets.Scripts
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSimulationGroup))]
    public partial struct CollisionDetectSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            state.Dependency = new CountNumCollisionEvents
            {
                Ecb = ecb
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }
    }

    [BurstCompile]
    public partial struct CountNumCollisionEvents : ITriggerEventsJob
    {

        public EntityCommandBuffer Ecb;

        public void Execute(TriggerEvent collisionEvent)
        {
            Ecb.DestroyEntity(collisionEvent.EntityA);
            Ecb.DestroyEntity(collisionEvent.EntityB);
        }
    }
}