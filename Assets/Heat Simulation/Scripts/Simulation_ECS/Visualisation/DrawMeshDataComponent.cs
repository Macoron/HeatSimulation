using Unity.Entities;
using UnityEngine;

namespace HeatSimulation.ECS
{
    [System.Serializable]
    public struct DrawMeshData : ISharedComponentData
    {
        public Material material;
        [HideInInspector]
        public Texture2D texture;
    }

    public class DrawMeshDataComponent : SharedComponentDataWrapper<DrawMeshData>
    {

    }
}