using System;
using UnityEngine;

namespace FatesPassage.Core
{
    [Serializable]
    public sealed class CharacterProfile
    {
        public string id = "";
        public string name = "";
        public string ancestry = "";
        public string specialization = "";
        public int level = 1;
        public long experience = 0;

        public void Validate()
        {
            level = Mathf.Max(1, level);
            experience = Math.Max(0L, experience);
            name = (name ?? "").Trim();
            id ??= "";
            ancestry ??= "";
            specialization ??= "";
        }
    }
}
