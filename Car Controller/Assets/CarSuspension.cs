using System;
using System.Collections.Generic;
using UnityEngine;

namespace CarPhysics
{
    public class CarSuspension : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Rigidbody m_RigidBody;
        [SerializeField] List<Transform> m_SuspensionTargets;
        [Header("Parameters")]
        [SerializeField] float m_ForceAmount;
        [SerializeField] float m_SuspensionLength;
        [Header("Visualization")]
        [SerializeField] List<Transform> m_TireTransforms; //Need to be in the same order as the suspension targets TODO Refactor

        public bool isGrounded { get; private set; }
        void FixedUpdate()
        {
            List<float> yOffsets = new List<float>();
            isGrounded = false;
            foreach (var target in m_SuspensionTargets)
            {
                RaycastHit hit;
                if (Physics.Raycast(target.position, -target.up, out hit, m_SuspensionLength))
                {
                    isGrounded = true;
                    Debug.DrawLine(target.position, hit.point, Color.green);
                    //Calculate the compression ratio
                    var distance = Vector3.Distance(target.position, hit.point);
                    var compressionRatio = (m_SuspensionLength - distance) / m_SuspensionLength;
                    m_RigidBody.AddForceAtPosition(target.up * m_ForceAmount * compressionRatio, target.position, ForceMode.Acceleration);
                    yOffsets.Add(distance);
                }
                else
                {
                    Debug.DrawLine(target.position, target.position + (-target.up * m_SuspensionLength), Color.red);
                    yOffsets.Add(m_SuspensionLength);
                }
            }
            
            UpdateTireYOffset(yOffsets);
        }

        void UpdateTireYOffset(List<float> offsets)
        {
            for (int i = 0; i < m_TireTransforms.Count; i++)
            {
                m_TireTransforms[i].localPosition = new Vector3(m_TireTransforms[i].localPosition.x, -offsets[i],
                    m_TireTransforms[i].localPosition.z);
            }
        }
    }
}
