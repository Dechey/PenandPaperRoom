using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;
using Photon.Voice.PUN;
using Photon;
using Photon.Pun;
using Photon.Voice;


public class VoiceTest : MonoBehaviour
{
    [SerializeField]
    private Recorder recorder;

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log(recorder.InterestGroup.ToString());
            recorder.InterestGroup = 1;
        }
        gameObject.GetComponent(typeof(Recorder));
    }
}
