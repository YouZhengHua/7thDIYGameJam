﻿using Scripts.Game.Data;
using UnityEngine;

namespace Scripts.Game
{
    public interface IAttributeHandle
    {
        /// <summary>
        /// 提升屬性值
        /// </summary>
        /// <param name="data"></param>
        public void UpdateAttribute(OptionData data);

        #region 槍械
        /// <summary>
        /// 槍械傷害
        /// </summary>
        public float GunDamage { get; }
        /// <summary>
        /// 槍械的擊退力道
        /// </summary>
        public float GunRepelForce { get; }
        /// <summary>
        /// 槍械的擊退持續時間
        /// </summary>
        public float GunRepelTime { get; }
        /// <summary>
        /// 單次射擊時射出的子彈數量
        /// </summary>
        public int OneShootAmmoCount { get; }
        /// <summary>
        /// 子彈飛行速度
        /// </summary>
        public float AmmoFlySpeed { get; }
        /// <summary>
        /// 射擊間隔
        /// </summary>
        public float ShootCooldownTime { get; }
        /// <summary>
        /// 子彈穿透數量
        /// </summary>
        public int AmmoPenetrationCount { get; }
        /// <summary>
        /// 槍械有效射程
        /// </summary>
        public float GunEffectiveRange { get; }
        /// <summary>
        /// 射擊音效
        /// </summary>
        public AudioClip ShootAudio { get; }
        /// <summary>
        /// 裝彈音效
        /// </summary>
        public AudioClip ReloadAudio { get; }
        /// <summary>
        /// 上膛音效
        /// </summary>
        public AudioClip LoadedAudio { get; }
        /// <summary>
        /// 空倉掛機音效
        /// </summary>
        public AudioClip NoAmmoAudio { get; }
        /// <summary>
        /// 切換射擊模式音效
        /// </summary>
        public AudioClip ChangeShootTypeAudio { get; }
        /// <summary>
        /// 是否需要掉落補血品
        /// </summary>
        public bool NeedDropHealth { get; }
        public string FirePointName { get; }
        /// <summary>
        /// 射擊偏移
        /// </summary>
        public Quaternion ShootOffset { get; }
        /// <summary>
        /// 增加射擊偏移量
        /// </summary>
        public void AddOffset();
        /// <summary>
        /// 恢復射擊偏移量
        /// </summary>
        /// <param name="recoverTime"></param>
        public void RecoverOffset(float recoverTime);
        #endregion

        #region 玩家相關
        /// <summary>
        /// 玩家受創時的擊退範圍
        /// </summary>
        public float PlayerRepelRadius { get; }
        /// <summary>
        /// 玩家受創時的擊退力道
        /// </summary>
        public float PlayerRepelForce { get; }
        /// <summary>
        /// 玩家受創時的擊退持續時間
        /// </summary>
        public float PlayerRepelTime { get; }
        /// <summary>
        /// 玩家血量
        /// </summary>
        public float PlayerHealthPoint { get; set; }
        /// <summary>
        /// 玩家最大血量
        /// </summary>
        public float PlayerMaxHealthPoint { get; set; }
        /// <summary>
        /// 玩家移動速度
        /// 基礎移動速度 * (移動倍率 + 升級移動倍率) * 槍械影響移動速度倍率 * 玩家狀態速度倍率影響
        /// </summary>
        public float PlayerMoveSpeed { get; }
        /// <summary>
        /// 拾取掉落物的範圍
        /// </summary>
        public float GetDropItemRadius { get; }
        /// <summary>
        /// 玩家無敵時間
        /// </summary>
        public float InvincibleTime { get; }
        #endregion

        #region 經驗值相關
        /// <summary>
        /// 取得總經驗值
        /// </summary>
        public float TotalExp { get; }
        /// <summary>
        /// 是否升級
        /// </summary>
        public bool IsLevelUp { get; }
        /// <summary>
        /// 取得玩家等級
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 增加經驗值
        /// </summary>
        /// <param name="exp"></param>
        public void AddExp(float exp);
        /// <summary>
        /// 經驗值百分比
        /// </summary>
        public float ExpPercentage { get; }
        /// <summary>
        /// 升級音效
        /// </summary>
        public AudioClip LevelUpAudio { get; }
        /// <summary>
        /// 受擊音效
        /// </summary>
        public AudioClip GetHitAudio { get; }
        public float NowExp { get; set; }
        /// <summary>
        /// 取得下一等級的經驗值門檻
        /// </summary>
        public float NextLevelExp { get; }
        #endregion

        #region DI 設計
        public IGameUIController SetGameUI { set; }
        #endregion
    }
}
