/***********************************************************************************************************
 * JELLY CUBE - GAME STARTER KIT - Compatible with Unity 5                                                 *
 * Produced by TROPIC BLOCKS - http://www.tropicblocks.com - http://www.twitter.com/tropicblocks           *
 * Developed by Rodrigo Pegorari - http://www.rodrigopegorari.com                                          *
 ***********************************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace JellyCube
{
    /// <summary>
    /// This is the Main class of the game
    /// Here you set what happens when all Checkpoints are complete
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        //Next level name to be loaded after the level completion
        public string m_NextLevelName;

        public Animator m_Animator;

        private Dictionary<string,float> m_AnimationClipsLength =  new Dictionary<string,float>();

        private CameraController m_CameraController;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            foreach (AnimationClip clip in m_Animator.runtimeAnimatorController.animationClips)
            {
                m_AnimationClipsLength.Add(clip.name, clip.length);
            }

            m_CameraController = CameraController.Instance;
        }

        public void SetLevelCompleted(bool levelCompleted)
        {
            if (levelCompleted)
            {
                Debug.Log("Level complete!");

                //Lock controls
                InputManager.Instance.LockControls();
                InputManager.Instance.enabled = false;

                //End of level effect transition
                StartCoroutine(LevelComplete());

                //In this place you can implement a rule to limit the number of moves if you wish
                //Debug.Log(CubeManager.Instance.m_NumberOfMoves);
            }
        }

        public void ReloadLevel()
        {
            StartCoroutine(ReloadLevelRoutine());
        }

        private IEnumerator ReloadLevelRoutine()
        {
            InputManager.Instance.LockControls();
            
            m_Animator.Play("LevelFadeIn");

            //Wait for the current animation time (plus one second) then load the next level
            yield return new WaitForSeconds(m_Animator.GetCurrentAnimatorStateInfo(0).length + 1);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //Finish Animation Sequence
        IEnumerator LevelComplete()
        {
            m_Animator.Play("LevelComplete");

            if (m_CameraController != null)
            {
                m_CameraController.SetLevelCompleteCamera();
            }

            //Wait for the LevelComplete UI Animation
            yield return new WaitForSeconds(m_AnimationClipsLength["LevelComplete"]);

            if (m_NextLevelName != "")
            {
                SceneManager.LoadScene(m_NextLevelName);
            }
            else
            {
                Debug.Log("Assign the var 'NextLevelName' of the component 'GameManager.cs' to load a next level.");
            }
        }
    }
}