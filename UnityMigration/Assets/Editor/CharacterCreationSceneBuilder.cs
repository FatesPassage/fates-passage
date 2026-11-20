#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using FatesPassage.CharacterCreation;

namespace FatesPassage.EditorTools
{
    public static class CharacterCreationSceneBuilder
    {
        [MenuItem("Fate's Passage/Build Character Creation Scene")]
        public static void Build()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var root = new GameObject("Fate's Passage - Character Creation");
            root.AddComponent<CharacterCreationPrototype>();

            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.025f, 0.045f, 0.05f);
            cameraObject.transform.position = new Vector3(0, 1, -10);

            if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
                AssetDatabase.CreateFolder("Assets", "Scenes");
            EditorSceneManager.SaveScene(scene, "Assets/Scenes/CharacterCreation.unity");
            AssetDatabase.SaveAssets();
            Debug.Log("Fate's Passage character creation scene generated. Open Assets/Scenes/CharacterCreation.unity and press Play.");
        }
    }
}
#endif
