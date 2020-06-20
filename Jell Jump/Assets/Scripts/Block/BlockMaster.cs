using UnityEngine;

public class BlockMaster : MonoBehaviour
{
    // Makes all cubes to inactive
    public void MakeAllChildToInactive()
    {
        int i = 1;

        foreach (CubeMaster cube in GetComponentsInChildren<CubeMaster>())
        {
            // Invokes cube's deactive states
            cube.DeactiveState(new Color(.83f, .82f, .68f), i * 0.75f);

            i++;
        }
    }
}
