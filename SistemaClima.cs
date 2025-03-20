using UnityEngine;

public class SistemaClima : MonoBehaviour
{
    [System.Serializable]
    public class ConfiguracaoClima
    {
        public string nome;
        public float intensidadeVento;
        public Vector3 direcaoVento;
        public float intensidadeTurbulencia;
        public float visibilidade;
        public Color corNuvens;
        public float densidadeNuvens;
        public bool temChuva;
        public float intensidadeChuva;
        public bool temTrovoada;
    }
    
    public ConfiguracaoClima[] configuracoesPredefinidas;
    public ConfiguracaoClima climaAtual;
    
    [Header("Referências")]
    public ParticleSystem sistemaChuva;
    public Light luzTrovoada;
    public Material materialCeu;
    
    [Header("Transição")]
    public bool transicaoAutomatica = false;
    public float duracaoTransicao = 60.0f;
    private float tempoProximaTransicao;
    
    void Start()
    {
        if (configuracoesPredefinidas.Length > 0)
        {
            // Iniciar com clima aleatório ou predefinido
            AplicarConfiguracao(configuracoesPredefinidas[0]);
        }
        
        tempoProximaTransicao = Time.time + duracaoTransicao;
    }
    
    void Update()
    {
        if (transicaoAutomatica && Time.time > tempoProximaTransicao)
        {
            // Selecionar próximo clima aleatoriamente
            int indice = Random.Range(0, configuracoesPredefinidas.Length);
            AplicarConfiguracao(configuracoesPredefinidas[indice]);
            
            tempoProximaTransicao = Time.time + duracaoTransicao;
        }
        
        // Atualizar efeitos de trovoada
        if (climaAtual.temTrovoada && Random.value < 0.002f)
        {
            StartCoroutine(EfeitoTrovoada());
        }
    }
    
    public void AplicarConfiguracao(ConfiguracaoClima config)
    {
        climaAtual = config;
        
        // Atualizar sistemas de partículas
        if (sistemaChuva != null)
        {
            var emissao = sistemaChuva.emission;
            if (climaAtual.temChuva)
            {
                sistemaChuva.Play();
                emissao.rateOverTime = climaAtual.intensidadeChuva * 500;
            }
            else
            {
                sistemaChuva.Stop();
            }
        }
        
        // Atualizar material do céu
        if (materialCeu != null)
        {
            materialCeu.SetColor("_CloudColor", climaAtual.corNuvens);
            materialCeu.SetFloat("_CloudDensity", climaAtual.densidadeNuvens);
        }
        
        // Notificar outros sistemas sobre mudança de clima
        FisicaDeVoo[] aeronaves = FindObjectsOfType<FisicaDeVoo>();
        foreach (var aeronave in aeronaves)
        {
            aeronave.intensidadeVento = climaAtual.intensidadeVento;
            aeronave.direcaoVento = climaAtual.direcaoVento;
            aeronave.intensidadeTurbulencia = climaAtual.intensidadeTurbulencia;
        }
    }
    
    System.Collections.IEnumerator EfeitoTrovoada()
    {
        if (luzTrovoada != null)
        {
            luzTrovoada.enabled = true;
            yield return new WaitForSeconds(0.1f);
            luzTrovoada.enabled = false;
            yield return new WaitForSeconds(0.05f);
            luzTrovoada.enabled = true;
            yield return new WaitForSeconds(0.15f);
            luzTrovoada.enabled = false;
        }
    }
}