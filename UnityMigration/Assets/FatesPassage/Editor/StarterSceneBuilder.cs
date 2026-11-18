using System.IO;
using FatesPassage.Core;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace FatesPassage.Editor
{
    public static class StarterSceneBuilder
    {
        [MenuItem("Fate's Passage/Create Starter Scene")]
        public static void Create()
        {
            const string folder = "Assets/FatesPassage/Scenes";
            Directory.CreateDirectory(folder);
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            var root = new GameObject("Fate's Passage - Bootstrap");
            root.AddComponent<GameBootstrap>();
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Prototype Ground";
            ground.transform.localScale = new Vector3(5, 1, 5);
            var marker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            marker.name = "Character Placeholder";
            marker.transform.position = new Vector3(0, 1, 0);
            EditorSceneManager.SaveScene(scene, folder + "/Bootstrap.unity");
            AssetDatabase.Refresh();
            Debug.Log("Created starter scene. This is a prototype, not migrated gameplay.");
        }
    }
}
