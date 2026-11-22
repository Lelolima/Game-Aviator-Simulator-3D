using UnityEngine;
using UnityEngine.UI;

public class Pontuacao : MonoBehaviour
{
    public Text pontuacaoTexto;
    [HideInInspector] public int pontuacaoAtual = 0;

    public void AdicionarPontos(int pontos)
    {
        pontuacaoAtual += pontos;
        if (pontuacaoTexto != null) pontuacaoTexto.text = "Pontuação: " + pontuacaoAtual;
    }
}
