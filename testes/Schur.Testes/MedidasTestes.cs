using Conde.Schur;
using Xunit;

namespace Conde.Schur.Testes;

/// <summary>
/// Os números do README presos aqui. Só entra o que eu vi o medidor imprimir.
/// </summary>
public class MedidasTestes
{
    [Theory]
    [InlineData(1, 2, 2, 2)]
    [InlineData(2, 4, 2, 6)]
    [InlineData(3, 8, 4, 10)]
    [InlineData(4, 16, 2, 18)]
    [InlineData(5, 32, 0, 22)]
    [InlineData(6, 64, 0, 22)]
    [InlineData(7, 128, 0, 22)]
    public void ATabelaDeDuasCores(int n, long reparticoes, long validas, long nos)
    {
        var l = Medidas.PorTamanho(2, [n])[0];
        Assert.Equal(reparticoes, l.Reparticoes);
        Assert.Equal(validas, l.ValidasForcaBruta);
        Assert.Equal(validas, l.ValidasBusca);
        Assert.Equal(nos, l.Nos);
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 6)]
    [InlineData(3, 18)]
    [InlineData(4, 30)]
    [InlineData(5, 66)]
    [InlineData(6, 120)]
    [InlineData(7, 258)]
    [InlineData(8, 288)]
    [InlineData(9, 546)]
    [InlineData(10, 300)]
    [InlineData(11, 186)]
    [InlineData(12, 114)]
    [InlineData(13, 18)]
    [InlineData(14, 0)]
    public void ATabelaDeTresCores(int n, long validas)
    {
        var l = Medidas.PorTamanho(3, [n])[0];
        Assert.Equal(validas, l.ValidasForcaBruta);
        Assert.Equal(validas, l.ValidasBusca);
    }

    [Fact]
    public void OPicoDeReparticoesEstaNoNoveENaoNoComeco()
    {
        var linhas = Medidas.PorTamanho(3, Enumerable.Range(1, 13));
        var pico = linhas.MaxBy(l => l.ValidasBusca);
        Assert.Equal(9, pico.N);
        Assert.Equal(546, pico.ValidasBusca);
    }

    [Fact]
    public void ComTrezeNumerosSaoTresReparticoesEssencialmenteDiferentes()
    {
        // 18 contando as trocas de cor, e 18 / 3! = 3.
        Assert.Equal(18, Medidas.PorTamanho(3, [13])[0].ValidasBusca);
        Assert.Equal(3, new Busca(13, 3).Contar());
    }

    [Fact]
    public void OPenultimoPassoCortaAMaiorParte()
    {
        var doze = Medidas.PorTamanho(3, [12])[0].ValidasBusca;
        var treze = Medidas.PorTamanho(3, [13])[0].ValidasBusca;
        Assert.Equal(114, doze);
        Assert.Equal(18, treze);
        Assert.True(treze < doze / 6.0 + 1);
    }

    [Theory]
    [InlineData(2, 5, 32, 11, 2.91)]
    [InlineData(2, 10, 1024, 11, 93.09)]
    [InlineData(2, 22, 4194304, 11, 381300.36)]
    [InlineData(3, 5, 243, 30, 8.1)]
    [InlineData(3, 10, 59049, 669, 88.26)]
    [InlineData(3, 14, 4782969, 978, 4890.56)]
    [InlineData(3, 22, 31381059609, 978, 32086973.02)]
    public void ATabelaDoCusto(int cores, int n, long reparticoes, long nos, double razao)
    {
        var l = Medidas.Custo(cores, [n])[0];
        Assert.Equal(reparticoes, l.Reparticoes);
        Assert.Equal(nos, l.Nos);
        Assert.Equal(razao, l.Razao);
    }

    [Theory]
    [InlineData(1, 2, 2, 1)]
    [InlineData(5, 32, 16, 3)]
    [InlineData(10, 1024, 151, 5)]
    [InlineData(12, 4096, 369, 6)]
    [InlineData(14, 16384, 847, 7)]
    [InlineData(16, 65536, 1954, 8)]
    [InlineData(18, 262144, 4398, 9)]
    [InlineData(20, 1048576, 9583, 10)]
    public void ATabelaDosLivres(int n, long subconjuntos, long livres, int maior)
    {
        var l = Medidas.Livres([n])[0];
        Assert.Equal(subconjuntos, l.Subconjuntos);
        Assert.Equal(livres, l.Livres);
        Assert.Equal(maior, l.Maior);
    }

    [Fact]
    public void AsDuasConstrucoesClassicasSaoOtimasEmTodoTamanhoMedido()
    {
        foreach (var l in Medidas.Livres([1, 2, 3, 4, 5, 6, 8, 10, 12, 14, 16, 18, 20]))
        {
            Assert.Equal(l.Maior, l.Impares);
            Assert.Equal(l.Maior, l.MetadeDeCima);
        }
    }

    [Fact]
    public void AFracaoDeLivresCaiPelaMetadeACadaDoisNumeros()
    {
        var linhas = Medidas.Livres([10, 12, 14, 16, 18, 20]);
        for (var i = 1; i < linhas.Count; i++)
        {
            var antes = (double)linhas[i - 1].Livres / linhas[i - 1].Subconjuntos;
            var depois = (double)linhas[i].Livres / linhas[i].Subconjuntos;
            Assert.InRange(depois / antes, 0.5, 0.62);
        }
    }

    [Theory]
    [InlineData(1, 1, 1, 2)]
    [InlineData(2, 4, 8, 11)]
    [InlineData(3, 13, 406, 978)]
    public void ATabelaDosNumerosDeSchur(int cores, int numero, long achar, long provar)
    {
        var l = Medidas.Numeros([cores], 30)[0];
        Assert.Equal(numero, l.Numero);
        Assert.Equal(achar, l.NosParaAchar);
        Assert.Equal(provar, l.NosParaProvar);
    }

    [Theory]
    [InlineData(1, "{1}")]
    [InlineData(2, "{1,4} {2,3}")]
    [InlineData(3, "{1,4,7,10,13} {2,3,11,12} {5,6,8,9}")]
    public void AReparticaoQueRealizaONumero(int cores, string esperado)
    {
        Assert.Equal(esperado, Medidas.Reparticao(cores));
    }
}
