namespace Conde.Schur;

/// <summary>
/// As tabelas do README. Cada número sai de um método daqui e está preso num
/// teste; só entra aqui o que eu vi o programa imprimir.
/// </summary>
public static class Medidas
{
    public readonly record struct LinhaCores(int N, long Reparticoes, long ValidasForcaBruta, long ValidasBusca, long Nos);

    public readonly record struct LinhaCusto(int Cores, int N, long Reparticoes, long Nos, double Razao);

    public readonly record struct LinhaLivre(int N, long Subconjuntos, long Livres, int Maior, int Impares, int MetadeDeCima);

    public readonly record struct LinhaNumero(int Cores, int Numero, long NosParaAchar, long NosParaProvar);

    /// <summary>
    /// Com um número fixo de cores, quantas repartições válidas existem em cada
    /// tamanho. A busca conta as essencialmente diferentes (com a simetria
    /// quebrada); a força bruta conta todas, e a razão entre as duas tem que ser
    /// o fatorial do número de cores usadas.
    /// </summary>
    public static List<LinhaCores> PorTamanho(int cores, IEnumerable<int> tamanhos)
    {
        var linhas = new List<LinhaCores>();

        foreach (var n in tamanhos)
        {
            var bruta = Varredura.Contar(n, cores);
            var busca = new Busca(n, cores, quebrarSimetria: false);
            var contadas = busca.Contar();

            if (contadas != bruta)
            {
                throw new InvalidOperationException(
                    $"a busca e a forca bruta discordaram em n={n}: {contadas} contra {bruta}");
            }

            linhas.Add(new LinhaCores(n, Varredura.Quantidade(n, cores), bruta, contadas, busca.Nos));
        }

        return linhas;
    }

    /// <summary>O que a poda e a quebra de simetria economizam.</summary>
    public static List<LinhaCusto> Custo(int cores, IEnumerable<int> tamanhos)
    {
        var linhas = new List<LinhaCusto>();

        foreach (var n in tamanhos)
        {
            var busca = new Busca(n, cores);
            busca.Contar();
            var reparticoes = Varredura.Quantidade(n, cores);
            linhas.Add(new LinhaCusto(cores, n, reparticoes, busca.Nos, Math.Round((double)reparticoes / busca.Nos, 2)));
        }

        return linhas;
    }

    /// <summary>
    /// O maior conjunto livre de soma dentro de <c>1..n</c>, comparado com as
    /// duas construções clássicas: os ímpares e a metade de cima.
    /// </summary>
    public static List<LinhaLivre> Livres(IEnumerable<int> tamanhos)
    {
        var linhas = new List<LinhaLivre>();

        foreach (var n in tamanhos)
        {
            var (maior, _) = Varredura.MaiorLivre(n);
            linhas.Add(new LinhaLivre(
                n,
                1L << n,
                Conjunto.TodosLivres(n).Count,
                maior,
                Conjunto.Impares(n).Count,
                Conjunto.MetadeDeCima(n).Count));
        }

        return linhas;
    }

    /// <summary>
    /// Os números de Schur: para cada quantidade de cores, o maior tamanho que
    /// ainda dá para repartir, com o custo de achar a repartição e o custo de
    /// provar que a seguinte não existe.
    /// </summary>
    public static List<LinhaNumero> Numeros(IEnumerable<int> quantidades, int limite)
    {
        var linhas = new List<LinhaNumero>();

        foreach (var cores in quantidades)
        {
            var numero = Busca.Numero(cores, limite);
            var achar = new Busca(numero, cores);
            achar.Primeira();
            var provar = new Busca(numero + 1, cores);
            provar.Primeira();
            linhas.Add(new LinhaNumero(cores, numero, achar.Nos, provar.Nos));
        }

        return linhas;
    }

    /// <summary>A repartição que realiza o número de Schur, escrita por extenso.</summary>
    public static string Reparticao(int cores)
    {
        var numero = Busca.Numero(cores, 60);
        var achada = new Busca(numero, cores).Primeira();
        return achada is null ? string.Empty : Juiz.Texto(achada, cores);
    }
}
