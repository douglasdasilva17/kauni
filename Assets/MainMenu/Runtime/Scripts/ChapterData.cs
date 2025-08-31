using UnityEngine;

namespace Compixel.UI
{
    [CreateAssetMenu(menuName ="Data/Chapter")]
    public class ChapterData : ScriptableObject
    {
        //The numerical representation of the chapter
        public string chapterLabel;

        //The title of the chapter
        public string chapterTitle;

        //The description of the chapter
        [TextArea(4,8)]
        public string chapterDescription;

        public Sprite chapterPortrait;
    }
}
