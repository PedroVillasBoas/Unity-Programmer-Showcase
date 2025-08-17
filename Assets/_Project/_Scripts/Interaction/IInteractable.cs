namespace GoodVillageGames.Core.Interfaces
{
    /// <summary>
    /// A universal contract for any object in the game world that Morgana can interact with.
    /// This allows me to make a decoupled and scalable interaction system. ;)
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// A popup displayed to the player when this object is in range.
        /// </summary>
        string InteractionPrompt { get; }

        /// <summary>
        /// The method called when the player interacts with this object.
        /// </summary>
        /// <returns>True if the interaction was successful, false if not.</returns>
        bool Interact();
    }
}
