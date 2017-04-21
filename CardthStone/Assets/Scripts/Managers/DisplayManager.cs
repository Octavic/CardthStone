//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="TestBuffAttackButton.cs" company="Yifei Xu">
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
    using UnityEngine.UI;
    using UI;
    using States;

    /// <summary>
    /// Manages how information of the player state is displayed
    /// </summary>
    public class DisplayManager : MonoBehaviour
    {
        /// <summary>
        /// The game object for the deck
        /// </summary>
        public GameObject Deck;

        /// <summary>
        /// The number of cards left in the deck
        /// </summary>
        public Text DeckCount;

        /// <summary>
        /// The player's hand
        /// </summary>
        public PlayerHand PlayerHandComponent;

        /// <summary>
        /// The creature area
        /// </summary>
        public CreatureArea CreatureArea;

        /// <summary>
        /// If the player's hand should be shown
        /// </summary>
        public bool IsOnBottom;

        /// <summary>
        /// The target player state that this display manager represents
        /// </summary>
        public PlayerState TargetPlayerState;

        /// <summary>
        /// used for initialization the target player state
        /// </summary>
        private void GetTargetPlayerState()
        {
            if (PlayerController.LocalPlayer == null)
            {
                return;
            }

            if (this.IsOnBottom)
            {
                this.TargetPlayerState = PlayerController.LocalPlayer.MyPlayerState;
            }
            else
            {
                this.TargetPlayerState = PlayerController.LocalPlayer.EnemyPlayerState;
            }

            this.TargetPlayerState.Displayer = this;
        }

        /// <summary>
        /// Updates all of the UI based on the given player state
        /// </summary>
        private void FixedUpdate()
        {
            if (PlayerController.LocalPlayer == null)
            {
                return;
            }

            var playerId = PlayerController.LocalPlayer.PlayerId;

            this.TargetPlayerState = this.IsOnBottom ? PlayerController.LocalPlayer.MyPlayerState : PlayerController.LocalPlayer.EnemyPlayerState;

            if (this.TargetPlayerState == null)
            {
                return;
            }

            this.TargetPlayerState.Displayer = this;

            var cardCount = this.TargetPlayerState.PlayerDrawDeck.Count;

            var cardBack = SpriteManager.CurrentInstance.CardBacks[this.IsOnBottom ? playerId : (playerId + 1) % 2];
            this.Deck.SetActive(cardCount > 0);
            this.Deck.GetComponent<Image>().sprite = cardBack;
            this.DeckCount.text = cardCount.ToString();
        }

        /// <summary>
        /// Re-renders the creature area to reflect changes
        /// </summary>
        public void RenderCreatureArea()
        {
            CreatureArea.RenderPlayerCreatures(this.TargetPlayerState);
        }

        /// <summary>
        /// Redraws the player's hand to reflect changes
        /// </summary>
        public void RenderPlayerHand()
        {
            PlayerHandComponent.RenderPlayerHand(this.TargetPlayerState, true);
        }
    }
}
