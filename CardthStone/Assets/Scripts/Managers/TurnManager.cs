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
		/// A list of buttons that can only be used during your own normal turn
		/// </summary>
		public List<Button> NormalActionOnlyButtons;

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
            this.SetAllButtonInteractState(this.NormalActionOnlyButtons, false);

			TurnManager.CurrentInstance = this;
        }

		/// <summary>
		/// Called when a new turn starts
		/// </summary>
		public void OnTurnStart()
		{
			var gameController = GameController.CurrentInstance;
			var localPlayer = PlayerController.LocalPlayer;

			// Draw cards if it's the beginning of current player's normal turn. if it's turn one, only draw one card
			if (gameController.CurrentPhase == GamePhaseEnum.Normal && gameController.CurrentPlayerId == localPlayer.PlayerId)
			{
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

			// Renders the show
			this.Render();
		}

        /// <summary>
        /// Called when turn related elements needs to be rendered
        /// </summary>
        public void Render()
        {
            var gameController = GameController.CurrentInstance;
			var localPlayer = PlayerController.LocalPlayer;

			// Check current phase. If mulligan, pull up mulligan UI
			if (gameController.CurrentPhase == GamePhaseEnum.Mulligan)
            {
                if (gameController.CurrentPlayerId == localPlayer.PlayerId)
                {
                    this.MulliganUI.MoveToLocalPositioin(Settings.MulliganUIFinalPosition, Settings.UIMovementDuration);
                }
            }
			// Normal turn
            else
            {
				// Check intent manager to see if it's this current user's turn for counter action
				if (IntentManager.CurrentInstance.ActionStack.Count > 0)
				{
					// Counter actions are happening, no normal action allowed
					this.SetAllButtonInteractState(this.NormalActionOnlyButtons, false);
				}
				else if (gameController.CurrentPlayerId == localPlayer.PlayerId)
				{
					// No actions in stack, and it's current player's turn. Go ahead
					this.SetAllButtonInteractState(this.NormalActionOnlyButtons, true);
				}
                else
                {
					// Not current player's normal or counter turn, no buttons can be used
                    this.SetAllButtonInteractState(this.NormalActionOnlyButtons, false);
				}
            }

			// Also redraw the intent buttons
			IntentManager.CurrentInstance.Render();
        }

		/// <summary>
		/// Loops through and sets the interactive state of every button
		/// </summary>
		/// <param name="buttons">List of buttons</param>
		/// <param name="desiredState">What state to set it to</param>
		private void SetAllButtonInteractState(IList<Button> buttons, bool desiredState)
        {
			foreach (var button in buttons)
			{
				button.interactable = desiredState;
			}
		}

		/// <summary>
		/// Loops through and sets the active state of every button
		/// </summary>
		/// <param name="buttons">List of buttons</param>
		/// <param name="desiredState">What state to set it to</param>
		private void SetAllButtonActiveState(IList<Button> buttons, bool desiredState)
		{
			foreach (var button in buttons)
			{
				button.gameObject.SetActive(desiredState);
			}
		}
    }
}
