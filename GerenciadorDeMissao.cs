using UnityEngine;
using UnityEngine.UI;

public class GerenciadorDeMissao : MonoBehaviour
{
    public Checkpoint[] checkpoints;
    private int checkpointAtual = 0;

    void Start()
    {
        AtivarCheckpoint(checkpointAtual);
    }

    public void ProximoCheckpoint()
    {
        if (checkpointAtual < checkpoints.Length - 1)
        {
            checkpointAtual++;
            AtivarCheckpoint(checkpointAtual);
        }
        else
        {
            Debug.Log("Todas as missões concluídas!");
        }
    }

    private void AtivarCheckpoint(int index)
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].gameObject.SetActive(i == index);
        }
    }
}
