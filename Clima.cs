using UnityEngine;
using UnityEngine.UI;

public class Clima : MonoBehaviour
{
    public ParticleSystem chuva;
    public Light direcionalLuz;

    void Start()
    {
        // Ativar ou desativar a chuva
        chuva.gameObject.SetActive(Random.value > 0.5f);

        // Ajustar a intensidade da luz para simular neblina
        direcionalLuz.intensity = Random.Range(0.5f, 1.0f);
    }
}
