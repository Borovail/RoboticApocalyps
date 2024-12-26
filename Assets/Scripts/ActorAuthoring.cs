using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts
{
    public class ActorAuthoring : MonoBehaviour
    {
        public TargetType TargetType;

        public class Baker : Baker<ActorAuthoring>
        {
            public override void Bake(ActorAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Actor
                {
                    TargetType = authoring.TargetType
                });
            }
        }

    }

    public struct Actor : IComponentData
    {
        public TargetType TargetType;
        public float3 Direction;
    }
}