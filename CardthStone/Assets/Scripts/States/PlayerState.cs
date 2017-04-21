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

            var letter = newCard.CardNumber.ToString();
            if (newCard.CardNumber == 1)
            {
                letter = "A";
            }
            else if (newCard.CardNumber == 11)
            {
                letter = "J";
            }
            else if (newCard.CardNumber == 12)
            {
                letter = "Q";
            }
            else if (newCard.CardNumber == 13)
            {
                letter = "K";
            }

            Debug.Log("Player " + PlayerId + " has drawn a card: " + letter + " of " + newCard.CardSuit.ToString());
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

            this.RpcRenderCreatureArea();
            this.RpcRenderPlayerHand();
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
                sharedState.PopulateTotalDeck();
                var totalDeck = sharedState.TotalDeck;

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