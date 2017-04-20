//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="PlayerController.cs" company="Yifei Xu">
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
    using UnityEngine.UI;
    using States;

    /// <summary>
    /// Controls the player behavior
    /// </summary>
    public class PlayerController : NetworkBehaviour
    {
        /// <summary>
        /// The player Id for the current player
        /// </summary>
        public int PlayerId;

        /// <summary>
        /// Gets the local player
        /// </summary>
        public static PlayerController LocalPlayer { get; private set; }

        /// <summary>
        /// The player state for the current player
        /// </summary>
        public PlayerState MyPlayerState;

        /// <summary>
        /// The player state of the enemy
        /// </summary>
        public PlayerState EnemyPlayerState;

        private void Update()
        {
            var gameControllerInstance = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameController>();
            this.MyPlayerState = gameControllerInstance.PlayerStates[this.PlayerId]; ;
            this.EnemyPlayerState =  gameControllerInstance.PlayerStates[(this.PlayerId + 1) % 2];
        }

        /// <summary>
        /// Called when the local player is started
        /// </summary>
        public override void OnStartLocalPlayer()
        {
            // Assigns a player ID starting from 0;
            var newPlayerId = GameObject.FindGameObjectsWithTag(Tags.Player).Count() - 1;
            this.PlayerId = newPlayerId;

            // Sets player ID
            this.CmdSetPlayerId(newPlayerId);

            // Update local player
            if (this.isLocalPlayer)
            {
                PlayerController.LocalPlayer = this;
            }

            base.OnStartLocalPlayer();
        }

        /// <summary>
        /// Sets the player id
        /// </summary>
        /// <param name="playerId">New player id</param>
        [Command]
        public void CmdSetPlayerId(int playerId)
        {
            this.PlayerId = playerId;
        }

        /// <summary>
        /// Calls the server to draw a card
        /// </summary>
        [Command]
        public void CmdDrawCard()
        {
            this.MyPlayerState.DrawCardFromDeck();
        }
    }
}