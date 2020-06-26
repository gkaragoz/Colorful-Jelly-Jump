using System.Collections.Generic;
using UnityEngine;

public class VFXSplatterOnCollision : MonoBehaviour {

    [SerializeField]
    private ParticleSystem _VFXDecal = null;
    [SerializeField]
    private float _decalSizeMin = .5f;
    [SerializeField]
    private float _decalSizeMax = 1.5f;
    [SerializeField]
    private Gradient _gradient = null;

    private ParticleSystem _VFXSplatter = null;
    private List<ParticleCollisionEvent> _collisionEvents = null;

    private void Start() {
        _collisionEvents = new List<ParticleCollisionEvent>();

        _VFXSplatter = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other) {
        ParticlePhysicsExtensions.GetCollisionEvents(_VFXSplatter, other, _collisionEvents);

        for (int ii = 0; ii < _collisionEvents.Count; ii++) {
            _VFXDecal.transform.position = _collisionEvents[ii].intersection + (Vector3.up * 0.01f);
            var main = _VFXDecal.main;
            main.startSize = Random.Range(_decalSizeMin, _decalSizeMax);
            main.startRotationZ = Random.Range(0f, 360f); 
            main.startColor = _gradient.Evaluate(Random.Range(0f, 1f));

            _VFXDecal.Emit(1);
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            Play(Vector3.right, Random.Range(5, 15), Random.Range(10, 25));
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            Play(Vector3.left, Random.Range(5, 15), Random.Range(10, 25));
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            Play(Vector3.up, Random.Range(5, 15), Random.Range(10, 25));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            Play(Vector3.down, Random.Range(5, 15), Random.Range(10, 25));
        }
        if (Input.GetKeyDown(KeyCode.Keypad0)) {
            Play(Vector3.forward, Random.Range(5, 15), Random.Range(10, 25));
        }
        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            Play(Vector3.forward * -1f, Random.Range(5, 15), Random.Range(10, 25));
        }
    }

    private void EmitAtLocation(Vector3 direction, float initialSpeed, int emissionAmount) {
        var main = _VFXSplatter.main;
        main.startColor = _gradient.Evaluate(Random.Range(0f, 1f));
        main.startSpeedMultiplier = initialSpeed;

        var shape = _VFXSplatter.shape;
        shape.rotation = Quaternion.LookRotation(direction).eulerAngles;

        _VFXSplatter.Emit(emissionAmount);
    }

    public void Play(Vector3 direction, float initialSpeed, int emissionAmount = 15) {
        EmitAtLocation(direction, initialSpeed, emissionAmount);
    }

}
