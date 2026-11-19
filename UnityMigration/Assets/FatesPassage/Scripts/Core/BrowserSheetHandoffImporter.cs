using System;
using UnityEngine;

namespace FatesPassage.Core
{
    /// <summary>Imports the exact fatesPassage.sheet.v1 handoff written by the browser game.</summary>
    public static class BrowserSheetHandoffImporter
    {
        [Serializable]
        private sealed class Handoff
        {
            public int[] hp, mp, en;
            public int res;
            public int lv2;
        }

        public static bool TryImport(string json, CharacterSheetData baseSheet,
            out CharacterSheetData imported, out string error)
        {
            imported = null;
            error = null;
            if (string.IsNullOrWhiteSpace(json) || baseSheet == null)
            {
                error = "A handoff payload and base character sheet are required.";
                return false;
            }

            try
            {
                var data = JsonUtility.FromJson<Handoff>(json);
                if (data == null || !ValidPair(data.hp) || !ValidPair(data.mp) || !ValidPair(data.en) || data.res < 0)
                    throw new FormatException("The browser sheet handoff is missing valid resource pairs or resonance.");

                var copy = JsonUtility.FromJson<CharacterSheetData>(JsonUtility.ToJson(baseSheet));
                ApplyPair(data.hp, ref copy.currentHealth, ref copy.maxHealth);
                ApplyPair(data.mp, ref copy.currentMana, ref copy.maxMana);
                ApplyPair(data.en, ref copy.currentEnergy, ref copy.maxEnergy);
                copy.resonance = data.res;

                if (data.lv2 != 0)
                {
                    copy.level = 2;
                    copy.experience = 0;
                    AddAttribute(copy, "toughness", 3);
                    AddAttribute(copy, "intellect", 2);
                    if (!HasGear(copy, "Moonhowl Helm"))
                        copy.equipment.Add(new GearEntry("Helm", "⛑️", "Uncommon", "Moonhowl Helm",
                            "Light Helm · Uncommon · +3 Toughness, +2 Intellect"));
                }

                copy.Validate();
                imported = copy;
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        private static bool ValidPair(int[] pair) =>
            pair != null && pair.Length == 2 && pair[0] >= 0 && pair[1] >= pair[0];

        private static void ApplyPair(int[] pair, ref int current, ref int maximum)
        {
            current = pair[0];
            maximum = pair[1];
        }

        private static void AddAttribute(CharacterSheetData sheet, string id, int amount)
        {
            var value = sheet.attributes.Find(a => a.id == id);
            if (value == null) throw new FormatException("Required source attribute not found: " + id);
            value.value += amount;
            value.maximum += amount;
        }

        private static bool HasGear(CharacterSheetData sheet, string name) =>
            sheet.equipment.Exists(g => g.name == name);
    }
}
