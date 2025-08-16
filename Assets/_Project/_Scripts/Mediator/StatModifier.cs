using System;
using GoodVillageGames.Core.Util.Timer;

namespace GoodVillageGames.Core.Mediator
{
    /// <summary>
    /// Base class for all stat modifiers (like: buffs, debuffs, permanent upgrades)
    /// </summary>
    public abstract class StatModifier : IDisposable
    {
        public bool MarkedForRemoval { get; protected set; }

        public event Action<StatModifier> OnDispose;

        protected readonly CountdownTimer _timer;

        protected StatModifier(float duration)
        {
            // A duration of 0 or less signifies a permanent modifier ;)
            if (duration <= 0) return;

            _timer = new CountdownTimer(duration);
            _timer.OnTimerStop += () => MarkedForRemoval = true;
            _timer.Start();
        }

        public virtual void Update(float deltaTime) => _timer?.Tick(deltaTime);

        public abstract void Handle(object sender, Query query);

        // Modifier/Upgrade/Buff is done and can be Removed com the List
        public void Dispose()
        {
            OnDispose?.Invoke(this);
            OnDisposed();
        }
        
        protected virtual void OnDisposed() { }
    }
}