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
				var selectedCard = CardBehavior.CurrentlySelected;
				if (selectedCard == null)
				{
					Debug.Log("No card selected!");
					return;
				}

				var selectedFriendly = CreatureBehavior.CurrentlySelectedFriendly;
				var selectedEnemy = CreatureBehavior.CurrentlySelectedEnemy;
				var selectedHealth = PlayerHealthCards.CurrentlySelected;

				int targetId = -1;
				if (selectedFriendly != null && selectedEnemy == null && selectedHealth == null)
				{
					targetId = selectedFriendly.TargetCreature.CreatureId;
					selectedFriendly.OnUserClick();
				}
				else if (selectedEnemy != null && selectedFriendly == null && selectedHealth == null)
				{
					targetId = selectedEnemy.TargetCreature.CreatureId;
					selectedEnemy.OnUserClick();
				}
				else if (selectedHealth != null && selectedFriendly == null && selectedEnemy == null)
				{
					targetId = selectedHealth.PlayerId;
					selectedHealth.OnUserClick();
				}
				else
				{
					Debug.Log("None or multiple creatures/health selected!");
					return;
				}

				// Decide intent
				IntentEnum intent;
				switch (selectedCard.PokerCard.CardSuit)
				{
					case CardSuitEnum.Spade:
						intent = IntentEnum.DirectDamage;
						break;
					case CardSuitEnum.Heart:
						if (targetId >= Settings.MaxPlayerCount)
						{
							Debug.Log("Heart cards can only be used on health!");
							return;
						}

						intent = IntentEnum.PlaceHeathCard;
						break;
					case CardSuitEnum.Club:
						if (targetId < Settings.MaxPlayerCount)
						{
							Debug.Log("Club cards cannot be used on health!");
							return;
						}

						intent = IntentEnum.BuffCreatureAttack;
						break;
					default:
						if (targetId < Settings.MaxPlayerCount)
						{
							Debug.Log("Club cards cannot be used on health!");
							return;
						}

						intent = IntentEnum.BuffCreatureDefense;
						break;
				}

				// Rpc or command based on if this is server or not
				var localPlayer = PlayerController.LocalPlayer;
				if (localPlayer.isServer)
				{
					localPlayer.RpcCommitCardUse(intent, localPlayer.PlayerId, selectedCard.PokerCard, -1, targetId);
				}
				else
				{
					localPlayer.CmdCommitCardUse(intent, localPlayer.PlayerId, selectedCard.PokerCard, -1, targetId);
					localPlayer.CommitCardUse(intent, localPlayer.PlayerId, selectedCard.PokerCard, -1, targetId);
				}

				TurnManager.CurrentInstance.Render();
			}
		}
	}
}
