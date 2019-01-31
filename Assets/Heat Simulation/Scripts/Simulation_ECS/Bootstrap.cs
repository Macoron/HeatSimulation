using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Unity.Rendering;

namespace HeatSimulation.ECS
{
    public class Bootstrap : MonoBehaviour
    {
        public HeatGridSettings gridSettings;

        [Header("Visualisation")]
        public float minTemparture;
        public float maxTemparture;
        public StructGradient structGradient;

        public Material material;

        private NativeArray<Color> colorTexture;

        private void Start()
        {
            var heatGridFactory = World.Active.GetOrCreateManager<HeatGridFactory>();
            heatGridFactory.GenerateGrid(gridSettings);

            var manager = World.Active.GetOrCreateManager<EntityManager>();
            var visData = manager.CreateEntity(typeof(TextureVisualisationData), typeof(DrawMeshData));

            colorTexture = new NativeArray<Color>(gridSettings.gridSizeX * gridSettings.gridSizeY, Allocator.Persistent);
            manager.SetSharedComponentData(visData, new TextureVisualisationData()
            {
                colorTexture = colorTexture,
                gradient = structGradient,
                minTemparture = minTemparture,
                maxTemparture = maxTemparture,
                witdh = gridSettings.gridSizeX,
                height = gridSettings.gridSizeY
            });

            var texture = new Texture2D(gridSettings.gridSizeX, gridSettings.gridSizeY, TextureFormat.RGB24, false);
            material.mainTexture = texture;

            manager.SetSharedComponentData(visData, new DrawMeshData()
            {
                material = material,
                texture = texture
            });
        }
    }
}