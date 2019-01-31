using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace HeatSimulation.ECS
{
    public struct GridCopyJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Entity> grid;
        [ReadOnly]
        public ComponentDataFromEntity<HeatCell> dataFromEntity;
        [WriteOnly]
        public NativeArray<HeatCell> heatCells;

        public void Execute(int index)
        {
            var entity = grid[index];
            var data = dataFromEntity[entity];

            heatCells[index] = data;
        }
    }
}