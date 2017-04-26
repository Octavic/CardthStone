//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="SharedState.cs" company="Yifei Xu">
//    Copyright (c) Yifei Xu.  All rights reserved.
//  </copyright>
//--------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.States
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using UnityEngine.Networking;

    /// <summary>
    /// A shared state such as total deck or graveyard
    /// </summary>
    public class SharedState : NetworkBehaviour
    {
        /// <summary>
        /// Gets the current instance of the shared state
        /// </summary>
        public static SharedState CurrentInstance { get; private set; }

        /// <summary>
        /// The player 0 state
        /// </summary>
        public PlayerState Player0State;

        /// <summary>
        /// The player 1 state
        /// </summary>
        public PlayerState Player1State;

        /// <summary>
        /// Gets the total deck that gets dealt out in the very beginning
        /// </summary>
        public SyncListCard TotalDeck;

        /// <summary>
        /// Gets a list of cards that's in the graveyard
        /// </summary>
        public SyncListCard Graveyard;

        /// <summary>
        /// The global random
        /// </summary>
        private static System.Random _globalRandom;

        /// <summary>
        /// Gets a globally random number
        /// </summary>
        public static int NextGlobalRandom
        {
            get
            {
                return _globalRandom.Next();
            }
        }

        /// <summary>
        /// Request the shared state to give 26 cards to each player
        /// </summary>
        [Command]
        public void CmdRequestDealToPlayer()
        {
            if (isServer)
            {
                this.RpcDealToPlayers();
            }
        }

        /// <summary>
        /// Deals cards to each player
        /// </summary>
        [ClientRpc]
        private void RpcDealToPlayers()
        {
            for (int i = 0; i < 26; i++)
            {
                Player0State.PlayerDrawDeck.Insert(i, this.TotalDeck[i]);
            }

            for (int i = 0; i < 26; i++)
            {
                Player0State.PlayerDrawDeck.Insert(i, this.TotalDeck[i + 26]);
            }
        }

        /// <summary>
        /// The synced seed for all randomness
        /// </summary>
        [SyncVar]
        private int _randomSeed;

        /// <summary>
        /// Populates the total deck
        /// </summary>
        public void PopulateTotalDeck()
        {
            _globalRandom = new System.Random(this._randomSeed);

            for (int suit = 0; suit < 4; suit++)
            {
                for (int number = 1; number <= 13; number++)
                {
                    var randomPos = _globalRandom.Next() % (TotalDeck.Count + 1);
                    var newCard = new Card((CardSuitEnum)suit, number);
                    TotalDeck.Insert(randomPos, newCard);
                }
            }
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        private void Awake()
        {
            TotalDeck = new SyncListCard();
            Graveyard = new SyncListCard();
            this._randomSeed = new System.Random().Next();
        }

        /// <summary>
        /// Called when a new instance is initialized
        /// </summary>
        private void Start()
        {
            CurrentInstance = this;
            

            _globalRandom = new System.Random(this._randomSeed);
        }
    }
}
