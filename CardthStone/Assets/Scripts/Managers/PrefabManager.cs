//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="PrefabManager.cs" company="Yifei Xu">
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

    /// <summary>
    /// Controls all of the prefabs
    /// </summary>
    public class PrefabManager : MonoBehaviour
    {
        /// <summary>
        /// Gets the current singleton instance of the prefab manager
        /// </summary>
        public static PrefabManager CurrentInstance { get; private set; }

        /// <summary>
        /// The prefabs for the suits that goes on cards and creatures
        /// </summary>
        public List<GameObject> CardSuitPrefabs;

        /// <summary>
        /// The prefabs for the numbers that goes on cards
        /// </summary>
        public List<GameObject> CardNumberPrefabs;

        /// <summary>
        /// A list of card back prefabs
        /// </summary>
        public List<GameObject> CardBackPrefabs;

        /// <summary>
        /// The prefab for the card UI
        /// </summary>
        public GameObject CardUIPrefab;

        /// <summary>
        /// The prefab for a plain card in UI
        /// </summary>
        public GameObject PlainUIPrefab;

        /// <summary>
        /// The prefab for a used card
        /// </summary>
        public GameObject UsedCardPrefab;

        /// <summary>
        /// The prefab for a creature
        /// </summary>
        public GameObject CreaturePrefab;

        /// <summary>
        /// used for initialization
        /// </summary>
        private void Start()
        {
            if (PrefabManager.CurrentInstance != null)
            {
                Destroy(PrefabManager.CurrentInstance);
            }

            PrefabManager.CurrentInstance = this;
        }

        /// <summary>
        /// Gets the correct card number prefab for the given card number
        /// </summary>
        /// <param name="cardNumber">The target card number</param>
        /// <returns>The correct card number prefab</returns>
        public GameObject GetCardNumberPrefab(int cardNumber)
        {
            return this.CardNumberPrefabs[cardNumber - 1];
        }

        /// <summary>
        /// Gets the correct suit prefab 
        /// </summary>
        /// <param name="cardSuit">Desired suit</param>
        /// <returns>The card suit prefab</returns>
        public GameObject GetCardSuitPrefab(CardSuitEnum cardSuit)
        {
            return this.CardSuitPrefabs[(int)cardSuit];
        }
    }
}
