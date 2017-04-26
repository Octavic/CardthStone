//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="Card.cs" company="Yifei Xu">
//    Copyright (c) Yifei Xu.  All rights reserved.
//  </copyright>
//--------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using System;

    /// <summary>
    /// Describes the strength of the card
    /// </summary>
    public struct Card
    {
        /// <summary>
        /// Gets the suit of the card
        /// </summary>
        public CardSuitEnum CardSuit;

        /// <summary>
        /// Gets the number of the card
        /// </summary>
        public int CardNumber;

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class
        /// </summary>
        /// <param name="suit">Desire suit for this card</param>
        /// <param name="number">Desired number for this card</param>
        public Card(CardSuitEnum suit, int number)
        {
            if (number < 1 || number > 13)
            {
                throw new IndexOutOfRangeException("Invalid card number: " + number);
            }

            this.CardSuit = suit;
            this.CardNumber = number;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class
        /// </summary>
        /// <param name="b">Creates a new copy of the given card</param>
        public Card(Card b)
        {
            this.CardSuit = b.CardSuit;
            this.CardNumber = b.CardNumber;
        }

        /// <summary>
        /// Gets current card's card strength
        /// </summary>
        /// <returns>The card strength when used as health or attack for creature or health cards</returns>
        public int GetStrength()
        {
            return this.CardNumber < 10 ? this.CardNumber : 10;
        }

        /// <summary>
        /// Write out the card in a string
        /// </summary>
        /// <returns>The card in a string</returns>
        public override string ToString()
        {
            string numberString = null;
            switch (this.CardNumber)
            {
                case 1:
                    numberString = "A";
                    break;
                case 11:
                    numberString = "J";
                    break;
                case 12:
                    numberString = "Q";
                    break;
                case 13:
                    numberString = "K";
                    break;
                default:
                    numberString = this.CardNumber.ToString();
                    break;
            }

            return numberString + " of " + this.CardSuit.ToString();
        }

        /// <summary>
        /// Overloads the == operand
        /// </summary>
        /// <param name="a">Card A</param>
        /// <param name="b">Card B</param>
        /// <returns>True if the two cards are the same card</returns>
        public static bool operator ==(Card a, Card b)
        {
            return (a.CardSuit == b.CardSuit && a.CardNumber == b.CardNumber);
        }

        /// <summary>
        /// Overloads the != operand
        /// </summary>
        /// <param name="a">Card A</param>
        /// <param name="b">Card B</param>
        /// <returns>True if the two cards are NOT the same card</returns>
        public static bool operator !=(Card a, Card b)
        {
            return (a.CardSuit != b.CardSuit || a.CardNumber != b.CardNumber);
        }

        /// <summary>
        /// Overrides the equals function
        /// </summary>
        /// <param name="obj">Compared object</param>
        /// <returns>True if the two objects are equal</returns>
        public override bool Equals(object obj)
        {
            if (obj is Card)
            {
                return this == (Card)obj;
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// Overrides the get hash function
        /// </summary>
        /// <returns>Hashed index</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Overloads the < operand
        /// </summary>
        /// <param name="a">Card A</param>
        /// <param name="b">Card B</param>
        /// <returns>True if card A is smaller than card B</returns>
        public static bool operator <(Card a, Card b)
        {
            if (a.CardNumber < b.CardNumber)
            {
                return true;
            }

            if (a.CardNumber > b.CardNumber)
            {
                return false;
            }

            return (int)a.CardSuit < (int)b.CardSuit;
        }

        /// <summary>
        /// Overloads the > operand
        /// </summary>
        /// <param name="a">Card A</param>
        /// <param name="b">Card B</param>
        /// <returns>True if card A is bigger than card B</returns>
        public static bool operator >(Card a, Card b)
        {
            if (a.CardNumber > b.CardNumber)
            {
                return true;
            }

            if (a.CardNumber < b.CardNumber)
            {
                return false;
            }

            return (int)a.CardSuit > (int)b.CardSuit;
        }
    }
}