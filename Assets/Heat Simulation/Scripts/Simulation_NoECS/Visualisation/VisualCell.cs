using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeatSimulation.NoECS
{
    public class VisualCell : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;

        public Color CurrentColor
        {
            set
            {
                if (spriteRenderer == null)
                    spriteRenderer = GetComponent<SpriteRenderer>();

                spriteRenderer.color = value;
            }
        }
    }
}