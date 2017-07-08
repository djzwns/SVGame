using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerState
{
    public class CIdleState : IPlayerState
    {
        void IPlayerState.Enter(CPlayer _player)
        {
            // 애니메이션 변경
        }

        void IPlayerState.Execute(CPlayer _player)
        {
            if (_player.isMove)
            {
                _player.ChangeState(CPlayerState.runningState);
                return;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                _player.ChangeState(CPlayerState.fastRunningState);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("앉기");
                return;
            }

            _player.Move(0);
        }

        void IPlayerState.Exit(CPlayer _player)
        {
        }
    }
}