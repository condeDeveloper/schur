namespace Conde.Schur;

/// <summary>
/// A busca com retrocesso: pinta 1, 2, 3... e abandona o ramo assim que a cor
/// escolhida fecha uma soma proibida.
///
/// A poda é a única esperteza, e ela é mínima: ao pintar <c>z</c> de uma cor,
/// basta procurar um par <c>x + y = z</c> com <c>x</c> e <c>y</c> já pintados
/// daquela cor. Nada mais é conferido, porque nada mais pode ter mudado.
///
/// Há uma segunda poda, essa de simetria: o número 1 fica sempre na cor 0, e
/// uma cor nova só pode ser usada depois que todas as anteriores já apareceram.
/// Sem ela, cada resposta seria contada <c>k!</c> vezes.
/// </summary>
public sealed class Busca
{
    private readonly int _n;
    private readonly int _cores;
    private readonly int[] _atual;
    private readonly bool _quebrarSimetria;

    public long Nos { get; private set; }

    public Busca(int n, int cores, bool quebrarSimetria = true)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(n);
        ArgumentOutOfRangeException.ThrowIfLessThan(cores, 1);
        _n = n;
        _cores = cores;
        _atual = new int[n + 1];
        _quebrarSimetria = quebrarSimetria;
    }

    /// <summary>A primeira repartição válida, ou nulo se não existir nenhuma.</summary>
    public int[]? Primeira()
    {
        Nos = 0;
        return Passo(1, 0) ? (int[])_atual.Clone() : null;
    }

    /// <summary>Quantas repartições válidas existem.</summary>
    public long Contar()
    {
        Nos = 0;
        return Todas(1, 0);
    }

    private bool Passo(int numero, int usadas)
    {
        if (numero > _n)
        {
            return true;
        }

        var limite = _quebrarSimetria ? Math.Min(usadas + 1, _cores) : _cores;
        for (var cor = 0; cor < limite; cor++)
        {
            Nos++;
            if (FechaSoma(numero, cor))
            {
                continue;
            }

            _atual[numero] = cor;
            if (Passo(numero + 1, Math.Max(usadas, cor + 1)))
            {
                return true;
            }
        }

        return false;
    }

    private long Todas(int numero, int usadas)
    {
        if (numero > _n)
        {
            return 1;
        }

        long achadas = 0;
        var limite = _quebrarSimetria ? Math.Min(usadas + 1, _cores) : _cores;
        for (var cor = 0; cor < limite; cor++)
        {
            Nos++;
            if (FechaSoma(numero, cor))
            {
                continue;
            }

            _atual[numero] = cor;
            achadas += Todas(numero + 1, Math.Max(usadas, cor + 1));
        }

        return achadas;
    }

    /// <summary>
    /// Pintar <paramref name="z"/> desta cor fecha uma soma proibida? Só os
    /// pares que somam <c>z</c> precisam ser olhados, e só os já pintados.
    /// </summary>
    private bool FechaSoma(int z, int cor)
    {
        for (var x = 1; x + x <= z; x++)
        {
            var y = z - x;
            if (_atual[x] == cor && _atual[y] == cor && x < z && y < z)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// O maior <c>n</c> que ainda dá para repartir em <paramref name="cores"/>
    /// conjuntos livres de soma: o número de Schur. Devolve zero se nem o
    /// limite dado chegar lá.
    /// </summary>
    public static int Numero(int cores, int limite)
    {
        for (var n = 1; n <= limite; n++)
        {
            if (new Busca(n, cores).Primeira() is null)
            {
                return n - 1;
            }
        }

        return 0;
    }
}
