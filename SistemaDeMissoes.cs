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
    
    [System.Serializable]
    public enum TipoMissao
    {
        Checkpoint,
        Transporte,
        Resgate,
        Combate,
        Corrida,
        Acrobacias
    }
    
    [System.Serializable]
    public class MissaoAvancada : Missao
    {
        public TipoMissao tipo;
        public int dificuldade; // 1-5
        public string[] objetivosSecundarios;
        public bool[] objetivosSecundariosCompletos;
        public int recompensaBonus;
        public GameObject[] objetosEspeciais; // NPCs, cargas, etc.
        public string cenaEspecifica; // Se a missão ocorre em um mapa específico
        public string[] dialogos; // Diálogos para a missão
        public AudioClip musicaFundo; // Música específica para a missão
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
    
    [Header("Sistema de Progressão")]
    public bool usarProgressao = true;
    public int nivelJogador = 1;
    public int experienciaAtual = 0;
    public int experienciaProximoNivel = 1000;
    public MissaoAvancada[] missoesAvancadas;
    public GameObject[] missoesNoMapa; // Ícones de missão no mapa do mundo
    
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
        
        // Adicionar experiência e recompensas
        AdicionarExperiencia(missaoAtual.recompensaPontos);
        
        // Verificar objetivos secundários
        MissaoAvancada missaoAvancada = missoesAvancadas[missaoAtualIndex];
        for (int i = 0; i < missaoAvancada.objetivosSecundarios.Length; i++)
        {
            if (missaoAvancada.objetivosSecundariosCompletos[i])
            {
                AdicionarExperiencia(missaoAvancada.recompensaBonus);
            }
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
    
    // Método para gerar missão aleatória
    public MissaoAvancada GerarMissaoAleatoria(int dificuldadeMinima, int dificuldadeMaxima)
    {
        // Criar uma nova missão com parâmetros aleatórios
        MissaoAvancada novaMissao = new MissaoAvancada();
        
        // Definir tipo aleatório
        novaMissao.tipo = (TipoMissao)Random.Range(0, System.Enum.GetValues(typeof(TipoMissao)).Length);
        
        // Definir dificuldade dentro do intervalo
        novaMissao.dificuldade = Random.Range(dificuldadeMinima, dificuldadeMaxima + 1);
        
        // Configurar parâmetros baseados no tipo e dificuldade
        ConfigurarMissaoAleatoria(novaMissao);
        
        return novaMissao;
    }
    
    private void ConfigurarMissaoAleatoria(MissaoAvancada missao)
    {
        // Configurar título e descrição
        string[] prefixos = { "Operação", "Missão", "Voo", "Desafio" };
        string[] sufixos = { "Águia", "Tempestade", "Relâmpago", "Horizonte", "Céu Aberto" };
        
        missao.titulo = prefixos[Random.Range(0, prefixos.Length)] + " " + 
                       sufixos[Random.Range(0, sufixos.Length)];
        
        // Configurar descrição baseada no tipo
        switch (missao.tipo)
        {
            case TipoMissao.Checkpoint:
                missao.descricao = "Navegue por todos os checkpoints dentro do tempo limite.";
                break;
            case TipoMissao.Transporte:
                missao.descricao = "Transporte a carga para o destino com segurança.";
                break;
            case TipoMissao.Resgate:
                missao.descricao = "Resgate os sobreviventes e leve-os para um local seguro.";
                break;
            // Outros tipos...
        }
        
        // Configurar recompensa baseada na dificuldade
        missao.recompensaPontos = missao.dificuldade * 500;
        missao.recompensaBonus = missao.dificuldade * 200;
        
        // Configurar tempo baseado na dificuldade
        missao.tempoLimite = 120 + (6 - missao.dificuldade) * 30; // Mais difícil = menos tempo
        
        // Configurar objetivos secundários
        int numObjetivos = Random.Range(1, 4);
        missao.objetivosSecundarios = new string[numObjetivos];
        missao.objetivosSecundariosCompletos = new bool[numObjetivos];
        
        string[] objetivosPossiveis = {
            "Mantenha a aeronave acima de 90% de integridade",
            "Complete a missão com pelo menos 30% de combustível",
            "Não ultrapasse 200 metros de altitude",
            "Complete a missão em menos de 3 minutos",
            "Colete todos os itens bônus"
        };
        
        // Selecionar objetivos aleatórios
        for (int i = 0; i < numObjetivos; i++)
        {
            int indice = Random.Range(0, objetivosPossiveis.Length);
            missao.objetivosSecundarios[i] = objetivosPossiveis[indice];
            missao.objetivosSecundariosCompletos[i] = false;
        }
    }
    
    // Método para adicionar experiência e subir de nível
    public void AdicionarExperiencia(int quantidade)
    {
        if (!usarProgressao) return;
        
        experienciaAtual += quantidade;
        
        // Verificar se subiu de nível
        while (experienciaAtual >= experienciaProximoNivel)
        {
            experienciaAtual -= experienciaProximoNivel;
            nivelJogador++;
            
            // Aumentar requisito para o próximo nível
            experienciaProximoNivel = 1000 + (nivelJogador - 1) * 500;
            
            // Notificar jogador
            Debug.Log("Subiu para o nível " + nivelJogador + "!");
            
            // Desbloquear novas missões ou aeronaves baseado no nível
            VerificarDesbloqueios();
        }
    }
    
    private void VerificarDesbloqueios()
    {
        // Implementar lógica de desbloqueio baseada no nível
        // Por exemplo, desbloquear novas aeronaves, missões, etc.
    }
}