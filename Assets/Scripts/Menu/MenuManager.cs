using Scripts.Game.Data;
using UnityEngine;

namespace Scripts.Menu
{
    public class MenuManager : MonoBehaviour, IMenuManager
    {
        [SerializeField] private FadeEffectUI fadeEffect;
        [SerializeField] private Transform disableAllUI;
        [SerializeField] private StoryManager storyManager;
        [SerializeField] private StageManager stageManager;
        [SerializeField] private StoryDialogueSO storyDialogueSO;

        private StageManager.stage currentStage;
        private int dialogueIndex;

        private void Awake()
        {
            if (StaticPrefs.IsFirstIn)
                FirstOpenGame();
        }

        private void Start() {
            currentStage = stageManager.GetCurrentStage();

            switch (currentStage) {
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

            foreach (StoryDialogueSO.stage_dialogue stage_Dialogue in storyDialogueSO.dialogues) {
                if (stage_Dialogue.stage == currentStage) {
                    dialogueIndex = stage_Dialogue.dialougeIndex[0];
                }
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
            foreach(StoryDialogueSO.stage_dialogue stage_Dialogue in storyDialogueSO.dialogues) {
                if (stage_Dialogue.stage == currentStage) {
                    DisableAllUI();
                    storyManager.StartStory(dialogueIndex, EnableAllUI);
                    dialogueIndex = ((dialogueIndex + 1) % stage_Dialogue.dialougeIndex.Count) + stage_Dialogue.dialougeIndex[0];
                }
            }
        }

        private void FadeIn() {
            fadeEffect.FadeIn();
        }

        private void FadeOut() {
            fadeEffect.FadeOut();
        }

        private void DisableAllUI() {
            disableAllUI.gameObject.SetActive(true);
        }

        private void EnableAllUI() {
            disableAllUI.gameObject.SetActive(false);
        }

        private void Prologue() {
            storyManager.StartStory(4, Level_1);
            stageManager.SetCurrentStage(StageManager.stage.Level_1);
        }

        private void Level_1() {
            FadeIn();
            DisableAllUI();
            storyManager.StartStory(5, EnableAllUI);
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