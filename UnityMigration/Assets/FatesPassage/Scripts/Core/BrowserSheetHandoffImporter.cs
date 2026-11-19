using System;
using System.Collections.Generic;
using System.Globalization;
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
                if (data == null || !ValidPair(data.hp) || !ValidPair(data.mp) ||
                    !ValidPair(data.en) || data.res < 0 || (data.lv2 != 0 && data.lv2 != 1))
                    throw new FormatException("The browser sheet handoff is missing valid resource pairs, resonance, or level flag.");

                var copy = JsonUtility.FromJson<CharacterSheetData>(JsonUtility.ToJson(baseSheet));
                if (copy == null) throw new FormatException("The base character sheet could not be copied.");
                ApplyPair(data.hp, ref copy.currentHealth, ref copy.maxHealth);
                ApplyPair(data.mp, ref copy.currentMana, ref copy.maxMana);
                ApplyPair(data.en, ref copy.currentEnergy, ref copy.maxEnergy);
                copy.resonance = data.res;

                if (data.lv2 == 1)
                {
                    if (copy.attributes == null || copy.equipment == null)
                        throw new FormatException("The base character sheet is missing required collections.");
                    copy.level = 2;
                    copy.experience = 0;
                    AddAttribute(copy, "toughness", 3);
                    AddAttribute(copy, "intellect", 2);
                    if (!HasGear(copy, "Moonhowl Helm"))
                        copy.equipment.Add(new GearEntry("Helm", "⛑️", "Uncommon", "Moonhowl Helm",
                            "Light Helm · Uncommon · +3 Toughness, +2 Intellect"));
                }

                // The source page preserves null entries and only overwrites charges for non-null slots.
                // Parse this one field separately because JsonUtility maps nullable integer entries to zero.
                if (TryReadNullableIntegerArray(json, "ch", out var charges))
                    ApplyCharges(copy, charges);

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

        private static void ApplyCharges(CharacterSheetData sheet, List<int?> charges)
        {
            if (sheet.activeSkills == null) throw new FormatException("The base character sheet has no active skill collection.");
            int count = Math.Min(sheet.activeSkills.Count, charges.Count);
            for (int i = 0; i < count; i++)
            {
                if (!charges[i].HasValue) continue;
                int value = charges[i].Value;
                var skill = sheet.activeSkills[i];
                if (skill == null || value < 0 || value > skill.maximumCharges)
                    throw new FormatException("Skill charge value is outside the valid range at slot " + i + ".");
                skill.currentCharges = value;
            }
        }

        private static bool TryReadNullableIntegerArray(string json, string property, out List<int?> values)
        {
            values = null;
            int depth = 0;
            for (int i = 0; i < json.Length; i++)
            {
                char ch = json[i];
                if (ch == '{') { depth++; continue; }
                if (ch == '}') { depth--; continue; }
                if (depth != 1 || ch != '"') continue;

                int keyStart = ++i;
                while (i < json.Length && json[i] != '"')
                {
                    if (json[i] == '\\\\' && i + 1 < json.Length) i++;
                    i++;
                }
                if (i >= json.Length) throw new FormatException("Handoff JSON contains an unterminated property name.");
                string key = json.Substring(keyStart, i - keyStart);
                i++;
                SkipWhitespace(json, ref i);
                if (i >= json.Length || json[i] != ':') throw new FormatException("Handoff JSON contains an invalid property.");
                i++;
                SkipWhitespace(json, ref i);
                if (key != property)
                {
                    SkipJsonValue(json, ref i);
                    i--;
                    continue;
                }

                if (i >= json.Length || json[i] != '[') throw new FormatException("Skill charge field must be an array.");
                var parsed = new List<int?>();
                i++;
                while (i < json.Length)
                {
                    SkipWhitespace(json, ref i);
                    if (i >= json.Length) throw new FormatException("Skill charge array is incomplete.");
                    if (json[i] == ']') { values = parsed; return true; }

                    int tokenStart = i;
                    while (i < json.Length && json[i] != ',' && json[i] != ']' && !char.IsWhiteSpace(json[i])) i++;
                    string token = json.Substring(tokenStart, i - tokenStart);
                    if (token == "null") parsed.Add(null);
                    else if (int.TryParse(token, NumberStyles.None, CultureInfo.InvariantCulture, out int value) && value >= 0)
                        parsed.Add(value);
                    else throw new FormatException("Skill charges must contain only nonnegative integers or null.");

                    SkipWhitespace(json, ref i);
                    if (i < json.Length && json[i] == ',') { i++; continue; }
                    if (i < json.Length && json[i] == ']') { values = parsed; return true; }
                    throw new FormatException("Skill charge array has invalid separators.");
                }
                throw new FormatException("Skill charge array is incomplete.");
            }
            return false;
        }

        private static void SkipJsonValue(string json, ref int index)
        {
            if (index >= json.Length) throw new FormatException("Handoff JSON ends unexpectedly.");
            char first = json[index];
            if (first == '"' )
            {
                index++;
                while (index < json.Length)
                {
                    if (json[index] == '\\\\') { index += 2; continue; }
                    if (json[index++] == '"') return;
                }
                throw new FormatException("Handoff JSON contains an unterminated string.");
            }

            if (first == '{' || first == '[')
            {
                int nesting = 0;
                bool inString = false;
                for (; index < json.Length; index++)
                {
                    char ch = json[index];
                    if (inString)
                    {
                        if (ch == '\\\\') { index++; continue; }
                        if (ch == '"') inString = false;
                        continue;
                    }
                    if (ch == '"') inString = true;
                    else if (ch == '{' || ch == '[') nesting++;
                    else if (ch == '}' || ch == ']')
                    {
                        nesting--;
                        if (nesting == 0) { index++; return; }
                    }
                }
                throw new FormatException("Handoff JSON contains an incomplete object or array.");
            }

            while (index < json.Length && json[index] != ',' && json[index] != '}') index++;
        }

        private static void SkipWhitespace(string value, ref int index)
        {
            while (index < value.Length && char.IsWhiteSpace(value[index])) index++;
        }
    }
}
