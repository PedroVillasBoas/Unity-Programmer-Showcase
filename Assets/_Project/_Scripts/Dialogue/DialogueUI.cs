using TMPro;
using Febucci.UI;
using UnityEngine;
using TriInspector;
using UnityEngine.UI;

namespace GoodVillageGames.Core.Dialogue
{
    /// <summary>
    /// Manages the visual components of the dialogue box.
    /// Receives commands from the DialogueManager.
    /// It is responsible for interfacing with the Text Animator package.
    /// </summary>
    public class DialogueUI : MonoBehaviour
    {
        [Title("UI References")]
        [Group("Panels")][SerializeField] private GameObject _dialoguePanel;
        [Group("Panels")][SerializeField] private GameObject _npcSpeakerInfoPanel;

        [Group("Speaker Info")][SerializeField] private Image _npcPortraitImage;
        [Group("Speaker Info")][SerializeField] private TextMeshProUGUI _npcSpeakerNameText;

        [Group("Dialogue Text")][SerializeField] private TextAnimator_TMP _textAnimator;
        [Group("Dialogue Text")][SerializeField] private TypewriterByCharacter _typewriter;

        [Group("Continue Icons")][SerializeField] private GameObject _nextIcon;
        [Group("Continue Icons")][SerializeField] private GameObject _endIcon;

        private bool _isCurrentLineTheLast = false;

        public bool IsLineFinishedTyping { get; private set; } = false;

        private void Awake()
        {
            _typewriter.onTextShowed.AddListener(OnLineFinished);
        }

        private void OnDisable()
        {
            _typewriter.onTextShowed.RemoveListener(OnLineFinished);
        }

        public void ShowPanel() => _dialoguePanel.SetActive(true);
        public void HidePanel() => _dialoguePanel.SetActive(false);

        public void DisplayLine(DialogueLine line, bool isLastLine)
        {
            IsLineFinishedTyping = false;
            _isCurrentLineTheLast = isLastLine;

            bool hasSpeaker = line.NpcSpeakerData != null;
            _npcSpeakerInfoPanel.SetActive(hasSpeaker);

            if (hasSpeaker)
            {
                _npcSpeakerNameText.text = line.NpcSpeakerData.NpcName;
                _npcSpeakerNameText.color = line.NpcSpeakerData.NpcNameColor;
                _npcPortraitImage.sprite = line.NpcSpeakerData.NpcPortrait;
            }

            _nextIcon.SetActive(false);
            _endIcon.SetActive(false);

            _typewriter.StopShowingText();
            _typewriter.ShowText(line.LineText);
        }

        public void SpeedUpTextAnimation()
        {
            _typewriter.SkipTypewriter();
        }

        private void OnLineFinished()
        {
            IsLineFinishedTyping = true;
            _nextIcon.SetActive(!_isCurrentLineTheLast);
            _endIcon.SetActive(_isCurrentLineTheLast);
        }
    }
}