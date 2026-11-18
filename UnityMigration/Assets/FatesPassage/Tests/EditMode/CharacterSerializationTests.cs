using NUnit.Framework;
using UnityEngine;
using FatesPassage.Core;

namespace FatesPassage.Tests
{
    public class CharacterSerializationTests
    {
        [Test]
        public void CharacterProfile_RoundTripsJson()
        {
            var original = new CharacterProfile
            {
                id = "test-001", name = "Corin", ancestry = "Human",
                specialization = "Naturalist", level = 5, experience = 1600
            };
            var json = JsonUtility.ToJson(original);
            var restored = JsonUtility.FromJson<CharacterProfile>(json);
            Assert.AreEqual(original.id, restored.id);
            Assert.AreEqual(original.name, restored.name);
            Assert.AreEqual(original.specialization, restored.specialization);
            Assert.AreEqual(original.level, restored.level);
            Assert.AreEqual(original.experience, restored.experience);
        }
    }
}
