using System.Buffers;
using System.Globalization;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.EventTrigger;


namespace Assets.Scripts
{
    public partial struct CloneActionSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CloneAction>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!Input.GetMouseButtonDown(1)) return;

            //var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            //var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (transform, spawnAction, entity) 
                     in SystemAPI.Query<RefRW<LocalTransform>, RefRO<CloneAction>>().WithDisabled<LifeTime>().WithEntityAccess())
            {
                var entities = new NativeArray<Entity>(spawnAction.ValueRO.Count, Allocator.Temp);

                state.EntityManager.Instantiate(spawnAction.ValueRO.Prefab, entities);
                foreach (var spawnedEntity in entities)
                {
                    SystemAPI.SetComponentEnabled<LifeTime>(spawnedEntity,true);
                    SystemAPI.SetComponent(spawnedEntity, new LifeTime { Value = spawnAction.ValueRO.Lifetime });

                    SystemAPI.SetComponent(spawnedEntity, LocalTransform.FromPositionRotationScale(
                        transform.ValueRO.Position,
                        transform.ValueRO.Rotation,
                        transform.ValueRO.Scale));
                }
                SystemAPI.SetComponentEnabled<CloneAction>(entity, false);
            }

        }

        
        

    }
}