using UnityEngine;
using TriInspector;
using GoodVillageGames.Core.Interfaces;

namespace GoodVillageGames.Core.Dialogue
{
    /// <summary>
    /// A reusable trigger that automatically calls the Interact() method on a
    /// target IInteractable component when the player enters its zone.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class AutoInteractTrigger : MonoBehaviour
    {
        [Title("Target")]
        [SerializeField] private MonoBehaviour targetInteractable;

        [Title("Trigger Settings")]
        [SerializeField] private bool triggerOnce = true;

        private IInteractable _interactable;
        private bool _hasBeenTriggered = false;

        private void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
            _interactable = targetInteractable as IInteractable;
            if (_interactable == null)
            {
                Debug.LogError("AutoInteractTrigger: The assigned target does not implement the IInteractable interface!", this);
                enabled = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (triggerOnce && _hasBeenTriggered) return;

            if (other.CompareTag("Player"))
            {
                _hasBeenTriggered = true;
                _interactable.Interact();
            }
        }
    }
}