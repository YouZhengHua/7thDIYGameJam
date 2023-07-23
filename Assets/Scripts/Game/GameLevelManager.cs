using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
    public class GameLevelManager : MonoBehaviour
    {
        [SerializeField, Header("關卡資料")]
        private LevelData[] _levels;
        private LevelData _level;
        [SerializeField, Header("進度管理")]
        private StageManager _stageManager;
        private Transform _playerContainer;
        private Transform _enemyContainer;

        private void Awake()
        {
            _playerContainer = GameObject.Find("PlayerContainer").GetComponent<Transform>();
            _enemyContainer = GameObject.Find("EnemyContainer").GetComponent<Transform>();
            _level = _levels[(int)_stageManager.GetCurrentStage() - 1];
        }

        private void Start()
        {
            Debug.Log(_level);
            AttributeHandle.Instance.TotalGameTime = _level.GameTime;
            foreach (LevelRound level in _level.LevelRounds)
            {
                foreach (LevelEnemyData enemyData in level.EnemyDatas)
                {
                    enemyData.NextTime = 0;
                }
            }
        }

        private void Update()
        {
            if(GameStateMachine.Instance.CurrectState == GameState.InGame)
            {
                EnemyHandel();
            }
        }

        private void EnemyHandel()
        {
            foreach (LevelRound level in _level.LevelRounds)
            {
                if (AttributeHandle.Instance.GameTime >= level.LevelStartTime && AttributeHandle.Instance.GameTime <= level.LevelEndTime && !AttributeHandle.Instance.IsTimeWin)
                {
                    foreach (LevelEnemyData enemyData in level.EnemyDatas)
                    {
                        if (AttributeHandle.Instance.GameTime >= enemyData.NextTime)
                        {
                            if (enemyData.IsGroup)
                            {
                                Vector3 nextPosition = _playerContainer.position + GetRandomPosition(enemyData.Distance);
                                for (int i = 0; i < enemyData.Quantity; i++)
                                {
                                    GameObject enemy = GameObject.Instantiate(enemyData.Prefab, _enemyContainer);
                                    enemy.transform.position = nextPosition + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                                    nextPosition = _playerContainer.position + GetRandomPosition(enemyData.Distance);
                                }
                            }
                            else if (enemyData.IsRound)
                            {
                                float angle = Random.Range(0f, 360f);
                                for (int i = 0; i < enemyData.Quantity; i++)
                                {
                                    GameObject enemy = GameObject.Instantiate(enemyData.Prefab, _enemyContainer);
                                    enemy.transform.position = _playerContainer.position + GetAnglePosition(enemyData.Distance, angle);
                                    angle += 360f / enemyData.Quantity;
                                }
                            }
                            else
                            {
                                for (int i = 0; i < enemyData.Quantity; i++)
                                {
                                    GameObject enemy = GameObject.Instantiate(enemyData.Prefab, _enemyContainer);
                                    enemy.transform.position = _playerContainer.position + GetRandomPosition(enemyData.Distance);
                                }
                            }
                            if (enemyData.WarmingAudio != null)
                            {
                                AudioController.Instance.PlayEffect(enemyData.WarmingAudio, enemyData.ExtendVolume);
                            }
                            enemyData.NextTime = AttributeHandle.Instance.GameTime + enemyData.Intervals;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 取得指定距離、隨機角度的座標
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        private Vector3 GetRandomPosition(float distance)
        {
            return GetAnglePosition(distance, Random.Range(0f, 360f));
        }

        /// <summary>
        /// 取得指定距離、指定角度的座標
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        private Vector3 GetAnglePosition(float distance, float angle)
        {
            float radians = angle * Mathf.Deg2Rad;
            float x = distance * Mathf.Cos(radians);
            float y = distance * Mathf.Sin(radians);
            return new Vector3(x, y);
        }
    }
}