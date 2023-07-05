using UnityEngine;

namespace Scripts.Game.Data
{
    [CreateAssetMenu(menuName = "Game/Gun")]
	public class GunData : BaseItemData
	{
        #region 槍械敘述
        /// <summary>
        /// 擊退等級
        /// </summary>
        [Header("擊退等級")]
        public string RepleLevel;
        /// <summary>
        /// 裝彈耗時等級
        /// </summary>
        [Header("裝彈耗時階級")]
        public string ReloadTimeLevel;
        #endregion

        /// <summary>
        /// 槍械編號
        /// </summary>
        [Header("槍械編號")]
        public GunIndex GunIndex;
        /// <summary>
        /// 每秒可射擊子彈數量
        /// </summary>
        [Header("每秒可射擊子彈數量"), Min(0.01f)]
        public float ShootCountPreSecond = 10f;
        /// <summary>
        /// 裝彈耗時(秒)
        /// </summary>
        [Header("裝彈耗時")]
        public float ReloadTime = 0.9f;
        /// <summary>
        /// 上膛耗時(秒)
        /// </summary>
        [Header("上膛耗時")]
        public float LoadedTime = 0.1f;

        /// <summary>
        /// 彈匣容量
        /// </summary>
        [Header("彈匣容量")]
        public int MagazineSize = 10;
        /// <summary>
        /// 裝彈類別
        /// </summary>
        [Header("裝彈類別")]
        public ReloadType ReloadType = ReloadType.All;
        private int _TotalAmmoCount = -1;
        /// <summary>
        /// 總彈量
        /// </summary>
        public int TotalAmmoCount
        {
            get
            {
                if(_TotalAmmoCount == -1)
                {
                    _TotalAmmoCount = MagazineCount * MagazineSize;
                }
                return _TotalAmmoCount;
            }
            set
            {
                _TotalAmmoCount = value;
            }
        }
        /// <summary>
        /// 攜彈量
        /// </summary>
        [Header("攜彈量")]
        public int MagazineCount = 30;
        /// <summary>
        /// 每次射擊的產生的子彈數量
        /// </summary>
        [Header("每次射擊的產生的子彈數量")]
        public int OneShootAmmoCount = 1;

        /// <summary>
        /// 傷害值
        /// </summary>
        [Header("傷害值")]
        public float Damage = 1;
        /// <summary>
        /// 子彈飛行速度
        /// </summary>
        [Header("子彈飛行速度")]
        [Range(0, 5000)]
        public float AmmoFlySpeed = 100f;
        /// <summary>
        /// 子彈穿透次數
        /// </summary>
        [Header("子彈穿透次數"), Min(0)]
        public int AmmoPenetrationCount = 1;
        [Header("子彈預製物")]
        public GameObject AmmoPrefab;
        [Header("有效射程"), Min(1f)]
        public float EffectiveRange = 15f;

        /// <summary>
        /// 射擊模式陣列
        /// </summary>
        public ShootType[] ShootTypes;
        #region 降速比例
        /// <summary>
        /// 射擊時移動速度比例
        /// </summary>
        [Header("射擊時移動速度比例"), Range(0.1f, 1f)]
        public float ShootSpeedRate = 0.8f;
        /// <summary>
        /// 裝彈時移動速度比例
        /// </summary>
        [Header("裝彈時移動速度比例"), Range(0.1f, 1f)]
        public float ReloadSpeedRate = 0.9f;
        /// <summary>
        /// 常態移動速度比例
        /// </summary>
        [Header("常態移動速度比例"), Range(0.1f, 5f)]
        public float NormalSpeedRate = 1f;
        #endregion

        [Header("武器 貼圖")]
        public Sprite GunSprite;

        #region 彈藥掉落
        [Header("彈藥盒掉落機率"), Range(0, 1)]
        public float DropAmmoRate = 0;
        [Header("彈藥盒補充彈藥比例"), Min(0)]
        public float AmmoBoxRate = 0.2f;
        [Header("彈藥掉落物貼圖")]
        public Sprite DropAmmoSprite;
        #endregion

        [Header("怪物僵直時間"), Min(0)]
        public float EnemyDelayTime = 0.3f;

        [Header("擊退力量"), Min(0)]
        public float Force = 30f;

        [Header("射擊音效")]
        public AudioClip ShootAudio;
        [Header("裝彈音效")]
        public AudioClip ReloadAudio;
        [Header("上膛音效")]
        public AudioClip LoadedAudio;
        [Header("空倉掛機音效")]
        public AudioClip NoAmmoAudio;
        [Header("切換射擊模式音效")]
        public AudioClip ChangeShootType;
        [Header("適用的升級選項")]
        public OptionData[] Options;
        [Header("子彈生成位置")]
        public string FirePointName;
        [Header("連續射擊最大偏移量"), Range(0f, 15f)]
        public float MaxOffset = 5f;
        [SerializeField, Header("每次射擊偏移量"), Range(0f, 15f)]
        public float OffsetAddCount = 0.1f;
        [SerializeField, Header("射擊偏移恢復時間"), Range(0f, 3f)]
        public float OffsetRecoverTime = 2f;

        public Vector3 OptionRotation = new Vector3(0, 0, 0);
        public Vector3 OptionScale = new Vector3(1, 1, 1);
    }
}