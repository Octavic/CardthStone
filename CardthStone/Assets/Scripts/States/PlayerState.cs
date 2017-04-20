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
        public SyncListCard PlayerHand;

        /// <summary>
        /// Gets the health cards for the player
        /// </summary>
        public SyncListCard HealthCards;

        /// <summary>
        /// Gets a list of creatures this player controls
        /// </summary>
        public SyncListCreature Creatures;

        /// <summary>
        /// The draw deck for the player
        /// </summary>
        public SyncListCard PlayerDrawDeck;

        /// <summary>
        /// Draws a new card from the deck
        /// </summary>
        public void DrawCardFromPlayerDeck()
        {
            if (this.PlayerDrawDeck.Count == 0)
            {
                return;
            }

            var newCard = this.PlayerDrawDeck.First();
            this.PlayerDrawDeck.RemoveAt(0);
            this.PlayerHand.Add(newCard);
            this.Displayer.RedrawPlayerHand();
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
    }
}