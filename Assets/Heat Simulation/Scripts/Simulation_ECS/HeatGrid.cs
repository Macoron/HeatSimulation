using Unity.Collections;
using Unity.Entities;

namespace HeatSimulation.ECS
{
    [System.Serializable]
    public struct HeatCell : IComponentData
    {
        // из-за особеностей работы ECS job system - нужно хранить текущую позицию блока в самом блоке
        // пока что я не нашел способа обойти это
        public int index;
        // текущая температура клетки
        public float temparture;
    }

    [System.Serializable]
    public struct HeatGrid : ISharedComponentData
    {
        // размер сетки
        public int gridSizeX, gridSizeY;
        // сама сетка
        public NativeArray<Entity> grid;
    }
}

