using UnityEngine;

public class Reset : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.DeleteAll();
    }

    
}
