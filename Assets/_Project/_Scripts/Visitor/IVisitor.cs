using UnityEngine;

namespace GoodVillageGames.Core.Interfaces
{
    /// <summary>
    /// Represents an operation to be performed on elements of an object structure.
    /// Part of the Visitor pattern.
    /// </summary>
    /// <remarks>
    /// This generic implementation is common but differs from the classic GoF pattern,
    /// which uses method overloads for each concrete visitable type (e.g., Visit(Player p)).
    /// The generic approach is flexible but may require runtime type checking inside the
    /// visitor, whereas the classic approach provides compile-time safety.
    /// But worry not. I made sure that my visitor implementations handle types gracefullyyy. ;)
    /// </remarks>
    public interface IVisitor
    {
        void Visit<T>(T visitable) where T : Component, IVisitable;
    }
}