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
		/// Called every frame
		/// </summary>
		private void Update()
		{
			// If the user left clicked on this frame, raycast and see if user clicked on a creature
			if (Input.GetMouseButtonDown(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

				// If the user clicked on a creature
				if (hit.collider != null && hit.collider.gameObject.tag == Tags.Creature)
				{
					hit.collider.GetComponent<CreatureBehavior>().OnUserClick();
				}
			}
		}
	}
}
