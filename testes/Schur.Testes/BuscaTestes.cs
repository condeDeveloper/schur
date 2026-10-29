using Conde.Schur;
using Xunit;

namespace Conde.Schur.Testes;

public class BuscaTestes
{
    [Fact]
    public void ABuscaSemSimetriaEAForcaBrutaContamOMesmo()
    {
        for (var n = 1; n <= 8; n++)
        {
            for (var cores = 1; cores <= 3; cores++)
            {
                var bruta = Varredura.Contar(n, cores);
                var busca = new Busca(n, cores, quebrarSimetria: false);
                Assert.Equal(bruta, busca.Contar());
            }
        }
    }

    [Fact]
    public void QuebrarASimetriaDivideAContaPeloFatorialDasCoresUsadas()
    {
        // Com 3 cores e n grande o bastante para as tres aparecerem, cada
        // reparticao essencialmente diferente e contada 3! = 6 vezes.
        for (var n = 5; n <= 13; n++)
        {
            var todas = new Busca(n, 3, quebrarSimetria: false).Contar();
            var essenciais = new Busca(n, 3).Contar();
            Assert.Equal(todas, essenciais * 6);
        }
    }

    [Fact]
    public void AReparticaoAchadaRealmenteEValida()
    {
        for (var cores = 1; cores <= 3; cores++)
        {
            var n = Busca.Numero(cores, 30);
            var achada = new Busca(n, cores).Primeira();
            Assert.NotNull(achada);
            Assert.True(Juiz.Valida(achada!, cores));
            Assert.Null(Juiz.Testemunha(achada!, cores));
        }
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 4)]
    [InlineData(3, 13)]
    public void OsNumerosDeSchur(int cores, int esperado)
    {
        Assert.Equal(esperado, Busca.Numero(cores, 30));
    }

    [Fact]
    public void AteONumeroSempreExisteReparticaoDepoisNunca()
    {
        foreach (var cores in new[] { 1, 2, 3 })
        {
            var s = Busca.Numero(cores, 30);
            for (var n = 1; n <= s; n++)
            {
                Assert.NotNull(new Busca(n, cores).Primeira());
            }

            Assert.Null(new Busca(s + 1, cores).Primeira());
            Assert.Null(new Busca(s + 5, cores).Primeira());
        }
    }

    [Fact]
    public void OJuizConcordaComABuscaEmTodaReparticaoQueElaAceita()
    {
        var atual = new int[14];
        var busca = new Busca(13, 3);
        var achada = busca.Primeira();
        Assert.NotNull(achada);
        Array.Copy(achada!, atual, achada!.Length);
        Assert.True(Juiz.Valida(atual, 3));
    }

    [Fact]
    public void OUmFicaSempreNaPrimeiraCorQuandoASimetriaEQuebrada()
    {
        for (var n = 1; n <= 13; n++)
        {
            var achada = new Busca(n, 3).Primeira();
            Assert.NotNull(achada);
            Assert.Equal(0, achada![1]);
        }
    }

    [Fact]
    public void ProvarCustaMaisQueAchar()
    {
        foreach (var cores in new[] { 2, 3 })
        {
            var s = Busca.Numero(cores, 30);
            var achar = new Busca(s, cores);
            achar.Primeira();
            var provar = new Busca(s + 1, cores);
            provar.Primeira();
            Assert.True(provar.Nos > achar.Nos);
        }
    }

    [Fact]
    public void LimiteCurtoDevolveZero()
    {
        Assert.Equal(0, Busca.Numero(3, 5));
    }

    [Fact]
    public void ArgumentosInvalidosNaoSaoAceitos()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Busca(-1, 2));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Busca(5, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Varredura.Quantidade(-1, 2));
        Assert.Throws<ArgumentOutOfRangeException>(() => Varredura.Contar(-1, 2));
    }
}
