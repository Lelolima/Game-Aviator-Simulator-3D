using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EfeitosVisuais : MonoBehaviour
{
    [Header("Referências")]
    public PostProcessVolume postProcessVolume;
    public ParticleSystem efeitoFogo;
    public ParticleSystem efeitoFumaca;
    public GameObject efeitoExplosao;
    
    [Header("Configurações")]
    public float intensidadeDanoMaxima = 1.0f;
    public float velocidadeFadeEfeitos = 0.5f;
    
    private Vignette vinheta;
    private ChromaticAberration aberracaoCromatica;
    private Dano scriptDano;
    private FisicaDeVoo scriptFisicaVoo;
    
    private float intensidadeDanoAtual = 0.0f;
    
    void Start()
    {
        // Obter componentes de post-processing
        postProcessVolume.profile.TryGetSettings(out vinheta);
        postProcessVolume.profile.TryGetSettings(out aberracaoCromatica);
        
        // Obter referências para outros scripts
        scriptDano = GetComponent<Dano>();
        scriptFisicaVoo = GetComponent<FisicaDeVoo>();
        
        // Desativar efeitos inicialmente
        if (efeitoFogo != null) efeitoFogo.Stop();
        if (efeitoFumaca != null) efeitoFumaca.Stop();
    }
    
    void Update()
    {
        // Atualizar efeitos baseados no dano
        if (scriptDano != null)
        {
            float percentualDano = 1.0f - (scriptDano.vidaAtual / scriptDano.vidaMaxima);
            AtualizarEfeitosDano(percentualDano);
        }
        
        // Atualizar efeitos baseados na velocidade
        if (scriptFisicaVoo != null)
        {
            AtualizarEfeitosVelocidade();
        }
        
        // Fade out dos efeitos de dano
        if (intensidadeDanoAtual > 0)
        {
            intensidadeDanoAtual = Mathf.Max(0, intensidadeDanoAtual - velocidadeFadeEfeitos * Time.deltaTime);
            AplicarEfeitosDano(intensidadeDanoAtual);
        }
    }
    
    public void AtualizarEfeitosDano(float percentualDano)
    {
        // Ativar efeitos de fumaça quando o dano for maior que 30%
        if (percentualDano > 0.3f && efeitoFumaca != null && !efeitoFumaca.isPlaying)
        {
            efeitoFumaca.Play();
        }
        
        // Ativar efeitos de fogo quando o dano for maior que 70%
        if (percentualDano > 0.7f && efeitoFogo != null && !efeitoFogo.isPlaying)
        {
            efeitoFogo.Play();
        }
        
        // Ajustar a intensidade dos efeitos de partículas
        if (efeitoFumaca != null)
        {
            var emissao = efeitoFumaca.emission;
            emissao.rateOverTime = percentualDano * 20; // Ajustar taxa de emissão baseada no dano
        }
    }
    
    public void AtualizarEfeitosVelocidade()
    {
        // Implementar efeitos baseados na velocidade, como motion blur
        float velocidade = scriptFisicaVoo.GetComponent<Rigidbody>().velocity.magnitude;
        
        // Exemplo: ajustar field of view baseado na velocidade
        Camera.main.fieldOfView = Mathf.Lerp(60, 75, velocidade / 100);
    }
    
    public void AplicarEfeitoDanoMomentaneo(float intensidade)
    {
        intensidadeDanoAtual = Mathf.Min(intensidadeDanoMaxima, intensidadeDanoAtual + intensidade);
        AplicarEfeitosDano(intensidadeDanoAtual);
    }
    
    private void AplicarEfeitosDano(float intensidade)
    {
        if (vinheta != null)
        {
            vinheta.intensity.value = intensidade * 0.5f;
            vinheta.color.value = Color.red;
        }
        
        if (aberracaoCromatica != null)
        {
            aberracaoCromatica.intensity.value = intensidade * 0.8f;
        }
    }
    
    public void CriarExplosao(Vector3 posicao)
    {
        if (efeitoExplosao != null)
        {
            Instantiate(efeitoExplosao, posicao, Quaternion.identity);
        }
    }
}