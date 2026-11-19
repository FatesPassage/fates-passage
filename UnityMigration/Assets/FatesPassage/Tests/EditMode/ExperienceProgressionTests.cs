using NUnit.Framework;
using FatesPassage.Core;

namespace FatesPassage.Tests
{
    public class ExperienceProgressionTests
    {
        [Test]
        public void Thresholds_DoublePerLevel()
        {
            Assert.AreEqual(100, ExperienceProgression.RequiredForNextLevel(1));
            Assert.AreEqual(200, ExperienceProgression.RequiredForNextLevel(2));
            Assert.AreEqual(400, ExperienceProgression.RequiredForNextLevel(3));
        }

        [Test]
        public void Experience_CarriesRemainder()
        {
            var profile = new CharacterProfile();
            Assert.AreEqual(2, ExperienceProgression.AddExperience(profile, 350));
            Assert.AreEqual(3, profile.level);
            Assert.AreEqual(50, profile.experience);
        }
    }
}
