using UnityEngine;
using UnityEngine.UI;

public class SistemaDeSalvamento : MonoBehaviour
{
    public Pontuacao pontuacao;

    public void SalvarProgresso()
    {
        PlayerPrefs.SetInt("Pontuacao", pontuacao.pontuacaoAtual);
        PlayerPrefs.Save();
        Debug.Log("Progresso salvo!");
    }

    public void CarregarProgresso()
    {
        if (PlayerPrefs.HasKey("Pontuacao"))
        {
            pontuacao.pontuacaoAtual = PlayerPrefs.GetInt("Pontuacao");
            Debug.Log("Progresso carregado!");
        }
    }
}
