using UnityEngine;
using UnityEngine.UI;

public class Dano : MonoBehaviour
{
    public float vidaMaxima = 100.0f;
    public float vidaAtual;
    private EfeitosVisuais efeitosVisuais;

    void Start()
    {
        vidaAtual = vidaMaxima;
        efeitosVisuais = GetComponent<EfeitosVisuais>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Reduz a vida com base na força da colisão
        float dano = collision.relativeVelocity.magnitude * 10;
        vidaAtual -= dano;
        Debug.Log("Dano recebido: " + dano);
        
        // Aplicar efeito visual de dano
        if (efeitosVisuais != null)
        {
            efeitosVisuais.AplicarEfeitoDanoMomentaneo(dano / vidaMaxima);
        }

        if (vidaAtual <= 0)
        {
            // Código para lidar com a destruição da aeronave
            Debug.Log("Aeronave destruída!");
            DestruirAeronave();
        }
    }
    
    public void RepararAeronave(float quantidadeReparo)
    {
        vidaAtual = Mathf.Min(vidaAtual + quantidadeReparo, vidaMaxima);
    }
    
    private void DestruirAeronave()
    {
        // Criar efeito de explosão
        if (efeitosVisuais != null)
        {
            efeitosVisuais.CriarExplosao(transform.position);
        }
        
        // Desativar controles
        FisicaDeVoo fisicaVoo = GetComponent<FisicaDeVoo>();
        if (fisicaVoo != null)
        {
            fisicaVoo.enabled = false;
        }
        
        // Mostrar tela de game over após alguns segundos
        Invoke("MostrarGameOver", 3.0f);
    }
    
    private void MostrarGameOver()
    {
        // Aqui você pode implementar a lógica para mostrar a tela de game over
        // ou reiniciar o nível
        Debug.Log("Game Over");
    }
}
