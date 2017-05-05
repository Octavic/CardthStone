
//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="TurnManager.cs" company="Yifei Xu">
//    Copyright (c) Yifei Xu.  All rights reserved.
//  </copyright>
//--------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using UnityEngine.UI;
	using Intent;

	/// <summary>
	/// Manages the player's turns
	/// </summary>
	public class TurnManager : MonoBehaviour
	{
		/// <summary>
		/// The UI for the mulligan
		/// </summary>
		public Movable MulliganUI;

		/// <summary>
		/// A list of shared actions that can be used in both counter turn and normal turn
		/// </summary>
		public List<Button> SharedActionButtons;

		/// <summary>
		/// A list of buttons that can only be used during your own normal turn
		/// </summary>
		public List<Button> NormalActionOnlyButtons;

		/// <summary>
		/// A list of buttons that can only be used when you are countering during someone else's turn
		/// </summary>
		public List<Button> CounterActionOnlyButtons;

		/// <summary>
		/// Gets the current instance of the turn manager
		/// </summary>
		public static TurnManager CurrentInstance { get; private set; }

		/// <summary>
		/// Ends the current turn
		/// </summary>
		public void EndCurrentTurn()
		{
			PlayerController.LocalPlayer.CmdEndCurrentTurn();
		}

        /// <summary>
        /// Used for initialization
        /// </summary>
        private void Start()
        {
            this.SetAllButtonsState(this.SharedActionButtons, false);
            this.SetAllButtonsState(this.NormalActionOnlyButtons, false);
            this.SetAllButtonsState(this.CounterActionOnlyButtons, false);

			TurnManager.CurrentInstance = this;
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        public void OnTurnStart()
        {
            var gameController = GameController.CurrentInstance;
			
			// Check current phase. If mulligan, pull up mulligan UI
            if (gameController.CurrentPhase == GamePhaseEnum.Mulligan)
            {
                if (gameController.CurrentPlayerId == PlayerController.LocalPlayer.PlayerId)
                {
                    this.MulliganUI.MoveToLocalPositioin(Settings.MulliganUIFinalPosition, Settings.UIMovementDuration);
                }
            }
            else if (gameController.CurrentPhase == GamePhaseEnum.Normal)
            {
				var localPlayer = PlayerController.LocalPlayer;

				// Check intent manager to see if it's this current user's counter turn
				if (IntentManager.CurrentInstance.ActionStack.Count > 0)
				{
					var lastAction = IntentManager.CurrentInstance.ActionStack.Peek();
					var isCurrentPlayerCountering = lastAction.IssuingPlayerId != localPlayer.PlayerId;
					this.SetAllButtonsState(this.SharedActionButtons, isCurrentPlayerCountering);
					this.SetAllButtonsState(this.CounterActionOnlyButtons, isCurrentPlayerCountering);
					this.SetAllButtonsState(this.NormalActionOnlyButtons, !isCurrentPlayerCountering);
				}
				else if (gameController.CurrentPlayerId == localPlayer.PlayerId)
				{
					// No actions in stack, and it's current player's turn. Go ahead
					this.SetAllButtonsState(this.SharedActionButtons, true);
					this.SetAllButtonsState(this.NormalActionOnlyButtons, true);
					this.SetAllButtonsState(this.CounterActionOnlyButtons, false);

					// Draw cards. if it's turn one, only draw one card
					if (gameController.TurnNumber == 1)
					{
						localPlayer.CmdDrawCard();
					}
					else
					{
						localPlayer.CmdDrawCard();
						localPlayer.CmdDrawCard();
					}
                }
                else
                {
					// Not current player's normal or counter turn, no buttons can be used
                    this.SetAllButtonsState(this.SharedActionButtons, false);
                    this.SetAllButtonsState(this.NormalActionOnlyButtons, false);
					this.SetAllButtonsState(this.CounterActionOnlyButtons, false);
				}
            }
        }

        /// <summary>
        /// Loops through and sets the interactive state of every button
        /// </summary>
        /// <param name="buttons">List of buttons</param>
        /// <param name="desiredState">What state to set it to</param>
        private void SetAllButtonsState(IList<Button> buttons, bool desiredState)
        {
            foreach (var button in buttons)
            {
                button.interactable = desiredState;
            }
        }
    }
}
