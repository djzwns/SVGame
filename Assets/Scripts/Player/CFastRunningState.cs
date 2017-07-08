using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerState
{
    public class CFastRunningState : IPlayerState
    {
        private float speed = 15f;
        private float jumpSpeed = 10f;

        void IPlayerState.Enter(CPlayer _player)
        {
            // 애니메이션 변경
        }

        void IPlayerState.Execute(CPlayer _player)
        {
            if (!_player.isMove)
            {
                _player.ChangeState(CPlayerState.idleState);
                return;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _player.ChangeState(CPlayerState.runningState);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _player.Jump(jumpSpeed);
                return;
            }

            _player.Move(speed);
        }

        void IPlayerState.Exit(CPlayer _player)
        {
        }
    }
}