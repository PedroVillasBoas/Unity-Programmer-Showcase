using UnityEngine;
using System.Collections.Generic;
using GoodVillageGames.Core.Itemization;

namespace GoodVillageGames.Core.Dialogue
{
    /// <summary>
    /// A simple data container for a single line of the NPC dialogue.
    /// It holds the NPC name and the text they will say.
    /// </summary>
    [System.Serializable]
    public class DialogueLine
    {
        public NpcSpeakerData NpcSpeakerData;
        [TextArea(5, 10)]
        public string LineText;
    }

    /// <summary>
    /// A ScriptableObject that has everything a NPC wants to say.
    /// It holds a list of DialogueLines that will be displayed in sequence.
    /// </summary>
    [CreateAssetMenu(fileName = "DialogueTree", menuName = "GoodVillageGames/Dialogue/DialogueTree")]
    public class DialogueTree : ScriptableObject
    {
        public List<DialogueLine> Lines;
        [Header("Reward")]
        [Tooltip("The item to give the player when this dialogue is completed. Can be null.")]
        public ItemData itemReward;
    }
}
