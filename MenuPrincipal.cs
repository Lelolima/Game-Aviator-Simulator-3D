using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Painéis")]
    public GameObject painelPrincipal;
    public GameObject painelOpcoes;
    public GameObject painelSelecaoAeronave;
    
    [Header("Configurações")]
    public Slider volumeSlider;
    public Slider sensibilidadeSlider;
    public Toggle efeitosVisuaisToggle;
    
    [Header("Aeronaves")]
    public GameObject[] aeronavesDisponiveis;
    public Text[] nomesAeronaves;
    public Text[] descricaoAeronaves;
    
    private int aeronaveAtualIndex = 0;
    
    void Start()
    {
        // Inicializar menu
        MostrarPainelPrincipal();
        
        // Carregar configurações salvas
        if (PlayerPrefs.HasKey("Volume"))
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        }
        
        if (PlayerPrefs.HasKey("Sensibilidade"))
        {
            sensibilidadeSlider.value = PlayerPrefs.GetFloat("Sensibilidade");
        }
        
        if (PlayerPrefs.HasKey("EfeitosVisuais"))
        {
            efeitosVisuaisToggle.isOn = PlayerPrefs.GetInt("EfeitosVisuais") == 1;
        }
        
        // Configurar música de fundo
        AudioListener.volume = volumeSlider.value;
    }
    
    public void IniciarJogo()
    {
        // Salvar a aeronave selecionada
        PlayerPrefs.SetInt("AeronaveSelecionada", aeronaveAtualIndex);
        
        // Carregar a cena do jogo
        SceneManager.LoadScene("Jogo");
    }
    
    public void SairJogo()
    {
        // Salvar configurações antes de sair
        SalvarConfiguracoes();
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    public void MostrarPainelPrincipal()
    {
        painelPrincipal.SetActive(true);
        painelOpcoes.SetActive(false);
        painelSelecaoAeronave.SetActive(false);
    }
    
    public void MostrarPainelOpcoes()
    {
        painelPrincipal.SetActive(false);
        painelOpcoes.SetActive(true);
        painelSelecaoAeronave.SetActive(false);
    }
    
    public void MostrarPainelSelecaoAeronave()
    {
        painelPrincipal.SetActive(false);
        painelOpcoes.SetActive(false);
        painelSelecaoAeronave.SetActive(true);
        
        // Mostrar a primeira aeronave
        MostrarAeronave(0);
    }
    
    public void ProximaAeronave()
    {
        aeronaveAtualIndex = (aeronaveAtualIndex + 1) % aeronavesDisponiveis.Length;
        MostrarAeronave(aeronaveAtualIndex);
    }
    
    public void AeronaveAnterior()
    {
        aeronaveAtualIndex--;
        if (aeronaveAtualIndex < 0) aeronaveAtualIndex = aeronavesDisponiveis.Length - 1;
        MostrarAeronave(aeronaveAtualIndex);
    }
    
    private void MostrarAeronave(int index)
    {
        // Desativar todas as aeronaves
        foreach (GameObject aeronave in aeronavesDisponiveis)
        {
            aeronave.SetActive(false);
        }
        
        // Ativar apenas a aeronave selecionada
        aeronavesDisponiveis[index].SetActive(true);
    }
    
    public void AjustarVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }
    
    public void SalvarConfiguracoes()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetFloat("Sensibilidade", sensibilidadeSlider.value);
        PlayerPrefs.SetInt("EfeitosVisuais", efeitosVisuaisToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}