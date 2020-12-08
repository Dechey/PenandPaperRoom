using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class RoomSetupManager : MonoBehaviour
{
    public void Start()
    {
        CreateUser();
    }

    private void CreateUser()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Creating User");
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "User"), Vector3.zero, Quaternion.identity);
        }
    }
}
