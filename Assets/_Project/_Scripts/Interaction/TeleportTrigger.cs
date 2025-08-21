using UnityEngine;
using TriInspector;

namespace GoodVillageGames.Player.Trigger
{
    /// <summary>
    /// A reusable trigger that teleports any object with the "Player" tag to a specified destination transform.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class TeleportTrigger : MonoBehaviour
    {
        [Title("Destination")]
        [SerializeField] private Transform teleportDestination;

        private void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                if (teleportDestination != null)
                {
                    // Move the player to the destination's position
                    other.transform.position = teleportDestination.position;
                }
                else
                {
                    Debug.LogError("TeleportTrigger: Destination is not assigned!", this);
                }
            }
        }
    }
}
