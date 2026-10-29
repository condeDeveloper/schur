namespace Conde.Schur;

/// <summary>
/// Um conjunto de inteiros de <c>1..n</c> guardado em bits, e a pergunta que
/// interessa sobre ele: existe soma de dois elementos que também é elemento?
///
/// Um conjunto é <b>livre de soma</b> quando não existe <c>x + y = z</c> com
/// os três dentro dele. O <c>x</c> e o <c>y</c> podem ser o mesmo número — é
/// por isso que <c>{2, 4}</c> não é livre, embora o 2 apareça uma vez só.
/// Essa é a convenção de Schur, e trocá-la muda todos os números deste
/// repositório.
/// </summary>
public static class Conjunto
{
    public static bool LivreDeSoma(IReadOnlyList<int> elementos)
    {
        ArgumentNullException.ThrowIfNull(elementos);
        var dentro = new HashSet<int>(elementos);
        foreach (var x in elementos)
        {
            foreach (var y in elementos)
            {
                if (dentro.Contains(x + y))
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// A soma proibida que estraga o conjunto, ou nulo se ele for livre. Serve
    /// para os testes poderem mostrar o motivo em vez de só dizer que falhou.
    /// </summary>
    public static (int X, int Y, int Z)? Testemunha(IReadOnlyList<int> elementos)
    {
        ArgumentNullException.ThrowIfNull(elementos);
        var dentro = new HashSet<int>(elementos);
        foreach (var x in elementos)
        {
            foreach (var y in elementos)
            {
                if (dentro.Contains(x + y))
                {
                    return (x, y, x + y);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Todos os subconjuntos livres de soma de <c>1..n</c>, pela força bruta
    /// sobre os 2ⁿ subconjuntos.
    /// </summary>
    public static List<List<int>> TodosLivres(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        var todos = new List<List<int>>();
        var limite = 1L << n;

        for (long mascara = 0; mascara < limite; mascara++)
        {
            var elementos = new List<int>();
            for (var i = 0; i < n; i++)
            {
                if ((mascara & (1L << i)) != 0)
                {
                    elementos.Add(i + 1);
                }
            }

            if (LivreDeSoma(elementos))
            {
                todos.Add(elementos);
            }
        }

        return todos;
    }

    /// <summary>
    /// Os ímpares de <c>1..n</c>: a soma de dois ímpares é par, então nenhum
    /// deles pode ser a soma de dois outros. É a construção mais conhecida de
    /// conjunto livre de soma.
    /// </summary>
    public static List<int> Impares(int n)
    {
        var impares = new List<int>();
        for (var i = 1; i <= n; i++)
        {
            if (i % 2 == 1)
            {
                impares.Add(i);
            }
        }

        return impares;
    }

    /// <summary>
    /// A metade de cima de <c>1..n</c>: a soma de dois números grandes passa de
    /// <c>n</c> e sai do conjunto. É a outra construção clássica, e ela tem
    /// exatamente o mesmo tamanho da primeira.
    /// </summary>
    public static List<int> MetadeDeCima(int n)
    {
        var grandes = new List<int>();
        for (var i = (n / 2) + 1; i <= n; i++)
        {
            grandes.Add(i);
        }

        return grandes;
    }
}
