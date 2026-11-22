using UnityEngine;
using UnityEngine.UI;

public class ControleDeVoo : MonoBehaviour
{
    public float velocidade = 10.0f;
    public float rotacaoVelocidade = 100.0f;

    void Update()
    {
        float movimentoFrontal = Input.GetAxis("Vertical") * velocidade * Time.deltaTime;
        float rotacao = Input.GetAxis("Horizontal") * rotacaoVelocidade * Time.deltaTime;

        transform.Translate(Vector3.forward * movimentoFrontal);
        transform.Rotate(Vector3.up * rotacao);
    }
}
