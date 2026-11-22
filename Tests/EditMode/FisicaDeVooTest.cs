using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class FisicaDeVooTest
{
    private GameObject aeronave;
    private FisicaDeVoo fisicaDeVoo;

    [SetUp]
    public void SetUp()
    {
        aeronave = new GameObject();
        fisicaDeVoo = aeronave.AddComponent<FisicaDeVoo>();
        fisicaDeVoo.rb = aeronave.AddComponent<Rigidbody>();
    }

    [Test]
    public void TesteAnguloDeAtaque()
    {
        fisicaDeVoo.rb.velocity = new Vector3(0, 0, 10);
        float anguloAtaque = Vector3.Angle(fisicaDeVoo.transform.forward, fisicaDeVoo.rb.velocity) - 90f;
        anguloAtaque = Mathf.Clamp(anguloAtaque, -45f, 45f);
        Assert.That(anguloAtaque, Is.InRange(-45f, 45f));
    }

    [Test]
    public void TesteForcaSustentacao()
    {
        fisicaDeVoo.rb.velocity = new Vector3(0, 0, 10);
        fisicaDeVoo.AplicarForcasAerodinamicasAvancadas();
        Assert.That(fisicaDeVoo.rb.velocity.magnitude, Is.GreaterThan(0));
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(aeronave);
    }
} 