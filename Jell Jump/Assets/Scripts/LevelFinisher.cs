using UnityEngine;

public class LevelFinisher : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        Character character = collider.GetComponent<Character>();

        if (GameManager.instance._gameState == GameState.ONPLAY)
        {
            if (character != null)
            {
                GameManager.instance.FinishGame();

                // Disable the script
                enabled = false;
            }
        }
        
            
    }
}
