//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="HealthAssignUI.cs" company="Yifei Xu">
//    Copyright (c) Yifei Xu.  All rights reserved.
//  </copyright>
//--------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.UI
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Intent;
	using Managers;
	using UnityEngine;
	using UnityEngine.UI;

	/// <summary>
	/// Displays the last played intent card
	/// </summary>
	public class LastPlayedIntentCard : MonoBehaviour
	{
		/// <summary>
		/// The sprite renderer responsible for the card number
		/// </summary>
		public Image CardNumberSpriteRenderer;

		/// <summary>
		/// The sprite renderer responsible for the card suit
		/// </summary>
		public Image CardSuitSpriteRenderer;

		/// <summary>
		/// The sprite renderer for the card base
		/// </summary>
		private Image _cardBaseeRenderer;

		/// <summary>
		/// Position for when the enemy plays the card
		/// </summary>
		public Vector2 TopPosition;

		/// <summary>
		/// Position for when the current player players the card
		/// </summary>
		public Vector2 BottomPosition;

		/// <summary>
		/// Renders the card
		/// </summary>
		public void Render()
		{
			var actionStack = IntentManager.CurrentInstance.ActionStack;

			// If there are no actions
			if (actionStack.Count == 0)
			{
				this.SetAllRenderStates(false);
				return;
			}
			
			// Check if the action is something that should be rendered;
			var lastAction = actionStack.Peek();

			if (lastAction.Intent == IntentEnum.PassTurn || lastAction.Intent == IntentEnum.CreatureAttack)
			{
				return;
			}

			// Sets the correct sprite and color
			var lastCard = lastAction.Card;
			var spriteManager = SpriteManager.CurrentInstance;
			this.SetAllRenderStates(true);
			this.CardNumberSpriteRenderer.sprite = spriteManager.CardNumberSprites[lastCard.CardNumber - 1];
			this.CardNumberSpriteRenderer.color = ColorManager.CurrentInstance.GetCardColorFromSuit(lastCard.CardSuit);
			this.CardSuitSpriteRenderer.sprite = spriteManager.SuitDoubleSprites[(int)lastCard.CardSuit];

			// Moves the card to right position
			if (lastAction.IssuingPlayerId == PlayerController.LocalPlayer.PlayerId)
			{
				this.transform.localPosition = this.BottomPosition;
			}
			else
			{
				this.transform.localPosition = this.TopPosition;
			}
		}

		/// <summary>
		/// Called as initialization
		/// </summary>
		private void Start()
		{
			this._cardBaseeRenderer = this.GetComponent<Image>();
			this.SetAllRenderStates(false);
		}

		/// <summary>
		/// Sets the enable state of all renderer
		/// </summary>
		/// <param name="target">The target goal</param>
		private void SetAllRenderStates(bool target)
		{
			this._cardBaseeRenderer.enabled = target;
			this.CardNumberSpriteRenderer.enabled = target;
			this.CardSuitSpriteRenderer.enabled = target;
		}
	}
}
