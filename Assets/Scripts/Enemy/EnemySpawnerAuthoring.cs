using Unity.Entities;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemySpawnerAuthoring : MonoBehaviour
    {
        public float Radius;
        public float Delay;
        public int CountAtTime;
        public GameObject Prefab;

        public class Baker : Baker<EnemySpawnerAuthoring>
        {
            public override void Bake(EnemySpawnerAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None), new EnemySpawnerData
                {
                    Radius = authoring.Radius,
                    CountAtTime = authoring.CountAtTime,
                    Delay = authoring.Delay,
                    Prefab = GetEntity(authoring.Prefab,TransformUsageFlags.Dynamic),
                    Timer = authoring.Delay
                });
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }
    }

    public struct EnemySpawnerData : IComponentData
    {
        public Entity Prefab;
        public float Radius;
        public float Delay;
        public int CountAtTime;
        public float Timer;
    }
}