//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="Settings.cs" company="Yifei Xu">
//    Copyright (c) Yifei Xu.  All rights reserved.
//  </copyright>
//--------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// A class containing a collection of various settings
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Gets the total number of allowed players
        /// </summary>
        public static int MaxPlayerCount
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// Gets the starting deck size for the player
        /// </summary>
        public static int PlayerStartingDeckSize
        {
            get
            {
                return 26;
            }
        }

        /// <summary>
        /// Gets the size of player's starting hand
        /// </summary>
        public static int PlayerStaringHandSize
        {
            get
            {
                return 7;
            }
        }

        /// <summary>
        /// Gets the height of a card that's selected
        /// </summary>
        public static float SelectedCardHeight
        {
            get
            {
                return 40;
            }
        }

        /// <summary>
        /// Gets the distance between each cards in the player's hand
        /// </summary>
        public static float PlayerHandCardDistance
        {
            get
            {
                return 45.0f;
            }
        }

        /// <summary>
        /// Gets the player hand limit
        /// </summary>
        public static int PlayerHandLimit
        {
            get
            {
                return 10;
            }
        }

        /// <summary>
        /// Gets the maximum amount of creatures that can be on the board for a single player
        /// </summary>
        public static int CreatureLimit
        {
            get
            {
                return 6;
            }
        }

        /// <summary>
        /// The horizontal distance between creatures
        /// </summary>
        public static float CreatureDistance
        {
            get
            {
                return 2f;
            }
        }

        /// <summary>
        /// The health assign UI final position
        /// </summary>
        private static Vector2 _healthAssignUIFinalPosition = new Vector2(642, 116);

        /// <summary>
        /// Gets the final position of the health assign UI
        /// </summary>
        public static Vector2 HealthAssignUIFinalPosition
        {
            get
            {
                return _healthAssignUIFinalPosition;
            }
        }

        /// <summary>
        /// The mulligan UI final position
        /// </summary>
        private static Vector2 _mulliganUIFinalPosition = new Vector2(585, 68);

        /// <summary>
        /// Gets the final position of the mulligan UI;
        /// </summary>
        public static Vector2 MulliganUIFinalPosition
        {
            get
            {
                return _mulliganUIFinalPosition;
            }
        }

        /// <summary>
        /// Gets the amount of time the health assign UI has to go to normal position
        /// </summary>
        public static float UIMovementDuration
        {
            get
            {
                return 0.2f;
            }
        }

		/// <summary>
		/// Gets the starting index number for creature ids
		/// </summary>
		public static int CreatureStartingId
		{
			get
			{
				return Settings.MaxPlayerCount;
			}
		}
    }
}
