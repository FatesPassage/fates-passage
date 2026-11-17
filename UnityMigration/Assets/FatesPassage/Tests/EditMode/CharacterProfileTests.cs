using NUnit.Framework;
using FatesPassage.Core;

namespace FatesPassage.Tests
{
    public class CharacterProfileTests
    {
        [Test]
        public void Validate_ClampsInvalidProgression()
        {
            var character = new CharacterProfile { name = "  Corin  ", level = -5, experience = -1 };
            character.Validate();
            Assert.AreEqual("Corin", character.name);
            Assert.AreEqual(1, character.level);
            Assert.AreEqual(0L, character.experience);
        }
    }
}
