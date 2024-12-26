using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.Transforms;

namespace Assets.Scripts
{
    //public partial struct EnemySpawnSystem : ISystem
    //{
    //    private EnemySpawnerData _spawnerData;
    //    private float _timer;

    //    [BurstCompile]
    //    public void OnCreate(ref SystemState state)
    //    {
    //        state.RequireForUpdate<EnemySpawnerData>();
    //        _timer = 999;
    //    }

    //    [BurstCompile]
    //    public void OnUpdate(ref SystemState state)
    //    {
    //        _spawnerData = SystemAPI.GetSingleton<EnemySpawnerData>();

    //        _timer += SystemAPI.Time.DeltaTime;

    //        if (_timer < _spawnerData.Delay) return;
    //        _timer = 0;


    //        var enemies = new NativeArray<Entity>(_spawnerData.CountAtTime, Allocator.Temp);
    //        state.EntityManager.Instantiate(_spawnerData.Prefab, enemies);

    //        foreach (var enemy in enemies)
    //        {
    //            var units = Random.insideUnitCircle;
    //            units = math.normalize(units);
    //            units *= _spawnerData.Radius;
    //            SystemAPI.SetComponent(enemy, LocalTransform.FromPositionRotationScale(
    //                new float3(units.x, 1f, units.y) ,
    //                quaternion.identity,
    //                1f
    //                ));
    //        }
    //    }
    //}

    public partial struct EnemySpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var transformLookup = SystemAPI.GetComponentLookup<LocalTransform>();
            new EnemySpawnJob
            {
                CommandBuffer = ecbSingleton.AsParallelWriter(),
                DeltaTime = SystemAPI.Time.DeltaTime,
                TransformLookup = transformLookup
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct EnemySpawnJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter CommandBuffer;
        public float DeltaTime;

        [ReadOnly] public ComponentLookup<LocalTransform> TransformLookup;

        [BurstCompile]
        private void Execute([ChunkIndexInQuery] int index, ref EnemySpawnerData enemySpawnerData)
        {
            enemySpawnerData.Timer += DeltaTime;
            if (enemySpawnerData.Timer < enemySpawnerData.Delay) return;
            enemySpawnerData.Timer = 0;

            var enemies = new NativeArray<Entity>(enemySpawnerData.CountAtTime, Allocator.Temp);
            CommandBuffer.Instantiate(index, enemySpawnerData.Prefab, enemies);

            for (var i = 0; i < enemies.Length; i++)
            {
                float angle = 2f * math.PI * i / enemySpawnerData.CountAtTime;
                float x = math.cos(angle);
                float y = math.sin(angle);

                float2 units = new float2(x, y) * enemySpawnerData.Radius;

                var transform = TransformLookup[enemySpawnerData.Prefab];
                transform.Position = new float3(units.x, 1f, units.y);
                CommandBuffer.SetComponent(index, enemies[i], transform);
            }
        }
    }
}