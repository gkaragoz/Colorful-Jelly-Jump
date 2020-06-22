using System.Collections;
using UnityEngine;

public class BlockMaster : MonoBehaviour
{
    [SerializeField]
    private Color _paleColor = Color.gray;

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
            // Invokes cube's deactive states
            cube.DeactiveState(_paleColor, i * 0.75f);

            i++;
        }

        // TODO
        // DEACTIVATE CHARACTER DETECTION CONTROL
        StopCoroutine("OnCharacterDetect");
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
