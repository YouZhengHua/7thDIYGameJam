﻿namespace Scripts.Game
{
    public interface IAmmoController
    {
        public void HitEmeny();
        public IGameFiniteStateMachine SetGameFiniteStateMachine { set; }
        public IAttributeHandle SetAttributeHandle { set; }
        public IPlayerController SetPlayer { set; }
        public IEndUIController SetEndUI { set; }
        public bool IsActive { get; }
        public int AmmoGroup { get; set; }
    }
}