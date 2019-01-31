using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

// класс настроек для генерация сетки
[System.Serializable]
public struct HeatGridSettings : IComponentData
{
    // размер сетки
    public int gridSizeX, gridSizeY;
}

public class HeatGridSettingsComponent : ComponentDataWrapper<HeatGridSettings>
{

}