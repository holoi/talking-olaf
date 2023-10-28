using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using ChatGPTWrapper;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    ChatGPTConversation chatGPT;

    [SerializeField]
    TMP_InputField iF_PlayerTalk;
    [SerializeField]
    TextMeshProUGUI tX_AIReply;
    [SerializeField]
    NPCController npc;

    string npcName = "Olaf";
    string playerName = "Player";

    [Header("Events")][SerializeField]
    UnityEvent StartTalkEvents;

    [HideInInspector]
    public string talkLine;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        //chatGPT._initialPrompt = string.Format(chatGPT._initialPrompt, npcName, playerName) + initialPrompt_CommonPart;

        //Enable ChatGPT
        chatGPT.Init();

    }

    private void Start()
    {
        //chatGPT.SendToChatGPT("{\"player_said\":" + "\"Hello! Who are you?\"}");
        //if (Application.isEditor)
        //{
        //    InitConversation();
        //}
    }

    public void InitConversation()
    {
        //Enable ChatGPT
        chatGPT.Init();

        chatGPT.SendToChatGPT("{\"player_said\":" + "\"Hello! Who are you?\"}");

        npc = FindObjectOfType<NPCController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!npc)
        {
            npc = FindObjectOfType<NPCController>();

        }
        if (Input.GetButtonUp("Submit"))
		{
			SubmitChatMessage();
		}
	}

    public void ReceiveChatGPTReply(string message)
    {
        //print(message);

        try
        {
            if (!message.EndsWith("}"))
            {
                if (message.Contains("}"))
                {
                    message = message.Substring(0, message.LastIndexOf("}") + 1);
                }
                else
                {
                    message += "}";
                }
            }
            NPCJSONReceiver npcJSON = JsonUtility.FromJson<NPCJSONReceiver>(message);
            talkLine = npcJSON.reply_to_player; // the works GPT replied
            // text to speech here...
            StartTalkEvents?.Invoke();

            tX_AIReply.text = "<color=#ff7082>" + npcName + ": </color>" + talkLine;

            npc.ShowAnimation(npcJSON.animation_name);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            string talkLine = "Don't say that!";
            tX_AIReply.text = "<color=#ff7082>" + npcName + ": </color>" + talkLine;
        }
    }

    public void SubmitChatMessage()
    {
        if (iF_PlayerTalk.text != "")
        {
            chatGPT.SendToChatGPT("{\"player_said\":\"" + iF_PlayerTalk.text + "\"}");
            ClearText();
        }
    }

    void ClearText()
    {
        iF_PlayerTalk.text = "";
    }
}