using UnityEngine;

public class BlockMaster : MonoBehaviour
{
    [SerializeField]
    private Color _paleColor;

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
    }

    private void OnTriggerEnter(Collider collider)
    {
        Character character = collider.GetComponent<Character>();

        if (character != null)
        {
            GetComponent<BoxCollider>().enabled = false;

            foreach (CubeMaster cube in GetComponentsInChildren<CubeMaster>())
            {
                // Invokes cube's deactive states
                cube.EnableCollider();
            }
        }
    }
}
