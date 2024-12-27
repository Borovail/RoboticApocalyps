using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public class BulletAuthoring : MonoBehaviour
    {
        public class Baker : Baker<BulletAuthoring>
        {
            public override void Bake(BulletAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic),new Bullet());
            }
        }
        
    }

    public  struct Bullet : IComponentData
    {
        
    }
}