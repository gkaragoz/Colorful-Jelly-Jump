using UnityEngine;
using Cinemachine;

public class CamManager : MonoBehaviour
{
    public void SET(float value)
    {
        FindObjectOfType<CinemachineVirtualCamera>().m_Lens.FieldOfView = value;
    }

    public float GET()
    {
        return FindObjectOfType<CinemachineVirtualCamera>().m_Lens.FieldOfView;
    }
}
