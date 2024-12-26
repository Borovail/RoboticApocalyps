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
            }
        }
    }

    public struct Enemy : IComponentData { }
}
