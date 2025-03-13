using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public SistemaDeMissoes sistemaMissoes;
    public AudioSource somCheckpoint;
    public ParticleSystem efeitoVisual;
    
    private bool ativado = true;
    
    void Start()
    {
        // Se não tiver referência ao sistema de missões, tentar encontrar
        if (sistemaMissoes == null)
        {
            sistemaMissoes = FindObjectOfType<SistemaDeMissoes>();
        }
        
        // Configurar visual do checkpoint
        GetComponent<Renderer>().material.color = Color.yellow;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (!ativado) return;
        
        if (other.CompareTag("Player"))
        {
            // Marcar como alcançado
            ativado = false;
            
            // Mudar cor para indicar que foi alcançado
            GetComponent<Renderer>().material.color = Color.green;
            
            // Tocar som e efeito visual
            if (somCheckpoint != null)
            {
                somCheckpoint.Play();
            }
            
            if (efeitoVisual != null)
            {
                efeitoVisual.Play();
            }
            
            // Notificar o sistema de missões
            if (sistemaMissoes != null)
            {
                sistemaMissoes.CheckpointAlcancado(gameObject);
            }
            else
            {
                Debug.LogWarning("Sistema de missões não encontrado!");
            }
            
            Debug.Log("Checkpoint alcançado!");
        }
    }
    
    // Método para resetar o checkpoint (para reutilização)
    public void Resetar()
    {
        ativado = true;
        GetComponent<Renderer>().material.color = Color.yellow;
    }
    
    void OnDrawGizmos()
    {
        // Visualizar o checkpoint no editor
        Gizmos.color = ativado ? Color.yellow : Color.green;
        Gizmos.DrawWireSphere(transform.position, 2.0f);
    }
}