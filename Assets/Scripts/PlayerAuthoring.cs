using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{

    public class PlayerAuthoring : MonoBehaviour
    {
        public class Baker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Player());
            }
        }
    }

    public struct Player : IComponentData
    {
    }
}