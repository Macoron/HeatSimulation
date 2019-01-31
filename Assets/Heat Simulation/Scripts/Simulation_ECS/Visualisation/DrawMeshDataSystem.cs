using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace HeatSimulation.ECS
{
    public class DrawMeshDataSystem : ComponentSystem
    {
        private struct TextureGroup
        {
            public readonly int Length;
            [ReadOnly]
            public SharedComponentDataArray<TextureVisualisationData> textures;
            [ReadOnly]
            public SharedComponentDataArray<DrawMeshData> meshData;
        }
        [Inject]
        private TextureGroup textureGroup;

        [Inject]
        private TextureVisualisationSystem visualisationJob;

        protected override void OnUpdate()
        {
            for (int i = 0; i < textureGroup.Length; i++)
            {
                visualisationJob.jobHandler.Complete();

                var texture = textureGroup.textures[i];
                var meshData = textureGroup.meshData[i];
                var colorArr = texture.colorTexture.ToArray();

                meshData.texture.SetPixels(colorArr);
                meshData.texture.Apply();
            }
        }
    }
}
