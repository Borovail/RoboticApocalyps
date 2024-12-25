using System.Linq;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    public partial struct CloneAbilityResetSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (_, entity) in SystemAPI.Query<RefRO<CloneAction>>().WithDisabled<CloneAction>().WithEntityAccess())
            {
                bool clones = false;

                foreach (var _ in SystemAPI.Query<RefRO<LifeTime>>().WithAny<Player>())
                {
                    clones = true;
                    break;
                }

                if (!clones)
                    state.EntityManager.SetComponentEnabled<CloneAction>(entity, true);
            }
        }
    }
}