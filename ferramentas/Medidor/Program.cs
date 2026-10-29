using Conde.Schur;

if (args.Length == 0)
{
    Console.WriteLine("uso: medidor <tamanhos|custo|livres|numeros>");
    return 1;
}

switch (args[0])
{
    case "tamanhos":
        Tamanhos();
        break;
    case "custo":
        Custo();
        break;
    case "livres":
        Livres();
        break;
    case "numeros":
        Numeros();
        break;
    default:
        Console.WriteLine($"comando desconhecido: {args[0]}");
        return 1;
}

return 0;

static void Tamanhos()
{
    foreach (var cores in new[] { 2, 3 })
    {
        Console.WriteLine($"Com {cores} cores: quantas reparticoes validas existem");
        Console.WriteLine("n    reparticoes  forca bruta  busca        nos");
        var ate = cores == 2 ? 7 : 15;
        foreach (var l in Medidas.PorTamanho(cores, Enumerable.Range(1, ate)))
        {
            Console.WriteLine($"{l.N,-5}{l.Reparticoes,-13}{l.ValidasForcaBruta,-13}{l.ValidasBusca,-13}{l.Nos}");
        }

        Console.WriteLine();
    }
}

static void Custo()
{
    Console.WriteLine("O que a poda e a quebra de simetria economizam");
    Console.WriteLine("cores  n    reparticoes      nos        razao");
    foreach (var cores in new[] { 2, 3 })
    {
        foreach (var l in Medidas.Custo(cores, [5, 10, 14, 18, 22]))
        {
            Console.WriteLine($"{l.Cores,-7}{l.N,-5}{l.Reparticoes,-17}{l.Nos,-11}{l.Razao}");
        }
    }
}

static void Livres()
{
    Console.WriteLine("O maior conjunto livre de soma dentro de 1..n");
    Console.WriteLine("n    subconjuntos  livres   maior  impares  metade de cima");
    foreach (var l in Medidas.Livres([1, 2, 3, 4, 5, 6, 8, 10, 12, 14, 16, 18, 20]))
    {
        Console.WriteLine($"{l.N,-5}{l.Subconjuntos,-14}{l.Livres,-9}{l.Maior,-7}{l.Impares,-9}{l.MetadeDeCima}");
    }
}

static void Numeros()
{
    Console.WriteLine("Os numeros de Schur");
    Console.WriteLine("cores  numero  nos para achar  nos para provar que nao ha mais");
    foreach (var l in Medidas.Numeros([1, 2, 3], 30))
    {
        Console.WriteLine($"{l.Cores,-7}{l.Numero,-8}{l.NosParaAchar,-16}{l.NosParaProvar}");
    }

    Console.WriteLine();
    foreach (var cores in new[] { 1, 2, 3 })
    {
        Console.WriteLine($"{cores} cores: {Medidas.Reparticao(cores)}");
    }
}
