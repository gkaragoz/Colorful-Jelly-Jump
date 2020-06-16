/***********************************************************************************************************
 * JELLY CUBE - GAME STARTER KIT - Compatible with Unity 5                                                 *
 * Produced by TROPIC BLOCKS - http://www.tropicblocks.com - http://www.twitter.com/tropicblocks           *
 * Developed by Rodrigo Pegorari - http://www.rodrigopegorari.com                                          *
 ***********************************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace JellyCube
{
    public class CubeManager : MonoBehaviour
    {
        public static CubeManager Instance;

        public Transform m_LevelRoot;

        [HideInInspector]
        public int m_NumberOfMoves = 0;

        //All cubes in scene
        private List<CubeController> m_CubeControllers;

        private List<CheckPoint> m_CheckPoints;

        //All cubes moving or rolling in scene
        private List<CubeController> m_CubeMoving;

        private bool m_PuzzleSolved = false;

        private bool m_DoCheckPuzzleSolution = false;

        [System.Serializable]
        public class PuzzleSolvedEvent : UnityEvent<bool> {}

        public PuzzleSolvedEvent m_OnPuzzleSolved;

        private void Awake()
        {
            Instance = this;

            m_CubeControllers = new List<CubeController>();
            m_CheckPoints = new List<CheckPoint>();
            m_CubeMoving = new List<CubeController>();

            if (m_LevelRoot == null)
            {
                Debug.LogError("Please place all your level content inside an empty object");
            }
        }

        //Add the CubeController into the general list. The Cubes never are removed from the list.
        public void RegisterCube(CubeController controller)
        {
            m_CubeControllers.Add(controller);
        }

        public void RegisterCheckpPoint(CheckPoint checkPoint)
        {
            m_CheckPoints.Add(checkPoint);
        }

        //Add the CubeController (that is Moving or Rolling) into the moving list
        public void RegisterMove(CubeController controller)
        {
            InputManager.Instance.LockControls();

            m_CubeMoving.Add(controller);
        }

        //Remove the CubeController (that is Moving or Rolling) from the moving list
        public void UnregisterMove(CubeController controller)
        {
            m_CubeMoving.Remove(controller);

            //When the cube moving list count is zero, means there is no cubes moving
            if (m_CubeMoving.Count == 0)
            {
                InputManager.Instance.UnlockControls();

                if (m_DoCheckPuzzleSolution)
                {
                    CheckPuzzleSolution();
                }
            }
        }

        public void CheckPuzzleStatus()
        {
            m_DoCheckPuzzleSolution = true;
        }

        private void CheckPuzzleSolution()
        {
            m_DoCheckPuzzleSolution = false;

            //if m_CheckGame variable is enabled, means a check box collider was triggered
            int activeCheckpointsNumber = 0;

            //Check all checkpoints in scene and verify if the checkpoint was fullfilled with a box
            foreach (CheckPoint cp in m_CheckPoints)
            {
                //m_Success is true when a Cube Collider has the same Tag name than the field m_CubeTag in the CheckPoint.cs 
                if (cp.m_Success)
                {
                    activeCheckpointsNumber++;
                }
            }

            //If the number of success checkpoints is the same of the checkpoint number, the level is complete
            if (activeCheckpointsNumber == m_CheckPoints.Count && !m_PuzzleSolved)
            {
                m_PuzzleSolved = true;
                m_OnPuzzleSolved.Invoke(true);

                Debug.Log("Puzzle Solved!");

                //Lock controls
                //InputManager.Instance.LockControls();

                //End of level effect transition
                //StartCoroutine(LevelComplete());
            }
            else if (activeCheckpointsNumber != m_CheckPoints.Count && m_PuzzleSolved)
            {
                m_PuzzleSolved = false;
                m_OnPuzzleSolved.Invoke(false);

                Debug.Log("Puzzle Not Solved!");
            }
        }

        public void Move(Vector3 inputDirection)
        {
            //If there is no cube interaction, can move
            if (m_CubeMoving.Count == 0)
            {
                int countMovingCubes = 0;

                foreach (CubeController controller in m_CubeControllers)
                {
                    if (controller.m_CanControl)
                    {
                        CubeController moved = controller.DoMove(inputDirection);

                        if (moved != null)
                        {
                            countMovingCubes++;
                        }
                    }
                }

                if (countMovingCubes > 0)
                {
                    m_NumberOfMoves++;

                    //Implement a rule to limit the number of moves if you wish
                    //Debug.Log(m_NumberOfMoves);
                }
            }
        }

    }
}