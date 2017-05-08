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
	using Intent;

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

		/// <summary>
		/// The boarder that shows up when a creature is selected
		/// </summary>
		public GameObject SelectBoarder;
		#endregion

		/// <summary>
		/// A list of all creatures by their creature Id
		/// </summary>
		public static IDictionary<int, CreatureBehavior> Creatures = new Dictionary<int, CreatureBehavior>();

		/// <summary>
		/// The currently selected creature
		/// </summary>
		public static CreatureBehavior CurrentlySelected;

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
		/// If this creature is selected
		/// </summary>
		private bool _isSelected;

        /// <summary>
        /// Spawns the creature
        /// </summary>
        /// <param name="targetCreature">The creature that needs to be spawned</param>
        public void Spawn(Creature targetCreature)
        {
            // Add the cards to the card list
            this.TargetCreature = targetCreature;
            this.UpdateIconAndNumber();

			// Add the newly created creature to the list
			CreatureBehavior.Creatures[targetCreature.CreatureId] = this; 
        }

		/// <summary>
		/// Called when a user clicks on the creature
		/// </summary>
		public void OnUserClick()
		{
			this._isSelected = !this._isSelected;

			// Pops the new selected card out
			this.SelectBoarder.SetActive(this._isSelected);

			// Unselect the current card
			if (CreatureBehavior.CurrentlySelected != null)
			{
				CreatureBehavior.CurrentlySelected.SelectBoarder.SetActive(false);

				if (CreatureBehavior.CurrentlySelected == this)
				{
					CreatureBehavior.CurrentlySelected = null;
				}
				else
				{
					CreatureBehavior.CurrentlySelected = this;
				}
			}
			else
			{
				CreatureBehavior.CurrentlySelected = this;
			}

			// Update the intent manager
			IntentManager.CurrentInstance.Render();
		}

        /// <summary>
        /// Generates the icon and number for attack/defense
        /// </summary>
        private void UpdateIconAndNumber()
        {
            if (_spriteManager == null)
            {
                _spriteManager = GameObject.FindGameObjectWithTag(Tags.SpriteManager).GetComponent<SpriteManager>();
            }

            if (_colorManager == null)
            {
                _colorManager = GameObject.FindGameObjectWithTag(Tags.ColorManager).GetComponent<ColorManager>();
            }

            var baseAttackCard = this.TargetCreature.AttackCard;
            var baseDefenseCard = this.TargetCreature.DefenseCard;

            this.CanTargetAttack = Helpers.SuitToColor(baseAttackCard.CardSuit) == CardColorEnum.Black;
            this.CanTargetBlock = Helpers.SuitToColor(baseDefenseCard.CardSuit) == CardColorEnum.Red;

            // Assign the suit sprite
            this.AttackSuitRenderer.sprite = _spriteManager.SuitIconSprites[(int)this.TargetCreature.AttackCard.CardSuit];
            this.DefenseSuitRenderer.sprite = _spriteManager.SuitIconSprites[(int)this.TargetCreature.DefenseCard.CardSuit];

            // Assign the number text
            this.AttackNumberMesh.text = TargetCreature.TotalAttackNumber.ToString();
            this.DefenseNumberMesh.text = TargetCreature.TotalDefenseNumber.ToString();

            // Assign colors for new texts
            this.AttackNumberMesh.color = _colorManager.GetCardColorFromSuit(this.TargetCreature.AttackCard.CardSuit);
            this.DefenseNumberMesh.color = _colorManager.GetCardColorFromSuit(this.TargetCreature.DefenseCard.CardSuit);
        }

        /// <summary>
        /// Used for initialization
        /// </summary>
        private void Start()
        {
            this.TargetCreature.IsTapped = false;
            this.TargetCreature.HaveSummoningSickness = true;
			this._isSelected = false;
        }
    }
}