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
    public class CubeController : MonoBehaviour
    {
        public enum MovementType
        {
            Roll,
            Slide
        }

        public enum PushType
        {
            DontPushCubes,
            PushCubesWhenMove,  //This cube will try to push another cube when it stops moving (if the another cube can move)
            PushCubesAfterMove  //This cube will try to push another cube when it starts moving (if the another cube can move)
        }

        [Header("Actions")]

        [Tooltip("This cube can be controlled (cube player)")]
        public bool m_CanControl = true;

        [Tooltip("This cube will try to push another cube when moving or after move completion (if the another cube can be pushed)")]
        public PushType m_PushCubeType = PushType.PushCubesAfterMove;

        [Header("Reactions")]

        [Tooltip("This will shake after a collision with a cube (moving or rolling) and there is no space to move")]
        public bool m_CanShake = true;

        [Tooltip("This cube will move when another cube (moving or rolling) collides")]
        public bool m_CanBePushed = true;

        [Header("Settings")]

        public MovementType m_MoveType = MovementType.Roll;
        public float m_MoveSpeed = 0.15f;

        [HideInInspector]
        public Vector3 m_LastMove = Vector3.zero;

        [HideInInspector]
        public Vector3 m_LastInputDirection = Vector3.zero;

        private Transform m_Pivot;
        private Transform m_Contents;

        private float m_CubeSize;

        private const float SHAKE_SCALE = 1.2f;

        private Collider m_Collider;

        public Collider Collider { get => m_Collider; }

        public UnityEvent m_OnMoveStart;
        public UnityEvent m_OnMoveEnd;
        public UnityEvent m_OnMoveByImpactSuccess;
        public UnityEvent m_OnMoveByImpactFail;

        void Start()
        {
            m_Pivot = new GameObject(transform.name + "_Pivot").transform;
            m_Pivot.SetParent(transform);
            m_Pivot.transform.localScale = Vector3.one;
            m_Pivot.transform.localPosition = Vector3.zero;

            m_Contents = new GameObject(transform.name + "_Contents").transform;
            m_Contents.SetParent(m_Pivot);
            m_Contents.transform.localScale = Vector3.one;
            m_Contents.transform.localPosition = Vector3.zero;

            m_CubeSize = transform.lossyScale.x;

            m_Collider = GetComponent<Collider>();

            CubeManager.Instance.RegisterCube(this);

            SetRootToPivot(Vector3.zero);
            ResetPosition();
        }

        void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            Gizmos.DrawSphere(m_Pivot.transform.position, m_CubeSize / 10f);
        }


        public CubeController DoMove(Vector3 inputDirection)
        {
            //If there is a neighbor cube in this direction (CheckCollisionRecursive != null)
            //and this neighbor was pushed (DoPush != null)
            //then this cube can move too

            if (CheckCollisionRecursive(transform, inputDirection) != null)
            {
                bool didPush = false;

                if (m_PushCubeType.Equals(PushType.PushCubesWhenMove))
                {
                    CubeController pushedCube = DoPush(inputDirection);

                    didPush = pushedCube != null;
                }

                if (!didPush)
                {
                    return null;
                }
            }

            ResetPosition();

            CubeManager.Instance.RegisterMove(this);

            switch (m_MoveType)
            {
                case MovementType.Roll:
                    DoRoll(inputDirection);
                    break;

                case MovementType.Slide:
                    DoSlide(inputDirection);
                    break;
            }

            m_OnMoveStart.Invoke();

            return this;
        }

        private void SetRootToPivot(Vector3 inputDirection)
        {
            int index = transform.GetSiblingIndex();

            m_Pivot.SetParent(transform.parent);
            m_Pivot.localRotation = Quaternion.identity;
            m_Pivot.transform.position = transform.position + GetCubeDirection(inputDirection) * m_CubeSize / 2f;
            m_Pivot.transform.localPosition -= new Vector3(0, 0.5f, 0);

            transform.SetParent(m_Pivot);
            m_Pivot.SetSiblingIndex(index);
        }

        private void DoRoll(Vector3 inputDirection)
        {
            SetRootToPivot(inputDirection);

            Vector3 targetRotation = new Vector3(inputDirection.z, inputDirection.y, -inputDirection.x) * 90;

            m_LastInputDirection = inputDirection;

            //You can replace with iTween or any tweener you like
            Tweener.RotateTo(m_Pivot.gameObject, m_Pivot.localRotation.eulerAngles, targetRotation, m_MoveSpeed, 0, Tweener.TweenerEaseType.Linear, OnMoveComplete);
        }

        private void DoSlide(Vector3 inputDirection)
        {
            m_LastInputDirection = inputDirection;

            //You can replace with iTween or any tweener you like
            Tweener.MoveTo(this.gameObject, transform.position, transform.position + GetCubeDirection(inputDirection) * m_CubeSize, m_MoveSpeed, 0, Tweener.TweenerEaseType.EaseOutSine, OnMoveComplete);
        }

        public CubeController DoPush(Vector3 inputDirection)
        {
            CubeController didPush = null;

            Vector3 origin = m_Collider.transform.position;

            RaycastHit outHit = new RaycastHit();

            if (Physics.Linecast(origin, origin + GetCubeDirection(inputDirection) * m_CubeSize, out outHit))
            {
                //if has any collision object, look for a CubeController its parent object, and then try to move it
                CubeController cube = outHit.collider.transform.GetComponentInParent<CubeController>();

                if (cube != null)
                {
                    if (cube.m_CanBePushed)
                    {
                        didPush = cube.DoMove(inputDirection);

                        if (didPush)
                        {
                            cube.m_OnMoveByImpactSuccess.Invoke();
                        }
                        else
                        {
                            cube.m_OnMoveByImpactFail.Invoke();
                            cube.DoShake();
                        }
                    }
                    else if (!cube.m_PushCubeType.Equals(PushType.DontPushCubes))
                    {
                        cube.DoPush(inputDirection);
                    }
                    
                }
            }
            
            return didPush;
        }

        Vector3 GetCubeDirection(Vector3 inputDirection)
        {
            return CubeManager.Instance.m_LevelRoot.TransformDirection(inputDirection);
        }

        Collider CheckCollisionRecursive(Transform cubeTransform, Vector3 inputDirection, Collider lastFound = null)
        {
            Vector3 origin = cubeTransform.position;

            RaycastHit outHit;

            if (Physics.Linecast(origin, origin + GetCubeDirection(inputDirection) * m_CubeSize, out outHit))
            {
                CubeController cubeController = outHit.transform.gameObject.GetComponent<CubeController>();

                if (cubeController != null && cubeController.m_CanBePushed)
                {
                    return CheckCollisionRecursive(cubeController.transform, inputDirection, cubeController.m_Collider);
                }
                else
                {
                    return outHit.collider;
                }
            }
            else
            {
                return lastFound;
            }
        }

        /// <summary>
        /// Complete Method is called after a Move event
        /// </summary>
        private void OnMoveComplete()
        {
            ResetPosition();
            
            CubeManager.Instance.UnregisterMove(this);

            if (m_PushCubeType.Equals(PushType.PushCubesAfterMove))
            {
                DoPush(m_LastInputDirection);
            }

            m_OnMoveEnd.Invoke();
        }

        private void DoShake()
        {
            if (m_CanShake)
            {
                //You can replace with iTween or any tweener you like
                Tweener.ScaleTo(this.gameObject, new Vector3(SHAKE_SCALE, SHAKE_SCALE, SHAKE_SCALE), Vector3.one, .5f, 0, Tweener.TweenerEaseType.EaseOutExpo);
            }
        }

        public Vector3 GetPositionOnFloor()
        {
            return new Vector3(transform.position.x, m_Pivot.transform.position.y, transform.position.z);
        }

        private void ResetPosition()
        {
            transform.SetParent(m_Pivot.parent);
            transform.SetSiblingIndex(m_Pivot.GetSiblingIndex());
            m_Pivot.SetParent(transform);

            //Snap angles to 90 degrees
            transform.localEulerAngles = RoundVector(transform.localEulerAngles, 90f);
            m_Collider.transform.localEulerAngles = RoundVector(m_Collider.transform.localEulerAngles, 90f);

            //Snap position to .5 units
            this.transform.localPosition = RoundVector(this.transform.localPosition, .5f);
            this.m_Collider.transform.localPosition = RoundVector(this.m_Collider.transform.localPosition, .5f);
        }

        private Vector3 RoundVector(Vector3 value, float snapValue = 1f)
        {
            value.x = Mathf.Round(value.x / snapValue) * snapValue;
            value.y = Mathf.Round(value.y / snapValue) * snapValue;
            value.z = Mathf.Round(value.z / snapValue) * snapValue;

            return value;
        }
    }
}
 