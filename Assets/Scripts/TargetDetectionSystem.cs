using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Scripts
{
    //[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    //[RequireMatchingQueriesForUpdate]
    //[UpdateBefore(typeof(ShootingSystem))]
    //public partial struct TargetDetectionSystem : ISystem
    //{
    //    public void OnUpdate(ref SystemState state)
    //    {
    //        foreach (var (shooter, shooterTransform)
    //                 in SystemAPI.Query<RefRW<Shooter>, RefRO<LocalTransform>>())
    //        {
    //            //other way to get the closest target would be to use smth like overlap collider

    //            var closestDistance = float.MaxValue;
    //            var closestTarget = float3.zero;

    //            foreach (var (target, targetTransform)
    //                     in SystemAPI.Query<RefRO<Target>, RefRO<LocalTransform>>())
    //            {
    //                if (shooter.ValueRO.TargetType != target.ValueRO.TargetType || target.Equals(shooter)) continue;

    //                var distance = math.distancesq(shooterTransform.ValueRO.Position, targetTransform.ValueRO.Position);

    //                if (!(distance < closestDistance)) continue;

    //                closestTarget = targetTransform.ValueRO.Position;
    //                closestDistance = distance;
    //            }

    //            if (closestDistance == float.MaxValue) continue;

    //            shooter.ValueRW.Direction = math.normalize(closestTarget - shooterTransform.ValueRO.Position);
    //        }
    //    }
    //}

    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [RequireMatchingQueriesForUpdate]
    public partial struct TargetDetectionSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (actor, actorTransform, actorEntity)
                     in SystemAPI.Query<RefRW<Actor>, RefRO<LocalTransform>>().WithEntityAccess())
            {
                //other way to get the closest target would be to use smth like overlap collider

                var closestDistance = float.MaxValue;
                var closestTarget = float3.zero;

                foreach (var (target, targetTransform, targetEntity)
                         in SystemAPI.Query<RefRO<Target>, RefRO<LocalTransform>>().WithEntityAccess())
                {
                    if (actor.ValueRO.TargetType != target.ValueRO.TargetType || actorEntity== targetEntity) continue;

                    var distance = math.distancesq(actorTransform.ValueRO.Position, targetTransform.ValueRO.Position);

                    if (!(distance < closestDistance)) continue;

                    closestTarget = targetTransform.ValueRO.Position;
                    closestDistance = distance;
                }

                if (closestDistance == float.MaxValue) continue;

                actor.ValueRW.Direction = math.normalize(closestTarget - actorTransform.ValueRO.Position);
            }
        }
    }

}