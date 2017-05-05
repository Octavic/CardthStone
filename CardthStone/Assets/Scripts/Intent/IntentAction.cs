//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="IntentAction.cs" company="Yifei Xu">
//    Copyright (c) Yifei Xu.  All rights reserved.
//  </copyright>
//--------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Intent
{
	/// <summary>
	/// An intent action stored in the stack
	/// </summary>
	public class IntentAction
	{
		/// <summary>
		/// Gets the intent
		/// </summary>
		public IntentEnum Intent { get; private set; }

		/// <summary>
		/// The player who issued this intent
		/// </summary>
		public int IssuingPlayerId { get; private set; }

		/// <summary>
		/// Gets the card used in this intent, null if creature combat
		/// </summary>
		public Card Card { get; private set; }

		/// <summary>
		/// Gets the source creature Id, null if direct card use
		/// </summary>
		public int SourceId { get; private set; }

		/// <summary>
		/// Gets the target creature/player Id, null if counter card
		/// </summary>
		public int TargetId { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="IntentAction"/> class
		/// </summary>
		/// <param name="intent">Target intent</param>
		/// <param name="issingPlayerId">The player who issued this intent</param>
		/// <param name="card">the card involved, null if creature combat</param>
		/// <param name="sourceId">the attacking creature, if applicable</param>
		/// <param name="targetId">the receiver, either creature Id or player Id if applicable</param>
		public IntentAction(IntentEnum intent, int issingPlayerId, Card card, int sourceId, int targetId)
		{
			this.Intent = intent;
			this.IssuingPlayerId = issingPlayerId;
			this.Card = card;
			this.SourceId = sourceId;
			this.TargetId = targetId;
		}
	}
}
