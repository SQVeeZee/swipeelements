using System.Collections.Generic;
using UnityEngine;

namespace Project.Level
{
    [CreateAssetMenu(menuName = "Configs/levels_config", fileName = "levels_config", order = 0)]
    public class LevelsConfig : ScriptableObject
    {
        [SerializeField]
        private List<TextAsset> _levelFiles;

        public int LevelCount => _levelFiles.Count;
        public TextAsset GetLevelFile(int index) => _levelFiles[index];
    }
}