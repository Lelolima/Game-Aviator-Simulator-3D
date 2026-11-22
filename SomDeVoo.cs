using UnityEngine;
using UnityEngine.UI;

public class SomDeVoo : MonoBehaviour
{
    public AudioSource motorAudio;
    public Rigidbody aeronaveRigidbody;

    void Update()
    {
        motorAudio.pitch = 1 + (aeronaveRigidbody.velocity.magnitude / 100);
    }
}
