using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerState
{
    public class CRunningState : IPlayerState
    {
        private float speed = 10f;

        void IPlayerState.Enter(CPlayerController _player)
        {
            // 애니메이션 변경
        }

        void IPlayerState.Execute(CPlayerController _player)
        {
            if (!_player.isMove)
            {
                _player.ChangeState(CPlayerState.idleState);
                return;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                _player.ChangeState(CPlayerState.fastRunningState);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _player.Rolling();
                return;
            }

            _player.Move(speed);
        }

        void IPlayerState.Exit(CPlayerController _player)
        {
        }
    }
}