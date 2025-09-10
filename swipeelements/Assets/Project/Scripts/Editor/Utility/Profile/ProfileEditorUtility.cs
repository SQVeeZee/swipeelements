using System.IO;
using UnityEditor;
using UnityEngine;

namespace Project.Editor.Utility
{
    public static class ProfileEditorUtility
    {
        private static string ProfilesPath => Application.persistentDataPath;

        [MenuItem("Tools/Profile/Clear All Profiles")]
        public static void ClearAllProfiles()
        {
            if (Directory.Exists(ProfilesPath))
            {
                Directory.Delete(ProfilesPath, true);
            }
        }

        [MenuItem("Tools/Profile/Open Profiles Folder")]
        public static void OpenProfilesFolder()
        {
            if (!Directory.Exists(ProfilesPath))
            {
                Directory.CreateDirectory(ProfilesPath);
            }
            EditorUtility.RevealInFinder(ProfilesPath);
        }
    }
}
