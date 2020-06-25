using UnityEngine;
using DG.Tweening;

public class CubeMaster : MonoBehaviour
{
    [SerializeField]
    private CubeState _cubeState = CubeState.NONEFFECTIVELY;

    [SerializeField]
    private int _damageRate = 0;

    [SerializeField]
    private int _pointsRate = 0;

    [SerializeField]
    private int _impactCount = 1;

    [SerializeField]
    private Color _paleColorEffect = Color.cyan;

    private void OnCollisionEnter(Collision collision)
    {
        Character character = collision.collider.GetComponent<Character>();

        if(GameManager.instance._gameState == GameState.ONPLAY)
        {
            if (character != null && _impactCount != 0)
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
                            character.DeadCharacter();

                            return;
                        }

                    case CubeState.NONEFFECTIVELY:
                        {
                            break;
                        }

                    case CubeState.USEFULLY:
                        {
                            // Give Level Points to character
                            character.IncreaseLevelPoint(_pointsRate);

                            // Activate combo
                            character.ComboActivater(_pointsRate);

                            break;
                        }

                    case CubeState.NICELY:
                        {
                            // Give Level Points to character
                            character.IncreaseLevelPoint(_pointsRate);

                            // Activate combo
                            character.ComboActivater(_pointsRate);

                            character.FeverJump();

                            break;
                        }

                }

                // Makes all block's child to deactivate and color changes
                GetComponentInParent<BlockMaster>().InitalizeBlockAction();
            }
        }

        else
        {
            if(GetComponentInParent<BlockMaster>()._blockState == BlockState.ENDGAME)
            {
                // Makes all block's child to deactivate and color changes
                GetComponentInParent<BlockMaster>().InitalizeBlockAction();
            }
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

    // Enables Pale effect on Cube
    public void PaleEffectOnCube(float duration)
    {
        // TODO
        GetComponent<MeshRenderer>().material.DOColor(_paleColorEffect, duration);
    }

    // Makes Cube Interaction to None
    public void MakeCubeNonInteractable()
    {
        _impactCount = 0;
    }
}