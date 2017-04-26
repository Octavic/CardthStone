//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="PlayerState.cs" company="Yifei Xu">
//    Copyright (c) Yifei Xu.  All rights reserved.
//  </copyright>
//--------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.States
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using UnityEngine.Networking;
    using Managers;

    /// <summary>
    /// A collection of all data relating to a player state
    /// </summary>
    public class PlayerState : NetworkBehaviour
    {
        /// <summary>
        /// Gets the player id for which this player state represents
        /// </summary>
        public int PlayerId;

        /// <summary>
        /// The manager in charge of displaying this player state
        /// </summary>
        public DisplayManager Displayer;

        /// <summary>
        /// Gets the player's hand
        /// </summary>
        [SyncVar]
        public SyncListCard PlayerHand;

        /// <summary>
        /// Gets the health cards for the player
        /// </summary>
        [SyncVar]
        public SyncListCard HealthCards;

        /// <summary>
        /// Gets a list of creatures this player controls
        /// </summary>
        [SyncVar]
        public SyncListCreature Creatures;

        /// <summary>
        /// The draw deck for the player
        /// </summary>
        [SyncVar]
        public SyncListCard PlayerDrawDeck;

        /// <summary>
        /// Draws a card from the deck
        /// </summary>
        public void DrawCardFromDeck()
        {
            var newCard = this.PlayerDrawDeck.First();
            this.PlayerDrawDeck.RemoveAt(0);
            this.PlayerHand.Add(newCard);

            if (this.isServer)
            {
                this.RpcRenderPlayerHand();
            }
            else
            {
                this.Displayer.RenderPlayerHand();
            }

            Debug.Log("Player " + PlayerId + " has drawn a card: " + newCard.ToString());
        }

        /// <summary>
        /// Summons a new creature from the player's hand
        /// </summary>
        /// <param name="attackCard">The attack card</param>
        /// <param name="defenseCard">The defense card</param>
        public void SummonCreature(Card attackCard, Card defenseCard)
        {
            // Validate to see if the player have those cards in hand
            if (!this.PlayerHand.Contains(attackCard) || !this.PlayerHand.Contains(defenseCard))
            {
                Debug.Log("Error summoning creature: Player does NOT have the right cards in hand");
                return;
            }

            // Remove the cards from hand
            this.PlayerHand.Remove(attackCard);
            this.PlayerHand.Remove(defenseCard);

            this.Creatures.Add(new Creature(attackCard, defenseCard));
            Debug.Log("Player " + PlayerId + " has summon creature with " + attackCard.ToString() + " and " + defenseCard.ToString());

            this.RpcRenderCreatureArea();
            this.RpcRenderPlayerHand();
        }

        /// <summary>
        /// Assigns the initial health cards
        /// </summary>
        /// <param name="suits">The suits of the cards</param>
        /// <param name="numbers">The numbers of the cards</param>
        public void AssignInitialHealthCards(int[] suits, int[] numbers)
        {
            var suitLength = suits.Length;
            // Check to make sure you can construct valid cards
            if (suitLength != numbers.Length)
            {
                throw new InvalidProgramException("Lengths of suits and numbers do not match!");
            }

            for (int i = 0; i < suitLength; i++)
            {
                var newCard = new Card((CardSuitEnum)suits[i], numbers[i]);
                this.PlayerHand.Remove(newCard);
                this.HealthCards.Add(newCard);
            }

            this.RpcRenderPlayerHand();
            this.RpcRenderPlayerHealthCards();
        }

        /// <summary>
        /// Re-renders the player's creature area on every client
        /// </summary>
        [ClientRpc]
        public void RpcRenderCreatureArea()
        {
            this.Displayer.RenderCreatureArea();
        }

        /// <summary>
        /// Re-renders the player's hand on every client
        /// </summary>
        [ClientRpc]
        public void RpcRenderPlayerHand()
        {
            this.Displayer.RenderPlayerHand();
        }

        /// <summary>
        /// Re-render's the player's health cards
        /// </summary>
        [ClientRpc]
        public void RpcRenderPlayerHealthCards()
        {
            this.Displayer.RenderPlayerHealthCards();
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        private void Awake()
        {
            // Initializes variables
            this.PlayerHand = new SyncListCard();
            this.HealthCards = new SyncListCard();
            this.Creatures = new SyncListCreature();
            this.PlayerDrawDeck = new SyncListCard();
        }

        /// <summary>
        /// Called when a new instance initializes
        /// </summary>
        private void Start()
        {
            if (isServer)
            {
                var sharedState = GameObject.FindGameObjectWithTag(Tags.SharedState).GetComponent<SharedState>();
                
                var totalDeck = sharedState.TotalDeck;

                // If the total deck is empty, populate it
                if (totalDeck.Count == 0)
                {
                    sharedState.PopulateTotalDeck();
                }

                int start = this.PlayerId * 26;
                int end = start + 26;
                for (int i = start; i < end; i++)
                {
                    this.PlayerDrawDeck.Add(totalDeck[i]);
                }
            }
        }
    }
}