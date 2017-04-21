//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="Helpers.cs" company="Yifei Xu">
//    Copyright (c) Yifei Xu.  All rights reserved.
//  </copyright>
//--------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// A collection of helper functions
    /// </summary>
    public static class Helpers
    {
        public static CardColorEnum SuitToColor(CardSuitEnum suit)
        {
            if (suit == CardSuitEnum.Club || suit == CardSuitEnum.Spade)
            {
                return CardColorEnum.Black;
            }

            return CardColorEnum.Red;
        }

        /// <summary>
        /// Gets the strength of a card's number. Eg: J, Q, K are all considered 10
        /// </summary>
        /// <param name="cardNumber">Target card number</param>
        /// <returns>The card's strength</returns>
        public static int GetCardStrengthByNumber(int cardNumber)
        {
            return cardNumber > 10 ? 10 : cardNumber;
        }
    }
}
