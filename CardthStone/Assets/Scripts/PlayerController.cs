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
        private PlayerState _myPlayerState;

        /// <summary>
        /// The player state of the enemy
        /// </summary>
        private PlayerState _enemyPlayerState;

        /// <summary>
        /// Called when the local player is started
        /// </summary>
        public override void OnStartLocalPlayer()
        {
            // Assigns a player ID starting from 0;
            var playerId = GameObject.FindGameObjectsWithTag(Tags.Player).Count() - 1;
            
            // Sets player ID
            this.CmdSetPlayerId(playerId);
            Helpers.CurrentPlayerId = playerId;

            // Get all the states
            this.PopulatePlayerStates();

            // Update local player
            if (isLocalPlayer)
            {
                PlayerController.LocalPlayer = this;
            }

            if (PlayerId == 1)
            {
                var sharedState = GameObject.FindGameObjectWithTag(Tags.SharedState).GetComponent<SharedState>();
                sharedState.CmdRequestDealToPlayer();
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
        /// Request the server to broadcast a card draw
        /// </summary>
        /// <param name="playerId">Id of the current player</param>
        [Command]
        public void CmdRequestCardDraw(int playerId)
        {
            this.RpcPlayerDrawCard(playerId);
        }

        /// <summary>
        /// Draws a card into the player's hand from the deck
        /// </summary>
        /// <param name="playerId">Id of the current player</param>
        [ClientRpc]
        private void RpcPlayerDrawCard(int playerId)
        {
            if (!isServer)
            {
                return;
            }
            if (this.PlayerId != playerId)
            {
                return;
            }

            if (this._myPlayerState == null)
            {
                this.PopulatePlayerStates();
            }

            this._myPlayerState.DrawCardFromPlayerDeck();
            Debug.Log("Player " + this.PlayerId + " draws a card");
        }

        /// <summary>
        /// Populates the player states
        /// </summary>
        private void PopulatePlayerStates()
        {
            var gameController = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameController>();
            var playerId = this.PlayerId;
            this._myPlayerState = playerId == 0 ? gameController.Player0State : gameController.Player1State;
            this._enemyPlayerState = playerId != 0 ? gameController.Player0State : gameController.Player1State;
        }
    }
}