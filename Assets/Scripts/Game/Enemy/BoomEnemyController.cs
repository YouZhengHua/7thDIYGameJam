using Scripts.Game.Base;
using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Scripts.Game
{
    /// <summary>
    /// 自爆怪物控制器
    /// </summary>
    public class BoomEnemyController : BaseEnemyController
    {
        [SerializeField, Header("自爆半徑")]
        private float _radius;
        [SerializeField, Header("玩家階層")]
        private LayerMask _playerLayer;
        private float delay = 0f;
        protected override void Dead()
        {
            base.Dead();
            StartCoroutine(BoomSelf());
        }

        private IEnumerator BoomSelf()
        {
            yield return new WaitForSeconds(delay);
            foreach (Collider2D player in Physics2D.OverlapCircleAll(this.transform.position, _radius * this.transform.localScale.x, _playerLayer))
            {
                player.GetComponent<PlayerDamageController>().GetDamage(EnemyData.Damage.Value);
            }
        }
    }
}