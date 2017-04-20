//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="CreatureBehavior.cs" company="Yifei Xu">
//    Copyright (c) Yifei Xu.  All rights reserved.
//  </copyright>
//--------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using Managers;

    /// <summary>
    /// Describes the behavior of a creature
    /// </summary>
    public class CreatureBehavior : MonoBehaviour
    {
        #region Unity editor elements
        /// <summary>
        /// The attack suit game object sprite renderer
        /// </summary>
        public SpriteRenderer AttackSuitRenderer;

        /// <summary>
        /// The defense suit game object sprite renderer
        /// </summary>
        public SpriteRenderer DefenseSuitRenderer;

        /// <summary>
        /// The attack number 
        /// </summary>
        public TextMesh AttackNumberMesh;

        /// <summary>
        /// The defense number
        /// </summary>
        public TextMesh DefenseNumberMesh;
        #endregion

        /// <summary>
        /// The creature that this creature behavior is representing
        /// </summary>
        public Creature TargetCreature;

        /// <summary>
        /// Gets a state indicating whether the creature can attack whatever it desires
        /// </summary>
        public bool CanTargetAttack { get; private set; }

        /// <summary>
        /// Get a state indicating whether the creature can jump in and block a creature's attack
        /// </summary>
        public bool CanTargetBlock { get; private set; }

        /// <summary>
        /// The current instance of the sprite manager
        /// </summary>
        private static SpriteManager _spriteManager;

        /// <summary>
        /// The current instance of the color manager
        /// </summary>
        private static ColorManager _colorManager;

        /// <summary>
        /// Spawns the creature
        /// </summary>
        /// <param name="attackCard">The attack card</param>
        /// <param name="defenseCard">The defense card</param>
        public void Spawn(Card attackCard, Card defenseCard)
        {
            // Add the cards to the card list
            this.UpdateIconAndNumber();
        }

        /// <summary>
        /// Generates the icon and number for attack/defense
        /// </summary>
        private void UpdateIconAndNumber()
        {
            var baseAttackCard = this.TargetCreature.AttackCards[0];
            var baseDefenseCard = this.TargetCreature.DefenseCards[0];

            this.CanTargetAttack = Helpers.SuitToColor((CardSuitEnum)baseAttackCard.CardSuit) == CardColorEnum.Black;
            this.CanTargetBlock = Helpers.SuitToColor((CardSuitEnum)baseDefenseCard.CardSuit) == CardColorEnum.Red;

            // Assign the suit sprite
            this.AttackSuitRenderer.sprite = _spriteManager.SuitSprites[(int)this.TargetCreature.AttackSuit];
            this.DefenseSuitRenderer.sprite = _spriteManager.SuitSprites[(int)this.TargetCreature.DefenseSuit];

            // Calculate sum of attack numbers, as well as update free attack status
            int newAttackNumber = baseAttackCard.CardNumber > 10 ? 10 : baseAttackCard.CardNumber;
            for (int i = 1; i < this.TargetCreature.AttackCards.Count; i++)
            {
                var curCard = this.TargetCreature.AttackCards[i];

                if (curCard.CardNumber > 10)
                {
                    newAttackNumber += 10;
                }
                else
                {
                    newAttackNumber += curCard.CardNumber;
                }
            }

            // Calculate sum of defense numbers, as well as update free block status
            int newDefenseNumber = baseDefenseCard.CardNumber > 10 ? 10 : baseDefenseCard.CardNumber;
            for (int i = 1; i < this.TargetCreature.DefenseCards.Count; i++)
            {
                var curCard = this.TargetCreature.DefenseCards[i];

                if (curCard.CardNumber > 10)
                {
                    newDefenseNumber += 10;
                }
                else
                {
                    newDefenseNumber += curCard.CardNumber;
                }
            }

            // Assign the numbers
            this.TargetCreature.TotalAttackNumber = newAttackNumber;
            this.TargetCreature.TotalDefenseNumber = newDefenseNumber;

            // Assign the number text
            this.AttackNumberMesh.text = newAttackNumber.ToString();
            this.DefenseNumberMesh.text = newDefenseNumber.ToString();

            // Assign colors for new texts
            this.AttackNumberMesh.color = _colorManager.GetCardColorFromSuit(this.TargetCreature.AttackSuit);
            this.DefenseNumberMesh.color = _colorManager.GetCardColorFromSuit(this.TargetCreature.DefenseSuit);
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        private void Start()
        {
            if (_spriteManager == null)
            {
                _spriteManager = GameObject.FindGameObjectWithTag(Tags.SpriteManager).GetComponent<SpriteManager>();
            }

            if (_colorManager == null)
            {
                _colorManager = GameObject.FindGameObjectWithTag(Tags.ColorManager).GetComponent<ColorManager>();
            }

            this.TargetCreature.IsTapped = false;
            this.TargetCreature.HaveSummoningSickness = true;
        }
    }
}