using UnityEngine;
using UnityEngine.Serialization;

namespace VLB.Samples
{
    public class Rotater : MonoBehaviour
    {
        [FormerlySerializedAs("m_EulerSpeed")]
        public Vector3 EulerSpeed = Vector3.zero;

        void Update()
        {
            /*
            var euler = transform.rotation.eulerAngles;
            euler += EulerSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(euler);
            */

            transform.rotation.SetEulerAngles(new Vector3(transform.rotation.x + EulerSpeed.x * Time.deltaTime, transform.rotation.y + EulerSpeed.y * Time.deltaTime, transform.rotation.z + EulerSpeed.z * Time.deltaTime));

        }
    }
}
