using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class RoomSetupManager : MonoBehaviour
{
    [SerializeField]
    private GroupManager groupManager;
    [SerializeField]
    private GameObject voice;

    public void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            CreateUser();
            voice.SetActive(true);
        }
    }

    private void CreateUser()
    {
        PhotonNetwork.Instantiate(Path.Combine("Prefabs", "User"), Vector3.zero, Quaternion.identity);
        groupManager.SetUpGroups();
    }
}
