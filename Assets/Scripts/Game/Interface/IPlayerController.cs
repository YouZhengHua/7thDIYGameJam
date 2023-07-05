using Scripts.Game.Data;
using UnityEngine;

namespace Scripts.Game
{
    /// <summary>
    /// 玩家控制器
    /// 1. 控制玩家面對方向
    /// 2. 控制玩家移動
    /// 3. 主武器射擊
    /// </summary>
    public interface IPlayerController
    {
        public IGameFiniteStateMachine SetGameFiniteStateMachine { set; }

        public Transform GetTransform { get; }

        public UserSetting SetUserSetting { set; }
        
        public IAmmoPool SetAmmoPool { set; }
        
        public IGameUIController SetGameUI { set; }
        
        public IEndUIController SetEndUI { set; }

        public IMeleeController SetMeleeController { set; }

        public GameObject SetContainer { set; }

        public IAttributeHandle SetAttributeHandle { set; }

        public IAudioContoller SetAudio { set; }
    }
}