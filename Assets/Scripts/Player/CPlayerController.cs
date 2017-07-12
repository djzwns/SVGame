using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using PlayerState;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class CPlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;

    private float h, v;
    private Vector3 movement;
    private IPlayerState currentState;

    public float jumpSpeed;
    private bool canJump;
    private bool isJumping;
    private bool isGrounded;
    private bool isFalling;

    public float rollSpeed;
    public float rollduration;
    private bool isRolling;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        currentState = CPlayerState.idleState;
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        currentState.Execute(this);
    }

    void FixedUpdate()
    {
        CheckForGrounded();
        PlayerMove();
        CheckFalling();
    }

    private void PlayerMove()
    {
        // 중력
        if (!isGrounded)
            movement.Set(movement.x, movement.y + (Physics.gravity * Time.deltaTime).y, movement.z);

        transform.position += movement * Time.deltaTime;
        //rb.velocity = movement;
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

    void CheckForGrounded()
    {
        float distanceToGround;
        float threshold = .45f;
        RaycastHit hit;
        Vector3 offset = new Vector3(0, .4f, 0);

        if (!Physics.Raycast((transform.position + offset), -Vector3.up, out hit, 100f))
        {
            isGrounded = false;
            return;
        }

        distanceToGround = hit.distance;

        if (distanceToGround < threshold)
        {
            isGrounded = true;
            canJump = true;
            isFalling = false;

            if (!isJumping)
                animator.SetInteger("Jumping", 0);
        }
        else
        {
            isGrounded = false;
        }
    }

    void CheckFalling()
    {
        if (rb.velocity.y < -1)
        {
            isFalling = true;
            animator.SetInteger("Jumping", 2);
            canJump = false;
        }
        else
        {
            isFalling = false;
        }
    }
    
    public void Jumping()
    {
        if (!isGrounded) return;
        if (!canJump) return;

        StartCoroutine(Jump());
    }

    private IEnumerator Jump()
    {
        isJumping = true;
        animator.SetInteger("Jumping", 1);
        animator.SetTrigger("JumpTrigger");

        rb.velocity += jumpSpeed * Vector3.up;
        canJump = false;
        yield return new WaitForSeconds(.5f);
        isJumping = false;
    }

    public void Rolling()
    {
        if (isRolling) return;
        if (!isGrounded) return;

        StartCoroutine(Roll());
    }

    private IEnumerator Roll()
    {
        animator.SetTrigger("RollTrigger");
        isRolling = true;
        yield return new WaitForSeconds(rollduration);
        isRolling = false;
    }



    /// <summary>
    /// 플레이어를 _speed 의 속도로 이동시킴.
    /// </summary>
    /// <param name="_speed">이동 속도</param>
    public void Move(float _speed)
    {
        if (!isGrounded)
        {
            // 땅이 아니면 움직임 제어 불가능.
            return;
        }

        // 키입력에 따라 이동 방향 지정
        movement = new Vector3(h, 0, v);
        movement.Normalize();
        movement *= _speed;

        // 구를 때
        if (isRolling)
            movement = movement.normalized * rollSpeed;

        // 이동 방향의 기준이 캐릭터를 기준으로 하기 때문에 카메라 시점만큼 돌려줌
        movement = Camera.main.transform.rotation * movement;

        // y축은 움직이면 안돼서 0으로 재설정
        movement.Set(movement.x, 0, movement.z);

        // movement 를 기준으로 캐릭터 회전
        Rotate();


        // 애니메이션
        float velocity = movement.normalized.magnitude;
        velocity = Mathf.Clamp(velocity, 0, 1);
        animator.SetFloat("Velocity", velocity);
        animator.SetBool("Moving", !movement.Equals(Vector3.zero));
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