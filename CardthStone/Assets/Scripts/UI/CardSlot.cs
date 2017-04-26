﻿//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="CardSlot.cs" company="Yifei Xu">
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
    using UnityEngine.Events;

    /// <summary>
    /// A slot where cards can be placed, used in creature summon, health, and mulligan. 
    /// </summary>
    public class CardSlot : MonoBehaviour
    {
        /// <summary>
        /// The delegate used 
        /// </summary>
        public UnityEvent CheckDelegate;

        /// <summary>
        /// The placed card
        /// </summary>
        public PlainCardBehavior PlacedCard { get; private set; }

        /// <summary>
        /// A list of all the card slots in the game
        /// </summary>
        private static IList<CardSlot> _instances;

        /// <summary>
        /// A list of actions to perform whenever change happens
        /// </summary>
        private static IList<UnityEvent> _checkActions;

        /// <summary>
        /// Called only once for initialization
        /// </summary>
        private void Awake()
        {
            CardSlot._instances = new List<CardSlot>();
            CardSlot._checkActions = new List<UnityEvent>();
        }

        /// <summary>
        /// Called when a new instance of the class is initialized
        /// </summary>
        private void Start()
        {
            CardSlot._instances.Add(this);

            if (this.CheckDelegate != null)
            {
                CardSlot._checkActions.Add(this.CheckDelegate);
            }
        }

        /// <summary>
        /// Called when the user clicks on the card slot
        /// </summary>
        public void OnUserClick()
        {
            // Removes the current card
            this.RemoveCard();
            
            // Grabs the currently selected card
            var selectedCard = CardBehavior.CurrentlySelected;
            if (selectedCard != null)
            {
                // Create a new card
                var newCard = this.CreateCard(selectedCard.PokerCard);
                newCard.transform.localPosition = new Vector3(0,0);
                this.PlacedCard = newCard;
                selectedCard.OnUserClick();

                // Check all card slots and remove those who already have this card
                foreach (var instance in CardSlot._instances)
                {
                    if (instance != this && instance.PlacedCard != null && instance.PlacedCard.PokerCard == newCard.PokerCard)
                    {
                        instance.RemoveCard();
                    }
                }
            }

            foreach (var action in CardSlot._checkActions)
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Removes the current card
        /// </summary>
        public void RemoveCard()
        {
            if (this.PlacedCard != null)
            {
                Destroy(this.PlacedCard.gameObject);
                this.PlacedCard = null;
            }

            foreach (var action in CardSlot._checkActions)
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Creates a new card 
        /// </summary>
        /// <param name="card">Target suit and number to be created</param>
        /// <returns>The class of the newly created card</returns>
        private PlainCardBehavior CreateCard(Card card)
        {
            var newCardObject = Instantiate(PrefabManager.CurrentInstance.PlainUIPrefab, this.transform);
            var newCardClass = newCardObject.GetComponent<PlainCardBehavior>();
            newCardClass.Create(card);

            return newCardClass;
        }
    }
}