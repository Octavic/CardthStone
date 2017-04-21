//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="PlainCardBehavior.cs" company="Yifei Xu">
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
    /// A plain card that cannot be clicked
    /// </summary>
    public class PlainCardBehavior : MonoBehaviour
    {
        /// <summary>
        /// The poker card that this card is representing
        /// </summary>
        public Card PokerCard { get; set; }

        /// <summary>
        /// The current instance of the prefab manager
        /// </summary>
        private static PrefabManager _prefabManager;

        /// <summary>
        /// The color manager
        /// </summary>
        private static ColorManager _colorManager;

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
