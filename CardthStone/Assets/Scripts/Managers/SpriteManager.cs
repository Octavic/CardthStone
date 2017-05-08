//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="SpriteManager.cs" company="Yifei Xu">
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
    /// Manages all of the sprites
    /// </summary>
    public class SpriteManager : MonoBehaviour
    {
        /// <summary>
        /// A list of suit sprites to be used as icon
        /// </summary>
        public List<Sprite> SuitIconSprites;

        /// <summary>
        /// A list of possible card backs
        /// </summary>
        public List<Sprite> CardBacks;

		/// <summary>
		/// A list of suit sprites that appears on the card
		/// </summary>
		public List<Sprite> SuitDoubleSprites;

		/// <summary>
		/// A list of card number sprites that appears on the card
		/// </summary>
		public List<Sprite> CardNumberSprites;

        /// <summary>
        /// Gets the current instance
        /// </summary>
        public static SpriteManager CurrentInstance { get; private set; }

        /// <summary>
        /// Used for initialization
        /// </summary>
        private void Start()
        {
            if (SpriteManager.CurrentInstance != null)
            {
                Destroy(SpriteManager.CurrentInstance);
            }

            SpriteManager.CurrentInstance = this;
        }
    }
}
