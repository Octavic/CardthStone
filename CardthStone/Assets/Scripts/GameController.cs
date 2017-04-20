//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="GameController.cs" company="Yifei Xu">
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
    using UnityEngine.Networking;
    using States;

    /// <summary>
    /// The unified game controller
    /// </summary>
    public class GameController : MonoBehaviour
    {
        /// <summary>
        /// The player state for player 0
        /// </summary>
        public PlayerState Player0State;

        /// <summary>
        /// The player state for player 1
        /// </summary>
        public PlayerState Player1State;

        /// <summary>
        /// Gets the current instance of the game controller
        /// </summary>
        public static GameController CurrentInstance { get; private set; }

        /// <summary>
        /// A list of player states
        /// </summary>
        public IList<PlayerState> PlayerStates { get; private set; }

        /// <summary>
        /// Used for initialization
        /// </summary>
        private void Awake()
        {
            if (GameController.CurrentInstance != null)
            {
                Destroy(GameController.CurrentInstance);
            }

            GameController.CurrentInstance = this;

            // Assigns the player state
            this.PlayerStates = new List<PlayerState>();
            this.PlayerStates.Add(this.Player0State);
            this.PlayerStates.Add(this.Player1State);
        }

        /// <summary>
        /// Gets the user's player state
        /// </summary>
        /// <param name="userId">The player Id</param>
        /// <returns>The player's state</returns>
        public PlayerState GetUserPlayerState(int playerId)
        {
            if (playerId < 0 || playerId > 1)
            {
                return null;
            }

            return this.PlayerStates[playerId];
        }

        /// <summary>
        /// Gets the enemy's player state
        /// </summary>
        /// <param name="playerId">Player Id</param>
        /// <returns>Player state of the enemy</returns>
        public PlayerState GetEnemyPlayerState(int playerId)
        {
            if (playerId == 0)
            {
                return this.PlayerStates[1];
            }
            else
            {
                return this.PlayerStates[0];
            }
        }
    }
}
