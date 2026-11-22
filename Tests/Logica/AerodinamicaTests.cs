using NUnit.Framework;

public class AerodinamicaTests
{
    [Test]
    public void DensidadeNoNivelDoMarEhUm()
    {
        Assert.That(Aerodinamica.DensidadeRelativa(0f), Is.EqualTo(1f).Within(1e-6));
    }

    [Test]
    public void DensidadeCaiParaUmSobreENaEscalaDeAltura()
    {
        Assert.That(Aerodinamica.DensidadeRelativa(Aerodinamica.EscalaAltura), Is.EqualTo(0.367879f).Within(1e-5));
    }

    [Test]
    public void DensidadeDiminuiComAltitude()
    {
        Assert.That(Aerodinamica.DensidadeRelativa(5000f), Is.LessThan(Aerodinamica.DensidadeRelativa(1000f)));
    }

    [Test]
    public void ForcaCresceComQuadradoDaVelocidade()
    {
        float f10 = Aerodinamica.Forca(1f, 10f, 0.5f);
        float f20 = Aerodinamica.Forca(1f, 20f, 0.5f);
        Assert.That(f10, Is.EqualTo(25f).Within(1e-4));
        Assert.That(f20 / f10, Is.EqualTo(4f).Within(1e-4));
    }

    [Test]
    public void ForcaNulaParadoOuSemCoeficiente()
    {
        Assert.That(Aerodinamica.Forca(1f, 0f, 0.5f), Is.EqualTo(0f));
        Assert.That(Aerodinamica.Forca(1f, 50f, 0f), Is.EqualTo(0f));
    }

    [TestCase(-90f, -45f)]
    [TestCase(-10f, -10f)]
    [TestCase(30f, 30f)]
    [TestCase(80f, 45f)]
    public void AnguloDeAtaqueLimitado(float entrada, float esperado)
    {
        Assert.That(Aerodinamica.LimitarAnguloAtaque(entrada), Is.EqualTo(esperado));
    }
}
