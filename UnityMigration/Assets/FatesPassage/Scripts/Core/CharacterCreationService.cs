using System;
namespace FatesPassage.Core
{
    public static class CharacterCreationService
    {
        public static CharacterProfile Create(string name, string ancestry, string specialization)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("A character name is required.", nameof(name));
            if (string.IsNullOrWhiteSpace(ancestry)) throw new ArgumentException("An ancestry is required.", nameof(ancestry));
            if (string.IsNullOrWhiteSpace(specialization)) throw new ArgumentException("A specialization is required.", nameof(specialization));
            return new CharacterProfile
            {
                id = Guid.NewGuid().ToString("N"),
                name = name.Trim(),
                ancestry = ancestry.Trim(),
                specialization = specialization.Trim(),
                level = 1,
                experience = 0
            };
        }
    }
}
