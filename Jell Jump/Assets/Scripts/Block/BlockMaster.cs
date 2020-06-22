using System.Collections;
using UnityEngine;

public class BlockMaster : MonoBehaviour
{
    [SerializeField]
    private Color _paleColor = Color.gray;

    [SerializeField]
    private BlockState _blockState = BlockState.NORMAL;

    private void Start()
    {
        StartCoroutine("OnCharacterDetect");
    }

    // Makes all cubes to inactive
    public void MakeAllChildToInactive()
    {
        int i = 1;

        foreach (CubeMaster cube in GetComponentsInChildren<CubeMaster>())
        {
            switch (_blockState)
            {
                case BlockState.NORMAL:
                    {
                        // Makes cube interaction to none
                        cube.MakeCubeNonInteractable();

                        break;
                    }

                case BlockState.STONED:
                    {
                        // Invokes cube's deactive states
                        cube.DeactiveState(_paleColor, i * 0.75f);

                        // DEACTIVATE CHARACTER DETECTION CONTROL
                        StopCoroutine("OnCharacterDetect");

                        break;
                    }

                case BlockState.FRAGILE:
                    {
                        // TODO

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

            if (GameManager.Character.transform.position.y >= transform.position.y + 0.35)
            {
                foreach (CubeMaster cube in GetComponentsInChildren<CubeMaster>())
                {
                    // Invokes cube's deactive states
                    cube.EnableCollider();
                }

                shouldCheck = true;
            }

            else if(GameManager.Character.transform.position.y <= transform.position.y - 0.35)
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
}
