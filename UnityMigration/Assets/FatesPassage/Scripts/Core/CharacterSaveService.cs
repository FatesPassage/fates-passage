using System;
using System.IO;
using UnityEngine;

namespace FatesPassage.Core
{
    public static class CharacterSaveService
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "character.json");

        public static void Save(CharacterProfile profile)
        {
            if (profile == null) throw new ArgumentNullException(nameof(profile));
            profile.Validate();
            var path = SavePath;
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            var temp = path + ".tmp";
            File.WriteAllText(temp, JsonUtility.ToJson(profile, true));
            if (File.Exists(path)) File.Delete(path);
            File.Move(temp, path);
        }

        public static bool TryLoad(out CharacterProfile profile)
        {
            profile = null;
            if (!File.Exists(SavePath)) return false;
            try
            {
                profile = JsonUtility.FromJson<CharacterProfile>(File.ReadAllText(SavePath));
                if (profile == null) return false;
                profile.Validate();
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Fate's Passage: failed to load character: " + ex.Message);
                return false;
            }
        }
    }
}
