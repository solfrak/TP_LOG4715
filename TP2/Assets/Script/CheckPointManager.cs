using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class CheckPointManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> m_Checkpoints;
        private int m_CurrentCheckpoint = -1;

        public void Next()
        {
            m_CurrentCheckpoint++;
        }

        public void ResetPosition(GameObject gameObject)
        {
            gameObject.transform.position = m_Checkpoints[m_CurrentCheckpoint].transform.position;
        }
    }
}