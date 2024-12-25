using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class CloneActionAuthoring : MonoBehaviour
    {
        public GameObject Prefab;
        public int Count = 1;
        public float Lifetime = 1;

        public class Baker : Baker<CloneActionAuthoring>
        {
            public override void Bake(CloneActionAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new CloneAction
                {
                    Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
                    Count = authoring.Count,
                    Lifetime = authoring.Lifetime
                });
                AddComponent(entity, new LifeTime ());
                SetComponentEnabled<LifeTime>(entity, false);
            }
        }
    }

    public struct CloneAction : IComponentData, IEnableableComponent
    {
        public Entity Prefab;
        public int Count;
        public float Lifetime;
    }

}