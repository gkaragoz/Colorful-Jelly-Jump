using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    #region Singleton

    public static CameraManager instance;

    private void Awake()
    {
        if (instance == null) {

            instance = this;

            // Assing current fow distance of vcam
            _defaultFowDistance = vcam.m_Lens.FieldOfView;
        }

        else if (instance != this) { Destroy(gameObject); }
    }

    #endregion

    [SerializeField]
    private float _targetFowDistanceOnGameOver = 20;

    private float _fowAnimationDuration = 3;

    [SerializeField]
    private float _targetFocusDistance = 30;

    [SerializeField]
    private float _focusAnimationDuration = .75f;

    // Stores camera fow distance value
    private float _defaultFowDistance = 0;

    [SerializeField]
    private CinemachineVirtualCamera vcam = null;


    public void SET(float value)
    {
        vcam.m_Lens.FieldOfView = value;
    }

    public float GET()
    {
        return vcam.m_Lens.FieldOfView;
    }

    // Handles camera states transition
    public void CameraAction(CameraAnimationState state, TweenCallback OnComplete)
    {
        switch (state)
        {
            case CameraAnimationState.ANIM_CLOSEFOCUS:
                {
                    vcam.LookAt = GameManager.instance.MyCharacter.transform;

                    DOTween.To(GET, SET, _targetFowDistanceOnGameOver, _fowAnimationDuration).OnComplete(() =>
                    {
                        // Invokes Tween Complete Callback
                        OnComplete?.Invoke();
                    });

                    return;
                }

            case CameraAnimationState.ANIM_FOCUS:
                {
                    DOTween.To(GET, SET, _targetFocusDistance, _focusAnimationDuration).OnComplete(() =>
                    {
                        // Invokes Tween Complete Callback
                        OnComplete?.Invoke();
                    });

                    return;
                }

            case CameraAnimationState.ANIM_REVERSEFOCUS:
                {
                    DOTween.To(GET, SET, _defaultFowDistance, _focusAnimationDuration).OnComplete(() =>
                    {
                        // Invokes Tween Complete Callback
                        OnComplete?.Invoke();
                    });

                    return;
                }

        }
    }
}
