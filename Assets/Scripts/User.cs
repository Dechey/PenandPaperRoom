using Photon.Pun;
using Photon.Voice.Unity;
using UnityEngine;

public class User : MonoBehaviour
{
    [SerializeField]
    private VoiceConnection voice;
    [SerializeField]
    private int userNr;
    private string userName;

    // Start is called before the first frame update
    void Start()
    {
        voice = GameObject.FindGameObjectWithTag("Voice").GetComponent<VoiceConnection>();
        userNr = transform.GetComponent<PhotonView>().ViewID;
        transform.name = "User: " + userNr;
        GroupManager groupManager = GameObject.FindGameObjectWithTag("GroupManager").GetComponent<GroupManager>();
        groupManager.SetUpGroups();
        SetUserName(transform.name);
        ActivateChat activateChat = GameObject.FindGameObjectWithTag("ChatManager").GetComponent<ActivateChat>();
        activateChat.Activate();
    }

    public void SetInterestGroup(byte interestGroup)
    {
        voice.SetInterestGroup(interestGroup);
    }

    public byte GetInterestGroup()
    {
        if(voice != null)
        {
            return voice.GetInterestGroup();
        }
        else
        {
            return 0;
        }
    }

    public int GetUserNr()
    {
        return userNr;
    }

    public string GetUserName()
    {
        return userName;
    }

    public void SetUserName(string newUserName)
    {
        userName = newUserName;
    }
}
