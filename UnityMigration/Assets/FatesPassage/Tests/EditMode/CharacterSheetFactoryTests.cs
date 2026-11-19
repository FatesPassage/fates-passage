using NUnit.Framework;
using FatesPassage.Core;

namespace FatesPassage.Tests
{
    public sealed class CharacterSheetFactoryTests
    {
        [Test]
        public void CorinStarterMatchesRecoveredSourceSheet()
        {
            var sheet = CharacterSheetFactory.CreateCorinStarter();
            Assert.AreEqual("Corin Thistledown", sheet.characterName);
            Assert.AreEqual("Naturalist", sheet.specialization);
            Assert.AreEqual("Verdantshade, Shadarill", sheet.zone);
            Assert.AreEqual(105, sheet.maxHealth);
            Assert.AreEqual(95, sheet.maxMana);
            Assert.AreEqual(95, sheet.maxEnergy);
            Assert.AreEqual(4, sheet.attributes.Count);
            Assert.AreEqual(10, sheet.resistances.Count);
            Assert.AreEqual("Root Snare", sheet.activeSkills[0].name);
            Assert.AreEqual("Gnarled Oak Staff", sheet.equipment[2].name);
        }

        [Test]
        public void SheetValidationRejectsResourcesAboveMaximum()
        {
            var sheet = CharacterSheetFactory.CreateCorinStarter();
            sheet.currentHealth = sheet.maxHealth + 1;
            Assert.Throws<System.InvalidOperationException>(() => sheet.Validate());
        }
    }
}
