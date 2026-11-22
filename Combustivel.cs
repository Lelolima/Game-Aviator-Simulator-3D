using UnityEngine;
using UnityEngine.UI;

public class Combustivel : MonoBehaviour
{
    public float combustivelMaximo = 100.0f;
    public float consumoPorSegundo = 1.0f;
    public Text combustivelTexto;

    [HideInInspector] public float combustivelAtual;

    void Start()
    {
        combustivelAtual = combustivelMaximo;
    }

    void Update()
    {
        combustivelAtual -= consumoPorSegundo * Time.deltaTime;
        combustivelAtual = Mathf.Max(combustivelAtual, 0);
        if (combustivelTexto != null) combustivelTexto.text = "Combustível: " + combustivelAtual.ToString("F2") + " L";

        if (combustivelAtual <= 0)
        {
            // Código para lidar com a falta de combustível
            Debug.Log("Sem combustível!");
        }
    }
}
