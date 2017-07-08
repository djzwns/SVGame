using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerState
{
    static public class CPlayerState
    {
        static private CIdleState IdleState;
        static public CIdleState idleState
        {
            get
            {
                if (IdleState != null)
                    return IdleState;

                else
                {
                    IdleState = new CIdleState();
                    return IdleState;
                }
            }
        }

        static private CRunningState RunningState;
        static public CRunningState runningState
        {
            get
            {
                if (RunningState != null)
                    return RunningState;

                else
                {
                    RunningState = new CRunningState();
                    return RunningState;
                }
            }
        }

        static private CFastRunningState FastRunningState;
        static public CFastRunningState fastRunningState
        {
            get
            {
                if (FastRunningState != null)
                    return FastRunningState;

                else
                {
                    FastRunningState = new CFastRunningState();
                    return FastRunningState;
                }
            }
        }
    }
}