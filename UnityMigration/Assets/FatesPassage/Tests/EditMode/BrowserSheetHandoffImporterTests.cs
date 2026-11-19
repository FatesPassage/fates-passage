using NUnit.Framework;
using FatesPassage.Core;

namespace FatesPassage.Tests
{
    public sealed class BrowserSheetHandoffImporterTests
    {
        [Test]
        public void ImportsResourcesAndResonanceWithoutMutatingTemplate()
        {
            var template = CharacterSheetFactory.CreateCorinStarter();
            const string json = "{\"hp\":[80,110],\"mp\":[60,100],\"en\":[70,95],\"res\":12,\"lv2\":0,\"ch\":[null,null,null]}";
            Assert.IsTrue(BrowserSheetHandoffImporter.TryImport(json, template, out var imported, out var error), error);
            Assert.AreEqual(80, imported.currentHealth);
            Assert.AreEqual(110, imported.maxHealth);
            Assert.AreEqual(12, imported.resonance);
            Assert.AreEqual(105, template.maxHealth);
        }

        [Test]
        public void ImportsNonNullSkillChargesAndPreservesNullSlots()
        {
            var template = CharacterSheetFactory.CreateCorinStarter();
            const string json = "{\"hp\":[105,105],\"mp\":[95,95],\"en\":[95,95],\"res\":5,\"lv2\":0,\"ch\":[7,null,0]}";
            Assert.IsTrue(BrowserSheetHandoffImporter.TryImport(json, template, out var imported, out var error), error);
            Assert.AreEqual(7, imported.activeSkills[0].currentCharges);
            Assert.AreEqual(20, imported.activeSkills[1].currentCharges);
            Assert.AreEqual(0, imported.activeSkills[2].currentCharges);
            Assert.AreEqual(22, template.activeSkills[0].currentCharges);
        }

        [Test]
        public void FindsNullableChargesRegardlessOfPropertyOrderAndQuotedText()
        {
            var template = CharacterSheetFactory.CreateCorinStarter();
            const string json = "{\\"note\\":\\"a quoted \\\\"value\\\\" in metadata\\",\\"ch\\":[3,null,null],\\"hp\\":[105,105],\\"mp\\":[95,95],\\"en\\":[95,95],\\"res\\":5}";
            Assert.IsTrue(BrowserSheetHandoffImporter.TryImport(json, template, out var imported, out var error), error);
            Assert.AreEqual(3, imported.activeSkills[0].currentCharges);
            Assert.AreEqual(20, imported.activeSkills[1].currentCharges);
        }

        [Test]
        public void RejectsSkillChargeAboveMaximum()
        {
            var template = CharacterSheetFactory.CreateCorinStarter();
            const string json = "{\"hp\":[105,105],\"mp\":[95,95],\"en\":[95,95],\"res\":5,\"ch\":[23,null,null]}";
            Assert.IsFalse(BrowserSheetHandoffImporter.TryImport(json, template, out _, out _));
        }

        [Test]
        public void LevelTwoHandoffAppliesSourceSheetBonuses()
        {
            var template = CharacterSheetFactory.CreateCorinStarter();
            const string json = "{\"hp\":[105,105],\"mp\":[95,95],\"en\":[95,95],\"res\":5,\"lv2\":1}";
            Assert.IsTrue(BrowserSheetHandoffImporter.TryImport(json, template, out var imported, out var error), error);
            Assert.AreEqual(2, imported.level);
            Assert.AreEqual(13, imported.attributes.Find(a => a.id == "toughness").value);
            Assert.AreEqual(17, imported.attributes.Find(a => a.id == "intellect").value);
            Assert.IsTrue(imported.equipment.Exists(g => g.name == "Moonhowl Helm"));
        }

        [Test]
        public void RejectsMalformedResourcePairs()
        {
            var template = CharacterSheetFactory.CreateCorinStarter();
            const string json = "{\"hp\":[120,100],\"mp\":[95,95],\"en\":[95,95],\"res\":5}";
            Assert.IsFalse(BrowserSheetHandoffImporter.TryImport(json, template, out _, out _));
        }
    }
}
