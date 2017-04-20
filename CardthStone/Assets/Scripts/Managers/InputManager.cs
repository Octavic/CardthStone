//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="InputManager.cs" company="Yifei Xu">
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

    /// <summary>
    /// The input manager
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        /// <summary>
        /// Draws a new card for the current player
        /// </summary>
        public void DrawCard()
        {
            var localPlayer = PlayerController.LocalPlayer;
            if (localPlayer.isServer)
            {
                localPlayer.MyPlayerState.DrawCardFromDeck();
            }
            else
            {
                localPlayer.CmdDrawCard();
            }
        }
    }
}
