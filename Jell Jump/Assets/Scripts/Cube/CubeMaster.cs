using UnityEngine;
using DG.Tweening;
using System.Collections;

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

    private MaterialPropertyBlock _blockMPB;
    private int _baseColorShaderID;
    private int _emissionColorShaderID;
    private Renderer _blockRenderer;

    private void Awake()
    {
        _blockMPB = new MaterialPropertyBlock();
        _baseColorShaderID = Shader.PropertyToID("_BaseColor");
        _emissionColorShaderID = Shader.PropertyToID("_EmissionColor");
        _blockRenderer = GetComponent<MeshRenderer>();
    }

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

        // TODO

        //StartCoroutine(IChangeColor(targetColor, duration));

        Debug.Log("Stoned effect on cube");
    }

    private IEnumerator IChangeColor(Color to, float duration)
    {
        while (false)
        {


            //Color changedBaseColor = Color.Lerp(from, to, t);
            //Color changedEmissionColor = Color.Lerp(from, to, t);

            //_blockMPB.SetColor(_baseColorShaderID, Color.red);
            //_blockMPB.SetColor(_emissionColorShaderID, Color.blue);

            //_blockRenderer.SetPropertyBlock(_blockMPB);

            yield return new WaitForEndOfFrame();
        }
    }

    // Enables Pale effect on Cube
    public void PaleEffectOnCube(float duration)
    {
        // TODO
       //  GetComponent<MeshRenderer>().material.DOColor(_paleColorEffect, duration);

        Debug.Log("Pale effect on cube");
    }

    // Makes Cube Interaction to None
    public void MakeCubeNonInteractable()
    {
        _impactCount = 0;
    }
}