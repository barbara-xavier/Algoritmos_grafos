namespace OrdenacaoTopologica_Denso
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
                AdicionarAresta(1, 3);
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
                AdicionarAresta(7, 2);//
                AdicionarAresta(7, 3);//
                AdicionarAresta(4, 6);
                AdicionarAresta(8, 5); //
                AdicionarAresta(6, 9);
                AdicionarAresta(7, 6);
                AdicionarAresta(2, 9); //20

                AdicionarAresta(1, 5);
                AdicionarAresta(2, 8);
                AdicionarAresta(3, 10);
                AdicionarAresta(4, 7);
                AdicionarAresta(5, 6);
                AdicionarAresta(6, 7);
                AdicionarAresta(7, 8);
                AdicionarAresta(8, 9);
                AdicionarAresta(9, 10);
                AdicionarAresta(4, 2); //30

                AdicionarAresta(1, 6);
                AdicionarAresta(9, 2);
                AdicionarAresta(3, 4);
                AdicionarAresta(8, 4);//
                AdicionarAresta(5, 7);
                AdicionarAresta(6, 8);
                AdicionarAresta(7, 9);
                AdicionarAresta(8, 10);
                AdicionarAresta(5, 4);
                AdicionarAresta(4, 10); //40

            }

            public bool ContemCiclo()
            {
                bool[] visitado = new bool[Vertices + 1];
                bool[] pilhaRecursao = new bool[Vertices + 1];

                for (int i = 1; i <= Vertices; i++)
                {
                    if (ContemCicloUtil(i, visitado, pilhaRecursao))
                    {
                        return true;
                    }
                }

                return false;
            }

            private bool ContemCicloUtil(int vertice, bool[] visitado, bool[] pilhaRecursao)
            {
                if (visitado[vertice] == false)
                {
                    visitado[vertice] = true;
                    pilhaRecursao[vertice] = true;

                    for (int i = 1; i <= Vertices; i++)
                    {
                        if (grafo[vertice, i] == 1)
                        {
                            if (!visitado[i] && ContemCicloUtil(i, visitado, pilhaRecursao))
                            {
                                return true;
                            }
                            else if (pilhaRecursao[i])
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