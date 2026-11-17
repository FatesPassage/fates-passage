using UnityEngine;
using UnityEngine.SceneManagement;

namespace FatesPassage.Core
{
    /// <summary>Bootstraps the migrated game without depending on browser APIs.</summary>
    public sealed class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private string firstScene = "CharacterCreation";

        public void StartNewGame()
        {
            if (Application.CanStreamedLevelBeLoaded(firstScene))
                SceneManager.LoadScene(firstScene);
            else
                Debug.LogError($"Fate's Passage: scene '{firstScene}' is missing from Build Settings.");
        }
    }
}
