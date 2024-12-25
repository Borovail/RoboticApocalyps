﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts
{
    public class MoverAuthoring : MonoBehaviour
    {
        public float MaxSpeed = 5f;
        public float RotationSpeed;
        public float Acceleration = 10f;

        public class Baker : Baker<MoverAuthoring>
        {
            public override void Bake(MoverAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Mover
                {
                    MaxSpeed = authoring.MaxSpeed,
                    RotationSpeed = authoring.RotationSpeed,
                    Acceleration = authoring.Acceleration,
                });
            }
        }

    }

    public struct Mover : IComponentData
    {
        public float MaxSpeed;
        public float RotationSpeed;
        public float Acceleration;
        public float3 AccumulatedForce;
        public float3 Direction;
    }
}
