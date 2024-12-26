using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Assets.Scripts
{
    public partial struct MeleeEnemySystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new MeleeEnemyJob().ScheduleParallel();
        }
    }

    [WithNone(typeof(Shooter))]
    [WithAll(typeof(Enemy))]
    [BurstCompile]
    public partial struct MeleeEnemyJob : IJobEntity
    {
        [BurstCompile]
        public void Execute(ref Actor actor, ref Mover mover)
        {
            mover.Direction = actor.Direction;
        }
    }
}
