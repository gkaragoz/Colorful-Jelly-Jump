using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private float _targetFowDistanceOnGameOver = 20;

    [SerializeField]
    private float _fowAnimationDuration = 1.25f;

    [SerializeField]
    private CinemachineVirtualCamera vcam;

    public void SET(float value)
    {
        FindObjectOfType<CinemachineVirtualCamera>().m_Lens.FieldOfView = value;
    }

    public float GET()
    {
        return FindObjectOfType<CinemachineVirtualCamera>().m_Lens.FieldOfView;
    }

    // Handles camera states transition
    public void CameraAction(CameraState currentState, TweenCallback OnComplete)
    {
        switch (currentState)
        {
            case CameraState.ONNORMAL: // Normal Mode Gameplay Camera Actions
                {
                    return;
                }

            case CameraState.ONFOCUS: // OnFocus Mode Camera Actions
                {
                    return;
                }

            case CameraState.ONGAMEOVER: // GameOver Mode Camera Actions
                {
                    vcam.LookAt = GameManager.Character.transform;

                    DOTween.To(GET, SET, _targetFowDistanceOnGameOver, 1.25f).OnComplete(OnComplete);

                    return;
                }
        }
    }
}
