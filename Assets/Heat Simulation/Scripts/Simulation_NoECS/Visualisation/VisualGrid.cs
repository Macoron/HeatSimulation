using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace HeatSimulation.NoECS
{
    public class VisualGrid : MonoBehaviour
    {
        public VisualCell visualCellPrefab;
        public HeatSimulation heatSimulation;

        // кол-во визуальных ячеек на логические
        public int visPerLogic = 10;

        // общий размер сетки
        public float grtdSize = 10;

        private int cellCountX, cellCountY;
        private Vector2Int gridSize;

        public Gradient gradient;
        public float minTemparture = 0;
        public float maxTemparture = 1;

        [HideInInspector]
        public VisualCell[] cells;

        private void Awake()
        {
            // генерируем поле
            gridSize = heatSimulation.gridSize;
            GenerateCells(gridSize);

            heatSimulation.OnNewFrameReady += OnNewFrameReady;
        }

        private void OnNewFrameReady(NativeArray<HeatCell> heatCells)
        {
            for (int x = 0; x < cellCountX; x++)
                for (int y = 0; y < cellCountY; y++)
                {
                    float avgInBlock = GetAvgInBlock(heatCells, x, y);

                    var normalizeTemp = (avgInBlock - minTemparture) / (maxTemparture - minTemparture);
                    var color = gradient.Evaluate(normalizeTemp);

                    int curIndex = x + y * cellCountX;
                    cells[curIndex].CurrentColor = color;
                }
        }

        private float GetAvgInBlock(NativeArray<HeatCell> heatCells, int blockX, int blockY)
        {
            int xStart = visPerLogic * blockX;
            int xEnd = visPerLogic * (blockX + 1);

            int yStart = visPerLogic * blockY;
            int yEnd = visPerLogic * (blockY + 1);

            float avg = 0;
            for (int x = xStart; x < xEnd; x++)
                for (int y = yStart; y < yEnd; y++)
                {
                    int curIndex = x + y * gridSize.x;
                    avg += heatCells[curIndex].temparture;
                }

            return avg / (visPerLogic * visPerLogic);
        }

        private void GenerateCells(Vector2Int size)
        {
            // определяем кол-во ячеек
            cellCountX = size.x / visPerLogic;
            cellCountY = size.y / visPerLogic;

            // определяем физический размер ячейки
            var cellSizeX = grtdSize / cellCountX;
            var cellSizeY = grtdSize / cellCountY;

            // определяем нижнию левую точку
            float gridStartPointX = -grtdSize / 2, gridStartPointY = -grtdSize / 2;

            var cellsList = new List<VisualCell>(cellCountX * cellCountY);
            for (int x = 0; x < cellCountX; x++)
                for (int y = 0; y < cellCountY; y++)
                {
                    var xPos = gridStartPointX + x * cellSizeX;
                    var yPos = gridStartPointY + y * cellSizeY;

                    var cellInst = Instantiate<VisualCell>(visualCellPrefab, transform);
                    cellInst.transform.localScale = new Vector3(cellSizeX, cellSizeY, 1f);
                    cellInst.transform.localPosition = new Vector2(xPos, yPos);

                    cellInst.name = string.Format("({0} {1})", x, y);

                    cellsList.Add(cellInst);
                }

            cells = cellsList.ToArray();
        }
    }
}

