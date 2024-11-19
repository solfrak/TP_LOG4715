using System;
using UnityEngine;

namespace Script
{
    public class Checkpoint : MonoBehaviour
    {
        private CheckPointManager m_Manager;
        private bool isActive = false;
        private void Awake()
        {
            m_Manager = GameObject.Find("Checkpoint Manager").GetComponent<CheckPointManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isActive)
            {
                m_Manager.Next();
                isActive = true;
            }
        }
    }
}