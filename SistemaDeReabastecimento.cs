using UnityEngine;
using UnityEngine.UI;

public class SistemaDeReabastecimento : MonoBehaviour
{
    [Header("Configurações")]
    public float taxaDeReabastecimento = 10.0f; // Unidades por segundo
    public float distanciaMaximaReabastecimento = 15.0f;
    public LayerMask camadaAeronave;
    
    [Header("UI")]
    public GameObject painelReabastecimento;
    public Slider sliderProgresso;
    public Text textoInstrucao;
    
    private Combustivel combustivelAlvo;
    private bool reabastecendo = false;
    private float progressoAtual = 0.0f;
    
    void Start()
    {
        if (painelReabastecimento != null)
        {
            painelReabastecimento.SetActive(false);
        }
    }
    
    void Update()
    {
        // Verificar se há uma aeronave próxima para reabastecer
        Collider[] colisores = Physics.OverlapSphere(transform.position, distanciaMaximaReabastecimento, camadaAeronave);
        
        if (colisores.Length > 0)
        {
            // Encontrou uma aeronave próxima
            GameObject aeronave = colisores[0].gameObject;
            combustivelAlvo = aeronave.GetComponent<Combustivel>();
            
            if (combustivelAlvo != null)
            {
                // Mostrar UI de reabastecimento
                if (painelReabastecimento != null && !painelReabastecimento.activeSelf)
                {
                    painelReabastecimento.SetActive(true);
                    textoInstrucao.text = "Pressione 'R' para reabastecer";
                }
                
                // Verificar input para iniciar reabastecimento
                if (Input.GetKeyDown(KeyCode.R))
                {
                    IniciarReabastecimento();
                }
                
                // Processar reabastecimento
                if (reabastecendo)
                {
                    ProcessarReabastecimento();
                }
            }
        }
        else
        {
            // Nenhuma aeronave próxima, esconder UI
            if (painelReabastecimento != null && painelReabastecimento.activeSelf)
            {
                painelReabastecimento.SetActive(false);
                CancelarReabastecimento();
            }
            
            combustivelAlvo = null;
        }
    }
    
    void IniciarReabastecimento()
    {
        if (combustivelAlvo == null) return;
        
        reabastecendo = true;
        progressoAtual = 0.0f;
        
        if (textoInstrucao != null)
        {
            textoInstrucao.text = "Reabastecendo...";
        }
        
        // Desativar controles da aeronave durante o reabastecimento
        FisicaDeVoo fisicaVoo = combustivelAlvo.GetComponent<FisicaDeVoo>();
        if (fisicaVoo != null)
        {
            fisicaVoo.motorLigado = false;
        }
    }
    
    void ProcessarReabastecimento()
    {
        if (combustivelAlvo == null)
        {
            CancelarReabastecimento();
            return;
        }
        
        // Verificar se o tanque já está cheio
        if (combustivelAlvo.combustivelAtual >= combustivelAlvo.combustivelMaximo)
        {
            CompletarReabastecimento();
            return;
        }
        
        // Adicionar combustível
        float quantidadeAdicionada = taxaDeReabastecimento * Time.deltaTime;
        combustivelAlvo.combustivelAtual = Mathf.Min(
            combustivelAlvo.combustivelAtual + quantidadeAdicionada,
            combustivelAlvo.combustivelMaximo
        );
        
        // Atualizar progresso
        progressoAtual = combustivelAlvo.combustivelAtual / combustivelAlvo.combustivelMaximo;
        if (sliderProgresso != null)
        {
            sliderProgresso.value = progressoAtual;
        }
        
        // Verificar se completou
        if (progressoAtual >= 1.0f)
        {
            CompletarReabastecimento();
        }
    }
    
    void CompletarReabastecimento()
    {
        reabastecendo = false;
        
        if (textoInstrucao != null)
        {
            textoInstrucao.text = "Reabastecimento completo!";
        }
        
        // Reativar controles da aeronave
        if (combustivelAlvo != null)
        {
            FisicaDeVoo fisicaVoo = combustivelAlvo.GetComponent<FisicaDeVoo>();
            if (fisicaVoo != null)
            {
                // Não ligar o motor automaticamente, deixar para o jogador
            }
        }
        
        // Esconder UI após alguns segundos
        Invoke("EsconderUI", 2.0f);
    }
    
    void CancelarReabastecimento()
    {
        reabastecendo = false;
        
        // Reativar controles da aeronave
        if (combustivelAlvo != null)
        {
            FisicaDeVoo fisicaVoo = combustivelAlvo.GetComponent<FisicaDeVoo>();
            if (fisicaVoo != null)
            {
                // Não ligar o motor automaticamente, deixar para o jogador
            }
        }
    }
    
    void EsconderUI()
    {
        if (painelReabastecimento != null)
        {
            painelReabastecimento.SetActive(false);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        // Visualizar a área de reabastecimento no editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaMaximaReabastecimento);
    }
}