using UnityEngine;
using UnityEngine.UI;

public class HUDAvancado : MonoBehaviour
{
    [Header("Referências")]
    public FisicaDeVoo fisicaVoo;
    public Combustivel combustivel;
    public SistemaDano sistemaDano;
    
    [Header("Painéis")]
    public GameObject painelPrincipal;
    public GameObject painelInstrumentos;
    public GameObject painelAlerta;
    public GameObject painelMapa;
    
    [Header("Instrumentos")]
    public RectTransform indicadorHorizonte;
    public RectTransform indicadorBussola;
    public Image medidorVelocidade;
    public Image medidorAltitude;
    public Image medidorCombustivel;
    public Image medidorIntegridade;
    public Text textoVelocidade;
    public Text textoAltitude;
    public Text textoCombustivel;
    public Text textoIntegridade;
    
    [Header("Alertas")]
    public GameObject alertaCombustivelBaixo;
    public GameObject alertaDanoCritico;
    public GameObject alertaEstol;
    
    [Header("Mapa")]
    public RawImage imagemMapa;
    public RectTransform indicadorJogador;
    public GameObject prefabMarcadorCheckpoint;
    public GameObject prefabMarcadorMissao;
    public Transform containerMarcadores;
    
    [Header("Configurações")]
    public bool mostrarHUD = true;
    public bool mostrarInstrumentos = true;
    public bool mostrarMapa = false;
    public float escalaHUD = 1.0f;
    
    private Rigidbody rb;
    private Camera mainCamera;
    
    void Start()
    {
        rb = fisicaVoo.GetComponent<Rigidbody>();
        mainCamera = Camera.main;
        
        // Configurar visibilidade inicial
        painelPrincipal.SetActive(mostrarHUD);
        painelInstrumentos.SetActive(mostrarInstrumentos);
        painelMapa.SetActive(mostrarMapa);
        painelAlerta.SetActive(false);
        
        // Configurar escala
        transform.localScale = Vector3.one * escalaHUD;
    }
    
    void Update()
    {
        if (!mostrarHUD) return;
        
        AtualizarInstrumentos();
        VerificarAlertas();
        
        if (mostrarMapa)
        {
            AtualizarMapa();
        }
        
        // Verificar input para alternar mapa
        if (Input.GetKeyDown(KeyCode.M))
        {
            mostrarMapa = !mostrarMapa;
            painelMapa.SetActive(mostrarMapa);
        }
    }
    
    void AtualizarInstrumentos()
    {
        if (!mostrarInstrumentos) return;
        
        // Horizonte artificial
        float pitch = transform.eulerAngles.x;
        if (pitch > 180) pitch -= 360;
        indicadorHorizonte.localRotation = Quaternion.Euler(0, 0, -fisicaVoo.inputRoll * 45);
        indicadorHorizonte.localPosition = new Vector3(0, -pitch * 2, 0);
        
        // Bússola
        float heading = transform.eulerAngles.y;
        indicadorBussola.localRotation = Quaternion.Euler(0, 0, -heading);
        
        // Velocidade
        float velocidade = rb.velocity.magnitude * 3.6f; // m/s para km/h
        textoVelocidade.text = Mathf.Round(velocidade) + " km/h";
        medidorVelocidade.fillAmount = Mathf.Clamp01(velocidade / 500);
        
        // Altitude
        float altitude = fisicaVoo.altitudeAtual;
        textoAltitude.text = Mathf.Round(altitude) + " m";
        medidorAltitude.fillAmount = Mathf.Clamp01(altitude / 2000);
        
        // Combustível
        if (combustivel != null)
        {
            float percentualCombustivel = combustivel.combustivelAtual / combustivel.combustivelMaximo;
            textoCombustivel.text = Mathf.Round(percentualCombustivel * 100) + "%";
            medidorCombustivel.fillAmount = percentualCombustivel;
        }
        
        // Integridade
        if (sistemaDano != null)
        {
            float percentualIntegridade = sistemaDano.vidaAtualAeronave / sistemaDano.vidaTotalAeronave;
            textoIntegridade.text = Mathf.Round(percentualIntegridade * 100) + "%";
            medidorIntegridade.fillAmount = percentualIntegridade;
        }
    }
    
    void VerificarAlertas()
    {
        bool temAlerta = false;
        
        // Alerta de combustível baixo
        if (combustivel != null && combustivel.combustivelAtual / combustivel.combustivelMaximo < 0.2f)
        {
            alertaCombustivelBaixo.SetActive(true);
            temAlerta = true;
        }
        else
        {
            alertaCombustivelBaixo.SetActive(false);
        }
        
        // Alerta de dano crítico
        if (sistemaDano != null && sistemaDano.vidaAtualAeronave / sistemaDano.vidaTotalAeronave < 0.3f)
        {
            alertaDanoCritico.SetActive(true);
            temAlerta = true;
        }
        else
        {
            alertaDanoCritico.SetActive(false);
        }
        
        // Alerta de estol
        float velocidade = rb.velocity.magnitude;
        if (velocidade < fisicaVoo.velocidadeEstol && fisicaVoo.emVoo)
        {
            alertaEstol.SetActive(true);
            temAlerta = true;
        }
        else
        {
            alertaEstol.SetActive(false);
        }
        
        // Mostrar painel de alerta se necessário
        painelAlerta.SetActive(temAlerta);
    }
    
    void AtualizarMapa()
    {
        // Atualizar posição do jogador no mapa
        Vector3 posicaoMundo = transform.position;
        Vector2 posicaoMapa = new Vector2(posicaoMundo.x, posicaoMundo.z) / 1000; // Escala do mapa
        indicadorJogador.anchoredPosition = posicaoMapa;
        indicadorJogador.localRotation = Quaternion.Euler(0, 0, -transform.eulerAngles.y);
    }
    
    public void AdicionarMarcadorMapa(Vector3 posicaoMundo, string tipo, string nome)
    {
        GameObject prefab = tipo == "checkpoint" ? prefabMarcadorCheckpoint : prefabMarcadorMissao;
        
        GameObject marcador = Instantiate(prefab, containerMarcadores);
        RectTransform rt = marcador.GetComponent<RectTransform>();
        
        // Converter posição do mundo para posição no mapa
        Vector2 posicaoMapa = new Vector2(posicaoMundo.x, posicaoMundo.z) / 1000; // Escala do mapa
        rt.anchoredPosition = posicaoMapa;
        
        // Configurar nome se tiver um componente Text
        Text texto = marcador.GetComponentInChildren<Text>();
        if (texto != null)
        {
            texto.text = nome;
        }
    }
    
    public void LimparMarcadoresMapa()
    {
        foreach (Transform child in containerMarcadores)
        {
            Destroy(child.gameObject);
        }
    }
} 