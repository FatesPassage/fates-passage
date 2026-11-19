using System;
using System.Collections.Generic;

namespace FatesPassage.Core
{
    [Serializable]
    public sealed class CharacterSheetData
    {
        public int schemaVersion = 1;
        public string characterName = "";
        public int level = 1;
        public string gender = "";
        public string sign = "";
        public string zone = "";
        public string specialization = "";
        public int currentHealth, maxHealth, currentMana, maxMana, currentEnergy, maxEnergy;
        public int experience, resonance, unspentAttributePoints;
        public List<AttributeValue> attributes = new List<AttributeValue>();
        public List<ResistanceValue> resistances = new List<ResistanceValue>();
        public List<SkillEntry> activeSkills = new List<SkillEntry>();
        public List<GearEntry> equipment = new List<GearEntry>();

        public void Validate()
        {
            if (schemaVersion != 1) throw new InvalidOperationException("Unsupported character sheet schema.");
            if (string.IsNullOrWhiteSpace(characterName)) throw new InvalidOperationException("Character name is required.");
            if (level < 1 || experience < 0 || resonance < 0 || unspentAttributePoints < 0)
                throw new InvalidOperationException("Character progression values are invalid.");
            if (maxHealth < 0 || maxMana < 0 || maxEnergy < 0 ||
                currentHealth < 0 || currentHealth > maxHealth ||
                currentMana < 0 || currentMana > maxMana ||
                currentEnergy < 0 || currentEnergy > maxEnergy)
                throw new InvalidOperationException("Character resource values are outside their valid ranges.");
            if (attributes == null || resistances == null || activeSkills == null || equipment == null)
                throw new InvalidOperationException("Character sheet collections cannot be null.");
        }
    }

    [Serializable] public sealed class AttributeValue
    {
        public string id, label;
        public int value, maximum;
        public AttributeValue(string id, string label, int value, int maximum)
        { this.id = id; this.label = label; this.value = value; this.maximum = maximum; }
    }

    [Serializable] public sealed class ResistanceValue
    {
        public string id, label;
        public int value, bonus;
        public ResistanceValue(string id, string label, int value, int bonus)
        { this.id = id; this.label = label; this.value = value; this.bonus = bonus; }
    }

    [Serializable] public sealed class SkillEntry
    {
        public string icon, name, category, description, cost;
        public int currentCharges, maximumCharges;
        public SkillEntry(string icon, string name, string category, string description,
            string cost, int currentCharges, int maximumCharges)
        {
            this.icon = icon; this.name = name; this.category = category; this.description = description;
            this.cost = cost; this.currentCharges = currentCharges; this.maximumCharges = maximumCharges;
        }
    }

    [Serializable] public sealed class GearEntry
    {
        public string slot, icon, rarity, name, details;
        public GearEntry(string slot, string icon, string rarity, string name, string details)
        { this.slot = slot; this.icon = icon; this.rarity = rarity; this.name = name; this.details = details; }
    }
}
