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

            if (Input.GetButtonDown("ButtonA"))
            {
                Debug.Log("앉기");
                return;
            }

            if (Input.GetButtonDown("ButtonX")) Debug.Log("button x");
            if (Input.GetButtonDown("ButtonY")) Debug.Log("button y");
            if (Input.GetButtonDown("ButtonB")) Debug.Log("button b");
            if (Input.GetButtonDown("BackButton")) Debug.Log("button back");
            if (Input.GetButtonDown("StartButton")) Debug.Log("button start");


            _player.Move(0);
        }

        void IPlayerState.Exit(CPlayerController _player)
        {
        }
    }
}