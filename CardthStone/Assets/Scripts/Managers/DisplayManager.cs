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
        /// If the player's hand should be shown
        /// </summary>
        public bool IsOnBottom;

        /// <summary>
        /// The target player state that this display manager represents
        /// </summary>
        private PlayerState _targetPlayerState;

        /// <summary>
        /// used for initialization the target player state
        /// </summary>
        private void GetTargetPlayerState()
        {
            if (Helpers.CurrentPlayerId < 0 || Helpers.CurrentPlayerId > 1)
            {
                return;
            }

            if (this.IsOnBottom)
            {
                this._targetPlayerState = GameController.CurrentInstance.PlayerStates[Helpers.CurrentPlayerId];
            }
            else
            {
                this._targetPlayerState = GameController.CurrentInstance.PlayerStates[Helpers.CurrentPlayerId == 0 ? 1 : 0];
            }

            this._targetPlayerState.Displayer = this;
        }

        /// <summary>
        /// Updates all of the UI based on the given player state
        /// </summary>
        private void FixedUpdate()
        {
            if (Helpers.CurrentPlayerId == -1)
            {
                return;
            }

            if (this._targetPlayerState == null)
            {
                this.GetTargetPlayerState();
            }

            var name = this.gameObject.name;

            var cardCount = this._targetPlayerState.PlayerDrawDeck.Count;
            var cardBack = SpriteManager.CurrentInstance.CardBacks[this.IsOnBottom ? Helpers.CurrentPlayerId : (Helpers.CurrentPlayerId + 1) % 2];
            this.Deck.SetActive(cardCount > 0);
            this.Deck.GetComponent<Image>().sprite = cardBack;
            this.DeckCount.text = cardCount.ToString();
            
        }

        /// <summary>
        /// Redraws the player's hand to reflect changes
        /// </summary>
        public void RedrawPlayerHand()
        {
            PlayerHandComponent.RearrangeHand(this._targetPlayerState, true);
        }
    }
}
