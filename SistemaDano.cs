using UnityEngine;
using System.Collections.Generic;

public class SistemaDano : MonoBehaviour
{
    [System.Serializable]
    public class ComponenteAeronave
    {
        public string nome;
        public float vidaMaxima = 100f;
        public float vidaAtual = 100f;
        public bool funcional = true;
        public GameObject modeloVisual;
        public GameObject modeloDanificado;
        public ParticleSystem efeitoDano;
    }
    
    [Header("Componentes")]
    public ComponenteAeronave[] componentes;
    
    [Header("Configurações")]
    public float vidaTotalAeronave = 100f;
    public float vidaAtualAeronave = 100f;
    
    [Header("Referências")]
    public EfeitosVisuais efeitosVisuais;
    public FisicaDeVoo fisicaVoo;
    
    private Dictionary<string, int> indiceComponentes = new Dictionary<string, int>();
    
    void Start()
    {
        // Inicializar dicionário para acesso rápido aos componentes
        for (int i = 0; i < componentes.Length; i++)
        {
            indiceComponentes[componentes[i].nome] = i;
        }
        
        // Garantir que todos os componentes estejam funcionais no início
        foreach (var componente in componentes)
        {
            componente.vidaAtual = componente.vidaMaxima;
            componente.funcional = true;
            
            if (componente.modeloVisual != null)
                componente.modeloVisual.SetActive(true);
                
            if (componente.modeloDanificado != null)
                componente.modeloDanificado.SetActive(false);
                
            if (componente.efeitoDano != null)
                componente.efeitoDano.Stop();
        }
    }
    
    public void AplicarDano(float quantidade, string componente = "")
    {
        // Dano em componente específico
        if (!string.IsNullOrEmpty(componente) && indiceComponentes.ContainsKey(componente))
        {
            int indice = indiceComponentes[componente];
            AplicarDanoComponente(indice, quantidade);
        }
        // Dano geral na aeronave
        else
        {
            // Distribuir dano entre componentes aleatórios
            int numComponentes = Mathf.Min(3, componentes.Length);
            float danoPorComponente = quantidade / numComponentes;
            
            for (int i = 0; i < numComponentes; i++)
            {
                int indiceAleatorio = Random.Range(0, componentes.Length);
                AplicarDanoComponente(indiceAleatorio, danoPorComponente);
            }
            
            // Atualizar vida total da aeronave
            AtualizarVidaTotal();
        }
        
        // Aplicar efeito visual de dano
        if (efeitosVisuais != null)
        {
            efeitosVisuais.AplicarEfeitoDanoMomentaneo(quantidade / 100f);
        }
    }
    
    private void AplicarDanoComponente(int indice, float quantidade)
    {
        ComponenteAeronave componente = componentes[indice];
        
        // Reduzir vida do componente
        componente.vidaAtual = Mathf.Max(0, componente.vidaAtual - quantidade);
        
        // Verificar se o componente ficou não-funcional
        if (componente.vidaAtual < componente.vidaMaxima * 0.3f && componente.funcional)
        {
            componente.funcional = false;
            
            // Atualizar visual
            if (componente.modeloVisual != null)
                componente.modeloVisual.SetActive(true);
                
            if (componente.modeloDanificado != null)
                componente.modeloDanificado.SetActive(false);
                
            // Ativar efeito de dano
            if (componente.efeitoDano != null)
                componente.efeitoDano.Play();
                
            // Aplicar efeitos na jogabilidade
            AplicarEfeitosDanoJogabilidade(componente.nome);
        }
    }
    
    private void AtualizarVidaTotal()
    {
        // Calcular vida total baseada na média ponderada dos componentes
        float vidaTotal = 0;
        foreach (var componente in componentes)
        {
            vidaTotal += componente.vidaAtual / componente.vidaMaxima;
        }
        
        vidaAtualAeronave = (vidaTotal / componentes.Length) * vidaTotalAeronave;
    }
    
    private void AplicarEfeitosDanoJogabilidade(string nomeComponente)
    {
        if (fisicaVoo == null) return;
        
        switch (nomeComponente)
        {
            case "Motor":
                fisicaVoo.potenciaMotor *= 0.5f;
                break;
            case "Asa Esquerda":
                fisicaVoo.inputRoll += 0.2f; // Tendência a rolar para a esquerda
                fisicaVoo.sustentacao *= 0.8f;
                break;
            case "Asa Direita":
                fisicaVoo.inputRoll -= 0.2f; // Tendência a rolar para a direita
                fisicaVoo.sustentacao *= 0.8f;
                break;
            case "Cauda":
                fisicaVoo.sensibilidadeControle *= 0.6f;
                break;
            case "Trem de Pouso":
                // Afetar pouso
                break;
        }
    }
    
    public bool RepararComponente(string nomeComponente, float quantidade)
    {
        if (!indiceComponentes.ContainsKey(nomeComponente))
            return false;
            
        int indice = indiceComponentes[nomeComponente];
        ComponenteAeronave componente = componentes[indice];
        
        // Aumentar vida do componente
        float vidaAnterior = componente.vidaAtual;
        componente.vidaAtual = Mathf.Min(componente.vidaMaxima, componente.vidaAtual + quantidade);
        
        // Verificar se o componente voltou a funcionar
        if (!componente.funcional && componente.vidaAtual > componente.vidaMaxima * 0.5f)
        {
            componente.funcional = true;
            
            // Atualizar visual
            if (componente.modeloVisual != null)
                componente.modeloVisual.SetActive(true);
                
            if (componente.modeloDanificado != null)
                componente.modeloDanificado.SetActive(false);
                
            // Desativar efeito de dano
            if (componente.efeitoDano != null)
                componente.efeitoDano.Stop();
                
            // Restaurar efeitos na jogabilidade
            RestaurarEfeitosDanoJogabilidade(componente.nome);
        }
        
        // Atualizar vida total
        AtualizarVidaTotal();
        
        return componente.vidaAtual > vidaAnterior;
    }
    
    private void RestaurarEfeitosDanoJogabilidade(string nomeComponente)
    {
        if (fisicaVoo == null) return;
        
        // Restaurar valores padrão (isso exigiria armazenar os valores originais)
        // Esta é uma implementação simplificada
        switch (nomeComponente)
        {
            case "Motor":
                fisicaVoo.potenciaMotor = 100.0f;
                break;
            case "Asa Esquerda":
            case "Asa Direita":
                fisicaVoo.inputRoll = 0f;
                fisicaVoo.sustentacao = 5.0f;
                break;
            case "Cauda":
                fisicaVoo.sensibilidadeControle = 3.0f;
                break;
        }
    }
}