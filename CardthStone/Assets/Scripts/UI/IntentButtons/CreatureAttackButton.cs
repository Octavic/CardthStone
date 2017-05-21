//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="TurnManager.cs" company="Yifei Xu">
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
	/// The button that indicates an attack action
	/// </summary>
	public class CreatureAttackButton : MonoBehaviour
	{
		/// <summary>
		/// Attack with the selected friendly creature
		/// </summary>
		public void AttackWithCreature()
		{
			var selectedFriendly = CreatureBehavior.CurrentlySelectedFriendly;
			var selectedEnemy = CreatureBehavior.CurrentlySelectedEnemy;
			var selectedHealth = PlayerHealthCards.CurrentlySelected;

			// Performs check to ensure that the attack is valid
			if (selectedFriendly == null)
			{
				Debug.Log("Cannot attack without a selected friendly creature");
				return;
			}

			if (selectedEnemy == null && selectedHealth == null)
			{
				Debug.Log("No target selected for the attack!");
				return;
			}

			if (selectedEnemy != null && selectedHealth != null)
			{
				Debug.Log("Too many targets selected");
				return;
			}

			var targetId = selectedEnemy != null ? selectedEnemy.TargetCreature.CreatureId : selectedHealth.PlayerId;
			if (targetId == PlayerController.LocalPlayer.PlayerId)
			{
				Debug.Log("Cannot attack your own health card");
				return;
			}

			// Rpc or command based on if this is server or not
			var localPlayer = PlayerController.LocalPlayer;
			var tempCard = new Card();
			if (localPlayer.isServer)
			{
				localPlayer.RpcCommitCardUse(IntentEnum.CreatureAttack, localPlayer.PlayerId, tempCard, selectedFriendly.TargetCreature.CreatureId, targetId);
			}
			else
			{
				localPlayer.RpcCommitCardUse(IntentEnum.CreatureAttack, localPlayer.PlayerId, tempCard, selectedFriendly.TargetCreature.CreatureId, targetId);
				localPlayer.CommitCardUse(IntentEnum.CreatureAttack, localPlayer.PlayerId, tempCard, selectedFriendly.TargetCreature.CreatureId, targetId);
			}

			TurnManager.CurrentInstance.Render();
		}
	}
}
