using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JellyCube
{
    public class CubeParticleEmitter : MonoBehaviour
    {
        public ParticleSystem m_ParticleSystem;
        public float m_HeightOffset = 0.1f;
        public Vector3 m_RotationOffset;

        public void Emit(CubeController cubeController)
        {
            ParticleSystem.EmitParams emitParameters = new ParticleSystem.EmitParams();
            emitParameters.position = cubeController.GetPositionOnFloor() + new Vector3(0, m_HeightOffset, 0);
            emitParameters.rotation3D = cubeController.transform.parent.rotation.eulerAngles + m_RotationOffset;
            m_ParticleSystem.Emit(emitParameters, 1);
        }
    }
}