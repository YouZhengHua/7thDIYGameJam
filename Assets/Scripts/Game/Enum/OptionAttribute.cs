namespace Scripts.Game
{
	public enum OptionAttribute
	{
		/// <summary>
		/// 調整武器傷害(固定值)
		/// </summary>
		GunDamage,
		/// <summary>
		/// 調整武器傷害(倍率)
		/// </summary>
		GunDamageMultiple,
		/// <summary>
		/// 調整槍械每秒可射擊子彈數量
		/// </summary>
		GunShootCountPreSecond,
		/// <summary>
		/// 調整槍械裝彈時間
		/// </summary>
		GunReloadTime,
		/// <summary>
		/// 調整槍械穿透
		/// </summary>
		GunPenetrationCount,
		/// <summary>
		/// 調整槍械有效射程
		/// </summary>
		GunEffectiveRange,
		/// <summary>
		/// 調整近戰武器傷害(固定值)
		/// </summary>
		MeleeDamage,
		/// <summary>
		/// 調整近戰武器傷害(倍率)
		/// </summary>
		MeleeDamageMultiple,
		/// <summary>
		/// 調整武器揮動速度
		/// </summary>
		MeleeSpeed,
		/// <summary>
		/// 調整近戰武器攻擊範圍
		/// </summary>
		MeleeRange,
		/// <summary>
		/// 增加子彈數量(以彈匣倍率增加)
		/// 例如增加三個彈匣的子彈數量
		/// </summary>
		AmmoCount,
		/// <summary>
		/// 擴增彈匣大小
		/// </summary>
		MagazineSize,
		/// <summary>
		/// 增加玩家移動速度(倍率)
		/// </summary>
		PlayerSpeed,
		/// <summary>
		/// 治癒玩家
		/// </summary>
		PlayerHeal,
		/// <summary>
		/// 增加玩家最大血量
		/// </summary>
		PlayerMaxHealth,
		/// <summary>
		/// 調整經驗值倍率
		/// </summary>
		ExtendExp,
		/// <summary>
		/// 調整拾取掉落物的範圍
		/// </summary>
		GetDropItemRadius,
		/// <summary>
		/// 增加遊戲分數
		/// </summary>
		AddScore,
		/// <summary>
		/// 增加射出子彈數目
		/// </summary>
		AddShootAmmoCount,
	}
}