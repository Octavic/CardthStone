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

    public class CreatureSummonUI : MonoBehaviour
    {
        #region Unity Editor Links
        /// <summary>
        /// The summon button, to be grayed out unless both cards are placed
        /// </summary>
        public Button SummonButton;

        /// <summary>
        /// The game object where cards are placed
        /// </summary>
        public GameObject CardsParent;

        /// <summary>
        /// The anchor point for the left card
        /// </summary>
        public Vector2 LeftCardAnchorPoint;

        /// <summary>
        /// The anchor point for the right card
        /// </summary>
        public Vector2 RightCardAnchorPoint;
        #endregion

        /// <summary>
        /// The current attack card
        /// </summary>
        public PlainCardBehavior PlacedAttackCard { get; private set; }

        /// <summary>
        /// The current defense card
        /// </summary>
        public PlainCardBehavior PlacedDefenseCard { get; private set; }

        /// <summary>
        /// Used for initialization
        /// </summary>
        private void Start()
        {
            this.SummonButton.interactable = false;
            this.PlacedAttackCard = null;
            this.PlacedDefenseCard = null;
        }

        /// <summary>
        /// When the user clicks the left button
        /// </summary>
        public void OnTryPlaceAttack()
        {
            // If there was a card already, no matter what happens, it's gone
            if (this.PlacedAttackCard != null)
            {
                Destroy(this.PlacedAttackCard.gameObject);
                this.PlacedAttackCard = null;
            }

            // Grab the selected card
            var selectedCard = CardBehavior.CurrentlySelected;
            if (selectedCard != null)
            {
                // Create a new card
                var newCard = this.CreateCard(selectedCard.PokerCard);
                newCard.transform.localPosition = this.LeftCardAnchorPoint;
                this.PlacedAttackCard = newCard;
                selectedCard.OnUserClick();

                // If the selected card is the same as right side, remove the right side card and assume user is moving the card from right to left
                if (this.PlacedDefenseCard != null)
                {
                    if (newCard.PokerCard == this.PlacedDefenseCard.PokerCard)
                    {
                        Destroy(this.PlacedDefenseCard.gameObject);
                        this.PlacedDefenseCard = null;
                    }
                }
            }

            this.CheckCreatureValibility();
        }

        /// <summary>
        /// When the user clicks the left button
        /// </summary>
        public void OnTryPlaceDefenseClick()
        {
            // If there was a card already, no matter what happens, it's gone
            if (this.PlacedDefenseCard != null)
            {
                Destroy(this.PlacedDefenseCard.gameObject);
                this.PlacedDefenseCard = null;
            }

            // Grab the selected card
            var selectedCard = CardBehavior.CurrentlySelected;
            if (selectedCard != null)
            {
                // Create a new card
                var newCard = this.CreateCard(selectedCard.PokerCard);
                newCard.transform.localPosition = this.RightCardAnchorPoint;
                this.PlacedDefenseCard = newCard;
                selectedCard.OnUserClick();

                // If the selected card is the same as right side, remove the right side card and assume user is moving the card from right to left
                if (this.PlacedAttackCard != null)
                {
                    if (newCard.PokerCard == this.PlacedAttackCard.PokerCard)
                    {
                        Destroy(this.PlacedAttackCard.gameObject);
                        this.PlacedAttackCard = null;
                    }
                }
            }

            this.CheckCreatureValibility();
        }

        /// <summary>
        /// Cancels the summon
        /// </summary>
        public void CloseSummonWindow()
        {
            // Delete the cards
            if (this.PlacedDefenseCard != null)
            {
                Destroy(this.PlacedDefenseCard.gameObject);
                this.PlacedDefenseCard = null;
            }

            if (this.PlacedAttackCard != null)
            {
                Destroy(this.PlacedAttackCard.gameObject);
                this.PlacedAttackCard = null;
            }

            this.SummonButton.interactable = false;

            // Set object inactive
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
                Debug.Log("Something wrong, creature should not be able to be summoned at the moment");
                return;
            }

            // Grabs the poker cards
            var attackCard = this.PlacedAttackCard != null ? this.PlacedAttackCard.PokerCard : this.PlacedDefenseCard.PokerCard;
            var defenseCard = this.PlacedDefenseCard != null ? this.PlacedDefenseCard.PokerCard : this.PlacedAttackCard.PokerCard;

            // Creates the creature
            var localPlayer = PlayerController.LocalPlayer;
            if (localPlayer.isServer)
            {
                PlayerController.LocalPlayer.MyPlayerState.SummonCreature(attackCard, defenseCard);
            }
            else
            {
                localPlayer.CmdSummonCreature(attackCard, defenseCard);
            }

            // All is done, close the summon window
            this.CloseSummonWindow();
        }

        /// <summary>
        /// Creates a new card 
        /// </summary>
        /// <param name="card">Target suit and number to be created</param>
        /// <returns>The class of the newly created card</returns>
        private PlainCardBehavior CreateCard(Card card)
        {
            var newCardObject = Instantiate(PrefabManager.CurrentInstance.PlainUIPrefab, this.CardsParent.transform);
            var newCardClass = newCardObject.GetComponent<PlainCardBehavior>();
            newCardClass.Create(card);

            return newCardClass;
        }

        /// <summary>
        /// Checks the currently placed cards to see if the creature is ready to be summoned
        /// </summary>
        private void CheckCreatureValibility()
        {
            if (this.PlacedAttackCard == null && (this.PlacedDefenseCard == null || this.PlacedDefenseCard.PokerCard.CardNumber != 2))
            {
                SummonButton.interactable = false;
                return;
            }

            if (this.PlacedDefenseCard == null && (this.PlacedAttackCard == null || this.PlacedAttackCard.PokerCard.CardNumber != 2))
            {
                SummonButton.interactable = false;
                return;
            }

            SummonButton.interactable = true;
            return;
        }
    }
}
