using UnityEngine;

public class UILookAt : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.LookAt(
            transform.position + Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up
        );
    }
}
