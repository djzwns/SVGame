using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FreeNet;
using GameServer;

public class CGameMain : MonoBehaviour
{
    //string input_text;
    List<string> received_texts;
    CNetworkManager network_manager;
    Vector2 currentScrollPos = new Vector2();

    public InputField input_field;
    public RectTransform scroll_rect;
    Vector2 rectPosition = new Vector2();
    
    public GameObject speech_bubble;
    public GameObject speech_attach;

    private void Awake()
    {
        //this.input_text = "";
        this.input_field.text = "";
        this.received_texts = new List<string>();
        this.network_manager = GameObject.Find("NetworkManager").GetComponent<CNetworkManager>();
        rectPosition = scroll_rect.position;

        speech_bubble.SetActive(false);
    }

    public void on_receive_chat_msg(string text)
    {
        this.received_texts.Add(text);
        this.currentScrollPos.y = float.PositiveInfinity;
    }
    
    private void OnGUI()
    {
        // Received text.
        GUI.BeginGroup(new Rect(rectPosition, scroll_rect.rect.size));

        GUILayout.BeginVertical();
        currentScrollPos = GUILayout.BeginScrollView(currentScrollPos, GUILayout.MaxWidth(scroll_rect.rect.width), GUILayout.MaxHeight(scroll_rect.rect.height));
        
        foreach (string text in this.received_texts)
        {
            GUILayout.BeginHorizontal();
            GUI.skin.label.wordWrap = true;
            GUILayout.Label(text);
            GUILayout.EndHorizontal();
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();

        GUI.EndGroup();

        //// Input.
        //GUILayout.BeginHorizontal();
        //GUI.SetNextControlName("TextFieldCtr");
        //this.input_text = GUILayout.TextField(this.input_text, GUILayout.MaxWidth(scroll_rect.rect.width), GUILayout.MaxHeight(scroll_rect.rect.height));

        //if (GUILayout.Button("Send", GUILayout.MaxWidth(100), GUILayout.MinWidth(100), GUILayout.MaxHeight(50), GUILayout.MinHeight(50)))
        //{
        //    CPacket msg = CPacket.create((short)PROTOCOL.CHAT_MSG_REQ); msg.push(this.input_text); this.network_manager.send(msg);
        //    this.input_text = "";
        //}

        //GUILayout.EndHorizontal();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (input_field.isFocused)
            {
                input_field.DeactivateInputField();
            }
            else
            {
                this.input_field.ActivateInputField();
                this.input_field.Select();
            }
        }
    }

    private void InputSubmitCallBack()
    {
        if (this.input_field.text == "") return;

        SpeechBubbleUpdate(this.input_field.text);

        CPacket msg = CPacket.create((short)PROTOCOL.CHAT_MSG_REQ);
        msg.push(this.input_field.text);
        this.network_manager.send(msg);
        this.input_field.text = "";
    }

    IEnumerator speech;
    private void SpeechBubbleUpdate(string text)
    {
        if(speech != null)
            StopCoroutine(speech);

        speech = EnableObject(speech_bubble, 3f);
        StartCoroutine(speech);

        Text speechText = speech_bubble.GetComponentInChildren<Text>();
        speechText.text = text;
    }

    private IEnumerator EnableObject(GameObject obj, float delay)
    {
        obj.SetActive(true);

        Vector2 pos = Camera.main.WorldToScreenPoint(speech_attach.transform.position);
        obj.transform.position = pos;

        yield return new WaitForSeconds(delay);

        obj.SetActive(false);
    }

    void OnEnable()
    {
        input_field.onEndEdit.AddListener(delegate { InputSubmitCallBack(); });
    }

    void OnDisable()
    {
        input_field.onEndEdit.RemoveAllListeners();
    }
}
