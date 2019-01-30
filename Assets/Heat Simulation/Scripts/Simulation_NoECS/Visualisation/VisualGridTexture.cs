using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

using System.Linq;

namespace HeatSimulation.NoECS
{
    public class VisualGridTexture : MonoBehaviour
    {
        public HeatSimulation heatSimulation;
        public Material targetMaterial;

        public Gradient gradient;
        public float minTemparture = 0;
        public float maxTemparture = 1;

        private Color[] pixels;
        private Vector2Int gridSize;
        private Texture2D targetTexture;

        private void Awake()
        {
            gridSize = heatSimulation.gridSize;
            GenerateCells(gridSize);

            heatSimulation.OnNewFrameReady += OnNewFrameReady;
        }

        private void OnNewFrameReady(NativeArray<HeatCell> frame)
        {
            for (int x = 0; x < gridSize.x; x++)
                for (int y = 0; y < gridSize.y; y++)
                {
                    int curIndex = x + y * gridSize.x;

                    var normalizeTemp = (frame[curIndex].temparture - minTemparture) / (maxTemparture - minTemparture);
                    var color = gradient.Evaluate(normalizeTemp);

                    pixels[curIndex] = color;
                }

            targetTexture.SetPixels(pixels);
            targetTexture.Apply();
        }

        private void GenerateCells(Vector2Int gridSize)
        {
            targetTexture = new Texture2D(gridSize.x, gridSize.y, TextureFormat.RGB24, false);
            pixels = new Color[gridSize.x * gridSize.y];

            targetMaterial.mainTexture = targetTexture;
        }
    }
}
