using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerState
{
    public interface IPlayerState
    {
        void Enter(CPlayer _player);
        void Execute(CPlayer _player);
        void Exit(CPlayer _player);
    }
}