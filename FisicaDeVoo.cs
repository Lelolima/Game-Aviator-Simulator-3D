using UnityEngine;
using UnityEngine.UI;

public class FisicaDeVoo : MonoBehaviour
{
    [Header("Configurações da Aeronave")]
    public float potenciaMotor = 100.0f;
    public float sustentacao = 5.0f;
    public float sensibilidadeControle = 3.0f;
    public float resistenciaAr = 0.01f;
    public float velocidadeEstol = 5.0f; // Velocidade mínima para manter voo
    
    [Header("Controles")]
    public float inputAcelerador = 0.0f;
    public float inputPitch = 0.0f; // Inclinar para cima/baixo
    public float inputRoll = 0.0f;  // Girar no eixo
    public float inputYaw = 0.0f;   // Virar para os lados
    
    [Header("Estado da Aeronave")]
    public bool motorLigado = false;
    public bool emVoo = false;
    public float altitudeAtual = 0.0f;
    
    [Header("UI")]
    public Text altitudeTexto;
    public Text estadoVooTexto;
    
    [Header("Efeitos Ambientais")]
    public float intensidadeVento = 0.0f;
    public Vector3 direcaoVento = Vector3.zero;
    public float intensidadeTurbulencia = 0.0f;

    [Header("Curvas Aerodinâmicas")]
    public AnimationCurve curvaSustentacao;
    public AnimationCurve curvaArrasto;
    
    [Header("Configurações Avançadas")]
    public float coeficienteLift = 0.5f;
    public float coeficienteDrag = 0.02f;
    public AnimationCurve curvaLift; // Curva de sustentação baseada no ângulo de ataque
    public bool usarModeloAvancado = true;
    
    private Rigidbody rb;
    private AudioSource motorSom;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        motorSom = GetComponent<AudioSource>();
        
        // Configurar física da aeronave
        rb.mass = 1000;
        rb.drag = 0.1f;
        rb.angularDrag = 0.5f;
    }
    
    void Update()
    {
        // Capturar inputs do jogador
        if (Input.GetKeyDown(KeyCode.Space))
        {
            motorLigado = !motorLigado;
            if (motorLigado)
            {
                motorSom.Play();
            }
            else
            {
                motorSom.Stop();
            }
        }
        
        inputAcelerador = Mathf.Clamp01(inputAcelerador + Input.GetAxis("Vertical") * Time.deltaTime);
        inputPitch = Input.GetAxis("Vertical");
        inputRoll = Input.GetAxis("Horizontal");
        inputYaw = Input.GetAxis("Horizontal") * 0.5f;
        
        // Atualizar UI
        altitudeAtual = transform.position.y;
        if (altitudeTexto != null)
        {
            altitudeTexto.text = "Altitude: " + altitudeAtual.ToString("F1") + " m";
        }
        
        if (estadoVooTexto != null)
        {
            estadoVooTexto.text = emVoo ? "Em voo" : "No solo";
        }
        
        // Verificar se está em voo
        RaycastHit hit;
        emVoo = !Physics.Raycast(transform.position, Vector3.down, out hit, 3.0f);
    }
    
    private void AplicarForcasAerodinamicasAvancadas()
    {
        if (!usarModeloAvancado) return;
        
        // Calcular ângulo de ataque
        float anguloAtaque = Vector3.Angle(transform.forward, rb.velocity) - 90f;
        // Verificar se o ângulo de ataque está dentro de um intervalo aceitável
        anguloAtaque = Mathf.Clamp(anguloAtaque, -45f, 45f);
        
        // Calcular forças apenas se houver mudanças significativas
        if (rb.velocity.magnitude > 0.1f) {
            // Obter coeficiente de sustentação da curva
            float coefSustentacao = curvaLift.Evaluate(anguloAtaque);
            
            // Calcular densidade do ar baseada na altitude
            float densidadeAr = Mathf.Exp(-altitudeAtual / 10000f);
            
            // Calcular velocidade relativa ao ar (considerando vento)
            Vector3 velocidadeRelativa = rb.velocity - direcaoVento * intensidadeVento;
            float velocidadeAr = velocidadeRelativa.magnitude;
            
            // Calcular forças
            float forcaSustentacao = 0.5f * densidadeAr * velocidadeAr * velocidadeAr * coefSustentacao * coeficienteLift;
            float forcaArrasto = 0.5f * densidadeAr * velocidadeAr * velocidadeAr * coeficienteDrag;
            
            // Aplicar forças
            Vector3 direcaoSustentacao = Vector3.Cross(velocidadeRelativa.normalized, transform.right);
            rb.AddForce(direcaoSustentacao * forcaSustentacao);
            rb.AddForce(-velocidadeRelativa.normalized * forcaArrasto);
        }
        
        // Aplicar turbulência
        if (intensidadeTurbulencia > 0)
        {
            Vector3 forcaTurbulencia = new Vector3(
                Mathf.PerlinNoise(Time.time, 0) - 0.5f,
                Mathf.PerlinNoise(0, Time.time) - 0.5f,
                Mathf.PerlinNoise(Time.time, Time.time) - 0.5f
            ) * intensidadeTurbulencia;
            
            rb.AddForce(forcaTurbulencia);
        }
    }

    void FixedUpdate()
    {
        AplicarForcasAerodinamicasAvancadas();
        
        if (!motorLigado) return;
        
        // Aplicar forças aerodinâmicas
        float velocidadeAtual = rb.velocity.magnitude;
        
        // Força do motor
        rb.AddForce(transform.forward * potenciaMotor * inputAcelerador);
        
        // Sustentação (apenas quando em movimento suficiente)
        if (velocidadeAtual > velocidadeEstol)
        {
            float forcaSustentacao = velocidadeAtual * sustentacao;
            rb.AddForce(transform.up * forcaSustentacao);
        }
        
        // Resistência do ar
        rb.AddForce(-rb.velocity * resistenciaAr * velocidadeAtual);
        
        // Controles de voo
        rb.AddTorque(transform.right * inputPitch * sensibilidadeControle);
        rb.AddTorque(-transform.forward * inputRoll * sensibilidadeControle);
        rb.AddTorque(transform.up * inputYaw * sensibilidadeControle);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        // Verificar se é uma colisão com o solo
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Verificar se a velocidade de impacto é segura para pouso
            float velocidadeImpacto = collision.relativeVelocity.magnitude;
            if (velocidadeImpacto < 5.0f)
            {
                Debug.Log("Pouso bem-sucedido!");
            }
            else
            {
                Debug.Log("Pouso muito forte! Velocidade de impacto: " + velocidadeImpacto);
            }
        }
    }
}