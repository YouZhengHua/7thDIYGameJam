using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Base;
using System;

namespace Scripts.Game
{
    public class GameFiniteStateMachine : BaseFiniteStateMachine<GameState>, IGameFiniteStateMachine
    {
        private PlayerState _playerState;
        public GameFiniteStateMachine(GameState currectState): base(currectState)
        {
            _playerState = PlayerState.Idle;
        }

        public GameFiniteStateMachine(GameState currectState, Action exitStateLamdba): base(currectState, exitStateLamdba)
        {
            _playerState = PlayerState.Idle;
        }

        protected override void ChangeState(GameState value)
        {
            base.ChangeState(value);
            if(value == GameState.InGame || value == GameState.SelectOption)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0;
            }
        }

        public PlayerState PlayerState { get => _playerState; }

        public void SetPlayerState(PlayerState value)
        {
            _playerState = value;
        }
    }
}