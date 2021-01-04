using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int multiplayerSceneIndex;


    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        StartRoom();
    }

    private void StartRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Starting Room");
            PhotonNetwork.LoadLevel(multiplayerSceneIndex);
        }
    }
}
