using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace PadariaPaoPrecioso
{
    [Serializable]
    struct dadosProduto
    {
        public string descricaoProduto;
        public float valorUnitario;
        public int qtdEstoque;
        public string dataFabricacao;
        public int prazoValidade; // Referente a X dias
    }

    class Program
    {
        static void Main()
        {
            dadosProduto[] produtos = new dadosProduto[100];
            int opcao;
            while (true)
            {
                Console.WriteLine("********************************************");
                Console.WriteLine("SISTEMA DE GESTÃO PANIFICAÇÃO - PÃO PRECIOSO");
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("1 - Cadastrar Produto");
                Console.WriteLine("2 - Listar Produtos");
                Console.WriteLine("3 - Registrar Venda");
                Console.WriteLine("3 - ");
                Console.WriteLine("0 - Sair do Sistema");
                Console.WriteLine("********************************************\n");
                Console.Write("Digite a opção desejada: ");
                opcao = int.Parse(Console.ReadLine());

                if (opcao == 0)
                {
                    Console.WriteLine("Saindo...");
                    break;
                }
                Console.Clear();
                switch (opcao)
                {
                    case 1:
                        CadastrarProduto(produtos);
                        break;
                    case 2:
                        ListarProdutos(produtos);
                        break;
                    case 3:
                        RegistrarVenda(produtos);
                        break;
                    default:
                        Console.WriteLine("Opção inválida...");
                        Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                        Console.ReadKey();
                        Console.Clear(); 
                        break;
                }
                /*Console.WriteLine("\n");
                Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                Console.ReadKey();
                Console.Clear();*/
            }
        }
        static public void CadastrarProduto(dadosProduto[] produtos) // Função para Cadastrar os produtos
        {
            Console.WriteLine("********************************************");
            Console.WriteLine("********** CADASTRO DE PRODUTOS ************");
            Console.WriteLine("--------------------------------------------");
            int indice = ObterIndice(produtos);
            Console.Write("Produto: ");
            produtos[indice].descricaoProduto = Console.ReadLine(); // Nome do Produto
            Console.Write("Valor Unitário do Produto: ");
            produtos[indice].valorUnitario = float.Parse(Console.ReadLine()); // Valor Unitário
            Console.Write("Quantidade inicial do estoque: ");
            produtos[indice].qtdEstoque = int.Parse(Console.ReadLine()); // Quantidade Estoque
            Console.Write("Data de Fabricação ex.: (22/11/23): ");
            produtos[indice].dataFabricacao = Console.ReadLine(); // Data de Fabricação
            Console.Write("Prazo de Validade: ");
            produtos[indice].prazoValidade = int.Parse(Console.ReadLine()); // Validade
            Console.Clear();
        }

        static public void ListarProdutos(dadosProduto[] produtos) 
        {
            Console.WriteLine("********************************************");
            Console.WriteLine("********** LISTA DE PRODUTOS ***************");
            Console.WriteLine("--------------------------------------------");
            int indice = ObterIndice(produtos);
            if (indice == 0)
            {
                Console.WriteLine("Nenhum Produto Encontrado!");
            }

            else
            {
                for (int i = 0; i < indice; i++)
                {
                    Console.WriteLine($"Produto: {produtos[i].descricaoProduto}");
                    Console.WriteLine($"Valor Unitário: R${produtos[i].valorUnitario}");
                    Console.WriteLine($"Quantidade Estoque: {produtos[i].qtdEstoque} unidades.");
                    Console.WriteLine($"Data de Fabricação: {produtos[i].dataFabricacao}");
                    Console.WriteLine($"Validade: {produtos[i].prazoValidade} dias.");
                    Console.WriteLine("--------------------------------------------\n");
                }
            }
            Console.WriteLine("\n");
            Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            Console.Clear();
        }

        static public int ObterIndice(dadosProduto[] produtos)
        {
            int indice;
            for (indice = 0; indice < produtos.Length; indice++)
            {
                if (produtos[indice].descricaoProduto == null)
                {
                    break;
                }
            }
            return indice;
        }

        static public void RegistrarVenda(dadosProduto[] produtos)
        {
            Console.WriteLine("********************************************");
            Console.WriteLine("************ REGRISTRAR VENDA **************");
            Console.WriteLine("--------------------------------------------");
        }

        static void LerDadosDoArquivo(dadosProduto[] produtos, ref int n)
        {
            IFormatter formatter = new BinaryFormatter(); //Permite operação binária no arquivo
            if (File.Exists("Produtos.bin"))
            {
                Stream rd = new FileStream("Produtos.bin", FileMode.Open, FileAccess.Read);
                //abre o arquivo somente leitura
                dadosProduto produto;
                try ///permite interceptar e tratar as exceções (erros)
                {
                    while (true)
                    {
                        produto = (dadosProduto)formatter.Deserialize(rd); //Obtem os dados do arquivo no formato prod
                        produtos[n] = produto;
                        n++;
                    }
                }
                catch (Exception err) //Se houver erro, ele é passado para err
                {
                    Console.Write("Dados transferidos para o vetor!");
                }
                rd.Close();
            }
        }

        static void GravarDadosNoArquivo(dadosProduto[] produtos, int n)
        {
            IFormatter formatter = new BinaryFormatter(); //Permite operação binária
            Stream wr = new FileStream("Produtos.bin", FileMode.Create, FileAccess.Write);
            //Cria o arquivo e abre um fluxo
            for (int i = 0; i < n; i++)
            {
                formatter.Serialize(wr, produtos[i]); //grava os elementos do vetor p serializados
                                               // no arquivo wr
            }
            wr.Close();
        }

        static int busca_bin(dadosProduto[] L, string chave, int tam)
        {
            int inf = 0, sup = tam, pos = -1, meio;
            while (inf <= sup)
            {
                meio = (inf + sup) / 2;
                if (L[meio].descricaoProduto == chave)
                {
                    pos = meio;
                    inf = sup + 1;
                }
                else if (L[meio].descricaoProduto.CompareTo(chave) < 0)
                    inf = meio + 1;
                else sup = meio - 1;
            }
            return pos;
        }

        static void relatorio(dadosProduto[] p, int q)
        {
            StreamWriter relprod = new StreamWriter("Produtos.txt", false);
            relprod.WriteLine("Relatório de Produtos");
            for (int i = 0; i < q; i++)
            {
                relprod.WriteLine($"Nome: {p[i].descricaoProduto}| Valor: {p[i].valorUnitario} | Estoque: {p[i].qtdEstoque} | Fabricação: {p[i].dataFabricacao} | Validade: {p[i].prazoValidade}");
            }
            relprod.Close();
        }
    }
}