using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Character _character;

    private void Awake()
    {
        _character = FindObjectOfType<Character>();
    }

    private void Start()
    {
        _character.OnCharacterDeathState += RestartLevel;
    }

    private void RestartLevel()
    {
        StartCoroutine("StartLevelTimer");
    }

    private IEnumerator StartLevelTimer()
    {
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
