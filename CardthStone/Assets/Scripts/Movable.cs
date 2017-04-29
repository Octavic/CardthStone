//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="Movable.cs" company="Yifei Xu">
//    Copyright (c) Yifei Xu.  All rights reserved.
//  </copyright>
//--------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// Describes an item that's movable
    /// </summary>
    public class Movable : MonoBehaviour
    {
        /// <summary>
        /// Where the current goal is
        /// </summary>
        private Vector3 _goalPosition;

        /// <summary>
        /// How much to move
        /// </summary>
        private Vector3 _velocity;

        /// <summary>
        /// If the object should be moving
        /// </summary>
        private bool _isMoving;

        /// <summary>
        /// Moves the object to the goal position
        /// </summary>
        /// <param name="goal">where to move to</param>
        /// <param name="time">How long it will take to move the object there</param>
        public void MoveToLocalPositioin(Vector2 goal, float time)
        {
            this._goalPosition = goal;
            this._velocity = (this._goalPosition - this.transform.localPosition) / time;
            this._isMoving = true;
        }

        /// <summary>
        /// Called once per frame
        /// </summary>
        private void Update()
        {
            if (!this._isMoving)
            {
                return;
            }

            var offset = this._velocity * Time.deltaTime;
            if ((this._goalPosition - this.transform.localPosition).magnitude < offset.magnitude)
            {
                this.transform.localPosition = this._goalPosition;
                this._velocity = Vector3.zero;
                this._isMoving = false;
            }
            else
            {
                this.transform.localPosition += offset;
            }
        }
    }
}
