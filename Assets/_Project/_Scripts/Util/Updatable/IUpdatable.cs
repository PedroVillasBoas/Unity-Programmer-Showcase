namespace GoodVillageGames.Core.Interfaces
{
    /// <summary>
    /// Defines a contract for objects that need to be updated on a frame-by-frame basis.
    /// This allows non-MonoBehaviour classes to participate in the game loop in a controlled way. ;)
    /// </summary>
    public interface IUpdatable
    {
        /// <summary>
        /// Ticks the object's logic forward in time.
        /// </summary>
        /// <param name="deltaTime">The time in seconds since the last frame. 
        /// But you already knew that... Right? Right?!</param>
        void Tick(float deltaTime);
    }
}