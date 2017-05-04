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
        public Card AttackCard;

        /// <summary>
        /// A list of defense cards, first one is base, rest is buff
        /// </summary>
        public Card DefenseCard;

        /// <summary>
        /// The temporary attack buff
        /// </summary>
        public int AttackBuff;
        
        /// <summary>
        /// The temporary defense buff
        /// </summary>
        public int DefenseBuff;

		/// <summary>
		/// The id of the user who owns the creature
		/// </summary>
		public int OwnerUserId;

        /// <summary>
        /// A state indicating whether the creature have summoning sickness or not. 
        /// Having summoning sickness means the creature cannot attack this turn
        /// </summary>
        public bool HaveSummoningSickness;

        /// <summary>
        /// Get the total attack number
        /// </summary>
        public int TotalAttackNumber
        {
            get
            {
                return Helpers.GetCardStrengthByNumber(this.AttackCard.CardNumber) + AttackBuff;
            }
        }

        /// <summary>
        /// Gets the total defense number
        /// </summary>
        public int TotalDefenseNumber
        {
            get
            {
                return Helpers.GetCardStrengthByNumber(this.DefenseCard.CardNumber) + DefenseBuff;
            }
        }

		/// <summary>
		/// Initializes a new instances of the <see cref="Creature"/> structure
		/// </summary>
		/// <param name="attackCard">The attack card</param>
		/// <param name="defenseCard">The defense card</param>
		/// <param name="ownerUserid">The user that summoned this creature</param>
		public Creature(Card attackCard, Card defenseCard, int ownerUserid)
        {
            this.AttackCard = attackCard;
            this.DefenseCard =defenseCard;
			this.OwnerUserId = ownerUserid;

            this.CurrentHealth = defenseCard.CardNumber > 10 ? 10 : defenseCard.CardNumber;
            this.AttackBuff = 0;
            this.DefenseBuff = 0;
            this.IsTapped = false;
            this.HaveSummoningSickness = true;
        }

        /// <summary>
        /// Flips the attack and defense stats
        /// </summary>
        public void FlipNumbers()
        {
            var swapCard = this.AttackCard;
            this.AttackCard = this.DefenseCard;
            this.DefenseCard = swapCard;
        }

        /// <summary>
        /// Buffs this creature's attack with the given card
        /// </summary>
        /// <param name="card">target card</param>
        public void BuffAttack(Card card)
        {
            this.AttackBuff += Helpers.GetCardStrengthByNumber(this.AttackCard.CardNumber);
            this.AttackCard = card;
        }

        /// <summary>
        /// Buffs this creature's defense with the given card
        /// </summary>
        /// <param name="card">target card</param>
        public void BuffDefense(Card card)
        {
            this.DefenseBuff += Helpers.GetCardStrengthByNumber(this.DefenseCard.CardNumber);
            this.DefenseCard = card;
            this.CurrentHealth += Helpers.GetCardStrengthByNumber(card.CardNumber);
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
            this.AttackBuff = 0;
            this.DefenseBuff = 0;

            this.CurrentHealth = this.TotalDefenseNumber;
        }

		/// <summary>
		/// Turns the creature to a string
		/// </summary>
		/// <returns>A string representing the creature</returns>
		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.Append(this.TotalAttackNumber + "(" + this.AttackCard.CardSuit + ")/");
			builder.Append(this.TotalDefenseNumber + "(" + this.DefenseCard.CardSuit + ") from player ");
			builder.Append(this.OwnerUserId);
			return builder.ToString();
		}

		/// <summary>
		/// Gets the hash code
		/// </summary>
		/// <returns>The hash code</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
