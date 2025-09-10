using Newtonsoft.Json;
using Project.Gameplay.Puzzles;
using UnityEngine;

namespace Level.Utility
{
    public static class LevelSerializerUtility
    {
        public static LevelData DeserializeLevelAsset(TextAsset levelAsset)
        {
            var json = levelAsset.text;
            var levelInfo = JsonConvert.DeserializeObject<LevelData>(json);
            return levelInfo;
        }
    }
}