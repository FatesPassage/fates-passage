using System;
using UnityEngine;

namespace FatesPassage.CharacterCreation
{
    [Serializable] public sealed class CreationPath
    {
        public string id, icon, name, color, resource, armor, roles;
        public int[] stats;
    }
    [Serializable] public sealed class CreationSign
    {
        public string id, icon, name, flavor;
        public string[] bonuses;
    }
    [Serializable] public sealed class CreationCatalog
    {
        public string source, sourceBlob;
        public CreationPath[] specializations;
        public CreationSign[] signs;
    }
    [Serializable] public sealed class CreationSelection
    {
        public string pathId, characterName, gender, signId;
        public int hair, facial, eyes, skin, body = 1;
        public string companionId;
    }

    /// <summary>Source-backed character creation foundation; appearance rendering and companion catalog still pending.</summary>
    public sealed class CharacterCreationPrototype : MonoBehaviour
    {
        private static readonly string[] Stages = { "PATH", "IDENTITY", "STARS", "LIKENESS", "PORTRAIT" };
        private readonly CreationSelection selection = new CreationSelection();
        private CreationCatalog catalog;
        private int step;
        private Vector2 scroll;
        private GUIStyle heading, detail, button;
        private string feedback = "";
        private bool starsSealed;
        private bool finished;

        private void Awake()
        {
            var asset = Resources.Load<TextAsset>("FatesPassageCharacterCreation");
            if (asset != null) catalog = JsonUtility.FromJson<CreationCatalog>(asset.text);
            if (catalog == null || catalog.specializations == null || catalog.signs == null)
                feedback = "Original character creation catalog is missing. Check Assets/Resources.";
        }

        private void Styles()
        {
            if (heading != null) return;
            heading = new GUIStyle(GUI.skin.label) { fontSize = 25, fontStyle = FontStyle.Bold, wordWrap = true };
            heading.normal.textColor = new Color(0.96f, 0.85f, 0.58f);
            detail = new GUIStyle(GUI.skin.label) { fontSize = 15, wordWrap = true };
            detail.normal.textColor = new Color(0.86f, 0.89f, 0.91f);
            button = new GUIStyle(GUI.skin.button) { fontSize = 16, wordWrap = true };
        }

        private CreationPath Path()
        {
            if (catalog == null || catalog.specializations == null) return null;
            foreach (var p in catalog.specializations) if (p.id == selection.pathId) return p;
            return null;
        }

        private CreationSign Sign()
        {
            if (catalog == null || catalog.signs == null) return null;
            foreach (var s in catalog.signs) if (s.id == selection.signId) return s;
            return null;
        }

        private bool CanContinue()
        {
            if (step == 0) return Path() != null;
            if (step == 1)
            {
                string n = (selection.characterName ?? "").Trim();
                if (n.Length < 2 || n.Length > 18 || string.IsNullOrEmpty(selection.gender)) return false;
                foreach (char c in n)
                    if (!char.IsLetter(c) && !char.IsWhiteSpace(c) && c != '-' && c != '\'' && c != '’') return false;
            }
            if (step == 2) return starsSealed && Sign() != null;
            return true;
        }

