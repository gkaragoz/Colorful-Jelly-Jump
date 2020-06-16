/***********************************************************************************************************
 * JELLY CUBE - GAME STARTER KIT - Compatible with Unity 5                                                 *
 * Produced by TROPIC BLOCKS - http://www.tropicblocks.com - http://www.twitter.com/tropicblocks           *
 * Developed by Rodrigo Pegorari - http://www.rodrigopegorari.com                                          *
 ***********************************************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace JellyCube
{
    public class CheckPoint : MonoBehaviour
    {
        public string m_CubeTag = "Player"; //fill this variable according to the Cube tag

        [HideInInspector]
        public bool m_Success = false;

        private CubeController m_Cube;

        [System.Serializable]
        public class CheckPointSolvedEvent : UnityEvent<bool> { }

        public CheckPointSolvedEvent m_OnCheckPointSolved;

        private void Start()
        {
            CubeManager.Instance.RegisterCheckpPoint(this);

            SetCheckPointSolved(m_Success);
        }

        void OnTriggerEnter(Collider collider)
        {
            //check if the cube object has the same tag of this checkpoint var 'cubeTag'
            if (collider.tag == m_CubeTag)
            {
                m_Cube = collider.GetComponent<CubeController>();
                if (m_Cube != null && m_Cube.Collider == collider)
                {
                    SetCheckPointSolved(true);
                }
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (m_Cube != null && m_Cube.Collider == collider)
            {
                SetCheckPointSolved(false);
            }
        }

        void SetCheckPointSolved(bool solved)
        {
            m_Success = solved;

            m_OnCheckPointSolved.Invoke(solved);

            CubeManager.Instance.CheckPuzzleStatus();
        }
    }
}