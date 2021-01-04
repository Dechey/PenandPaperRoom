using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    private ChatClient chatClient;
    [SerializeField]
    private string userID;
    [SerializeField]
    private GameObject chatPanel;
    private string selectedChannel;
    [SerializeField]
    private Text currentChannelText;
    private readonly Dictionary<string, Toggle> channelToggles = new Dictionary<string, Toggle>();
    [SerializeField]
    private InputField inputFieldChat;


    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == DebugLevel.ERROR)
        {

        }
        else if (level == DebugLevel.WARNING)
        {
            Debug.LogWarning(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    public void OnChatStateChange(ChatState state)
    {
        //useless
    }

    public void OnConnected()
    {
        chatClient.Subscribe("Channel1");
        ShowChannel(selectedChannel);
    }

    public void OnDisconnected()
    {
        //useless
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName.Equals(this.selectedChannel))
        {
            // update text
            this.ShowChannel(this.selectedChannel);
        }
    }

    public void ShowChannel(string channelName)
    {
        if (string.IsNullOrEmpty(channelName))
        {
            return;
        }
        ChatChannel channel = null;
        bool found = this.chatClient.TryGetChannel(channelName, out channel);
        if (!found)
        {
            Debug.Log("ShowChannel failed to find channel: " + channelName);
            return;
        }

        //channel = chatChannels.GetChannel(channelName);
        this.selectedChannel = channelName;
        this.currentChannelText.text = channel.ToStringMessages();
        Debug.Log("ShowChannel: " + this.selectedChannel);

        foreach (KeyValuePair<string, Toggle> pair in this.channelToggles)
        {
            pair.Value.isOn = pair.Key == channelName ? true : false;
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        byte[] msgBytes = message as byte[];
        if (msgBytes != null)
        {
            Debug.Log("Message with byte[].Length: " + msgBytes.Length);
        }
        if (this.selectedChannel.Equals(channelName))
        {
            this.ShowChannel(channelName);
        }
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        //useless
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        selectedChannel = channels[0];
        Debug.Log("Subscribed to: " + channels[0] + " result: " + results[0]);
    }

    public void OnUnsubscribed(string[] channels)
    {
        Debug.Log("Unsubscribed from channel: " + channels[0]);
    }

    public void OnUserSubscribed(string channel, string user)
    {
        //useless
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        //useless
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        GameObject[] users = GameObject.FindGameObjectsWithTag("User");
        for(int i = 0; i < users.Length; i++)
        {
            if (users[i].GetComponent<PhotonView>().IsMine)
            {
                this.userID = users[i].GetComponent<User>().GetUserName();
            }
        }

        this.chatClient = new ChatClient(this);
        chatClient.ChatRegion = "EU";
        Connect();
    }

    public void Connect()
    {
        chatClient.AuthValues = new AuthenticationValues();
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues());
        chatClient.AuthValues.UserId = userID;
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    // Update is called once per frame
    void Update()
    {
        if(chatClient != null)
        {
            chatClient.Service();
        }
        OnEnterSend();
    }

    //Subscribe to channel
    public void Subscribe(string channel)
    {
        this.chatClient.Subscribe(new string[] { channel });
        selectedChannel = channel;
    }

    public void Unsubscribe(string channel)
    {
        chatClient.Unsubscribe(new string[] { channel });
    }

    public void AddMessageToSelectedChannel(string msg)
    {
        ChatChannel channel = null;
        bool found = this.chatClient.TryGetChannel(this.selectedChannel, out channel);
        if (!found)
        {
            Debug.Log("AddMessageToSelectedChannel failed to find channel: " + this.selectedChannel);
            return;
        }

        if (channel != null)
        {
            channel.Add("bot", msg, 0); //TODO: how to use msgID?
        }
    }

    public void OnDestroy()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Disconnect();
        }
    }

    public void OnApplicationQuit()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Disconnect();
        }
    }

    public void OnEnterSend()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            Debug.Log("send: " + this.inputFieldChat.text);
            this.SendChatMessage(this.inputFieldChat.text);
            this.inputFieldChat.text = "";
        }
    }

    public void OnClickSend()
    {
        if (this.inputFieldChat != null)
        {
            this.SendChatMessage(this.inputFieldChat.text);
            this.inputFieldChat.text = "";
        }
    }

    private void SendChatMessage(string inputLine)
    {
        if (string.IsNullOrEmpty(inputLine))
        {
            return;
        }
        bool doingPrivateChat = this.chatClient.PrivateChannels.ContainsKey(this.selectedChannel);
        string privateChatTarget = string.Empty;
        if (doingPrivateChat)
        {
            string[] splitNames = this.selectedChannel.Split(new char[] { ':' });
            privateChatTarget = splitNames[1];
        }

        if (doingPrivateChat)
        {
            this.chatClient.SendPrivateMessage(privateChatTarget, inputLine);
        }
        else
        {
            this.chatClient.PublishMessage(this.selectedChannel, inputLine);
            ShowChannel(selectedChannel);
        }
    }
}
