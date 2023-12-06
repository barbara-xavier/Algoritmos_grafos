namespace Ordenacao_Topologica_Esparso
{
    internal class Program
    {

        class GrafoDirecionado
        {
            private int[,] grafo;
            private int Vertices;

            public GrafoDirecionado(int vertices)
            {
                Vertices = vertices;
                grafo = new int[vertices + 1, vertices + 1];
               
            }

            public void AdicionarAresta(int de, int para)
            {
                grafo[de, para] = 1;
            }

            public int[,] ObterGrafo()
            {
                return grafo;
            }

            public void MontarGrafo()
            {
                
                AdicionarAresta(1, 2);
                AdicionarAresta(1, 5);
                AdicionarAresta(1, 4);
                AdicionarAresta(2, 5);
                AdicionarAresta(2, 6);
                AdicionarAresta(3, 5);
                AdicionarAresta(3, 8);
                AdicionarAresta(4, 9);
                AdicionarAresta(5, 10);
                AdicionarAresta(6, 10);
                AdicionarAresta(7, 10);
                AdicionarAresta(8, 10);
                AdicionarAresta(9, 10);
                AdicionarAresta(2, 7);
                AdicionarAresta(3, 7);
                AdicionarAresta(4, 6);
                AdicionarAresta(5, 8);
                AdicionarAresta(6, 9);
                AdicionarAresta(7, 6);
                AdicionarAresta(9, 2);
                

            }

            public bool ContemCiclo()
            {
                bool[] visitado = new bool[Vertices + 1]; //verifica se o vértice já foi visitado
                bool[] pilhaRecursao = new bool[Vertices + 1];

                for (int i = 1; i <= Vertices; i++) //anda pelos vértices
                {
                    if (ContemCicloUtil(i, visitado, pilhaRecursao)) //se um vértice conter ciclo, retorna true
                    {
                        return true;
                    }
                }

                return false;
            }

            private bool ContemCicloUtil(int vertice, bool[] visitado, bool[] pilhaRecursao)
            {
                if (visitado[vertice] == false) // se vértice atual não foi visitado
                {
                    visitado[vertice] = true; //passa a ser true agora

                    pilhaRecursao[vertice] = true; //adiciona na pilha de recursão

                    for (int i = 1; i <= Vertices; i++) //loop pelos vértices
                    {
                        if (grafo[vertice, i] == 1) // se for adjacente ao atual
                        {
                            if (!visitado[i] && ContemCicloUtil(i, visitado, pilhaRecursao)) //busca vizinhos do vértice para encontrar ciclos
                            {
                                return true;
                            }
                            else if (pilhaRecursao[i]) // se o i está na pilha de recusão
                            {
                                // Ciclo encontrado, remover aresta
                                grafo[vertice, i] = 0;
                            }
                        }
                    }
                }

                pilhaRecursao[vertice] = false;
                return false;
            }



            public List<int> OrdenacaoTopologicaKahn()
            {
                int[] grauEntrada = new int[Vertices + 1];
                Queue<int> fila = new Queue<int>();

                // Calcula o grau de entrada para cada vértice
                for (int i = 1; i <= Vertices; i++)
                {
                    for (int j = 1; j <= Vertices; j++)
                    {
                        if (grafo[i, j] != 0)
                        {
                            grauEntrada[j]++;
                        }
                    }
                }

                // Adiciona vértices com grau de entrada zero à fila
                for (int i = 1; i <= Vertices; i++)
                {
                    if (grauEntrada[i] == 0)
                    {
                        fila.Enqueue(i);
                    }
                }

                List<int> ordenacaoTopologica = new List<int>();

                // Processa os vértices da fila
                while (fila.Count > 0)
                {
                    int vertice = fila.Dequeue();
                    ordenacaoTopologica.Add(vertice);

                    // Reduz o grau de entrada dos vértices adjacentes
                    for (int i = 1; i <= Vertices; i++)
                    {
                        if (grafo[vertice, i] != 0)
                        {
                            grauEntrada[i]--;

                            // Se o grau de entrada se tornar zero, adicione à fila
                            if (grauEntrada[i] == 0)
                            {
                                fila.Enqueue(i);
                            }
                        }
                    }
                }

                return ordenacaoTopologica;
            }

            public void ExibirGrafoAposRemocaoCiclos()
            {
                Console.WriteLine("\n \n Grafo após a REMOÇÃO de ciclos:\n ");
                ExibirGrafo(grafo);
            }


        }

        static void ExibirGrafo(int[,] grafo)
        {
            for (int i = 1; i <= grafo.GetLength(0) - 1; i++)
            {
                Console.WriteLine($"O vértice {i} está conectado a: ");
                for (int j = 1; j <= grafo.GetLength(1) - 1; j++)
                {
                    if (grafo[i, j] != 0)
                    {
                        Console.Write($"Vértice {j}, ");
                    }
                }
                Console.WriteLine("\n");    
            }
        }
        static void Main(string[] args)
        {
            int vertices = 10;
            GrafoDirecionado grafo = new GrafoDirecionado(vertices);

            grafo.MontarGrafo();

            Console.WriteLine("Grafo original:");
            ExibirGrafo(grafo.ObterGrafo());



            if (grafo.ContemCiclo())
            {
                Console.WriteLine("O grafo contém ciclos. A ordenação topológica não é possível.");
            }
            else
            {
                grafo.ExibirGrafoAposRemocaoCiclos();

                List<int> ordenacaoTopologica = grafo.OrdenacaoTopologicaKahn();

                Console.WriteLine("\nOrdenação Topológica:");
                foreach (int vertice in ordenacaoTopologica)
                {
                    Console.Write($"{vertice} ");
                }


            }

        }
    }
}