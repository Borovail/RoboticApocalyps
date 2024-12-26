using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts
{
    public class ShooterAuthoring : MonoBehaviour
    {
        public double Cooldown;
        public GameObject ProjectilePrefab;

        public class Baker : Baker<ShooterAuthoring>
        {
            public override void Bake(ShooterAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Shooter
                {
                    Cooldown = authoring.Cooldown,
                    ProjectilePrefab = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic)
                });
            }
        }
    }

    public struct Shooter : IComponentData
    {
        public double Cooldown;
        public double LastTimeShoot;
        public Entity ProjectilePrefab;
    }
}