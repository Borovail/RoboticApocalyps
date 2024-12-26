using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class TargetAuthoring : MonoBehaviour
    {
        public TargetType TargetType;

        public class Baker : Baker<TargetAuthoring>
        {
            public override void Bake(TargetAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Target
                {
                    TargetType = authoring.TargetType
                });
            }
        }
    }

    public struct Target : IComponentData
    {
        public TargetType TargetType;
    }

    public enum TargetType
    {
        Player,
        Enemy
    }
}