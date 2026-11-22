// Fórmulas aerodinâmicas puras (sem dependência do Unity), usadas por FisicaDeVoo.
// Separadas para poderem ser testadas com `dotnet test` fora do editor.
public static class Aerodinamica
{
    // Escala de altura da atmosfera simplificada usada pelo jogo (metros).
    public const float EscalaAltura = 10000f;

    // Densidade relativa do ar (1 ao nível do mar), decaindo exponencialmente com a altitude.
    public static float DensidadeRelativa(float altitude)
    {
        return System.MathF.Exp(-altitude / EscalaAltura);
    }

    // Força aerodinâmica: 0,5 * densidade * v² * coeficiente.
    public static float Forca(float densidade, float velocidadeAr, float coeficiente)
    {
        return 0.5f * densidade * velocidadeAr * velocidadeAr * coeficiente;
    }

    // Ângulo de ataque limitado ao intervalo usado pelo jogo (-45° a 45°).
    public static float LimitarAnguloAtaque(float angulo)
    {
        return System.Math.Clamp(angulo, -45f, 45f);
    }
}
