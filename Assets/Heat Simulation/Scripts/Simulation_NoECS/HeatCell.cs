using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace HeatSimulation.NoECS
{
    public struct HeatCell : IComponentData
    {
        public float temparture;
    }
}

