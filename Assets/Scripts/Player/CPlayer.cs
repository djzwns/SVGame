using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using PlayerState;

public class CPlayer : MonoBehaviour
{

    private CharacterController characterCtrl;
    private IPlayerState currentState;

    private float h, v;
    private Vector3 movement;

    void Awake()
    {
        characterCtrl = this.GetComponent<CharacterController>();
    }

    void Start()
    {
        currentState = CPlayerState.idleState;
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        currentState.Execute(this);

        PlayerMove();
    }

    private void PlayerMove()
    {
        // 중력
        if (!characterCtrl.isGrounded)
            movement.Set(movement.x, movement.y + (Physics.gravity * Time.deltaTime).y, movement.z);

        characterCtrl.Move(movement * Time.deltaTime);
    }

    /// <summary>
    /// 플레이어의 상태를 전이 시킴.
    /// </summary>
    /// <param name="_state">전이시킬 상태</param>
    public void ChangeState(IPlayerState _state)
    {
        Type currentStateType = this.currentState.GetType();
        Type paramStateType = _state.GetType();

        // 전이될 상태가 현재 상태와 같지 않으면 상태 전이.
        if (!currentStateType.Equals(paramStateType))
        {
            currentState = _state;
            currentState.Enter(this);
        }

        Debug.Log(currentState.GetType());
    }

    /// <summary>
    /// 플레이어를 _speed 의 속도로 이동시킴.
    /// </summary>
    /// <param name="_speed">이동 속도</param>
    public void Move(float _speed)
    {
        if (!characterCtrl.isGrounded)
        {
            return;
        }
        
        // 키입력에 따라 이동 방향 지정
        movement = new Vector3(h, 0, v) * _speed;

        // 이동 방향의 기준이 캐릭터를 기준으로 하기 때문에 카메라 시점만큼 돌려줌
        movement = Camera.main.transform.rotation * movement;

        // y축은 움직이면 안돼서 0으로 재설정
        movement.Set(movement.x, 0, movement.z);

        // movement 를 기준으로 캐릭터 회전
        Rotate();
    }

    private float rotationSpeed = 10f;
    private void Rotate()
    {
        if (movement != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(movement.normalized);
            this.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void Jump(float _jumpPower)
    {
        if (characterCtrl.isGrounded)
            movement.Set(movement.x, _jumpPower, movement.z);
    }

    public bool isMove
    {
        get
        {
            if (h != 0 || v != 0)
            {
                return true;
            }

            return false;
        }
    }
}
