using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace HeatSimulation.ECS
{
    public class TextureVisualisationSystem : JobComponentSystem
    {
        private struct TextureGroup
        {
            public readonly int Length;
            [ReadOnly]
            public SharedComponentDataArray<TextureVisualisationData> cellGrids;
        }
        [Inject]
        private TextureGroup textureGroup;

        public JobHandle jobHandler;

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            for (int i = 0; i < textureGroup.Length; i++)
            {
                var curTexture = textureGroup.cellGrids[i];
                var job = new TextureVisualisationJob()
                {
                    visData = curTexture
                };

                jobHandler = job.Schedule(this, inputDeps);
                return jobHandler;
            }

            return base.OnUpdate(inputDeps);
        }

        protected override void OnStopRunning()
        {
            base.OnStopRunning();
            for (int i = 0; i < textureGroup.Length; i++)
            {
                textureGroup.cellGrids[i].colorTexture.Dispose();
            }
        }
    }
}