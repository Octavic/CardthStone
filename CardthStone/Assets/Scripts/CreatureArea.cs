//--------------------------------------------------------------------------------------------------------------------
//  <copyright file="CreatureArea.cs" company="Yifei Xu">
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
    using Managers;
    using States;

    /// <summary>
    /// The creature area for spawning creatures
    /// </summary>
    public class CreatureArea : MonoBehaviour
    {
        /// <summary>
        /// A list of creature behaviors
        /// </summary>
        private IList<CreatureBehavior> Creatures;

        /// <summary>
        /// Creates a new creature
        /// </summary>
        /// <param name="targetCreature">The creature that needs to be spawned</param>
        /// <returns>The class of the newly created creature</returns>
        private CreatureBehavior CreateCreature(Creature targetCreature)
        {
            var newCreatureObject = Instantiate(PrefabManager.CurrentInstance.CreaturePrefab, this.transform);
            var newCreatureClass = newCreatureObject.GetComponent<CreatureBehavior>();
            newCreatureClass.Spawn(targetCreature);

            return newCreatureClass;
        }

        /// <summary>
        /// Renders the player's creatures
        /// </summary>
        public void RenderPlayerCreatures(PlayerState playerState)
        {
            // Erase all current cards and recreate
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            this.Creatures = new List<CreatureBehavior>();

            // Add the correct cards
            for (int i = 0; i < playerState.Creatures.Count; i++)
            {
                var creature = playerState.Creatures[i];
                var newCreature = this.CreateCreature(creature);
                this.Creatures.Add(newCreature);
            }

            // Rearrange cards
            for (int i = 0; i < this.Creatures.Count; i++)
            {
                var curCreature = this.Creatures[i];
                curCreature.transform.localPosition = new Vector3(i * Settings.CreatureDistance, 0);
            }

            this.transform.localPosition = new Vector3(-this.Creatures.Count * Settings.CreatureDistance / 2, 0);
        }
    }
}
