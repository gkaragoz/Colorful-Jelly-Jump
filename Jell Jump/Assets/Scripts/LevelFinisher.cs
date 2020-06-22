using UnityEngine;

public class LevelFinisher : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        Character character = collider.GetComponent<Character>();

        if (character != null)
            LevelManager.FinishLevel();
    }
}
