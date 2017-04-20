//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="Creature.cs" company="Yifei Xu">
//    Copyright (c) Yifei Xu.  All rights reserved.
//  </copyright>
//--------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A struct describing all information about a creature
    /// </summary>
    public struct Creature
    {
        /// <summary>
        /// The suit of the creature's attack
        /// </summary>
        public CardSuitEnum AttackSuit
        {
            get
            {
                return this.AttackCards[0].CardSuit;
            }
        }

        /// <summary>
        /// Get the total attack number
        /// </summary>
        public int TotalAttackNumber;

        /// <summary>
        /// The suit of the creature's defense
        /// </summary>
        public CardSuitEnum DefenseSuit
        {
            get
            {
                return this.DefenseCards[0].CardSuit;
            }
        }

        /// <summary>
        /// Gets the total defense number
        /// </summary>
        public int TotalDefenseNumber;

        /// <summary>
        /// Gets or sets the current health of the creature
        /// </summary>
        public int CurrentHealth;

        /// <summary>
        /// Gets or sets a state indicating whether the creature is tapped or not
        /// </summary>
        public bool IsTapped;

        /// <summary>
        /// A list of attack cards, first one is base, rest is buff
        /// </summary>
        public SyncListCard AttackCards;

        /// <summary>
        /// A list of defense cards, first one is base, rest is buff
        /// </summary>
        public SyncListCard DefenseCards;

        /// <summary>
        /// Gets or sets a state indicating whether the creature have summoning sickness or not. Having summoning sickness means the creature cannot attack this turn
        /// </summary>
        public bool HaveSummoningSickness;

        /// <summary>
        /// Flips the attack and defense stats
        /// </summary>
        public void Flip()
        {
            var swapCards = this.AttackCards;
            this.AttackCards = this.DefenseCards;
            this.DefenseCards = swapCards;
        }

        /// <summary>
        /// Buffs this creature's attack with the given card
        /// </summary>
        /// <param name="card">target card</param>
        public void BuffAttack(Card card)
        {
            this.AttackCards.Add(card);
        }

        /// <summary>
        /// Buffs this creature's defense with the given card
        /// </summary>
        /// <param name="card">target card</param>
        public void BuffDefense(Card card)
        {
            this.DefenseCards.Add(card);
            this.CurrentHealth += card.CardNumber;
        }

        /// <summary>
        /// Resets the creature when it's the current player's turn again
        /// </summary>
        public void OnTurnStart()
        {
            // Reset states
            this.IsTapped = false;
            this.HaveSummoningSickness = false;

            // Remove all cards under the buff
            var newAttackCards = new SyncListCard { this.AttackCards.Last() };
            this.AttackCards = newAttackCards;
            var newDefenseCards = new SyncListCard { this.DefenseCards.Last() };
            this.DefenseCards = newDefenseCards;

            this.CurrentHealth = this.TotalDefenseNumber;
        }
    }
}
