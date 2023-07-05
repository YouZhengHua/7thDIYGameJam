using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Base
{
    public interface IBaseUIController
    {
        /// <summary>
        /// 顯示介面
        /// </summary>
        public abstract void ShowCanvas();
        /// <summary>
        /// 隱藏介面
        /// </summary>
        public abstract void HideCanvas();
    }
}