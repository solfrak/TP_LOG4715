using System;
using Script;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private CheckPointManager m_Manager;

    private void Awake()
    {
        m_Manager = GameObject.Find("Checkpoint Manager").GetComponent<CheckPointManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var myGameObject = other.gameObject;
        while (true)
        {
            if (myGameObject.name == "MaleFree1")
            {
                break;
            }
            if (myGameObject.transform.parent == null)
            {
                return;
            }
            
            myGameObject = myGameObject.transform.parent.gameObject;
        }
        
        
        m_Manager.ResetPosition(myGameObject);
    }
}
