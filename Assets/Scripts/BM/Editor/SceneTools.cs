#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace PDR.Editor
{
    public static class SceneTools
    {
        //我觉得大概需要开屏（登录）、主页面、选章、选关、社区、设置、个人信息、练习啥的。
        [MenuItem("Scene Tools/Into Entry")]
        private static void IntoEntry()
        {
            IntoScene("EntryScene");
        }

        [MenuItem("Scene Tools/Into Main")]
        private static void IntoMain()
        {
            IntoScene("MainScene");
        }

        [MenuItem("Scene Tools/Into ChapterSelect")]
        private static void IntoChapterSelect()
        {
            IntoScene("ChapterSelectScene");
        }

        [MenuItem("Scene Tools/Into MapScene")]
        private static void PracticeScene()
        {
            IntoScene("MapScene");
        }

        [MenuItem("Scene Tools/Into CommunityScene")]
        private static void CommunityScene()
        {
            IntoScene("CommunityScene");
        }

        [MenuItem("Scene Tools/Into LevelSelect")]
        private static void IntoLevelSelect()
        {
            IntoScene("LevelSelectionScene");
        }

        [MenuItem("Scene Tools/Into GameplayScene")]
        private static void IntoGameScene()
        {
            IntoScene("GameplayScene");
        }

        [MenuItem("Scene Tools/Into Result")]
        private static void IntoResultShow()
        {
            IntoScene("ResultScene");
        }

        [MenuItem("Scene Tools/Into Settings")]
        private static void IntoSettings()
        {
            IntoScene("SettingsScene");
        }

        [MenuItem("Scene Tools/Into CharacterSelect")]
        private static void IntoCharacterSelect()
        {
            IntoScene("CharaSelectScene");
        }
        
        [MenuItem("Scene Tools/Into CharacterShower")]
        private static void IntoCharacterShower()
        {
            IntoScene("CharaShowScene");
        }

        [MenuItem("Scene Tools/Into Offset")]
        private static void IntoOffset()
        {
            IntoScene("OffsetScene");
        }

        private static void IntoScene(string name)
        {
            if (EditorApplication.isPlaying)
            {
                SceneManager.LoadSceneAsync($"Scenes/{name}");
                return;
            }
            EditorSceneManager.OpenScene($"Assets/Scenes/{name}.unity");
        }
    }
}

#endif
