using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _endPointPrefab = null;

    private void Start()
    {
        StartCoroutine("LevelStatusController");
    }

    private IEnumerator LevelStatusController()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);

            if (DistanceController())
            {
                break;
            }
        }
    }

    private bool DistanceController()
    {
        float distance = Mathf.Abs(GameManager.Character.transform.position.y - _endPointPrefab.transform.position.y);
        Debug.LogWarning(distance);
        return distance < 0.25f ? true : false;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void FinishLevel()
    {
        // TODO
        Debug.Log("Level Finished...");
    }

    public static void LoadNewLevel()
    {
        // TODO
    }
}
