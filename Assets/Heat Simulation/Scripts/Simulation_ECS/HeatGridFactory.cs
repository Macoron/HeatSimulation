using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace HeatSimulation.ECS
{
    public class HeatGridFactory : ComponentSystem
    {
        [Inject]
        private EntityManager entityManager;

        protected override void OnUpdate()
        {
            // do nothing
        }

        public Entity GenerateGrid(HeatGridSettings settings)
        {
            // Создаем тепловые клетки
            var heatCellArch = entityManager.CreateArchetype(typeof(HeatCell));        
            var entityGrid = new NativeArray<Entity>(settings.gridSizeX * settings.gridSizeY, Allocator.Persistent);
            entityManager.CreateEntity(heatCellArch, entityGrid);

            // задаем начальные свойства клеткам
            for (int x = 0; x < settings.gridSizeX; x++)
                for (int y = 0; y < settings.gridSizeY; y++)
                {
                    // сохраняем полученную сущность
                    var curIndex = x + y * settings.gridSizeX;

                    // задаем начальную температуру клетке
                    var initTemparture = GetInitTemparture(x, y, settings);
                    var heatCell = new HeatCell()
                    {
                        temparture = initTemparture,
                        index = curIndex
                    };


                    entityManager.SetComponentData(entityGrid[curIndex], heatCell);
                }

            // Теперь создаем свойства самой сетки
            var heatGrid = entityManager.CreateEntity(typeof(HeatGrid));
            entityManager.SetSharedComponentData<HeatGrid>(heatGrid, new HeatGrid()
            {
                gridSizeX = settings.gridSizeX,
                gridSizeY = settings.gridSizeY,
                grid = entityGrid
            });

            return heatGrid;
        }

        private float GetInitTemparture(int x, int y, HeatGridSettings settings)
        {
            if (x > 0 && x < settings.gridSizeX - 1 && y > 0 && y < settings.gridSizeY - 1)
            {
                return 1f;
            }
            else
                return 0f;
        }
    }

}

