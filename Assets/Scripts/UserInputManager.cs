using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class UserInputManager : MonoBehaviour
    {
        [SerializeField] private Joystick _joystick;

        private void Update()
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var query = new EntityQueryBuilder(Allocator.Temp).WithAll<Player>().WithAll<Mover>().Build(entityManager);
            var entities = query.ToEntityArray(Allocator.Temp);
            var moverUnits = query.ToComponentDataArray<Mover>(Allocator.Temp);

            for (int i = 0; i < moverUnits.Length; i++)
            {
                var unit = moverUnits[i];
                unit.Direction = _joystick.Direction;
                entityManager.SetComponentData(entities[i], unit);
            }
        }
    }
}
