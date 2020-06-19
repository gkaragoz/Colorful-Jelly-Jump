using UnityEngine;

public class CubeMaster : MonoBehaviour
{
    [SerializeField]
    private CubeState _cubeState;

    [SerializeField]
    private int _damageRate;

    [SerializeField]
    private int _pointsRate;

    [SerializeField]
    private int _impactCount = 1;

    private void OnCollisionEnter(Collision collision)
    {
        Character character = collision.collider.GetComponent<Character>();

        if(character != null && _impactCount != 0)
        {
            _impactCount--;

            switch (_cubeState)
            {
                case CubeState.DANGEROUSLY:
                    {
                        // Damage to character
                        character.DoDamage(_damageRate);

                        break;
                    }

                case CubeState.DEADLY:
                    {
                        // Dead to character
                        character.DoDamage(character.Health());

                        break;
                    }

                case CubeState.NONEFFECTIVELY:
                    {
                        break;
                    }

                case CubeState.USEFULLY:
                    {
                        // Give Points to character
                        character.IncreasePoint(_pointsRate);

                        break;
                    }

            }
        }
    }
}