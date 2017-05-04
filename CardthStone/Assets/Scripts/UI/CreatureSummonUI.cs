//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="CreatureSummonUI.cs" company="Yifei Xu">
//    Copyright (c) Yifei Xu.  All rights reserved.
//  </copyright>
//--------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Managers;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// The UI that controls creature summoning, etc
    /// </summary>
    public class CreatureSummonUI : MonoBehaviour
    {
        #region Unity Editor Links
        /// <summary>
        /// The summon button, to be grayed out unless both cards are placed
        /// </summary>
        public Button SummonButton;

        /// <summary>
        /// Card slot for the attack card
        /// </summary>
        public CardSlot AttackCardSlot;

        /// <summary>
        /// Card slot for the defense card
        /// </summary>
        public CardSlot DefenseCardSlot;
        #endregion

        /// <summary>
        /// Used for initialization
        /// </summary>
        private void Start()
        {
            this.SummonButton.interactable = false;
        }
        
        /// <summary>
        /// Cancels the summon
        /// </summary>
        public void CloseSummonWindow()
        {
            // Delete the cards
            this.AttackCardSlot.RemoveCard();
            this.DefenseCardSlot.RemoveCard();

            // Set object inactive
            this.SummonButton.interactable = false;
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// Commits the summon
        /// </summary>
        public void CommitSummon()
        {
            // If the summon button isn't even suppose to be clicked, something is wrong
            this.CheckCreatureValibility();
            if (!this.SummonButton.interactable)
            {
                Debug.Log("Something went wrong, creature should not be able to be summoned at the moment");
                return;
            }

            // Grabs the poker cards
            var attackCard = this.AttackCardSlot.PlacedCard != null ? AttackCardSlot.PlacedCard.PokerCard : this.DefenseCardSlot.PlacedCard.PokerCard;
            var defenseCard = this.DefenseCardSlot.PlacedCard != null ? this.DefenseCardSlot.PlacedCard.PokerCard : AttackCardSlot.PlacedCard.PokerCard;

            // Creates the creature
            var localPlayer = PlayerController.LocalPlayer;
            if (localPlayer.isServer)
            {
                PlayerController.LocalPlayer.MyPlayerState.SummonCreature(attackCard, defenseCard, localPlayer.PlayerId);
            }
            else
            {
                localPlayer.CmdSummonCreature(attackCard, defenseCard, localPlayer.PlayerId);
            }

            // All is done, close the summon window
            this.CloseSummonWindow();
        }

        /// <summary>
        /// Checks the currently placed cards to see if the creature is ready to be summoned
        /// </summary>
        public void CheckCreatureValibility()
        {
            if (this.AttackCardSlot.PlacedCard == null && (this.DefenseCardSlot.PlacedCard == null || this.DefenseCardSlot.PlacedCard.PokerCard.CardNumber != 2))
            {
                SummonButton.interactable = false;
                return;
            }

            if (this.DefenseCardSlot.PlacedCard == null && (this.AttackCardSlot.PlacedCard == null || this.AttackCardSlot.PlacedCard.PokerCard.CardNumber != 2))
            {
                SummonButton.interactable = false;
                return;
            }

            SummonButton.interactable = true;
            return;
        }
    }
}
