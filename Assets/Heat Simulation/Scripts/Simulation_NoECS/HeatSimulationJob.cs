using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace HeatSimulation.NoECS
{
    public struct HeatSimulationJob : IJobParallelFor
    {
        public const float alpha = 0.25f; 

        // Предыдущий кадр
        [ReadOnly]
        public NativeArray<HeatCell> prevFrame;
        // Следующий кадр сетки
        public NativeArray<HeatCell> nextFrame;

        // Размер сетки
        public Vector2Int gridSize;

        // Взял за основу реализацию https://rohitnarurkar.wordpress.com/2013/11/21/2-d-transient-heat-conduction-part-1/
        public void Execute(int index)
        {
            int x = index % gridSize.x;
            int y = (index - x) / gridSize.x;

            // проверяем что сейчас не пограничная клетка
            if (x > 0 && x < gridSize.x - 1 && y > 0 && y < gridSize.y - 1)
            {
                // лево, право
                var im10 = index - 1;
                var ip10 = index + 1;

                // низ, вверх
                var i0m1 = index - gridSize.x;
                var i0p1 = index + gridSize.x;

                float d2tdx2 = prevFrame[im10].temparture - 2 * prevFrame[index].temparture + prevFrame[ip10].temparture;
                float d2tdy2 = prevFrame[i0m1].temparture - 2 * prevFrame[index].temparture + prevFrame[i0p1].temparture;

                // обновляем температуру в nextFrame
                var heatCell = nextFrame[index];
                heatCell.temparture = prevFrame[index].temparture + alpha * (d2tdx2 + d2tdy2);
                nextFrame[index] = heatCell;
            }


        }

    }
}