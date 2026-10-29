namespace Conde.Schur;

/// <summary>
/// A força bruta: gera as <c>cores^n</c> repartições de <c>1..n</c>, uma a uma,
/// e pergunta ao juiz. Sem poda, sem quebra de simetria, sem nada.
///
/// Ela existe para a busca com retrocesso ter contra quem se conferir, e para
/// medir quanto custa não ser esperto.
/// </summary>
public static class Varredura
{
    public static long Quantidade(int n, int cores)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        ArgumentOutOfRangeException.ThrowIfLessThan(cores, 1);

        long total = 1;
        for (var i = 0; i < n; i++)
        {
            total *= cores;
        }

        return total;
    }

    /// <summary>Quantas repartições válidas existem, contando todas as trocas de cor.</summary>
    public static long Contar(int n, int cores)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        var atual = new int[n + 1];
        return Passo(1);

        long Passo(int numero)
        {
            if (numero > n)
            {
                return Juiz.Valida(atual, cores) ? 1 : 0;
            }

            long achadas = 0;
            for (var cor = 0; cor < cores; cor++)
            {
                atual[numero] = cor;
                achadas += Passo(numero + 1);
            }

            return achadas;
        }
    }

    /// <summary>
    /// O maior subconjunto livre de soma de <c>1..n</c>, pela força bruta sobre
    /// os 2ⁿ subconjuntos.
    /// </summary>
    public static (int Tamanho, List<int> Exemplo) MaiorLivre(int n)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        var melhor = new List<int>();
        foreach (var livre in Conjunto.TodosLivres(n))
        {
            if (livre.Count > melhor.Count)
            {
                melhor = livre;
            }
        }

        return (melhor.Count, melhor);
    }
}
