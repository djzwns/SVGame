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
        
        // 중력
        if (!characterCtrl.isGrounded)
            movement.Set(movement.x, movement.y + (Physics.gravity * Time.deltaTime).y, movement.z);


        characterCtrl.Move(movement * Time.deltaTime);
    }

    // 상태 전이
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

    // 플레이어 이동
    public void Move(float _speed)
    {
        if (!characterCtrl.isGrounded)
        {
            return;
        }
        
        movement = new Vector3(h, 0, v) * _speed;

        Rotate();
        //movement = transform.TransformDirection(movement);
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
