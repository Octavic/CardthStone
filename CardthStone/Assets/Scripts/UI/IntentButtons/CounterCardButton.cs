//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="CounterCardButton.cs" company="Yifei Xu">
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

	/// <summary>
	/// The button for countering used cards
	/// </summary>
	public class CounterCardButton : MonoBehaviour
	{
		/// <summary>
		/// Activates the card
		/// </summary>
		public void Counter()
		{
			// Grab the selected card and creature and verify
			var card = CardBehavior.CurrentlySelected;
			if (card == null)
			{
				Debug.Log("No card selected!");
				return;
			}

			// Toggles the card
			card.OnUserClick();

			// Verify to see if the counter is valid
			var actionStack = IntentManager.CurrentInstance.ActionStack;
			if (actionStack.Count == 0)
			{
				Debug.Log("Cannot counter, action stack is empty!");
				return;
			}

			var counterTargetCard = actionStack.Peek().Card;
			if (counterTargetCard.CardNumber <= card.PokerCard.CardNumber
				|| Helpers.SuitToColor(counterTargetCard.CardSuit) == Helpers.SuitToColor(card.PokerCard.CardSuit))
			{
				Debug.Log(card.PokerCard.ToString() + " cannot be used to counter " + counterTargetCard);
			}

			// Rpc or command based on if this is server or not
			var localPlayer = PlayerController.LocalPlayer;
			if (localPlayer.isServer)
			{
				localPlayer.RpcCommitCardUse(IntentEnum.Counter, localPlayer.PlayerId, card.PokerCard, -1, -1);
			}
			else
			{
				localPlayer.CmdCommitCardUse(IntentEnum.Counter, localPlayer.PlayerId, card.PokerCard, -1, -1);
				localPlayer.CommitCardUse(IntentEnum.Counter, localPlayer.PlayerId, card.PokerCard, -1, -1);
			}

			TurnManager.CurrentInstance.Render();
		}
	}
}
