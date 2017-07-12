using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerState
{
    public interface IPlayerState
    {
        void Enter(CPlayerController _player);
        void Execute(CPlayerController _player);
        void Exit(CPlayerController _player);
    }
}