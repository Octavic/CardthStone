//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="ColorManager.cs" company="Yifei Xu">
//    Copyright (c) Yifei Xu.  All rights reserved.
//  </copyright>
//--------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Manages the color
    /// </summary>
    public class ColorManager : MonoBehaviour
    {
        /// <summary>
        /// The red and black color tints
        /// </summary>
        public List<Color> NumberColors;

        /// <summary>
        /// Gets the current singleton instance
        /// </summary>
        public static ColorManager CurrentInstance { get; private set; }

        /// <summary>
        /// Gets the correct color from the suit
        /// </summary>
        /// <param name="suit">Target suit</param>
        /// <returns>Correct color tint</returns>
        public Color GetCardColorFromSuit(CardSuitEnum suit)
        {
            return this.NumberColors[(int)Helpers.SuitToColor(suit)];
        }

        /// <summary>
        /// used for initialization
        /// </summary>
        private void Start()
        {
            if (ColorManager.CurrentInstance != null)
            {
                Destroy(ColorManager.CurrentInstance);
            }

            ColorManager.CurrentInstance = this;
        }
    }
}
