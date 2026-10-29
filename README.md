# Números de Schur

Reparta os números de 1 a `n` em `k` montes, de modo que **nenhum monte contenha
`x + y = z`** — nem mesmo com `x` e `y` sendo o mesmo número. A partir de certo `n` isso
deixa de ser possível, e o maior `n` que ainda dá é o número de Schur.

Com dois montes o limite é 4. Com três, é 13. Aqui esses números não são citados: são
achados, e a impossibilidade do seguinte é provada ramo por ramo.

```
dotnet test testes/Schur.Testes/Schur.Testes.csproj -c Release
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- numeros
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- tamanhos
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- custo
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- livres
```

```
cores  numero  nos para achar  nos para provar que nao ha mais
1      1       1               2
2      4       8               11
3      13      406             978

1 cores: {1}
2 cores: {1,4} {2,3}
3 cores: {1,4,7,10,13} {2,3,11,12} {5,6,8,9}
```

## A convenção que muda tudo

`{2, 4}` **não** é livre de soma, porque `2 + 2 = 4`. O 2 aparece uma vez só, e mesmo
assim conta. Essa é a convenção de Schur, está escrita no código e tem um teste só dela,
porque trocá-la muda todos os números deste repositório sem quebrar nada.

## Os dois caminhos

A **força bruta** gera as `k^n` repartições e pergunta ao juiz, que separa os montes e
confere todas as somas. A **busca com retrocesso** pinta 1, 2, 3... e abandona o ramo
assim que a cor escolhida fecha uma soma — e para pintar `z` basta olhar os pares
`x + y = z` já pintados, porque nada mais pode ter mudado.

Os dois contam a mesma coisa em todo tamanho em que os dois cabem, e o medidor **quebra
sozinho** se discordarem.

A busca tem ainda uma quebra de simetria: o 1 fica sempre no primeiro monte e um monte
novo só abre depois que os anteriores já foram usados. Isso divide a conta por `k!` — e
essa divisão é testada, não suposta.

## Os achados

### 1. Com 13 números existem exatamente três repartições, e não mais

```
n    reparticoes  validas
9    19683        546
10   59049        300
11   177147       186
12   531441       114
13   1594323      18
14   4782969      0
```

As 18 repartições válidas de `1..13` são **três**, cada uma contada seis vezes pelas
trocas de cor — e `6 = 3!`. A divisão é exata, o que também diz que as três usam os três
montes.

Treze números, mais de um milhão e meio de maneiras de reparti-los em três, e **três**
funcionam. Uma delas é `{1,4,7,10,13} {2,3,11,12} {5,6,8,9}`: o primeiro monte é uma
progressão de passo 3, e os outros dois não têm padrão nenhum que eu consiga descrever.

### 2. O número de repartições sobe até o nove e desaba

```
n     1   2   3   4   5   6   7   8   9   10  11  12  13  14
val   3   6   18  30  66  120 258 288 546 300 186 114 18  0
```

Eu esperava uma descida desde o começo: mais números, mais chance de dar soma. O que
acontece é que o número de repartições válidas **cresce até 546 no `n = 9`** e só então
cai. E a queda é cada vez mais brutal: de 114 para 18 no último passo, 84% do que restava.

### 3. As duas construções clássicas são ótimas em todos os tamanhos medidos

Dentro de `1..n`, qual é o maior monte livre de soma que existe? Há duas receitas
conhecidas, e elas não se parecem: os **ímpares** (soma de dois ímpares é par) e a
**metade de cima** (soma de dois grandes passa de `n`).

```
n    subconjuntos  livres   maior  impares  metade de cima
10   1024          151      5      5        5
14   16384         847      7      7        7
18   262144        4398     9      9        9
20   1048576       9583     10     10       10
```

A força bruta sobre o **um milhão de subconjuntos** de `1..20` não acha nada melhor que
10, que é o que as duas receitas dão. Duas ideias completamente diferentes, o mesmo teto,
em todos os treze tamanhos medidos.

### 4. Conjunto livre de soma é raro, e fica raro depressa

Na mesma tabela: 151 dos 1.024 subconjuntos de `1..10` são livres (14,7%), e 9.583 do
1.048.576 de `1..20` (0,91%). A **fração cai para pouco mais da metade a cada dois
números**, enquanto o total de subconjuntos quadruplica.

Para efeito de comparação, a busca com retrocesso para três cores abre **978 nós** para
`n = 22`, contra 31.381.059.609 repartições da força bruta: 32 milhões de vezes menos. E
são os mesmos 978 nós de `n = 14` — passado o número de Schur, o problema para de crescer
para quem desiste cedo.

## O que as medidas me corrigiram

- **Eu escrevi que `{1,3}` não é livre de soma, dentro de um teste.** É livre: `1+1=2`,
  `1+3=4` e `3+3=6`, e nenhum dos três está no conjunto. Eu olhei para o 1 e o 3 e vi "dá
  para somar", sem conferir se a soma caía dentro. É a terceira vez neste lote que eu
  digito um número ou um valor que "sei" num arquivo cuja função é impedir exatamente
  isso.

- **Eu esperava que a quantidade de repartições caísse desde o começo.** Ela sobe até o
  `n = 9`, onde são 546. A intuição de que mais números é mais aperto vale no fim e não no
  começo — o mesmo erro que eu cometi noutro repositório deste lote, no mesmo dia, com
  outro problema.

- **Eu ia dizer que a metade de cima é "outra construção, provavelmente pior".** Ela dá
  exatamente o mesmo tamanho dos ímpares, em todos os tamanhos medidos. "Provavelmente" é
  a palavra que aparece quando eu não medi.

- **A divisão por `k!` eu ia deixar como comentário.** Virou teste, e foi bom: ela só vale
  quando todas as cores são usadas, e nos tamanhos pequenos isso não acontece. O teste
  começa no `n = 5` por causa disso, e saber onde a regra passa a valer é parte de saber a
  regra.

## Estrutura

```
src/Schur/
  Conjunto.cs   livre de soma pela definicao, e as duas construcoes classicas
  Juiz.cs       separa os montes e confere, sem poda: a resposta de referencia
  Varredura.cs  a forca bruta sobre as k^n reparticoes
  Busca.cs      o retrocesso, com a poda da soma e a quebra de simetria
  Medidas.cs    as tabelas deste README
testes/Schur.Testes/
ferramentas/Medidor/
```

76 testes.
