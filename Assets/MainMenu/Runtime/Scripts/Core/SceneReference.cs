using UnityEngine;
using UnityEngine.SceneManagement;

namespace Compixel
{
    [CreateAssetMenu(fileName = "Scene Reference", menuName = "Compixel/Scene Reference")]
    public class SceneReference : ScriptableObject
    {
        public string Name;

        public bool IsLoaded()
        {
            for (int i = 0; i < SceneManager.sceneCount; ++i)
            {
                if (SceneManager.GetSceneAt(i).name == Name)
                    return true;
            }

            return false;
        }
    }
}