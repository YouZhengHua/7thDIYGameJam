using UnityEngine;

namespace Scripts.Game.Data
{
	public class BaseItemData : ScriptableObject
	{
        /// <summary>
        /// 名稱
        /// </summary>
        public string Name;

        /// <summary>
        /// 價格
        /// </summary>
        [Header("價格"), Min(0)]
        public int Price = 0;

        /// <summary>
        /// 是否解鎖
        /// </summary>
        [Header("預設是否解鎖")]
        public bool DefaultIsUnlocked = false;
    }
}