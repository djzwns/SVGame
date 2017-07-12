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

            if (Input.GetButton("Run") || Input.GetAxis("Run") > 0.5)
            {
                _player.ChangeState(CPlayerState.fastRunningState);
                return;
            }

            if (Input.GetButtonDown("ButtonA"))
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