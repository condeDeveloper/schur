using Conde.Schur;
using Xunit;

namespace Conde.Schur.Testes;

public class ConjuntoTestes
{
    [Fact]
    public void OConjuntoVazioEOUnitarioSaoLivres()
    {
        Assert.True(Conjunto.LivreDeSoma([]));
        Assert.True(Conjunto.LivreDeSoma([1]));
        Assert.True(Conjunto.LivreDeSoma([7]));
    }

    [Fact]
    public void OMesmoNumeroPodeSerOsDoisParcelas()
    {
        // {2,4} nao e livre porque 2+2=4, mesmo o 2 aparecendo uma vez so.
        // Trocar esta convencao muda todos os numeros do repositorio.
        Assert.False(Conjunto.LivreDeSoma([2, 4]));
        Assert.Equal((2, 2, 4), Conjunto.Testemunha([2, 4]));
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, false)]
    [InlineData(new[] { 1, 3 }, true)]
    [InlineData(new[] { 1, 4 }, true)]
    [InlineData(new[] { 2, 3 }, true)]
    [InlineData(new[] { 1, 2, 3 }, false)]
    [InlineData(new[] { 1, 4, 7, 10, 13 }, true)]
    [InlineData(new[] { 2, 3, 11, 12 }, true)]
    [InlineData(new[] { 5, 6, 8, 9 }, true)]
    public void CasosConhecidos(int[] elementos, bool livre)
    {
        Assert.Equal(livre, Conjunto.LivreDeSoma(elementos));
        Assert.Equal(livre, Conjunto.Testemunha(elementos) is null);
    }

    [Fact]
    public void ATestemunhaERealmenteUmaSomaDeDentro()
    {
        for (var n = 1; n <= 12; n++)
        {
            foreach (var sub in Conjunto.TodosLivres(n))
            {
                Assert.Null(Conjunto.Testemunha(sub));
            }
        }

        var quebrado = new[] { 3, 5, 8 };
        var t = Conjunto.Testemunha(quebrado);
        Assert.NotNull(t);
        Assert.Equal(t!.Value.X + t.Value.Y, t.Value.Z);
        Assert.Contains(t.Value.Z, quebrado);
    }

    [Fact]
    public void OsImparesSaoSempreLivres()
    {
        for (var n = 1; n <= 40; n++)
        {
            Assert.True(Conjunto.LivreDeSoma(Conjunto.Impares(n)));
        }
    }

    [Fact]
    public void AMetadeDeCimaESempreLivre()
    {
        for (var n = 1; n <= 40; n++)
        {
            Assert.True(Conjunto.LivreDeSoma(Conjunto.MetadeDeCima(n)));
        }
    }

    [Fact]
    public void AsDuasConstrucoesTemOMesmoTamanho()
    {
        for (var n = 1; n <= 40; n++)
        {
            Assert.Equal(Conjunto.Impares(n).Count, Conjunto.MetadeDeCima(n).Count);
            Assert.Equal((n + 1) / 2, Conjunto.Impares(n).Count);
        }
    }

    [Fact]
    public void AsDuasConstrucoesSaoDiferentesAPartirDeTres()
    {
        for (var n = 3; n <= 20; n++)
        {
            Assert.NotEqual(Conjunto.Impares(n), Conjunto.MetadeDeCima(n));
        }
    }

    [Fact]
    public void TodoSubconjuntoDeUmLivreTambemELivre()
    {
        foreach (var livre in Conjunto.TodosLivres(9))
        {
            for (var fora = 0; fora < livre.Count; fora++)
            {
                var menor = new List<int>(livre);
                menor.RemoveAt(fora);
                Assert.True(Conjunto.LivreDeSoma(menor));
            }
        }
    }

    [Fact]
    public void NuloOuNegativoNaoEAceito()
    {
        Assert.Throws<ArgumentNullException>(() => Conjunto.LivreDeSoma(null!));
        Assert.Throws<ArgumentNullException>(() => Conjunto.Testemunha(null!));
        Assert.Throws<ArgumentOutOfRangeException>(() => Conjunto.TodosLivres(-1));
    }
}
