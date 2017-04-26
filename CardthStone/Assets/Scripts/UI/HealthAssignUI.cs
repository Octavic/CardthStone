//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="HealthAssignUI.cs" company="Yifei Xu">
//    Copyright (c) Yifei Xu.  All rights reserved.
//  </copyright>
//--------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// The UI used to assign the health cards to a certain player
    /// </summary>
    public class HealthAssignUI : MonoBehaviour
    {
        /// <summary>
        /// The accept button
        /// </summary>
        public Button AcceptButton;

        /// <summary>
        /// A list of card slots for the UI
        /// </summary>
        public List<CardSlot> CardSlots;

        /// <summary>
        /// Checks to see the availability of the accept button
        /// </summary>
        public void CheckAvailability()
        {
            // If the player has chosen any card, then the player may commit
            foreach (var slot in CardSlots)
            {
                if (slot.PlacedCard != null)
                {
                    AcceptButton.interactable = true;
                    return;
                }
            }

            // No card was chosen, cannot accept
            AcceptButton.interactable = false;
        }

        /// <summary>
        /// Commits the assigning of health cards
        /// </summary>
        public void CommitHealthAssign()
        {
            // Count the number of valid cards
            int cardCount = 0;
            foreach (var slot in this.CardSlots)
            {
                if (slot.PlacedCard != null)
                {
                    cardCount++;
                }
            }

            if (cardCount == 0)
            {
                Debug.Log("Something went wrong, no cards are selected in health UI, but player is allowed to continue");
                return;
            }

            // Construct suit and number array
            var suits = new int[cardCount];
            var numbers = new int[cardCount];
            var curIndex = 0;
            for (int i = 0; i < this.CardSlots.Count; i++)
            {
                var placedCard = this.CardSlots[i].PlacedCard;
                if (placedCard != null)
                {
                    suits[curIndex] = (int)placedCard.PokerCard.CardSuit;
                    numbers[curIndex] = (int)placedCard.PokerCard.CardNumber;
                    curIndex++;
                }
            }

            // Assign the new health cards
            var localPlayer = PlayerController.LocalPlayer;
            if (localPlayer.isServer)
            {
                PlayerController.LocalPlayer.MyPlayerState.AssignInitialHealthCards(suits, numbers);
            }
            else
            {
                localPlayer.CmdAssignInitialHealthCards(suits, numbers);
            }

            // Done, close window and exit
            foreach (var slot in this.CardSlots)
            {
                slot.RemoveCard();
            }

            this.gameObject.SetActive(false);
        }
    }
}
