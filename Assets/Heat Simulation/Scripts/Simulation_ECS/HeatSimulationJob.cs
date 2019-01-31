using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace HeatSimulation.ECS
{
    public struct HeatSimulationJob : IJobProcessComponentData<HeatCell>
    {
        public const float alpha = 0.25f;

        [ReadOnly]
        public NativeArray<HeatCell> prevGrid;
        [ReadOnly]
        public int gridSizeX;
        [ReadOnly]
        public int gridSizeY;

        public void Execute(ref HeatCell currentCell)
        {
            int index = currentCell.index;
            int x = index % gridSizeX;
            int y = (index - x) / gridSizeX;

            if (x > 0 && x < gridSizeX - 1 && y > 0 && y < gridSizeY - 1)
            {
                // лево, право
                var im10 = index - 1;
                var ip10 = index + 1;

                // низ, вверх
                var i0m1 = index - gridSizeX;
                var i0p1 = index + gridSizeX;

                float d2tdx2 = prevGrid[im10].temparture - 2 * prevGrid[index].temparture + prevGrid[ip10].temparture;
                float d2tdy2 = prevGrid[i0m1].temparture - 2 * prevGrid[index].temparture + prevGrid[i0p1].temparture;

                currentCell.temparture = prevGrid[index].temparture + alpha * (d2tdx2 + d2tdy2);
            }
        }
    }
}