using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject createRoomButton;
    [SerializeField]
    private GameObject joinRoomButton;
    [SerializeField]
    private int roomSize;
    public Text roomName;

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        createRoomButton.SetActive(true);
        joinRoomButton.SetActive(true);
    }

    public void CreateRoom()
    {
        Debug.Log("Creating new room");
        createRoomButton.SetActive(false);
        joinRoomButton.SetActive(false);

        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
        PhotonNetwork.CreateRoom(roomName.text, roomOps);
        Debug.Log("New room created" + roomName.text);
    }

    public void JoinRoom()
    {
        Debug.Log("Joining room now");
        createRoomButton.SetActive(false);
        joinRoomButton.SetActive(false);

        PhotonNetwork.JoinRoom(roomName.text);
        Debug.Log("Joined to " + roomName.text);
    }

    public int GetRoomSize()
    {
        return roomSize;
    }
}
