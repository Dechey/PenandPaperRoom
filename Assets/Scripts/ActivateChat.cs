using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateChat : MonoBehaviour
{
    [SerializeField]
    private GameObject child;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Activate()
    {
        child.SetActive(true);
    }
}
