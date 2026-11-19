namespace FatesPassage.Core
{
    /// <summary>Source-backed starter sheet recovered from the original Corin character sheet.</summary>
    public static class CharacterSheetFactory
    {
        public static CharacterSheetData CreateCorinStarter()
        {
            var sheet = new CharacterSheetData
            {
                characterName = "Corin Thistledown",
                level = 1,
                gender = "Other",
                sign = "🌲 Woodsman",
                zone = "Verdantshade, Shadarill",
                specialization = "Naturalist",
                currentHealth = 105, maxHealth = 105,
                currentMana = 95, maxMana = 95,
                currentEnergy = 95, maxEnergy = 95,
                experience = 0,
                resonance = 5,
                unspentAttributePoints = 0
            };

            sheet.attributes.Add(new AttributeValue("strength", "Strength", 10, 10));
            sheet.attributes.Add(new AttributeValue("agility", "Agility", 15, 15));
            sheet.attributes.Add(new AttributeValue("intellect", "Intellect", 15, 15));
            sheet.attributes.Add(new AttributeValue("toughness", "Toughness", 10, 10));

            string[] resistanceIds = { "fire", "frost", "shadow", "light", "electric", "poison", "nature", "melee", "ranged", "environmental" };
            string[] resistanceNames = { "Fire", "Frost", "Shadow", "Light", "Electric", "Poison", "Nature", "Melee", "Ranged", "Environmental" };
            for (int i = 0; i < resistanceIds.Length; i++)
                sheet.resistances.Add(new ResistanceValue(resistanceIds[i], resistanceNames[i], 10, 0));

            sheet.activeSkills.Add(new SkillEntry("🕸️", "Root Snare", "Starter",
                "Roots one enemy 5s; 8% INT Nature damage each second rooted.", "6% Mana", 22, 22));
            sheet.activeSkills.Add(new SkillEntry("🍃", "Nature’s Embrace", "Starter",
                "Heals self or ally 13% of target max HP, plus 0.9% max HP/s for 6s.", "8% Mana", 20, 20));
            sheet.activeSkills.Add(new SkillEntry("🦊", "Forest Companion", "Summon",
                "Summons your fox; it auto-attacks for 70% of your damage.", "No cost", 0, 0));

            sheet.equipment.Add(new GearEntry("Chest", "🥼", "Normal", "Verdant Tunic", "Light Chest · Normal"));
            sheet.equipment.Add(new GearEntry("Legs", "👖", "Normal", "Verdant Trousers", "Light Legs · Normal"));
            sheet.equipment.Add(new GearEntry("Weapon 1 (active)", "🪄", "Normal", "Gnarled Oak Staff", "Staff (2H) · Normal"));
            sheet.Validate();
            return sheet;
        }
    }
}
