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
	using UnityEngine;

	/// <summary>
	/// /The pass button
	/// </summary>
	public class PassButton : MonoBehaviour
	{
		/// <summary>
		/// Passes the current turn while countering
		/// </summary>
		public void PassTurn()
		{
			// Rpc or command based on if this is server or not
			var localPlayer = PlayerController.LocalPlayer;
			if (localPlayer.isServer)
			{
				localPlayer.RpcPassTurn(localPlayer.PlayerId);
			}
			else
			{
				localPlayer.CmdPassTurn(localPlayer.PlayerId);
				IntentManager.CurrentInstance.AddIntent(IntentEnum.PassTurn, localPlayer.PlayerId, new Card(), -1, -1);
			}
		}
	}
}
