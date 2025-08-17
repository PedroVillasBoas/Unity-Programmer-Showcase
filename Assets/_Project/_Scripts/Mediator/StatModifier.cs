using System;
using GoodVillageGames.Core.Util.Timer;

namespace GoodVillageGames.Core.Mediator
{
    /// <summary>
    /// Base class for all stat modifiers (like: buffs, debuffs, permanent upgrades)
    /// </summary>
    public abstract class StatModifier : IDisposable
    {
        public object Source { get; private set; }

        public bool MarkedForRemoval { get; protected set; }
        public event Action<StatModifier> OnDispose;
        protected readonly CountdownTimer _timer;

        protected StatModifier(object source, float duration)
        {
            Source = source;

            if (duration <= 0) return;

            _timer = new CountdownTimer(duration);
            _timer.OnTimerStop += () => MarkedForRemoval = true;
            _timer.Start();
        }

        public virtual void Update(float deltaTime) => _timer?.Tick(deltaTime);
        public abstract void Handle(object sender, Query query);

        public void Dispose()
        {
            OnDispose?.Invoke(this);
            OnDisposed();
        }

        protected virtual void OnDisposed() { }
    }
}