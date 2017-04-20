//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="CardBehavior.cs" company="Yifei Xu">
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
    using Managers;

    /// <summary>
    /// Describes the behavior of a card
    /// </summary>
    public class CardBehavior : MonoBehaviour
    {
        /// <summary>
        /// The poker card that this card is representing
        /// </summary>
        public Card PokerCard { get; set; }

        /// <summary>
        /// Gets the card that's currently selected
        /// </summary>
        public static CardBehavior CurrentlySelected { get; private set; }

        /// <summary>
        /// The current instance of the prefab manager
        /// </summary>
        private static PrefabManager _prefabManager;

        /// <summary>
        /// The color manager
        /// </summary>
        private static ColorManager _colorManager;

        /// <summary>
        /// True if the card is selected
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// Used for initialization
        /// </summary>
        private void Start()
        {
            this.isSelected = false;
        }

        /// <summary>
        /// What happens when the player chooses the card
        /// </summary>
        public void OnUserClick()
        {
           this.isSelected = !this.isSelected;

            // Pops the new selected card out
            float newY = 0;
            if (this.isSelected)
            {
                newY = Settings.SelectedCardHeight;
            }

            this.transform.localPosition = new Vector3(this.transform.localPosition.x, newY);

            // Unselect the current card
            if (CardBehavior.CurrentlySelected != null)
            {
                if (CardBehavior.CurrentlySelected.PokerCard == this.PokerCard)
                {
                    CardBehavior.CurrentlySelected = null;
                }
                else
                {
                    CardBehavior.CurrentlySelected.OnUserClick();
                    CardBehavior.CurrentlySelected = this;
                }
            }
            else
            {
                CardBehavior.CurrentlySelected = this;
            }
        }

        /// <summary>
        /// Creates the card, and populates the suit as well as the number with correct color
        /// </summary>
        public void Create(Card card)
        {
            // Makes sure the managers has been assigned
            if (_prefabManager == null)
            {
                _prefabManager = GameObject.FindGameObjectWithTag(Tags.PrefabManager).GetComponent<PrefabManager>();
            }

            if (_colorManager == null)
            {
                _colorManager = GameObject.FindGameObjectWithTag(Tags.ColorManager).GetComponent<ColorManager>();
            }

            this.PokerCard = card;

            var suit = Instantiate(_prefabManager.GetCardSuitPrefab((CardSuitEnum)card.CardSuit), this.transform);
            var number = Instantiate(_prefabManager.GetCardNumberPrefab(card.CardNumber), this.transform);

            suit.transform.localPosition = Vector3.zero;
            number.transform.localPosition = Vector3.zero;

            number.GetComponent<Image>().color = _colorManager.GetCardColorFromSuit((CardSuitEnum)card.CardSuit);
        }
    }
}
