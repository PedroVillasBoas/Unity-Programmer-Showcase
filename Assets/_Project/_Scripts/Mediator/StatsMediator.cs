using System;
using System.Linq;
using System.Collections.Generic;

namespace GoodVillageGames.Core.Mediator
{
    /// <summary>
    /// This is The Broker that sits between the stat provider and the stat modifiers
    /// It manages event subscriptions and broadcasts queries to all active modifiers
    /// </summary>
    public class StatsMediator
    {
        // List<T> generally has better performance for iteration and cache locality, unless we plan to have a very high frequency of 
        // insertions/deletions in the middle of the collection, which is not the use case here since it's a demo project and eveything ;)
        private readonly List<StatModifier> _modifiers = new();
        private readonly List<StatModifier> _modifiersToRemove = new();

        // Perfoming a Query/Consult whenever something wants to know about a specific Stat
        public event EventHandler<Query> Queries;

        public void PerformQuery(object sender, Query query)
        {
            Queries?.Invoke(sender, query);
        }

        public void AddModifier(StatModifier modifier)
        {
            _modifiers.Add(modifier);
            Queries += modifier.Handle;

            modifier.OnDispose += HandleModifierDisposal;
        }

        public T GetModifier<T>() where T : StatModifier
        {
            // I'm using LINQ's OfType to find the first modifier of a specific type
            return _modifiers.OfType<T>().FirstOrDefault();
        }

        private void HandleModifierDisposal(StatModifier modifier)
        {
            // As always... Unsubscribe from events to prevent memory leaks
            modifier.OnDispose -= HandleModifierDisposal;
            Queries -= modifier.Handle;
            
            // I'm scheduling for removal instead of modifying the list directly,
            // which is safer and prevents collection modification errors during iteration ;)
            _modifiersToRemove.Add(modifier);
        }

        public void Update(float deltaTime)
        {            
            // First, update all modifiers
            foreach (var modifier in _modifiers)
            {
                modifier.Update(deltaTime);
                if (modifier.MarkedForRemoval)
                {
                    modifier.Dispose();
                }
            }

            // Then, the code can safelly remove all modifiers that were marked and disposed
            // If there were any ;)
            if (_modifiersToRemove.Count > 0)
            {
                foreach (var modifier in _modifiersToRemove)
                {
                    _modifiers.Remove(modifier);
                }
                _modifiersToRemove.Clear();
            }
        }
    }
}