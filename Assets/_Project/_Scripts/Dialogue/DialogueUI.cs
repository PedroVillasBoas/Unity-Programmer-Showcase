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

        private bool _isLineFinishedTyping = false;
        private bool _isCurrentLineTheLast = false;

        private void Awake()
        {
            _typewriter.onTextShowed.AddListener(OnLineFinished);
        }

        private void OnDisable()
        {
            _typewriter.onTextShowed.RemoveListener(OnLineFinished);
        }

        private void Update()
        {
            // Advancing dialogue
            if (Input.GetMouseButtonDown(0) && _dialoguePanel.activeSelf)
            {
                if (_isLineFinishedTyping)
                {
                    DialogueManager.Instance.DisplayNextLine();
                }
                else
                {
                    DialogueManager.Instance.SpeedUpText();
                }
            }
        }

        public void ShowPanel() => _dialoguePanel.SetActive(true);
        public void HidePanel() => _dialoguePanel.SetActive(false);

        public void DisplayLine(DialogueLine line, bool isLastLine)
        {
            _isLineFinishedTyping = false;
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

            _typewriter.ShowText(line.LineText);
        }

        public void SpeedUpTextAnimation()
        {
            _typewriter.SkipTypewriter();
        }

        private void OnLineFinished()
        {
            _isLineFinishedTyping = true;

            _nextIcon.SetActive(!_isCurrentLineTheLast);
            _endIcon.SetActive(_isCurrentLineTheLast);
        }
    }
}