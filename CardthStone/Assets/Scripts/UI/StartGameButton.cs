//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="StartGameButton.cs" company="Yifei Xu">
//    Copyright (c) Yifei Xu.  All rights reserved.
//  </copyright>
//--------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.UI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    
    /// <summary>
    /// The start button that draws the player their initial hand
    /// </summary>
    public class StartGameButton : MonoBehaviour
    {
        /// <summary>
        /// The health UI
        /// </summary>
        public Movable HealthUI;

        /// <summary>
        /// Draws a new card for the current player
        /// </summary>
        public void StartGame()
        {
            var localPlayer = PlayerController.LocalPlayer;
            if (localPlayer.isServer)
            {
                for (int i = 0; i < Settings.PlayerStaringHandSize; i++)
                {
                    localPlayer.MyPlayerState.DrawCardFromDeck();
                }
            }
            else
            {
                for (int i = 0; i < Settings.PlayerStaringHandSize; i++)
                {
                    localPlayer.CmdDrawCard();
                }
            }

            this.HealthUI.MoveToLocalPositioin(Settings.HealthAssignUIFinalPosition, Settings.UIMovementDuration);
            this.gameObject.SetActive(false);
        }
    }
}
