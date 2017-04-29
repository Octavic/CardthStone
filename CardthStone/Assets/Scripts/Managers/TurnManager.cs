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

    /// <summary>
    /// Manages the player's turns
    /// </summary>
    public class TurnManager : MonoBehaviour
    {
        /// <summary>
        /// The UI for the mulligan
        /// </summary>
        public GameObject MulliganUI;

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
        /// Used for initialization
        /// </summary>
        private void Start()
        {
            this.SetAllButtonsState(this.SharedActionButtons, false);
            this.SetAllButtonsState(this.NormalActionOnlyButtons, false);
            this.SetAllButtonsState(this.CounterActionOnlyButtons, false);
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        private void Update()
        {
            var gameController = GameController.CurrentInstance;
            if (gameController.CurrentPhase == GamePhaseEnum.Mulligan)
            {
                if (gameController.CurrentPlayerId == PlayerController.LocalPlayer.PlayerId)
                {
                    this.MulliganUI.transform.localPosition = Settings.MulliganUIFinalPosition;
                }
                else
                {
                    this.MulliganUI.transform.localPosition = new Vector2(585, 600);
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
