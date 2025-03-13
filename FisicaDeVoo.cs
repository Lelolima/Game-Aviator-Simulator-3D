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
    
    void FixedUpdate()
    {
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