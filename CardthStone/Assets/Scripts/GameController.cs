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
    using Managers;

    /// <summary>
    /// A server-side only object that controls the overall flow of the game
    /// </summary>
    public class GameController : NetworkBehaviour
    {
        /// <summary>
        /// The component for the player state manager
        /// </summary>
        public PlayerStateManager PlayerStateManagerComponent;

        /// <summary>
        /// Gets the current instance of the game controller
        /// </summary>
        public static GameController CurrentInstance { get; private set; }

        /// <summary>
        /// Gets or sets the current turn number. -1 if the game have not started yet
        /// </summary>
        public int TurnNumber;

        /// <summary>
        /// Gets the current phase of game
        /// </summary>
        public GamePhaseEnum CurrentPhase;

        /// <summary>
        /// Gets Id of the player who is in charge of the current turn
        /// </summary>
        public int CurrentPlayerId;

        /// <summary>
        /// Called when the turn is over
        /// </summary>
		[ClientRpc]
        public void RpcEndCurrentTurn()
        {
            this.TurnNumber++;

            if (this.CurrentPhase == GamePhaseEnum.Mulligan && this.TurnNumber >= 3)
            {
                this.TurnNumber = 1;
                this.CurrentPhase = GamePhaseEnum.Normal;
            }
            else
            {
                this.CurrentPlayerId = (this.CurrentPlayerId + 1) % Settings.MaxPlayerCount;
            }

			// Trigger turn manager new turn 
			if (this.isServer)
			{
				this.RpcOnStartTurn();
			}
			else
			{
				TurnManager.CurrentInstance.OnTurnStart();
			}
		}

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
        }

        /// <summary>
        /// Called every frame
        /// </summary>
        private void Update()
        {
            var player0HealthCards = this.PlayerStateManagerComponent.Player0State.HealthCards;
            var player1HealthCards = this.PlayerStateManagerComponent.Player1State.HealthCards;

            // Check to see if both players has placed down health cards
            if (this.CurrentPhase == GamePhaseEnum.Setup && player0HealthCards.Count > 0 && player1HealthCards.Count > 0)
            {
                this.StartGame();
            }
        }

        /// <summary>
        /// Reveal the health cards to the player
        /// </summary>
        private void StartGame()
        {
            // Compare health cards to see who goes first
            var player0HealthCard = PlayerStateManager.CurrentInstance.Player0State.HealthCards.Last();
            var player1HealthCard = PlayerStateManager.CurrentInstance.Player1State.HealthCards.Last();

            this.CurrentPhase = GamePhaseEnum.Mulligan;
            this.TurnNumber = 1;
            this.CurrentPlayerId = player0HealthCard > player1HealthCard ? 0 : 1;

			if (this.isServer)
			{
				this.RpcOnStartTurn();
			}
			else
			{
				TurnManager.CurrentInstance.OnTurnStart();
			}
		}

		/// <summary>
		/// Calls the turn manager to start a new turn
		/// </summary>
		[ClientRpc]
		private void RpcOnStartTurn()
		{
			TurnManager.CurrentInstance.OnTurnStart();
		}
    }
}
