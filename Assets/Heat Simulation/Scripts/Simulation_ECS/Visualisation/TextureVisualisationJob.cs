using Unity.Entities;
using UnityEngine;

namespace HeatSimulation.ECS
{
    public struct TextureVisualisationJob : IJobProcessComponentData<HeatCell>
    {
        public TextureVisualisationData visData;

        public void Execute(ref HeatCell data)
        {
            var cellColor = GetColor(data.temparture);
            var index = data.index;

            visData.colorTexture[index] = cellColor;
        }

        private Color GetColor(float val)
        {
            var normVal = (val - visData.minTemparture) / (visData.maxTemparture - visData.minTemparture);
            var clampedVal = Mathf.Clamp(normVal, visData.minTemparture, visData.maxTemparture);

            Color firstColor, secondColor;
            float lerpVal;

            if (clampedVal <= 0.5f)
            {
                firstColor = visData.gradient.startKey;
                secondColor = visData.gradient.midKey;
                lerpVal = clampedVal / 0.5f;
            }
            else
            {
                firstColor = visData.gradient.midKey;
                secondColor = visData.gradient.endKey;
                lerpVal = (clampedVal - 0.5f) / 0.5f;
            }

            return Color.Lerp(firstColor, secondColor, lerpVal);
        }
    }
}