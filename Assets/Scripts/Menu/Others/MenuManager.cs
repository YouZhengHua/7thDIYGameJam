using Scripts.Game.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu
{
    public class MenuManager : MonoBehaviour, IMenuManager
    {
        [SerializeField] private FadeEffectUI fadeEffect;
        [SerializeField] private Transform disableAllUI;
        [SerializeField] private StoryManager storyManager;
        [SerializeField] private StageManager stageManager;
        [SerializeField] private StoryDialogueSO storyDialogueSO;
        [SerializeField] private Transform endGameCheckBox;
        [SerializeField] private Animator prologueEffect;
        [SerializeField] private bool isDebug = false;

        [SerializeField, Header("使用者設定UI")]
        private Canvas _settingCanvas;
        [SerializeField, Header("使用者設定")]
        private UserSetting _nowSetting;
        [SerializeField, Header("預設設定")]
        private UserSetting _defaultSetting;
        private Scripts.Game.SettingUIController settingUI;

        private List<int> currentDialogueList;
        private int dialogueCycleIndex;
        

        private void Awake()
        {
            CheckerUtility.SceneLoadChecker();
            if (StaticPrefs.IsFirstIn)
                FirstOpenGame();

            if (settingUI == null)
                settingUI = new Game.SettingUIController(_settingCanvas, _defaultSetting, _nowSetting);
            settingUI.HideCanvas();
            AudioController.Instance.SetUserSetting(_nowSetting);
        }

        public void InvokeSettingUICanvas() {
            
            settingUI.ShowCanvas();
        }
        private void Start() {
            Debug.Log("MenuManager being called");
            Time.timeScale = 1.0f;
            if (!isDebug) {
                stageManager.Load();
            }

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

            currentDialogueList = GetDialogueIndexList();
            dialogueCycleIndex = 0;

            AudioController.Instance.UpdateAudioVolume();
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
            LoadingScreen.instance.LoadScene(sceneName, stageManager.LoadingImage, stageManager.LoadingTip);
        }

        public void TalkToGirl() {
            DisableAllUI();
            storyManager.StartStory(currentDialogueList[dialogueCycleIndex], EnableAllUI);
            dialogueCycleIndex = (dialogueCycleIndex + 1) % currentDialogueList.Count;

            if (stageManager.GetCurrentStage() == StageManager.stage.gameEnd) {
                endGameCheckBox.gameObject.SetActive(true);
            }
        }
        private void FadeIn() {
            fadeEffect.FadeIn();
        }

        private void FadeOut() {
            fadeEffect.FadeOut();
        }

        private void PrologueFadeIn() {
            fadeEffect.PrologueFadeIn();
        }

        private void DisableAllUI() {
            disableAllUI.gameObject.SetActive(true);
        }

        private void EnableAllUI() {
            disableAllUI.gameObject.SetActive(false);
        }

        private void PrologueEffect() {
            prologueEffect.Play("PrologueFadeOut");
            Invoke("Level_1", 7.5f);
        }

        private void SetCurrentState(StageManager.stage stage) {
            stageManager.SetCurrentStage(stage);
        }

        private void Prologue() {
            stageManager.isSecInTheSameLevel = false;
            storyManager.StartStory(4, () => {
                Invoke("PrologueEffect", 2f);
            });
            SetCurrentState(StageManager.stage.Level_1);
        }

        private void Level_1() {
            if (stageManager.isSecInTheSameLevel) {
                FadeIn();
            } else {
                PrologueFadeIn();
                DisableAllUI();
                storyManager.StartStory(5, EnableAllUI);
                currentDialogueList = GetDialogueIndexList();
                stageManager.isSecInTheSameLevel = true;
            }
        }

        private void Level_2() {
            Debug.Log("Entering Level2");

            if (stageManager.isSecInTheSameLevel) {
                FadeIn();
            } else {
                FadeIn();
                stageManager.isSecInTheSameLevel = true;
                DisableAllUI();
                storyManager.StartStory(9, () => { FadeOut();
                    storyManager.StartStory(2, () => { FadeIn();
                        storyManager.StartStory(10, EnableAllUI);
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
                storyManager.StartStory(15, () => {
                    FadeOut();
                    storyManager.StartStory(2, () => {
                        FadeIn();
                        storyManager.StartStory(16, EnableAllUI);
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
                storyManager.StartStory(23, () => {
                    FadeOut();
                    storyManager.StartStory(2, () => {
                        FadeIn();
                        storyManager.StartStory(24, EnableAllUI);
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
                storyManager.StartStory(31, () => {
                    FadeOut();
                    storyManager.StartStory(2, () => {
                        FadeIn();
                        storyManager.StartStory(32, () => {
                            EnableAllUI();
                            endGameCheckBox.gameObject.SetActive(true);
                        });
                    });
                });
            }
        }

        private List<int> GetDialogueIndexList() {
            List<int> returnList = new List<int>();

            foreach (StoryDialogueSO.stage_dialogue stage_Dialogue in storyDialogueSO.dialogues) {
                if (stage_Dialogue.stage == stageManager.GetCurrentStage()) {
                    if (stageManager.isPlayerDefeated) {
                        returnList.AddRange(stage_Dialogue.defeatedDialogueIndex);
                    }
                    returnList.AddRange(stage_Dialogue.dialougeIndex);
                }
            }

            return returnList;
        }
    }
}