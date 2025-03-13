using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SistemaDeMissoes : MonoBehaviour
{
    [System.Serializable]
    public class Missao
    {
        public string titulo;
        public string descricao;
        public int recompensaPontos;
        public float tempoLimite;
        public GameObject[] checkpoints;
        public bool concluida;
    }
    
    [Header("Configurações")]
    public Missao[] missoes;
    public int missaoAtualIndex = 0;
    
    [Header("UI")]
    public Text tituloMissaoTexto;
    public Text descricaoMissaoTexto;
    public Text tempoRestanteTexto;
    public GameObject painelMissaoConcluida;
    public Text recompensaTexto;
    
    private float tempoRestante;
    private bool missaoAtiva = false;
    private Pontuacao pontuacaoScript;
    
    void Start()
    {
        pontuacaoScript = FindObjectOfType<Pontuacao>();
        IniciarMissao(missaoAtualIndex);
    }
    
    void Update()
    {
        if (missaoAtiva)
        {
            // Atualizar tempo restante
            tempoRestante -= Time.deltaTime;
            if (tempoRestanteTexto != null)
            {
                tempoRestanteTexto.text = "Tempo: " + tempoRestante.ToString("F1") + "s";
            }
            
            // Verificar se o tempo acabou
            if (tempoRestante <= 0)
            {
                FalharMissao("Tempo esgotado!");
            }
        }
    }
    
    public void IniciarMissao(int index)
    {
        if (index < 0 || index >= missoes.Length) return;
        
        missaoAtualIndex = index;
        Missao missaoAtual = missoes[missaoAtualIndex];
        
        // Configurar UI
        if (tituloMissaoTexto != null)
        {
            tituloMissaoTexto.text = missaoAtual.titulo;
        }
        
        if (descricaoMissaoTexto != null)
        {
            descricaoMissaoTexto.text = missaoAtual.descricao;
        }
        
        // Configurar tempo
        tempoRestante = missaoAtual.tempoLimite;
        
        // Ativar checkpoints da missão atual
        for (int i = 0; i < missoes.Length; i++)
        {
            foreach (GameObject checkpoint in missoes[i].checkpoints)
            {
                checkpoint.SetActive(i == missaoAtualIndex);
            }
        }
        
        missaoAtiva = true;
    }
    
    public void CompletarMissao()
    {
        if (!missaoAtiva) return;
        
        Missao missaoAtual = missoes[missaoAtualIndex];
        missaoAtual.concluida = true;
        missaoAtiva = false;
        
        // Adicionar pontos
        if (pontuacaoScript != null)
        {
            pontuacaoScript.AdicionarPontos(missaoAtual.recompensaPontos);
        }
        
        // Mostrar painel de conclusão
        if (painelMissaoConcluida != null)
        {
            painelMissaoConcluida.SetActive(true);
            if (recompensaTexto != null)
            {
                recompensaTexto.text = "Recompensa: " + missaoAtual.recompensaPontos + " pontos";
            }
        }
        
        Debug.Log("Missão concluída: " + missaoAtual.titulo);
        
        // Verificar se há mais missões
        if (missaoAtualIndex < missoes.Length - 1)
        {
            // Preparar próxima missão
            Invoke("ProximaMissao", 3.0f);
        }
        else
        {
            Debug.Log("Todas as missões foram concluídas!");
        }
    }
    
    public void FalharMissao(string motivo)
    {
        if (!missaoAtiva) return;
        
        missaoAtiva = false;
        Debug.Log("Missão falhou: " + motivo);
        
        // Reiniciar a mesma missão após um tempo
        Invoke("ReiniciarMissaoAtual", 3.0f);
    }
    
    public void ProximaMissao()
    {
        if (missaoAtualIndex < missoes.Length - 1)
        {
            missaoAtualIndex++;
            IniciarMissao(missaoAtualIndex);
        }
    }
    
    public void ReiniciarMissaoAtual()
    {
        IniciarMissao(missaoAtualIndex);
    }
    
    // Método para ser chamado pelos checkpoints
    public void CheckpointAlcancado(GameObject checkpoint)
    {
        if (!missaoAtiva) return;
        
        Missao missaoAtual = missoes[missaoAtualIndex];
        
        // Verificar se este checkpoint pertence à missão atual
        bool pertenceAMissaoAtual = false;
        foreach (GameObject cp in missaoAtual.checkpoints)
        {
            if (cp == checkpoint)
            {
                pertenceAMissaoAtual = true;
                break;
            }
        }
        
        if (pertenceAMissaoAtual)
        {
            // Desativar o checkpoint alcançado
            checkpoint.SetActive(false);
            
            // Verificar se todos os checkpoints foram alcançados
            bool todosAlcancados = true;
            foreach (GameObject cp in missaoAtual.checkpoints)
            {
                if (cp.activeSelf)
                {
                    todosAlcancados = false;
                    break;
                }
            }
            
            if (todosAlcancados)
            {
                CompletarMissao();
            }
        }
    }
}