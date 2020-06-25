using System.Collections;
using UnityEngine;
using DG.Tweening;

public class BlockMaster : MonoBehaviour
{
    [SerializeField]
    private Color _paleColor = Color.gray;

    [SerializeField]
    public BlockState _blockState = BlockState.NORMAL;

    [SerializeField]
    private float _fragileBlockScaleDuration = 1f;

    [SerializeField]
    private float _fragileBlockWaitDuration = 1f;

    [SerializeField]
    private float _paleEffectDuration = 1f;

    private void Start()
    {
        if(_blockState != BlockState.ENDGAME)
        {
            StartCoroutine("OnCharacterDetect");
        }
    }

    // Makes all cubes to inactive
    public void InitalizeBlockAction()
    {
        int i = 1;

        // CubeList in the block
        CubeMaster[] cubeMasters = GetComponentsInChildren<CubeMaster>();

        foreach (CubeMaster cube in cubeMasters)
        {
            switch (_blockState)
            {
                case BlockState.NORMAL:
                    {
                        // Makes cube interaction to none
                        cube.MakeCubeNonInteractable();

                        // Pale efects on cube activater
                        cube.PaleEffectOnCube(_paleEffectDuration);

                        break;
                    }

                case BlockState.STONED:
                    {
                        // Invokes cube's deactive states
                        cube.DeactiveState(_paleColor, i * 0.75f);

                        if (i == cubeMasters.Length)
                        {
                            // DEACTIVATE CHARACTER DETECTION CONTROL
                            StopCoroutine("OnCharacterDetect");
                        }

                        break;
                    }

                case BlockState.FRAGILE:
                    {
                        // Invokes cube's deactive states
                        cube.DeactiveState(_paleColor, i * 0.75f);

                        if(i == cubeMasters.Length)
                        {
                            StartCoroutine("OnFragileBlock");
                        }

                        break;
                    }

                case BlockState.ENDGAME:
                    {
                        // Invokes cube's deactive states
                        cube.DeactiveState(_paleColor, 1);

                        break;
                    }
            }

            i++;
        }
    }

    // Fires when character detect
    private IEnumerator OnCharacterDetect()
    {
        bool shouldCheck = true;

        while (true)
        {
            yield return new WaitForSeconds(.1f);

            if (GameManager.instance.MyCharacter.transform.position.y >= transform.position.y + 0.35)
            {
                foreach (CubeMaster cube in GetComponentsInChildren<CubeMaster>())
                {
                    // Invokes cube's deactive states
                    cube.EnableCollider();
                }

                shouldCheck = true;
            }

            else if (GameManager.instance.MyCharacter.transform.position.y <= transform.position.y - 0.35)
            {
                if (shouldCheck)
                {
                    foreach (CubeMaster cube in GetComponentsInChildren<CubeMaster>())
                    {
                        // Invokes cube's deactive states
                        cube.DisableCollider();

                        shouldCheck = false;
                    }
                }
            }
        }
    }

    // Fires when fragile block actives
    private IEnumerator OnFragileBlock()
    {
        // DEACTIVATE CHARACTER DETECTION CONTROL
        StopCoroutine("OnCharacterDetect");

        yield return new WaitForSeconds(_fragileBlockWaitDuration);

        // Fragile Blocks
        transform.DOScale(0, _fragileBlockScaleDuration).OnComplete(DeactivateBlock);
    }

    // Activates blocks
    public void ActivateBlock()
    {
        enabled = true;
    }

    // Deactivates blocks
    public void DeactivateBlock()
    {
        enabled = false;
    }
}
