//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="MulliganUI.cs" company="Yifei Xu">
//    Copyright (c) Yifei Xu.  All rights reserved.
//  </copyright>
//--------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.UI
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using Managers;
	using UnityEngine;

	/// <summary>
	/// The UI for mulligan
	/// </summary>
	public class MulliganUI : MonoBehaviour
    {
        /// <summary>
        /// A list of card slots for the mulligan UI
        /// </summary>
        public List<CardSlot> CardSlots;

        /// <summary>
        /// Commits the mulligan
        /// </summary>
        public void CommitMulligan()
        {
            var localPlayer = PlayerController.LocalPlayer;
            foreach (var slot in CardSlots)
            {
                // Check each slot to see if a card has been placed. If so, mulligan it
                if (slot.PlacedCard != null)
                {
                    if (localPlayer.isServer)
                    {
                        PlayerController.LocalPlayer.MyPlayerState.MulliganCard(slot.PlacedCard.PokerCard);
                    }
                    else
                    {
                        localPlayer.CmdMulliganCard(slot.PlacedCard.PokerCard);
                    }
                }
            }

			// End the current turn
			if (localPlayer.isServer)
			{
				GameController.CurrentInstance.RpcEndCurrentTurn();
			}
			else
			{
				PlayerController.LocalPlayer.CmdEndCurrentTurn();
			}

            this.gameObject.SetActive(false);
        }
    }
}
