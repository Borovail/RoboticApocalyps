using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Scripts
{
    public partial struct ShootingSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            var moverLookup = SystemAPI.GetComponentLookup<Mover>();
            var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>();

            new ShootingJob
            {
                Ecb = ecbSingleton.AsParallelWriter(),
                DeltaTime = SystemAPI.Time.DeltaTime,
                MoverLookup = moverLookup,
                TransformLookup = transformLookup
            }.ScheduleParallel();
        }
    }
    public partial struct ShootingJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter Ecb;
        public double DeltaTime;

        [ReadOnly] public ComponentLookup<Mover> MoverLookup;
        [ReadOnly] public ComponentLookup<LocalTransform> TransformLookup;

        [BurstCompile]
        public void Execute([ChunkIndexInQuery] in int index, ref Shooter shooter, in LocalTransform transform)
        {
            shooter.LastTimeShoot -= DeltaTime;
            if (shooter.LastTimeShoot > 0 || shooter.Direction.Equals(float3.zero)) return;

            shooter.LastTimeShoot = shooter.Cooldown;

            var bullet = Ecb.Instantiate(index, shooter.ProjectilePrefab);

            var bulletTransform = TransformLookup[shooter.ProjectilePrefab];
            var bulletMover = MoverLookup[shooter.ProjectilePrefab];

            bulletTransform.Position = transform.Position;
            bulletMover.Direction = shooter.Direction;

            Ecb.SetComponent(index, bullet, bulletTransform);
            Ecb.SetComponent(index, bullet, bulletMover);
        }
    }
}