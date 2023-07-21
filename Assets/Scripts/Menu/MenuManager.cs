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

        private int dialogueIndex;
        private int dialogueCycleIndex;

        private void Awake()
        {
            if (StaticPrefs.IsFirstIn)
                FirstOpenGame();
        }

        private void Start() {
            Debug.Log("MenuManager being called");

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

            foreach (StoryDialogueSO.stage_dialogue stage_Dialogue in storyDialogueSO.dialogues) {
                if (stage_Dialogue.stage == stageManager.GetCurrentStage()) {
                    dialogueIndex = stage_Dialogue.dialougeIndex[0];
                }
            }

            dialogueCycleIndex = 0;
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
            Debug.Log("Talk to girl with dialogue index " + dialogueIndex);
            DisableAllUI();
            storyManager.StartStory(dialogueIndex, EnableAllUI);

            foreach (StoryDialogueSO.stage_dialogue stage_Dialogue in storyDialogueSO.dialogues) {
                if (stage_Dialogue.stage == stageManager.GetCurrentStage()) {
                    Debug.Log("Cycle Index: " + dialogueCycleIndex);
                    dialogueCycleIndex = (dialogueCycleIndex + 1) % stage_Dialogue.dialougeIndex.Count;
                    dialogueIndex = dialogueCycleIndex + stage_Dialogue.dialougeIndex[0];
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

        private void SetCurrentState(StageManager.stage stage) {
            stageManager.SetCurrentStage(stage);
        }

        private void Prologue() {
            stageManager.isSecInTheSameLevel = false;
            storyManager.StartStory(4, Level_1);
            SetCurrentState(StageManager.stage.Level_1);
        }

        private void Level_1() {
            if (stageManager.isSecInTheSameLevel) {
                FadeIn();
            } else {
                FadeIn();
                DisableAllUI();
                storyManager.StartStory(5, EnableAllUI);
                dialogueIndex = 6;
                stageManager.isSecInTheSameLevel = true;
            }
        }

        private void Level_2() {
            if (stageManager.isSecInTheSameLevel) {
                FadeIn();
            } else {
                FadeIn();
                stageManager.isSecInTheSameLevel = true;
                DisableAllUI();
                storyManager.StartStory(8, () => { FadeOut();
                    storyManager.StartStory(2, () => { FadeIn();
                        storyManager.StartStory(9, EnableAllUI);
                    }); });
            }
        }

        private void Level_3() {
            if (stageManager.isSecInTheSameLevel) {
                FadeIn();
            } else {
                FadeIn();
                stageManager.isSecInTheSameLevel = true;
                DisableAllUI();
                storyManager.StartStory(12, () => {
                    FadeOut();
                    storyManager.StartStory(2, () => {
                        FadeIn();
                        storyManager.StartStory(13, EnableAllUI);
                    });
                });
            }
        }

        private void Level_4() {
            if (stageManager.isSecInTheSameLevel) {
                FadeIn();
            } else {
                FadeIn();
                stageManager.isSecInTheSameLevel = true;
                DisableAllUI();
                storyManager.StartStory(16, () => {
                    FadeOut();
                    storyManager.StartStory(2, () => {
                        FadeIn();
                        storyManager.StartStory(17, EnableAllUI);
                    });
                });
            }
        }

        private void GameEnd() {
            if (stageManager.isSecInTheSameLevel) {
                FadeIn();
            } else {
                FadeIn();
                stageManager.isSecInTheSameLevel = true;
                DisableAllUI();
                storyManager.StartStory(21, () => {
                    FadeOut();
                    storyManager.StartStory(2, () => {
                        FadeIn();
                        storyManager.StartStory(22, EnableAllUI);
                    });
                });
            }
        }
    }
}