namespace Conde.Schur;

/// <summary>
/// O juiz: dada uma repartição de <c>1..n</c> em cores, diz se alguma cor tem
/// uma soma proibida.
///
/// Ele separa os números por cor e usa a mesma definição do
/// <see cref="Conjunto"/>, sem poda e sem atalho. É a resposta de referência
/// contra a qual a busca com retrocesso é conferida.
/// </summary>
public static class Juiz
{
    public static bool Valida(IReadOnlyList<int> cores, int quantasCores)
    {
        ArgumentNullException.ThrowIfNull(cores);
        for (var cor = 0; cor < quantasCores; cor++)
        {
            var elementos = new List<int>();
            for (var i = 1; i < cores.Count; i++)
            {
                if (cores[i] == cor)
                {
                    elementos.Add(i);
                }
            }

            if (!Conjunto.LivreDeSoma(elementos))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>A soma proibida e a cor em que ela está, ou nulo se não houver.</summary>
    public static (int Cor, int X, int Y, int Z)? Testemunha(IReadOnlyList<int> cores, int quantasCores)
    {
        ArgumentNullException.ThrowIfNull(cores);
        for (var cor = 0; cor < quantasCores; cor++)
        {
            var elementos = new List<int>();
            for (var i = 1; i < cores.Count; i++)
            {
                if (cores[i] == cor)
                {
                    elementos.Add(i);
                }
            }

            var soma = Conjunto.Testemunha(elementos);
            if (soma is not null)
            {
                return (cor, soma.Value.X, soma.Value.Y, soma.Value.Z);
            }
        }

        return null;
    }

    public static string Texto(IReadOnlyList<int> cores, int quantasCores)
    {
        ArgumentNullException.ThrowIfNull(cores);
        var partes = new List<string>();
        for (var cor = 0; cor < quantasCores; cor++)
        {
            var elementos = new List<int>();
            for (var i = 1; i < cores.Count; i++)
            {
                if (cores[i] == cor)
                {
                    elementos.Add(i);
                }
            }

            partes.Add("{" + string.Join(",", elementos) + "}");
        }

        return string.Join(" ", partes);
    }
}
