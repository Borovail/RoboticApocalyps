using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics.Systems;
using Unity.Physics;
using UnityEngine;

namespace Assets.Scripts
{
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateBefore(typeof(PhysicsSimulationGroup))] // We are updating before `PhysicsSimulationGroup` - this means that we will get the events of the previous frame
    public partial struct CollisionDetectSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {

            state.Dependency = new CountNumCollisionEvents()
                .Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
        }
    }

    [BurstCompile]
    public partial struct CountNumCollisionEvents : ICollisionEventsJob
    {
        public void Execute(CollisionEvent collisionEvent)
        {
            Debug.Log("Collision");
        }
    }
}