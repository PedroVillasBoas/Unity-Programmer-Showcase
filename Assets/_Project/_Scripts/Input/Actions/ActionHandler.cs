using UnityEngine;
using GoodVillageGames.Core.Character;
using GoodVillageGames.Core.Character.Attributes;

namespace GoodVillageGames.Core.Actions
{
    /// <summary>
    /// Base class for all action handlers to avoid repetitive boilerplate code. (been there, done that. Not anymore!)
    /// Provides easy access to core components like IStatProvider and Rigidbody.
    /// </summary>
    [RequireComponent(typeof(Entity))]
    public abstract class ActionHandler : MonoBehaviour
    {
        protected IStatProvider Stats { get; private set; }
        protected Rigidbody2D Rb { get; private set; }

        protected virtual void Start()
        {
            // The Presenter will require these components, so we can safely get them. (:
            Stats = GetComponent<Entity>().Stats;
            Rb = GetComponent<Rigidbody2D>();
        }
    }
}