using UnityEngine;

namespace Scripts.Game
{
    /// <summary>
    /// 玩家自動恢復控制器
    /// </summary>
    public class PlayerRecoverController : MonoBehaviour
    {
        private float _nextTime = 0f;
        private void Update()
        {
            if (GameStateMachine.Instance.CurrectState == GameState.InGame && AttributeHandle.Instance.PlayerAutoRecoverTime > 0f && AttributeHandle.Instance.PlayerAutoRecoverPoint > 0f)
            {
                if (_nextTime >= AttributeHandle.Instance.PlayerAutoRecoverTime)
                {
                    _nextTime = 0f;
                    AttributeHandle.Instance.HealPlayer(AttributeHandle.Instance.PlayerAutoRecoverPoint);
                }
                _nextTime += Time.deltaTime;
            }
        }
    }
}