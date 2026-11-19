#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using FatesPassage.Core;

namespace FatesPassage.EditorTools
{
    /// <summary>Builds a stylized temporary arena for validating the native gameplay loop.</summary>
    public static class PrototypeSceneBuilder
    {
        private static Material MakeMaterial(string name, Color color, float metallic = 0f, float smoothness = 0.25f)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Standard");
            var material = new Material(shader) { name = name, color = color };
            if (material.HasProperty("_Metallic")) material.SetFloat("_Metallic", metallic);
            if (material.HasProperty("_Smoothness")) material.SetFloat("_Smoothness", smoothness);
            return material;
        }

        private static void EnsureFolder(string parent, string child)
        {
            string path = parent + "/" + child;
            if (!AssetDatabase.IsValidFolder(path)) AssetDatabase.CreateFolder(parent, child);
        }

        private static Material SaveMaterial(string folder, string name, Color color, float metallic = 0f, float smoothness = 0.25f)
        {
            string path = folder + "/" + name + ".mat";
            var existing = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (existing != null) return existing;
            var material = MakeMaterial(name, color, metallic, smoothness);
            AssetDatabase.CreateAsset(material, path);
            return material;
        }

        private static GameObject Primitive(PrimitiveType type, string name, Vector3 position,
            Vector3 scale, Material material, Transform parent = null)
        {
            var item = GameObject.CreatePrimitive(type);
            item.name = name;
            item.transform.position = position;
            item.transform.localScale = scale;
            if (parent != null) item.transform.SetParent(parent, true);
            item.GetComponent<Renderer>().sharedMaterial = material;
            return item;
        }

        [MenuItem("Fate's Passage/Build Combat Test Scene")]
        public static void Build()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            const string materialFolder = "Assets/FatesPassage/PrototypeMaterials";
            EnsureFolder("Assets", "FatesPassage");
            EnsureFolder("Assets/FatesPassage", "PrototypeMaterials");

            var stone = SaveMaterial(materialFolder, "Arena_ObsidianStone", new Color(0.105f, 0.13f, 0.18f), 0.22f);
            var trim = SaveMaterial(materialFolder, "Arena_AgedBronze", new Color(0.43f, 0.25f, 0.105f), 0.72f, 0.62f);
            var playerMat = SaveMaterial(materialFolder, "Character_PlayerJade", new Color(0.12f, 0.43f, 0.34f), 0.18f, 0.48f);
            var enemyMat = SaveMaterial(materialFolder, "Character_TrainingEmber", new Color(0.62f, 0.19f, 0.12f), 0.25f, 0.42f);
            var glow = SaveMaterial(materialFolder, "Arena_RuneGlow", new Color(0.08f, 0.48f, 0.62f), 0.15f, 0.7f);

            var environment = new GameObject("Environment and Architecture");
            Primitive(PrimitiveType.Cube, "Arena Foundation", new Vector3(0, -0.35f, 0), new Vector3(24, 0.7f, 24), stone, environment.transform);
            Primitive(PrimitiveType.Plane, "Arena Combat Surface", new Vector3(0, 0.02f, 0), new Vector3(2.25f, 1, 2.25f), stone, environment.transform);

            // Raised perimeter and architectural rhythm make the combat space readable.
            Primitive(PrimitiveType.Cube, "North Boundary", new Vector3(0, 1.2f, 11.2f), new Vector3(23, 2.4f, 0.55f), stone, environment.transform);
            Primitive(PrimitiveType.Cube, "South Boundary", new Vector3(0, 1.2f, -11.2f), new Vector3(23, 2.4f, 0.55f), stone, environment.transform);
            Primitive(PrimitiveType.Cube, "East Boundary", new Vector3(11.2f, 1.2f, 0), new Vector3(0.55f, 2.4f, 23), stone, environment.transform);
            Primitive(PrimitiveType.Cube, "West Boundary", new Vector3(-11.2f, 1.2f, 0), new Vector3(0.55f, 2.4f, 23), stone, environment.transform);

            for (int i = -1; i <= 1; i += 2)
            for (int j = -1; j <= 1; j += 2)
            {
                Vector3 pos = new Vector3(i * 8.8f, 2.1f, j * 8.8f);
                Primitive(PrimitiveType.Cylinder, "Bronze Pillar", pos, new Vector3(1.0f, 2.1f, 1.0f), trim, environment.transform);
                Primitive(PrimitiveType.Cylinder, "Pillar Rune Light", pos + Vector3.up * 1.75f,
                    new Vector3(1.04f, 0.12f, 1.04f), glow, environment.transform);
            }

            for (int i = -2; i <= 2; i++)
            {
                Primitive(PrimitiveType.Cube, "Inlaid Floor Rune", new Vector3(i * 2.2f, 0.045f, 0),
                    new Vector3(0.07f, 0.025f, 10.5f), trim, environment.transform);
                Primitive(PrimitiveType.Cube, "Inlaid Cross Rune", new Vector3(0, 0.047f, i * 2.2f),
                    new Vector3(10.5f, 0.025f, 0.07f), trim, environment.transform);
            }

            var lightObject = new GameObject("Warm Key Light");
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.35f;
            light.color = new Color(1f, 0.84f, 0.68f);
            light.shadows = LightShadows.Soft;
            lightObject.transform.rotation = Quaternion.Euler(48, -32, 0);

            var fillObject = new GameObject("Cool Ambient Fill");
            var fill = fillObject.AddComponent<Light>();
            fill.type = LightType.Point;
            fill.range = 18;
            fill.intensity = 2.2f;
            fill.color = new Color(0.12f, 0.55f, 0.78f);
            fillObject.transform.position = new Vector3(-5, 5, 4);

            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.22f, 0.25f, 0.32f);
            RenderSettings.fog = true;
            RenderSettings.fogColor = new Color(0.20f, 0.23f, 0.29f);
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = 0.012f;

            var player = Primitive(PrimitiveType.Capsule, "Player Prototype", new Vector3(0, 1.1f, -1.5f),
                new Vector3(0.85f, 1.1f, 0.85f), playerMat);
            Object.DestroyImmediate(player.GetComponent<CapsuleCollider>());
            player.AddComponent<CharacterController>();
            player.AddComponent<CharacterVitalsBehaviour>();
            player.AddComponent<CombatFeedback>();
            player.AddComponent<Combatant>();
            player.AddComponent<MovementMotor>();
            player.AddComponent<PlayerInputAdapter>();
            var attack = player.AddComponent<PrototypeAttackController>();

            var enemy = Primitive(PrimitiveType.Capsule, "Training Dummy", new Vector3(0, 1.1f, 2.5f),
                new Vector3(0.95f, 1.15f, 0.95f), enemyMat);
            enemy.AddComponent<CharacterVitalsBehaviour>();
            enemy.AddComponent<CombatFeedback>();
            var enemyCombatant = enemy.AddComponent<Combatant>();
            attack.SetTarget(enemyCombatant);

            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            var camera = cameraObject.AddComponent<Camera>();
            camera.fieldOfView = 58;
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

            EnsureFolder("Assets", "Scenes");
            AssetDatabase.SaveAssets();
            EditorSceneManager.SaveScene(scene, "Assets/Scenes/CombatPrototype.unity");
            Debug.Log("Fate's Passage stylized combat prototype generated. WASD move, Space attack, F3 diagnostics.");
        }
    }
}
#endif
