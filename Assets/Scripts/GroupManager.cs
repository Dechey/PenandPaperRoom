using ExitGames.Client.Photon.StructWrapping;
using Photon.Chat;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class GroupManager : MonoBehaviour//, Photon.Pun.IPunObservable
{
    [SerializeField]
    private User[] user; //all users in the room
    private byte[] groups; //groups in which each user is
    [SerializeField]
    private ChatManager chatManager;
    public byte test;

    // Start is called before the first frame update
    void Start()
    {
        test = 1;
        SetUpGroups();
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            User[] utest = new User[1];
            for (int i = 0; i < user.Length; i++)
            {
                if (user[i].GetComponentInParent<PhotonView>().IsMine)
                {
                    utest[0] = user[i];
                }
            }
            test++;
            SetGroup(user, test);
        }
    }

    //All selected users interestGroup is set to the given group, the groups array get updated to know, which user is in which group
    public void SetGroup(User[] users, byte group)
    {
        for(int i = 0; i < users.Length; i++)
        {
            users[i].SetInterestGroup(group);
            groups[i] = group;
        }
        SetChatChannel(users, group);
    }

    //The user and groups array got updated, will be used when a new user joins the room
    public void SetUpGroups()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("User");
        user = new User[temp.Length];
        groups = new byte[temp.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            user[i] = temp[i].GetComponent<User>();
            groups[i] = user[i].GetInterestGroup();
        }
        SortUser();
    }

    //A free group is determined and all selected users go in this group
    public void NewGroup(User[] users)
    {
        byte group = 0;
        for(int i = 0; i < groups.Length; i++)
        {
            if(groups[i] == group)
            {
                group++;
            }
        }

        SetGroup(users, group);
    }

    //Sort user by PhotonViewId
    public void SortUser()
    {
        if(user.Length > 0)
        {
            User[] temp = new User[user.Length];
            bool[] sorted = new bool[user.Length];

            for (int i = 0; i < sorted.Length; i++)
            {
                sorted[i] = false;
            }

            int smalest = user[0].GetUserNr();
            int pos = 0;

            for (int i = 0; i < temp.Length; i++)
            {
                for (int j = 0; j < user.Length; j++)
                {
                    if (!sorted[j] && user[j].GetUserNr() < smalest)
                    {
                        smalest = user[j].GetUserNr();
                        pos = j;
                    }
                }

                temp[i] = user[pos];
                groups[i] = temp[i].GetInterestGroup();
                sorted[pos] = true;

                for (int j = 0; j < sorted.Length; j++)
                {
                    if (!sorted[j])
                    {
                        smalest = user[j].GetUserNr();
                        pos = j;
                        break;
                    }
                }
            }

            user = temp;
        }
    }

    public void SetChatChannel(User[] users, byte channelNr)
    {
        for(int i = 0; i < users.Length; i++)
        {
            if (users[i].GetComponentInParent<PhotonView>().IsMine)
            {
                string channel = "Channel" + channelNr;
                chatManager.Subscribe(channel);
                Debug.Log("user: " + users[i].GetUserName() + " gmSub: " + channel);
            }
        }
    }
}
