//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="IntentManager.cs" company="Yifei Xu">
//    Copyright (c) Yifei Xu.  All rights reserved.
//  </copyright>
//--------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Intent
{
	using System.Collections.Generic;
	using Managers;
	using UI;
	using UnityEngine;

	/// <summary>
	/// Manages all intent related calculations
	/// </summary>
	public class IntentManager : MonoBehaviour
	{
		/// <summary>
		/// Uses the card
		/// </summary>
		public GameObject UseCardButton;

		/// <summary>
		/// 
		/// </summary>
		public GameObject UseAcePowerButton;

		/// <summary>
		/// Gets the current instance
		/// </summary>
		public static IntentManager CurrentInstance { get; private set; }

		/// <summary>
		/// The pending action stack
		/// </summary>
		public Stack<IntentAction> ActionStack;

		/// <summary>
		/// Adds a new intent
		/// </summary>
		/// <param name="intent">Target intent</param>
		/// <param name="issuingPlayerId">The player who issued this intent</param>
		/// <param name="card">the card involved, null if creature combat</param>
		/// <param name="sourceId">the attacking creature, if applicable</param>
		/// <param name="targetId">the receiver, either creature Id or player Id if applicable</param>
		public void AddIntent(IntentEnum intent, int issuingPlayerId, Card card, int sourceId, int targetId)
		{
			var newIntent = new IntentAction(intent, issuingPlayerId, card, sourceId, targetId);
			this.ActionStack.Push(newIntent);

			TurnManager.CurrentInstance.OnTurnStart();
		}
		
		/// <summary>
		/// Called once when a new instance of the class is initialized
		/// </summary>
		private void Awake()
		{
			IntentManager.CurrentInstance = this;
			this.ActionStack = new Stack<IntentAction>();
		}

		/// <summary>
		/// Called every fixed frame
		/// </summary>
		private void FixedUpdate()
		{
			var selectedCard = CardBehavior.CurrentlySelected;
			var selectedCreature = CreatureBehavior.CurrentlySelected;

			if (selectedCard != null && selectedCreature != null)
			{
				this.UseCardButton.SetActive(true);
				this.UseAcePowerButton.SetActive(selectedCard.PokerCard.CardNumber == 1);
			}
			else
			{
				this.UseAcePowerButton.SetActive(false);
				this.UseCardButton.SetActive(false);
			}
		}
	}
}
