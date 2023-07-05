using Scripts.Game;
using UnityEngine;

namespace Scripts
{
    public class StaticPrefs
    {
        /// <summary>
        /// 取得分數
        /// </summary>
        public static float Score
        {
            get => PlayerPrefs.GetFloat("Score", 0f);
            set
            {
                PlayerPrefs.SetFloat("Score", value);
            }
        }

        /// <summary>
        /// 取得槍械編號
        /// </summary>
        public static int GunIndex
        {
            get => PlayerPrefs.GetInt("GunIndex", 0);
            set
            {
                PlayerPrefs.SetInt("GunIndex", value);
            }
        }

        /// <summary>
        /// 取得槍枝是否解鎖
        /// </summary>
        /// <param name="index"></param>
        /// <param name="defaultIsUnlocked"></param>
        /// <returns></returns>
        public static bool GetGunIsUnlocked(GunIndex index, bool defaultIsUnlocked)
        {
            int isUnlocked = PlayerPrefs.GetInt($"Gun_{index}", 0);
            return isUnlocked == 0 ? defaultIsUnlocked : isUnlocked > 0;
        }

        /// <summary>
        /// 設定槍枝是否解鎖
        /// </summary>
        /// <param name="index"></param>
        /// <param name="defaultIsUnlocked"></param>
        public static void SetGunIsUnlocked(GunIndex index, bool isUnlocked)
        {
            PlayerPrefs.SetInt($"Gun_{index}", isUnlocked ? 1 : -1);
        }

        public static float Scale { get => Screen.width / 1920f; }

        public static bool IsFirstIn { get => PlayerPrefs.GetInt("FirstIn", 1) == 1; set => PlayerPrefs.SetInt("FirstIn", value ? 1 : 0); }
    }
}
