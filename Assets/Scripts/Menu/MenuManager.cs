using Scripts.Game.Data;
using UnityEngine;

namespace Scripts.Menu
{
    public class MenuManager : MonoBehaviour, IMenuManager
    {
        [SerializeField] private FadeEffectUI fadeEffect;
        [SerializeField] private StoryManager storyManager;
        [SerializeField] private StageManager stageManager;

        private void Awake()
        {
            if (StaticPrefs.IsFirstIn)
                FirstOpenGame();
        }

        private void Start() {
            switch (stageManager.GetCurrentStage()) {
                case StageManager.stage.firstStartGame:
                    Prologue();
                    break;
                case StageManager.stage.Level_1:
                    Level_1();
                    break;
                case StageManager.stage.Level_2:
                    Level_2();
                    break;
                case StageManager.stage.Level_3:
                    Level_3();
                    break;
                case StageManager.stage.Level_4:
                    Level_4();
                    break;
                case StageManager.stage.gameEnd:
                    GameEnd();
                    break;
                default:
                    GameEnd();
                    break;
            }
        }

        private void FirstOpenGame()
        {
            Debug.Log("FirstOpenGame");
            StaticPrefs.IsFirstIn = false;
        }

        /// <summary>
        /// 載入指定的 Scene
        /// </summary>
        /// <param name="sceneName">場景名稱</param>
        public void LoadScene(string sceneName)
        {
            LoadingScreen.instance.LoadScene(sceneName, false);
        }

        public void TalkToGirl() {

        }

        private void FadeIn() {
            fadeEffect.FadeIn();
        }

        private void FadeOut() {
            fadeEffect.FadeOut();
        }

        private void Prologue() {
            storyManager.StartStory(4, FadeIn);
            stageManager.SetCurrentStage(StageManager.stage.Level_1);
        }

        private void Level_1() {
            FadeIn();
        }

        private void Level_2() {
            FadeIn();
        }

        private void Level_3() {
            FadeIn();
        }

        private void Level_4() {
            FadeIn();
        }

        private void GameEnd() {
            FadeIn();
        }
    }
}