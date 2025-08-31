using UnityEngine;

namespace Compixel.UI
{
    [CreateAssetMenu(menuName = "Data/Loading Item")]
    public class LoadingData : ScriptableObject
    {
        public string loadingTitle;
        [TextArea(4, 8)]
        public string loadingDescription;
        public Sprite loadingPlaceholder;
    }
}
