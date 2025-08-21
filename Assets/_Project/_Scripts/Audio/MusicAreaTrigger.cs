using UnityEngine;

namespace GoodVillageGames.Core.Audio
{
    /// <summary>
    /// A simple trigger that tells the MusicManager which area the player has entered.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class MusicAreaTrigger : MonoBehaviour
    {
        [SerializeField] private MusicArea area;

        private void Awake()
        {
            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) MusicManager.Instance.SetArea(area);
        }
    }
}