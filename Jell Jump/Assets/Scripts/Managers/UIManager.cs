using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _healtText = null;

    [SerializeField]
    private TextMeshProUGUI _pointText = null;

    private Character _myCharacter;

    private void Awake()
    {
        _myCharacter = FindObjectOfType<Character>();
    }

    private void Update()
    {
        _healtText.text = "Health: " + _myCharacter.Health();

        _pointText.text = "Point: " + _myCharacter.TotalPoint();
    }
}
