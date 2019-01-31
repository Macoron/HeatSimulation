using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace HeatSimulation.ECS
{
    public class HeatSimulationSystem : JobComponentSystem
    {
        private struct Group
        {
            public readonly int Length;

            [ReadOnly]
            public SharedComponentDataArray<HeatGrid> cellGrids;
            [ReadOnly]
            public EntityArray entities;
        }
        [Inject]
        private Group injectedGroup;

        [Inject]
        private EntityManager entityManager;

        private NativeArray<HeatCell>? gridArr;

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            // хоть здесь и цикл - возьмем первую сетку из списка
            for (int i = 0; i < injectedGroup.Length; i++)
            {
                var mainGrid = injectedGroup.cellGrids[i];

                var myTypeFromEntity = GetComponentDataFromEntity<HeatCell>(true);

                // инициализируем массив, если он ещё не создан
                if (gridArr == null)
                    gridArr = new NativeArray<HeatCell>(mainGrid.gridSizeX * mainGrid.gridSizeY, Allocator.Persistent);

                // заполняем массив, в том случае, если он изменился из вне
                var gridArrVal = gridArr.Value;
                var copyJob = new GridCopyJob()
                {
                    dataFromEntity = myTypeFromEntity,
                    heatCells = gridArrVal,
                    grid = mainGrid.grid
                };
                var gridCopyJob = copyJob.Schedule(mainGrid.gridSizeX * mainGrid.gridSizeY, 128, inputDeps);

                // тут проводим обработку
                var simJob = new HeatSimulationJob()
                {
                    gridSizeX = mainGrid.gridSizeX,
                    gridSizeY = mainGrid.gridSizeY,
                    prevGrid = gridArrVal
                };

                var jobHandle = simJob.Schedule(this, gridCopyJob);
                return jobHandle;
            }

            return base.OnUpdate(inputDeps);
        }

        protected override void OnDestroyManager()
        {
            base.OnDestroyManager();
            for (int i = 0; i < injectedGroup.Length; i++)
            {
                var mainGrid = injectedGroup.cellGrids[i];
                mainGrid.grid.Dispose();
            }

            if (gridArr != null)
                gridArr.Value.Dispose();
        }
    }
}
