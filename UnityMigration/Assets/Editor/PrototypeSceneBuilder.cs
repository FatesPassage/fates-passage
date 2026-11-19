#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using FatesPassage.Core;

namespace FatesPassage.EditorTools
{
    public static class PrototypeSceneBuilder
    {
        [MenuItem("Fate's Passage/Build Combat Test Scene")]
        public static void Build()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var lightObject = new GameObject("Directional Light");
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.2f;
            lightObject.transform.rotation = Quaternion.Euler(45, -35, 0);

            var floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "Test Arena";
            floor.transform.localScale = new Vector3(3, 1, 3);

            var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player Prototype";
            player.transform.position = new Vector3(0, 1.1f, 0);
            Object.DestroyImmediate(player.GetComponent<CapsuleCollider>());
            player.AddComponent<CharacterController>();
            player.AddComponent<CharacterVitalsBehaviour>();
            player.AddComponent<CombatFeedback>();
            player.AddComponent<Combatant>();
            player.AddComponent<MovementMotor>();
            player.AddComponent<PlayerInputAdapter>();
            var attack = player.AddComponent<PrototypeAttackController>();

            var enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = "Training Dummy";
            enemy.transform.position = new Vector3(0, 1.1f, 2.5f);
            enemy.AddComponent<CharacterVitalsBehaviour>();
            enemy.AddComponent<CombatFeedback>();
            var enemyCombatant = enemy.AddComponent<Combatant>();
            attack.SetTarget(enemyCombatant);

            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            var camera = cameraObject.AddComponent<Camera>();
            cameraObject.transform.position = new Vector3(0, 6, -9);
            cameraObject.transform.rotation = Quaternion.Euler(25, 0, 0);
            camera.clearFlags = CameraClearFlags.Skybox;
            cameraObject.AddComponent<PrototypeCameraFollow>().SetTarget(player.transform);

            var hud = new GameObject("Diagnostics and Vitals");
            hud.AddComponent<PerformanceHud>();
            hud.AddComponent<PrototypeCombatStatus>().SetEnemy(enemy.GetComponent<CharacterVitalsBehaviour>());
            var vitalsHud = hud.AddComponent<CharacterVitalsHud>();
            var so = new SerializedObject(vitalsHud);
            so.FindProperty("character").objectReferenceValue = player.GetComponent<CharacterVitalsBehaviour>();
            so.ApplyModifiedPropertiesWithoutUndo();

            const string folder = "Assets/Scenes";
            if (!AssetDatabase.IsValidFolder(folder)) AssetDatabase.CreateFolder("Assets", "Scenes");
            EditorSceneManager.SaveScene(scene, folder + "/CombatPrototype.unity");
            Debug.Log("Fate's Passage combat prototype created. Enable legacy Input Manager or Both; WASD to move, Space to attack, F3 diagnostics.");
        }
    }
}
#endif
