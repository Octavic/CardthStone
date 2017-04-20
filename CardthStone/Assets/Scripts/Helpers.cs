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
    }
}
