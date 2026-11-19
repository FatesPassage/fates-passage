using System;
using System.IO;
using UnityEngine;

namespace FatesPassage.Core
{
    /// <summary>Versioned, validated character-sheet persistence with rollback-safe replacement.</summary>
    public static class CharacterSheetSaveService
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "fates-passage-character-sheet.json");
        private static string TempPath => SavePath + ".tmp";
        private static string BackupPath => SavePath + ".bak";

        public static void Save(CharacterSheetData sheet)
        {
            if (sheet == null) throw new ArgumentNullException(nameof(sheet));
            sheet.Validate();
            string directory = Path.GetDirectoryName(SavePath);
            if (string.IsNullOrEmpty(directory)) throw new IOException("Character save directory is unavailable.");
            Directory.CreateDirectory(directory);
            File.WriteAllText(TempPath, JsonUtility.ToJson(sheet, true));

            bool movedOld = false;
            try
            {
                if (File.Exists(BackupPath)) File.Delete(BackupPath);
                if (File.Exists(SavePath))
                {
                    File.Move(SavePath, BackupPath);
                    movedOld = true;
                }
                File.Move(TempPath, SavePath);
                if (File.Exists(BackupPath)) File.Delete(BackupPath);
            }
            catch
            {
                if (File.Exists(TempPath)) File.Delete(TempPath);
                if (movedOld && !File.Exists(SavePath) && File.Exists(BackupPath))
                    File.Move(BackupPath, SavePath);
                throw;
            }
        }

        public static bool TryLoad(out CharacterSheetData sheet)
        {
            sheet = null;
            try
            {
                string path = File.Exists(SavePath) ? SavePath : (File.Exists(BackupPath) ? BackupPath : null);
                if (path == null) return false;
                var loaded = JsonUtility.FromJson<CharacterSheetData>(File.ReadAllText(path));
                if (loaded == null) return false;
                loaded.Validate();
                sheet = loaded;
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Fate's Passage: character sheet could not be loaded: " + ex.Message);
                return false;
            }
        }
    }
}
