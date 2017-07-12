using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCameraMove : MonoBehaviour
{
    private GameObject player;

    private float rotate_x_speed = 10f;
    private float rotate_y_speed = 50f;

    private float cam_x;
    private float cam_y;

    private float max_cam_y = 60f;
    private float min_cam_y = -10f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 angles = this.transform.eulerAngles;
        cam_y = angles.y;
        cam_x = angles.x;
    }

    void LateUpdate()
    {
        if (player == null) return;

        Rotate();
        FollowMove();
    }

    private float zDist = -10f;
    private float yDist = 2f;
    /// <summary>
    /// 플레이어를 따라 이동.
    /// </summary>
    void FollowMove()
    {
        // 플레이어 포지션
        Vector3 playerPos = player.transform.position;

        // 카메라의 회전값을 기준으로 거리를 두도록 함.
        Vector3 pos = playerPos + this.transform.rotation * new Vector3(0, 0, zDist) + Vector3.up * yDist;

        this.transform.position = pos;
    }

    /// <summary>
    /// 카메라의 앵글을 상하좌우로 회전.
    /// </summary>
    void Rotate()
    {
        float mouse_x = Input.GetAxis("Mouse X");
        float mouse_y = Input.GetAxis("Mouse ScrollWheel");

        cam_x += mouse_x * rotate_x_speed;
        cam_y -= mouse_y * rotate_y_speed;

        cam_y = ClampAngle(cam_y, min_cam_y, max_cam_y);

        Quaternion rotation = Quaternion.Euler(cam_y, cam_x, 0);

        this.transform.rotation = rotation;
    }

    float ClampAngle(float _angle, float _min, float _max)
    {
        if (_angle < -360)
            _angle += 360;
        if (_angle > 360)
            _angle -= 360;

        return Mathf.Clamp(_angle, _min, _max);
    }
}
