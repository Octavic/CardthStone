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
		#region Unity Editor Links
		/// <summary>
		/// The button that uses the card normally
		/// </summary>
		public GameObject UseCardButton;

		/// <summary>
		/// The button that uses the card as an ace card
		/// </summary>
		public GameObject UseAcePowerButton;

		/// <summary>
		/// The button that uses the current card to counter the target card
		/// </summary>
		public GameObject CounterSpellButton;

		/// <summary>
		/// The button that passes the current mini-turn in counter action
		/// </summary>
		public GameObject PassTurnButton;

		/// <summary>
		/// The button that accepts the creature combat action
		/// </summary>
		public GameObject CreatureAttackButton;

		/// <summary>
		/// The button that initiates creature block action
		/// </summary>
		public GameObject CreatureBlockButton;

		/// <summary>
		/// Component for the last played intent card object
		/// </summary>
		public LastIntentIndicator LastPlayedIntentCardComponent;
		#endregion

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
			// If current intent is pass, check last action to see if it was also pass. If so, resolve
			if (intent == IntentEnum.PassTurn && this.ActionStack.Count > 0 && this.ActionStack.Peek().Intent == IntentEnum.PassTurn)
			{
				this.ResolvePendingActions();
			}
			else
			{
				var newIntent = new IntentAction(intent, issuingPlayerId, card, sourceId, targetId);
				this.ActionStack.Push(newIntent);
			}

			// Renders all components
			TurnManager.CurrentInstance.Render();
			this.LastPlayedIntentCardComponent.Render();
			this.Render();
		}

		/// <summary>
		/// Renders all intent related buttons
		/// </summary>
		public void Render()
		{
			// Grabs the selected card and selected creature
			var selectedCard = CardBehavior.CurrentlySelected;
			var selectedFriendCreature = CreatureBehavior.CurrentlySelectedFriendly;
			var selectedEnemyCreature = CreatureBehavior.CurrentlySelectedEnemy;
			var selectedHealthCard = PlayerHealthCards.CurrentlySelected;

			// Check controllers
			var gameController = GameController.CurrentInstance;
			if (gameController == null)
			{
				return;
			}

			var playerController = PlayerController.LocalPlayer;
			if (playerController == null)
			{
				return;
			}

			// Determine turn related variables
			var isPlayerNormalTurn = gameController.CurrentPlayerId == playerController.PlayerId;
			bool isPlayerCounterTurn = false;
			var hasPendingIntentions = this.ActionStack.Count > 0;

			IntentAction lastAction = this.ActionStack.Count == 0 ? null : this.ActionStack.Peek();
			if (lastAction != null && lastAction.IssuingPlayerId != playerController.PlayerId)
			{
				isPlayerCounterTurn = true;
			}

			// Sets the state of the pass button
			if (!hasPendingIntentions)
			{
				this.PassTurnButton.SetActive(false);
			}
			else
			{
				this.PassTurnButton.SetActive(isPlayerCounterTurn);
			}

			// If it's not the player's turn to counter or normal, then there is nothing that could be done
			if (!isPlayerCounterTurn && !isPlayerNormalTurn)
			{
				this.CounterSpellButton.SetActive(false);
				this.UseAcePowerButton.SetActive(false);
				this.UseCardButton.SetActive(false);
				return;
			}

			// If there is a card selected
			if (selectedCard != null)
			{
				// Card selected, cannot initiate creature combat
				this.CreatureAttackButton.SetActive(false);

				// Grabs the card value
				var selectedCardValue = selectedCard.PokerCard;

				// Check if only one of the three is selected
				if (selectedFriendCreature != null
				^ selectedEnemyCreature != null
				^ selectedHealthCard != null)
				{
					// Check if it's player's turn to do anything and if the card is NOT a relic
					if ((isPlayerCounterTurn || isPlayerNormalTurn)
						&& selectedCardValue.CardNumber <= 10)
					{
						// Check suits to see if the selected suit is valid
						// Check spade, can be used on creatures and health cards
						if (selectedCardValue.CardSuit == CardSuitEnum.Spade)
						{
							this.UseCardButton.SetActive(true);
							this.UseAcePowerButton.SetActive(selectedCardValue.CardNumber == 1);
						}
						// Check creature buff/debuff, must NOT have health selected
						else if (selectedCardValue.CardSuit == CardSuitEnum.Club || selectedCardValue.CardSuit == CardSuitEnum.Diamond)
						{
							this.UseCardButton.SetActive(!selectedHealthCard);
							this.UseAcePowerButton.SetActive(!selectedHealthCard && selectedCardValue.CardNumber == 1);
						}
						// Check place health card, must only have health selected
						else
						{
							this.UseCardButton.SetActive(selectedHealthCard);
							this.UseAcePowerButton.SetActive(selectedHealthCard && selectedCardValue.CardNumber == 1);
						}

						this.UseAcePowerButton.SetActive(selectedCardValue.CardNumber == 1);
					}
					// Selected card is relic, or player is not in turn, or the card and creature/health card combo does not work
					else
					{
						this.UseCardButton.SetActive(false);
						this.UseAcePowerButton.SetActive(false);
					}
				}
				// Check if none of the 3 is selected for counter
				else if (selectedEnemyCreature == null
					&& selectedFriendCreature == null
					&& selectedHealthCard == null)
				{
					// Can't just use card
					this.UseCardButton.SetActive(false);
					this.UseAcePowerButton.SetActive(false);

					// Only a card is selected, check if possible counter
					if (lastAction != null
						&& hasPendingIntentions
						&& isPlayerCounterTurn
						// Cannot counter creature attack or turn pass
						&& lastAction.Intent != IntentEnum.CreatureAttack
						&& lastAction.Intent != IntentEnum.PassTurn
						&& Helpers.SuitToColor(lastAction.Card.CardSuit) != Helpers.SuitToColor(selectedCardValue.CardSuit)
						&& selectedCardValue.CardNumber < lastAction.Card.CardNumber)
					{
						this.CounterSpellButton.SetActive(true);
					}
					else
					{
						this.CounterSpellButton.SetActive(false);
					}
				}
				// Card and multiple targets selected, does not work
				else
				{
					this.CounterSpellButton.SetActive(false);
					this.UseAcePowerButton.SetActive(false);
					this.UseCardButton.SetActive(false);
				}
			}
			// No card selected
			else
			{
				// Can't use any card related buttons if there are no cards selected
				this.CounterSpellButton.SetActive(false);
				this.UseAcePowerButton.SetActive(false);
				this.UseCardButton.SetActive(false);

				// Check if there's an attacker selected
				if (selectedFriendCreature == null)
				{
					this.CreatureAttackButton.SetActive(false);
					this.CreatureBlockButton.SetActive(false);
				}
				else
				{
					// Decides the state of the attack button. It can only happen as the first attack
					if (this.ActionStack.Count == 0)
					{
						// If only health card is selected, any creature can attack that
						if (selectedHealthCard != null && selectedEnemyCreature == null)
						{
							this.CreatureAttackButton.SetActive(true);
						}
						// If creature try to attack creature, it must have black as attack
						else if (
							selectedEnemyCreature != null
							&& selectedFriendCreature.CanTargetAttack
							&& selectedHealthCard == null)
						{
							this.CreatureAttackButton.SetActive(true);
						}
						else
						{
							this.CreatureAttackButton.SetActive(false);
						}
					}
					// There's already pending actions, cannot initiate new attack
					else
					{
						this.CreatureAttackButton.SetActive(false);
					}

					// Decides the state of the block button. It can only be done if there's an attack happening
					if (this.ActionStack.Count == 1 && this.ActionStack.Peek().Intent == IntentEnum.CreatureAttack && isPlayerCounterTurn)
					{
						// Must have only an ally unit selected, and the ally must be able to block
						if (
							selectedFriendCreature != null 
							&& selectedFriendCreature.CanBlock
							&& selectedEnemyCreature == null 
							&& selectedHealthCard == null
							&&isPlayerCounterTurn)
						{
							this.CreatureBlockButton.SetActive(true);
						}
						else
						{
							this.CreatureBlockButton.SetActive(false);
						}
					}
					else
					{
						this.CreatureBlockButton.SetActive(false);
					}
				}
			}
		}

		/// <summary>
		/// Resolves the pending actions
		/// </summary>
		private void ResolvePendingActions()
		{
			var playerController = PlayerController.LocalPlayer;

			// Go through the action stack
			while (this.ActionStack.Count > 0)
			{
				var curIntent = this.ActionStack.Pop();
				CreatureBehavior curCreature = null;

				// Grab the current target creature if applicable
				if (curIntent.TargetId >= Settings.MaxPlayerCount 
					&& ((curIntent.TargetId -Settings.MaxPlayerCount)< CreatureBehavior.Creatures.Count))
				{
					curCreature = CreatureBehavior.Creatures[curIntent.TargetId];
				}

				// Grab the corresponding playerState

				switch (curIntent.Intent)
				{
					case IntentEnum.PassTurn:
						continue;
					case IntentEnum.Counter:
						this.ActionStack.Pop();
						continue;
					case IntentEnum.BuffCreatureAttack:
						// Search through creatures and buff attack
						if (curCreature == null)
						{
							Debug.Log("Error: Null creature when trying to buff attack!");
							continue;
						}

						var creatures = PlayerStateManager.CurrentInstance.PlayerStates[curCreature.TargetCreature.OwnerUserId].Creatures;
						for (int i = 0; i < creatures.Count; i++)
						{
							if (creatures[i].CreatureId == curCreature.TargetCreature.CreatureId)
							{
								var newCreature = creatures[i];
								newCreature.BuffAttack(curIntent.Card);
								creatures.RemoveAt(i);
								creatures.Insert(i, newCreature);

								// Discard
								PlayerStateManager.CurrentInstance.PlayerStates[curIntent.IssuingPlayerId].PlayerHand.Remove(curIntent.Card);
							}
						}
						break;
					case IntentEnum.BuffCreatureDefense:
						// Search through creatures and buff defense
						if (curCreature == null)
						{
							Debug.Log("Error: Null creature when trying to buff defense!");
							continue;
						}

						creatures = PlayerStateManager.CurrentInstance.PlayerStates[curCreature.TargetCreature.OwnerUserId].Creatures;
						for (int i = 0; i < creatures.Count; i++)
						{
							if (creatures[i].CreatureId == curCreature.TargetCreature.CreatureId)
							{
								var newCreature = creatures[i];
								newCreature.BuffDefense(curIntent.Card);
								creatures.RemoveAt(i);
								creatures.Insert(i, newCreature);

								// Discard
								PlayerStateManager.CurrentInstance.PlayerStates[curIntent.IssuingPlayerId].PlayerHand.Remove(curIntent.Card);
							}
						}
						break;
				}
			}

			// Re-render creature area
			TurnManager.CurrentInstance.Render();
			this.Render();
			playerController.MyPlayerState.Displayer.RenderAll();
			playerController.EnemyPlayerState.Displayer.RenderAll();
		}

		/// <summary>
		/// Called once when a new instance of the class is initialized
		/// </summary>
		private void Awake()
		{
			IntentManager.CurrentInstance = this;
			this.ActionStack = new Stack<IntentAction>();

			this.DisableAllButtons();
		}

		/// <summary>
		/// Disables all buttons
		/// </summary>
		private void DisableAllButtons()
		{
			this.PassTurnButton.SetActive(false);
			this.CounterSpellButton.SetActive(false);
			this.UseAcePowerButton.SetActive(false);
			this.UseCardButton.SetActive(false);
			this.CreatureAttackButton.SetActive(false);
			this.CreatureBlockButton.SetActive(false);
		}
	}
}
