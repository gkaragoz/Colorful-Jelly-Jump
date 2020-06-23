using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton

    public static UIManager instance;

    private void Awake()
    {
        if (instance == null) { instance = this; }

        else if (instance != this) { Destroy(gameObject); }
    }

    #endregion

    [SerializeField]
    private TextMeshProUGUI _pointText = null;

    // Stores UI's current state
    private UIState _state = UIState.UI_STARTGAME;

    public void UpdateHealthBar(float currentHealth)
    {
        // TODO
    }

    public void UpdateLevelBar(float currentDistance)
    {
        // TODO
    }

    public void UpdateTotalScore(int currentTotalScore)
    {
        // TODO
    }

    public void UpdateLevelScore(int currentScore)
    {
        // TODO
    }
}