        private void OnGUI()
        {
            Styles();
            var safe = Screen.safeArea;
            float width = Mathf.Min(880f, Mathf.Max(280f, safe.width - 24f));
            float left = safe.xMin + (safe.width - width) * 0.5f;
            float top = Screen.height - safe.yMax + 12f;
            GUILayout.BeginArea(new Rect(left, top, width, Mathf.Max(180f, safe.height - 24f)), GUI.skin.box);
            GUILayout.Label("FATE'S PASSAGE  /  CHARACTER CREATION", heading);
            GUILayout.Label(finished ? "PORTRAIT COMPLETE" : Stages[step] + "  •  " + (step + 1) + " / " + Stages.Length, detail);
            GUILayout.Space(12);
            scroll = GUILayout.BeginScrollView(scroll, GUILayout.ExpandHeight(true));
            if (catalog == null)
                GUILayout.Label(feedback, detail);
            else if (finished)
                GUILayout.Label("Character selection recorded for this session. Game-world handoff is not connected yet.", detail);
            else if (step == 0)
            {
                GUILayout.Label("Choose your path. Each specialization comes from the original HTML.", detail);
                foreach (var p in catalog.specializations)
                {
                    bool selected = p.id == selection.pathId;
                    if (GUILayout.Button((selected ? "● " : "○ ") + p.name + "  —  " + p.roles, button, GUILayout.MinHeight(44)))
                        selection.pathId = p.id;
                }
                var p = Path();
                if (p != null) GUILayout.Label("STR " + p.stats[0] + "  AGI " + p.stats[1] + "  INT " + p.stats[2] + "  TOU " + p.stats[3] + "\n" + p.armor + "  •  " + p.resource, detail);
            }
            else if (step == 1)
            {
                GUILayout.Label("Name your Fatewalker (2–18 characters).", detail);
                selection.characterName = GUILayout.TextField(selection.characterName ?? "", 18, GUILayout.MinHeight(40));
                foreach (var gender in new[] { "Male", "Female", "Other" })
                    if (GUILayout.Button((selection.gender == gender ? "● " : "○ ") + gender, button, GUILayout.MinHeight(40))) selection.gender = gender;
            }
            else if (step == 2)
            {
                GUILayout.Label("Read the stars. Choose one of the original ten signs; sealing locks your choice.", detail);
                foreach (var sign in catalog.signs)
                    if (GUILayout.Button((selection.signId == sign.id ? "● " : "○ ") + sign.name, button, GUILayout.MinHeight(38)) && !starsSealed) selection.signId = sign.id;
                var selected = Sign();
                if (selected != null)
                {
                    GUILayout.Label(selected.flavor, detail);
                    foreach (var blessing in selected.bonuses) GUILayout.Label("• " + blessing, detail);
                    if (!starsSealed && GUILayout.Button("SEAL THIS SIGN", button, GUILayout.MinHeight(42))) starsSealed = true;
                    if (starsSealed) GUILayout.Label("Sign sealed.", detail);
                }
            }
            else if (step == 3)
            {
                GUILayout.Label("Appearance choices are recorded; original visual options and portraits still require source extraction.", detail);
                AppearanceChoice("Hair", ref selection.hair);
                AppearanceChoice("Facial", ref selection.facial);
                AppearanceChoice("Eyes", ref selection.eyes);
                AppearanceChoice("Skin", ref selection.skin);
                AppearanceChoice("Body", ref selection.body);
            }
            else
            {
                GUILayout.Label("Character: " + selection.characterName + "\nPath: " + Path()?.name + "\nSign: " + Sign()?.name + "\nGender: " + selection.gender, detail);
                if (Path()?.id == "naturalist") GUILayout.Label("Naturalist companion selection is not yet ported; original companion choices must be added before final handoff.", detail);
            }
            GUILayout.EndScrollView();
            GUILayout.Space(8);
            GUILayout.BeginHorizontal();
            GUI.enabled = step > 0 && !finished;
            if (GUILayout.Button("BACK", button, GUILayout.Height(44))) { step--; scroll = Vector2.zero; }
            GUI.enabled = !finished && CanContinue();
            if (GUILayout.Button(step == Stages.Length - 1 ? "CONFIRM PORTRAIT" : "CONTINUE", button, GUILayout.Height(44)))
            {
                if (step == Stages.Length - 1) finished = true;
                else { step++; scroll = Vector2.zero; }
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void AppearanceChoice(string name, ref int value)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(name + "  " + (value + 1) + " / 4", detail, GUILayout.Width(150));
            if (GUILayout.Button("◀", button, GUILayout.Width(52))) value = (value + 3) % 4;
            if (GUILayout.Button("▶", button, GUILayout.Width(52))) value = (value + 1) % 4;
            GUILayout.EndHorizontal();
        }
    }
}
