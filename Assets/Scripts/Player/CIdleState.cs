using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerState
{
    public class CIdleState : IPlayerState
    {
        void IPlayerState.Enter(CPlayerController _player)
        {
            // 애니메이션 변경
        }

        void IPlayerState.Execute(CPlayerController _player)
        {
            if (_player.isMove)
            {
                _player.ChangeState(CPlayerState.runningState);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("앉기");
                return;
            }

            _player.Move(0);
        }

        void IPlayerState.Exit(CPlayerController _player)
        {
        }
    }
}