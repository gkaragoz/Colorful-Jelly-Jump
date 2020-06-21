using UnityEngine;
using DG.Tweening;

public class CubeMaster : MonoBehaviour
{
    [SerializeField]
    private CubeState _cubeState = CubeState.NONEFFECTIVELY;

    [SerializeField]
    private int _damageRate = 0;

    [SerializeField]
    private int _pointsRate =0;

    [SerializeField]
    private int _impactCount = 1;

    private void OnCollisionEnter(Collision collision)
    {
        Character character = collision.collider.GetComponent<Character>();

        if(character != null && _impactCount != 0)
        {
            _impactCount--;

            switch (_cubeState)
            {
                case CubeState.DANGEROUSLY:
                    {
                        // Damage to character
                        character.DoDamage(_damageRate);

                        break;
                    }

                case CubeState.DEADLY:
                    {
                        // Dead to character
                        character.DoDamage(character.Health());

                        return;
                    }

                case CubeState.NONEFFECTIVELY:
                    {
                        break;
                    }

                case CubeState.USEFULLY:
                    {
                        // Give Points to character
                        character.IncreasePoint(_pointsRate);

                        break;
                    }

            }

            // Makes all block's child to deactivate and color changes
            GetComponentInParent<BlockMaster>().MakeAllChildToInactive();
        }
    }

    // Disables cube' collider
    public void DisableCollider()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    // Enables cube' collider
    public void EnableCollider()
    {
        GetComponent<BoxCollider>().enabled = true;
    }

    // Changes Cube Color
    public void DeactiveState(Color targetColor, float duration)
    {
        _impactCount = 0;

        GetComponent<MeshRenderer>().material.DOColor(targetColor, duration);
    }
}