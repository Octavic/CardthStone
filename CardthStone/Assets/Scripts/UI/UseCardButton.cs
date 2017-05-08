//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="UseCardButton.cs" company="Yifei Xu">
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
	/// The button for using card
	/// </summary>
	public class UseCardButton : MonoBehaviour
	{
		/// <summary>
		/// Uses the card
		/// </summary>
		/// <param name="isAce">If this button is used as an ace card</param>
		public void UseCard(bool isAce)
		{
			// Normal card use
			if (!isAce)
			{
				// Grab the selected card and creature and verify
				var card = CardBehavior.CurrentlySelected;
				if (card == null)
				{
					Debug.Log("No card selected!");
					return;
				}

				card.OnUserClick();
				var creature = CreatureBehavior.CurrentlySelected;
				if (creature == null)
				{
					Debug.Log("No creature selected!");
					return;
				}

				// Decide intent
				IntentEnum intent;
				switch (card.PokerCard.CardSuit)
				{
					case CardSuitEnum.Club:
						intent = IntentEnum.BuffCreatureAttack;
						break;
					case CardSuitEnum.Diamond:
						intent = IntentEnum.DirectDamage;
						break;
					case CardSuitEnum.Heart:
						intent = IntentEnum.PlaceHeathCard;
						break;
					default:
						intent = IntentEnum.BuffCreatureDefense;
						break;
				}

				// Rpc or command based on if this is server or not
				var localPlayer = PlayerController.LocalPlayer;
				if (localPlayer.isServer)
				{
					localPlayer.RpcCommitNormalCardUse(intent, localPlayer.PlayerId, card.PokerCard, -1, creature.TargetCreature.CreatureId);
				}
				else
				{
					localPlayer.CmdCommitNormalCardUse(intent, localPlayer.PlayerId, card.PokerCard, -1, creature.TargetCreature.CreatureId);
				}

				TurnManager.CurrentInstance.Render();
			}
		}
	}
}
