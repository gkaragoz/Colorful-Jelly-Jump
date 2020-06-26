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

    [SerializeField]
    private Color _initialBaseColor = Color.cyan;

    [SerializeField]
    private Color _targetBaseColor = Color.cyan;

    [SerializeField]
    private Vector3 _initialEmissionColor = Vector3.zero;

    [SerializeField]
    private Vector3 _targetEmissionColor = Vector3.zero;

    //

    private MaterialPropertyBlock _blockMPB;

    private int _baseColorShaderID;

    private int _emissionColorShaderID;

    private Renderer _cubeRenderer;

    //

    private void Awake()
    {
        _blockMPB = new MaterialPropertyBlock();

        _baseColorShaderID = Shader.PropertyToID("_BaseColor");

        _emissionColorShaderID = Shader.PropertyToID("_EmissionColor");

        _cubeRenderer = GetComponent<MeshRenderer>();
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
    public void DeactiveState()
    {
        _impactCount = 0;

        StartCoroutine("IChangeStoneColor");
    }

    private IEnumerator IChangeStoneColor()
    {
        float i = 0;

        while (true)
        {
            Color changedBaseColor = Color.Lerp(_initialBaseColor, _targetBaseColor, i);

            Vector3 changedEmissionColor = Vector3.zero;

            _blockMPB.SetColor(_baseColorShaderID, changedBaseColor);

            _blockMPB.SetColor(_emissionColorShaderID, new Color(changedEmissionColor.x, changedEmissionColor.y,
                changedEmissionColor.z));

            _cubeRenderer.SetPropertyBlock(_blockMPB);

            yield return new WaitForSeconds(.05f);

            i += 0.1f;

            if (i >= 2)
            {
                break;
            }
        }
    }

    private IEnumerator IChangePaleColor()
    {
        float i = 0;

        while (true)
        {
            Color changedBaseColor = Color.Lerp(_initialBaseColor, _paleColorEffect, i);

            _blockMPB.SetColor(_baseColorShaderID, changedBaseColor);

            _cubeRenderer.SetPropertyBlock(_blockMPB);

            yield return new WaitForSeconds(.05f);

            i += 0.1f;

            if (i >= 1)
            {
                break;
            }
        }
    }

    // Enables Pale effect on Cube
    public void PaleEffectOnCube(float duration)
    {
        StartCoroutine("IChangePaleColor");
    }

    // Makes Cube Interaction to None
    public void MakeCubeNonInteractable()
    {
        _impactCount = 0;
    }
}