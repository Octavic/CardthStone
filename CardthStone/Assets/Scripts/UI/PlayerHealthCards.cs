//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="PlayerHealthCards.cs" company="Yifei Xu">
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
	using States;
	using Intent;

	/// <summary>
	/// Shows the player's health card
	/// </summary>
	public class PlayerHealthCards : MonoBehaviour
    {
		/// <summary>
		/// The boarder game object
		/// </summary>
		public GameObject BoarderGameObject;

		/// <summary>
		/// Gets the currently selected health card
		/// </summary>
		public static PlayerHealthCards CurrentlySelected { get; private set; }

		/// <summary>
		/// Gets the player id that this health card is for
		/// </summary>
		public int PlayerId { get; private set; }

        /// <summary>
        /// The current list of card objects
        /// </summary>
        private IList<GameObject> CardGameObjects;

        /// <summary>
        /// Moves the current health cards to their correct position
        /// </summary>
        /// <param name="playerState">The player state that this player hand belongs to</param>
        /// <param name="showCards">True if the cards should be shown</param>
        public void RenderPlayerHealthCards(PlayerState playerState, bool showCards = true)
        {
			// Sets the player Id
			this.PlayerId = playerState.PlayerId;

            // Erase all current cards and recreate
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            this.CardGameObjects = new List<GameObject>();

            // Add the correct cards
            if (showCards)
            { 
                for (int i = 0; i < playerState.HealthCards.Count; i++)
                {
                    var card = playerState.HealthCards[i];
                    var newCard = this.CreateCard(card);
                    this.CardGameObjects.Add(newCard);
                }
            }
            else
            {
                var cardBackPrefab = PrefabManager.CurrentInstance.CardBackPrefabs[playerState.PlayerId];
                for (int i = 0; i < playerState.HealthCards.Count; i++)
                {
                    var newCard = this.CreateCardBack(cardBackPrefab);
                    this.CardGameObjects.Add(newCard);
                }
            }

            // Rearrange cards
            for (int i = 0; i < this.CardGameObjects.Count; i++)
            {
                var curCard = this.CardGameObjects[i];
                curCard.transform.localPosition = new Vector3(i * Settings.PlayerHandCardDistance, 0);
            }

            this.transform.localPosition = new Vector3(-this.CardGameObjects.Count * Settings.PlayerHandCardDistance / 2, 0);
        }

		/// <summary>
		/// When the player hearth card is clicked
		/// </summary>
		public void OnUserClick()
		{
			// Toggle boarder
			this.BoarderGameObject.SetActive(!this.BoarderGameObject.activeSelf);

			// If there was one already selected, unselect
			if (PlayerHealthCards.CurrentlySelected != null)
			{
				PlayerHealthCards.CurrentlySelected.BoarderGameObject.SetActive(false);

				if (PlayerHealthCards.CurrentlySelected == this)
				{
					PlayerHealthCards.CurrentlySelected = null;
				}
				else
				{
					PlayerHealthCards.CurrentlySelected = this;
				}
			}
			else
			{
				PlayerHealthCards.CurrentlySelected = this;
			}

			IntentManager.CurrentInstance.Render();
		}
		
        /// <summary>
        /// Creates a new card 
        /// </summary>
        /// <param name="card">Target suit and number to be created</param>
        /// <returns>The class of the newly created card</returns>
        private GameObject CreateCard(Card card)
        {
            var newCardObject = Instantiate(PrefabManager.CurrentInstance.PlainUIPrefab, this.transform);
            var newCardClass = newCardObject.GetComponent<PlainCardBehavior>();
            newCardClass.Create(card);

            return newCardObject;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private GameObject CreateCardBack(GameObject cardBackPrefab)
        {
            return Instantiate(cardBackPrefab, this.transform);
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        private void Start()
        {
            this.CardGameObjects = new List<GameObject>();
        }
    }
}
