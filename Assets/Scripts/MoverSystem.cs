using System.Collections.Generic;
using System.Numerics;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Scripts
{
    //public partial struct MoverSystem : ISystem
    //{
    //    [BurstCompile]
    //    public void OnCreate(ref SystemState state)
    //    {
    //        state.RequireForUpdate<Mover>();
    //    }

    //    [BurstCompile]
    //    public void OnUpdate(ref SystemState state)
    //    {
    //        var deltaTime = SystemAPI.Time.DeltaTime;

    //        foreach (var (transform, mover, velocity, mass)
    //                 in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Mover>, RefRW<PhysicsVelocity>, RefRO<PhysicsMass>>())
    //        {
    //            // Movement
    //            var direction = mover.ValueRO.Direction;
    //            var desiredVelocity = direction * mover.ValueRO.MaxSpeed;

    //            var velocityDiff = desiredVelocity - velocity.ValueRO.Linear;
    //            var accelarationForce = velocityDiff * mover.ValueRO.Acceleration;

    //            velocity.ValueRW.Linear += (accelarationForce * mass.ValueRO.InverseMass) * deltaTime;

    //            // Rotation
    //            velocity.ValueRW.Angular = float3.zero;

    //            if (!direction.Equals(float3.zero))
    //                transform.ValueRW.Rotation = math.slerp(transform.ValueRO.Rotation, 
    //                quaternion.LookRotation(direction, math.up()), mover.ValueRO.RotationSpeed * SystemAPI.Time.DeltaTime);
    //        }
    //    }
    //}

    [RequireMatchingQueriesForUpdate]
    public partial struct MoverSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new MoverJob
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct MoverJob : IJobEntity
    {
        public float DeltaTime;

        [BurstCompile]
        private void Execute([ChunkIndexInQuery] in int index,
            ref LocalTransform transform, ref Mover mover, ref PhysicsVelocity velocity, in PhysicsMass mass)
        {
            // Movement
            var direction = mover.Direction;
            var desiredVelocity = direction * mover.MaxSpeed;
            var velocityDiff = desiredVelocity - velocity.Linear;
            var accelarationForce = velocityDiff * mover.Acceleration;
            velocity.Linear += (accelarationForce * mass.InverseMass) * DeltaTime;
            // Rotation
            velocity.Angular = float3.zero;
            if (!direction.Equals(float3.zero))
                transform.Rotation = math.slerp(transform.Rotation,
                    quaternion.LookRotation(direction, math.up()), mover.RotationSpeed * DeltaTime);
        }
    }
}