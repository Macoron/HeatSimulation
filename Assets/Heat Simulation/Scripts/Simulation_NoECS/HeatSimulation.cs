using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace HeatSimulation.NoECS
{
    public class HeatSimulation : MonoBehaviour
    {
        public Vector2Int gridSize = new Vector2Int(100, 100);

        public event System.Action<NativeArray<HeatCell>> OnNewFrameReady;

        private NativeArray<HeatCell> prevFrame, currentFrame;
        private JobHandle? jobHandle;
        private const int batchCount = 100;

        private void Awake()
        {
            GenerateGrid();
        }

        public void Update()
        {
            //if (Input.GetKeyDown(KeyCode.E))
                MakeStep();
        }

        private void GenerateGrid()
        {
            prevFrame = new NativeArray<HeatCell>(gridSize.x * gridSize.y, Allocator.Persistent);
            currentFrame = new NativeArray<HeatCell>(gridSize.x * gridSize.y, Allocator.Persistent);

            // задаем начальные свойства клетке
            for (int x = 0; x < gridSize.x; x++)
                for (int y = 0; y < gridSize.y; y++)
                {
                    // задаем начальную температуру клетке
                    var initTemparture = GetInitTemparture(x, y);
                    var heatCell = new HeatCell()
                    {
                        temparture = initTemparture
                    };

                    // присваеваем её начальном приближению
                    var curIndex = x + y * gridSize.x;
                    prevFrame[curIndex] = heatCell;
                    currentFrame[curIndex] = heatCell;
                }
        }

        private void MakeStep()
        {
            // предыдущий процесс ещё не закончил
            if (jobHandle != null)
            {
                if (jobHandle.Value.IsCompleted)
                    OnJobCompleted();
                else
                    return;
            }

            var jobData = new HeatSimulationJob();
            jobData.prevFrame = prevFrame;
            jobData.nextFrame = currentFrame;
            jobData.gridSize = gridSize;

            jobHandle = jobData.Schedule(gridSize.x * gridSize.y, batchCount);
        }

        private void OnJobCompleted()
        {
            jobHandle.Value.Complete();

            // меняем местами кадры
            var temp = prevFrame;
            prevFrame = currentFrame;
            currentFrame = temp;

            if (OnNewFrameReady != null)
                OnNewFrameReady(currentFrame);
        }

        private float GetInitTemparture(int x, int y)
        {
            if (x > 0 && x < gridSize.x - 1 && y > 0 && y < gridSize.y - 1)
            {
                return 1f;
            }
            else
                return 0f;

        }

        private void OnDisable()
        {
            if (jobHandle != null)
                jobHandle.Value.Complete();

            prevFrame.Dispose();
            currentFrame.Dispose();
        }
    }
}
