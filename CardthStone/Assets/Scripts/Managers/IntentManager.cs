//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="IntentManager.cs" company="Yifei Xu">
//    Copyright (c) Yifei Xu.  All rights reserved.
//  </copyright>
//--------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Managers
{
	using UnityEngine;

	/// <summary>
	/// Manages all intent related calculations
	/// </summary>
	public class IntentManager : MonoBehaviour
	{
		/// <summary>
		/// Gets the current instance
		/// </summary>
		public static IntentManager CurrentInstance { get; private set; }

		/// <summary>
		/// Gets the current intent
		/// </summary>
		public IntentEnum CurrentIntent { get; private set; }

		/// <summary>
		/// Called once when a new instance of the class is initialized
		/// </summary>
		private void Start()
		{
			IntentManager.CurrentInstance = this;
		}

		/// <summary>
		/// Adds a health card to the target player
		/// </summary>
		/// <param name="targetPlayerId">The target player to add health to</param>
		public void AddHealthCard(int targetPlayerId)
		{

		}
	}
}
