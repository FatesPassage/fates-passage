using System;
using NUnit.Framework;
using FatesPassage.Core;

namespace FatesPassage.Tests
{
    public class CharacterCreationTests
    {
        [Test]
        public void Create_AssignsStartingValues()
        {
            var profile = CharacterCreationService.Create(" Corin ", "Human", "Naturalist");
            Assert.AreEqual("Corin", profile.name);
            Assert.AreEqual(1, profile.level);
            Assert.AreEqual(0, profile.experience);
            Assert.IsNotEmpty(profile.id);
        }

        [Test]
        public void Create_RejectsEmptyName()
        {
            Assert.Throws<ArgumentException>(() => CharacterCreationService.Create(" ", "Human", "Naturalist"));
        }
    }
}
