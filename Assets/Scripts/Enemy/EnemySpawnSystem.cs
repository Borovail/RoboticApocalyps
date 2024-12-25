using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public partial struct EnemySpawnSystem : ISystem
    {
        private EnemySpawnerData _spawnerData;
        private float _timer;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EnemySpawnerData>();
            _timer = 999;
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _spawnerData = SystemAPI.GetSingleton<EnemySpawnerData>();

            _timer += SystemAPI.Time.DeltaTime;

            if (_timer < _spawnerData.Delay) return;
            _timer = 0;


            var enemies = new NativeArray<Entity>(_spawnerData.CountAtTime, Allocator.Temp);
            state.EntityManager.Instantiate(_spawnerData.Prefab, enemies);

            foreach (var enemy in enemies)
            {
                var units = Random.insideUnitCircle;
                units = math.normalize(units);
                units *= _spawnerData.Radius;
                SystemAPI.SetComponent(enemy, LocalTransform.FromPositionRotationScale(
                    new float3(units.x, 1f, units.y) ,
                    quaternion.identity,
                    1f
                    ));
            }
        }
    }
}