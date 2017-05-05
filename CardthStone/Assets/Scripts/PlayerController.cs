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
	using Managers;
	using Intent;

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
        public PlayerState MyPlayerState
        {
            get
            {
                var gameControllerInstance = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<PlayerStateManager>();
                return gameControllerInstance.PlayerStates[this.PlayerId];
            }
        }

        /// <summary>
        /// The player state of the enemy
        /// </summary>
        public PlayerState EnemyPlayerState
        {
            get
            {
                var gameControllerInstance = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<PlayerStateManager>();
                return gameControllerInstance.PlayerStates[(this.PlayerId + 1) % Settings.MaxPlayerCount];
            }
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        private void Update()
        {
            var gameControllerInstance = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<PlayerStateManager>();
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

        /// <summary>
        /// Ends the current turn
        /// </summary>
        [Command]
        public void CmdEndCurrentTurn()
        {
            if (this.PlayerId != GameController.CurrentInstance.CurrentPlayerId)
            {
                Debug.Log("Error: Trying to end turn when it wasn't the player's turn to begin with!");
                return;
            }

			if (this.isServer)
			{
				GameController.CurrentInstance.RpcEndCurrentTurn();
			}
		}

        /// <summary>
        /// Calls the server to assign the initial health cards
        /// </summary>
        /// <param name="suits">The suits of the cards</param>
        /// <param name="numbers">The numbers of the cards</param>
        [Command]
        public void CmdAssignInitialHealthCards(int[] suits, int[] numbers)
        {
            this.MyPlayerState.AssignInitialHealthCards(suits, numbers);
        }

		/// <summary>
		/// Calls the server to summon a creature
		/// </summary>
		/// <param name="attackCard">The attack card used to summon the creature</param>
		/// <param name="defenseCard">The defense card used to summon the creature</param>
		/// <param name="ownerUserId">The Id of the user who summoned the creature</param>
		[Command]
        public void CmdSummonCreature(Card attackCard, Card defenseCard, int ownerUserId)
        {
            this.MyPlayerState.SummonCreature(attackCard, defenseCard, ownerUserId);
        }

        /// <summary>
        /// Calls the server to mulligan a card in the user's hand
        /// </summary>
        /// <param name="targetCard">The card to be mulligan'ed</param>
        [Command]
        public void CmdMulliganCard(Card targetCard)
        {
            this.MyPlayerState.MulliganCard(targetCard);
        }

		/// <summary>
		/// Commits a normal card use on a creature or health card
		/// </summary>
		/// <param name="intent">Target intent</param>
		/// <param name="issuingPlayerId">The player who issued this intent</param>
		/// <param name="card">the card involved, null if creature combat</param>
		/// <param name="sourceId">the attacking creature, if applicable</param>
		/// <param name="targetId">the receiver, either creature Id or player Id if applicable</param>
		[Command]
		public void CmdCommitNormalCardUse(IntentEnum intent, int issuingPlayerId, Card card, int sourceId, int targetId)
		{
			this.CommitNormalCardUse(intent, issuingPlayerId, card, sourceId, targetId);
		}

		/// <summary>
		/// Commits a normal card use on a creature or health card
		/// </summary>
		/// <param name="intent">Target intent</param>
		/// <param name="issuingPlayerId">The player who issued this intent</param>
		/// <param name="card">the card involved, null if creature combat</param>
		/// <param name="sourceId">the attacking creature, if applicable</param>
		/// <param name="targetId">the receiver, either creature Id or player Id if applicable</param>
		[ClientRpc]
		public void RpcCommitNormalCardUse(IntentEnum intent, int issuingPlayerId, Card card, int sourceId, int targetId)
		{
			this.CommitNormalCardUse(intent, issuingPlayerId, card, sourceId, targetId);
		}

		/// <summary>
		/// Commits a normal card use on a creature or health card
		/// </summary>
		/// <param name="intent">Target intent</param>
		/// <param name="issuingPlayerId">The player who issued this intent</param>
		/// <param name="card">the card involved, null if creature combat</param>
		/// <param name="sourceId">the attacking creature, if applicable</param>
		/// <param name="targetId">the receiver, either creature Id or player Id if applicable</param>
		private void CommitNormalCardUse(IntentEnum intent, int issuingPlayerId, Card card, int sourceId, int targetId)
		{
			IntentManager.CurrentInstance.AddIntent(intent, issuingPlayerId, card, sourceId, targetId);
			TurnManager.CurrentInstance.OnTurnStart();
		}
	}
}