using UnityEngine;
using TriInspector;
using GoodVillageGames.Core.Mediator;
using GoodVillageGames.Core.Interfaces;
using GoodVillageGames.Core.Character.Attributes;

namespace GoodVillageGames.Core.Character
{
    /// <summary>
    /// Base class for all interactive and mobile characters from the project.
    /// Manages the entity's stats and component interactions.
    /// </summary>
    public abstract class Entity : MonoBehaviour, IVisitable
    {
        [SerializeField, InlineEditor, Required] 
        private BaseStats _statsDefinition;

        public IStatProvider Stats { get; private set; }

        protected virtual void Awake()
        {
            var statsMediator = new StatsMediator();
            Stats = new CharacterStats(statsMediator, _statsDefinition);
        }

        public virtual void Update()
        {
            // The Stats object (which is a CharacterStats instance) handles its own update logic ;)
            if (Stats is IUpdatable updatableStats)
            {
                updatableStats.Tick(Time.deltaTime);
            }
        }

        public void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}