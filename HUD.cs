using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text velocidadeTexto;
    public Rigidbody aeronaveRigidbody;

    void Update()
    {
        float velocidadeAtual = aeronaveRigidbody.velocity.magnitude;
        velocidadeTexto.text = "Velocidade: " + velocidadeAtual.ToString("F2") + " m/s";
    }
}
