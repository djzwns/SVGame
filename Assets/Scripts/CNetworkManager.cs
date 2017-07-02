using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FreeNet;
using FreeNetUnity;
using GameServer;

public class CNetworkManager : MonoBehaviour
{
    CFreeNetUnityService gameserver;

    void Awake()
    {
        // 네트워크 통신을 위해 CFreeNetUnityService 객체를 추가합니다.
        this.gameserver = gameObject.AddComponent<CFreeNetUnityService>();

        // 상태 변화(접속, 끊김 등) 를 통보 받을 델리게이트 설정.
        this.gameserver.appcallback_on_status_changed += on_status_changed;

        // 패킷 수신 델리게이트 설정.
        this.gameserver.appcallback_on_message += on_message;
    }

    void Start() { connect(); }

    void connect() { this.gameserver.connect("127.0.0.1", 7979); }

    /// <summary>
    /// 네트워크 상태 변경 시 호출될 콜백 메소드
    /// </summary>
    /// <param name="status"></param>
    void on_status_changed(NETWORK_EVENT status)
    {
        switch (status)
        {
            case NETWORK_EVENT.connected:
                {
                    Debug.Log("on connected");
                    CPacket msg = CPacket.create((short)PROTOCOL.CHAT_MSG_REQ);
                    msg.push("Hello!!");
                    this.gameserver.send(msg);
                }
                break;

            case NETWORK_EVENT.disconnected:
                {
                    Debug.Log("disconnect");
                }
                break;
        }
    }

    void on_message(CPacket msg)
    {
        // 제일 먼저 프로토콜 아이디를 꺼내온다.
        PROTOCOL protocol_id = (PROTOCOL)msg.pop_protocol_id();

        // 프로토콜에 따른 분기 처리
        switch (protocol_id)
        {
            case PROTOCOL.CHAT_MSG_ACK:
                {
                    string text = msg.pop_string();
                    GameObject.Find("GameMain").GetComponent<CGameMain>().on_receive_chat_msg(text);
                }
                break;
        }
    }

    public void send(CPacket msg)
    {
        this.gameserver.send(msg);
    }
}
