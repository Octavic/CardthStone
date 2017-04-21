//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="ToggleButton.cs" company="Yifei Xu">
//    Copyright (c) Yifei Xu.  All rights reserved.
//  </copyright>
//--------------------------------------------------------------------------------------------------------------------

namespace Assets.Scripts.Misc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    /// <summary>
    /// A button that simple toggles the active/inactive state of a game object
    /// </summary>
    public class ToggleButton : MonoBehaviour
    {
        /// <summary>
        /// The target object to be toggled 
        /// </summary>
        public GameObject Target;

        /// <summary>
        /// Toggles the state of the target
        /// </summary>
        public void Toggle()
        {
            Target.SetActive(!Target.activeSelf);
        }
    }
}
