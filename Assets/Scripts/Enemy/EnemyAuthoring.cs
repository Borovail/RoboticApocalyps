using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyAuthoring : MonoBehaviour
    {
        public class Baker : Baker<EnemyAuthoring>
        {
            public override void Bake(EnemyAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None), new Enemy());
                AddComponent(GetEntity(TransformUsageFlags.Dynamic),new LifeTime{Value = 10});
            }
        }
    }

    public struct Enemy : IComponentData { }
}
