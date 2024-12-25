using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class LifetimeAuthoring : MonoBehaviour
    {
        public float LifeTime;

        public class Baker : Baker<LifetimeAuthoring>
        {
            public override void Bake(LifetimeAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic),new LifeTime{Value = authoring.LifeTime});
            }
        }
    }

    public struct LifeTime : IComponentData, IEnableableComponent
    {
        public float Value;
    }
}
