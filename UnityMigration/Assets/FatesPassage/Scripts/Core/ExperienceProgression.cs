using System;

namespace FatesPassage.Core
{
    /// <summary>Provisional progression rules; verify against source before shipping.</summary>
    public static class ExperienceProgression
    {
        public static long RequiredForNextLevel(int level)
        {
            if (level < 1) throw new ArgumentOutOfRangeException(nameof(level));
            long value = 100;
            for (var i = 1; i < level; i++)
            {
                if (value > long.MaxValue / 2) return long.MaxValue;
                value *= 2;
            }
            return value;
        }

        public static int AddExperience(CharacterProfile profile, long gained)
        {
            if (profile == null) throw new ArgumentNullException(nameof(profile));
            if (gained < 0) throw new ArgumentOutOfRangeException(nameof(gained));
            profile.Validate();
            profile.experience = gained > long.MaxValue - profile.experience ? long.MaxValue : profile.experience + gained;
            var levelsGained = 0;
            while (profile.level < 50)
            {
                var required = RequiredForNextLevel(profile.level);
                if (profile.experience < required) break;
                profile.experience -= required;
                profile.level++;
                levelsGained++;
            }
            return levelsGained;
        }
    }
}
