using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace HeatSimulation.ECS
{
    // нету стандартного градиента в виде структуры
    [System.Serializable]
    public struct StructGradient
    {
        public Color startKey;
        public Color midKey;
        public Color endKey;
    }

    public struct TextureVisualisationData : ISharedComponentData
    {
        public float minTemparture;
        public float maxTemparture;

        public NativeArray<Color> colorTexture;
        public StructGradient gradient;

        [HideInInspector]
        public int witdh;
        [HideInInspector]
        public int height;
    }
}