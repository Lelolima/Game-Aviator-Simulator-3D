using UnityEngine;
using UnityEngine.UI;

public class CameraSeguir : MonoBehaviour
{
    public Transform alvo;
    public Vector3 offset;

    void LateUpdate()
    {
        transform.position = alvo.position + offset;
        transform.LookAt(alvo);
    }
}
