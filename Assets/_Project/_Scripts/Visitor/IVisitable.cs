namespace GoodVillageGames.Core.Interfaces
{
    /// <summary>
    /// Represents an object that can be "visited" by a visitor object.
    /// Part of the Visitor pattern.
    /// </summary>
    /// <remarks>
    /// <see cref="IVisitor"/>
    /// </remarks>
    public interface IVisitable
    {
        void Accept(IVisitor visitor);
    }
}