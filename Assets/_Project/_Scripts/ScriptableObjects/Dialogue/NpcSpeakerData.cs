using UnityEngine;

namespace GoodVillageGames.Core.Dialogue
{
    /// <summary>
    /// A ScriptableObject that holds all the necessary data for a character
    /// who can speak in the dialogue system.
    /// </summary>
    [CreateAssetMenu(fileName = "NpcSpeakerData", menuName = "GoodVillageGames/Dialogue/NPC Speaker Data")]
    public class NpcSpeakerData : ScriptableObject
    {
        public string NpcName;
        public Sprite NpcPortrait;
        public Color NpcNameColor = Color.white;
    }
}