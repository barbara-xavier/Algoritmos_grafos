namespace Algoritmo_Prim
{
    internal class Program
    {
        class GrafoEsparso
        {
            private int[,] grafo;
            private int Vertices;

            public GrafoEsparso(int vertices)
            {
                Vertices = vertices;
                grafo = new int[vertices + 1, vertices + 1];
                Preencher();
            }

            private void Preencher()
            {
                int qtdAresta = 0;

                for (int i = 1; i <= Vertices; i++)
                {
                    for (int j = i + 1; j <= Vertices; j++)
                    {
                        if (qtdAresta < 20) //máximo de 20 arestas
                        {
                            int peso = grafo[i, j] = (i + j) % 9 + 1; ; // Peso variando de 1 a 9
                            AdicionarAresta(i, j, peso);
                            qtdAresta++;
                        }
                        else
                        {
                            break;
                        }

                    }
                }
            }

            private void AdicionarAresta(int de, int para, int peso)
            {
                //duas arestas para um grafo não direcionado
                grafo[de, para] = peso;
                grafo[para, de] = peso;
            }

            public void Imprimir()
            {
                for (int i = 1; i <= Vertices; i++)
                {
                    Console.WriteLine($"Vértice {i} está conectado a: ");
                    for (int j = 1; j <= Vertices; j++)
                    {
                        if (grafo[i, j] > 0)
                        {
                            Console.WriteLine($"Vértice: {j}, Peso: {grafo[i, j]}");
                        }

                    }
                    Console.WriteLine();
                }

            }

            private int VerticeMinimo(int[] distancia, bool[] inserido)
            {
                //pegar aresta de menor peso
                int arestaMinima = int.MaxValue;
                int indice = 1;

                for (int vertice = 1; vertice <= Vertices; vertice++)
                {
                    if (inserido[vertice] == false && distancia[vertice] < arestaMinima) // se o vértice não estiver na AGM e a aresta for mínima
                    {
                        arestaMinima = distancia[vertice]; // recebe o peso da menor aresta
                        indice = vertice; // recebe o índice selecionado
                    }
                }

                return indice;
            }

            public int[,] Prim()
            {
                bool[] inserido = new bool[Vertices + 1];
                int[] distancia = new int [Vertices+1];
                int[] pai = new int[Vertices+1];

                //todo o vetor de distancia recebe um valor máximo
                //todo o vetor de inserido recebe falso
                for (int i = 1; i <= Vertices; i++)
                {
                    distancia[i] = int.MaxValue;
                    inserido[i] = false;
                }

                distancia[1] = 0;
                pai[1] = 1; 

                int[,] grafoM = new int[Vertices - 1, 3]; //matriz de arestas (vertice1, vertice2, peso)


                for (int i = 0; i < Vertices-1; i++) //repete até existir N-1 vértices
                {
                    int vertice = VerticeMinimo(distancia,inserido); //obtem o peso e o vértice selecionado                    
                    inserido[vertice] = true; // incluído na AGM

                    for (int j = 1; j <= Vertices; j++) //percorre todos os vértices adjacentes a que não estão na AGM
                    {
                        if (grafo[vertice, j] > 0 && inserido[j] == false)
                        {
                         
                            // Verificar se há uma aresta paralela e se seu peso é menor
                            if (grafo[j, pai[j]] > 0 && grafo[j, pai[j]] < distancia[j])
                            {
                                pai[j] = vertice;
                                distancia[j] = grafo[j, pai[j]];
                            }
                            else if (grafo[vertice, j] < distancia[j])
                            {
                                pai[j] = vertice;
                                distancia[j] = grafo[vertice, j];
                            }
                        }
                       
                    }
                        grafoM[i, 0] = pai[vertice];
                        grafoM[i, 1] = vertice;
                        grafoM[i, 2] = distancia[vertice];
                    
                    if (grafoM[i, 0] == grafoM[i, 1]) //caso sejam vértices iguais
                    {
                        i--; // decrementa o índice para reprocessar essa iteração
                    }
                }
                return grafoM;
            }
        }

        static void Main(string[] args)
        {
            int vertices = 10;
            GrafoEsparso grafo = new GrafoEsparso(vertices);

            grafo.Imprimir();

            Console.WriteLine("\nAplicando o algoritmo de Prim:\n");

            int[,] agm = grafo.Prim();

            
            for (int i = 0; i < agm.GetLength(0); i++)
            {
                Console.WriteLine($"Aresta: {agm[i, 0]} - {agm[i, 1]}, Peso: {agm[i, 2]}");
            }

        }
    }
}