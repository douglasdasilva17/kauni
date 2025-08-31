using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Compixel.UI
{
    public class ChapterDataLoader : MonoBehaviour
    {
        public ChapterData data;

        public TextMeshProUGUI chapterLabelText;
        public TextMeshProUGUI chapterTitleText;
        public TextMeshProUGUI chapterDescriptionText;
        public Image placeholder;

        public bool loadChapter;
        public bool loadPrompt;

        private void Start()
        {
            LoadData();
        }

        private void LoadData()
        {
            if (loadChapter)
            {
                chapterLabelText.text = data.chapterLabel;
                chapterTitleText.text = data.chapterTitle;
                chapterDescriptionText.text = data.chapterDescription;
                placeholder.sprite = data.chapterPortrait;
            }

            if (loadPrompt)
            {
                chapterDescriptionText.text = "ARE YOU SURE YOU WANT TO PLAY \"" + data.chapterTitle + "\"?\r\nTHIS WILL OVERWRITE YOUR AUTOSAVE FILE.";
            }
        }
    }
}
