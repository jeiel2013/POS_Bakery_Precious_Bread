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
            int quant = 0;
            LerDadosDoArquivo(produtos, ref quant);
            int opcao;
            while (true)
            {
                Console.WriteLine("********************************************");
                Console.WriteLine("SISTEMA DE GESTÃO PANIFICAÇÃO - PÃO PRECIOSO");
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("1 - Cadastrar Produto");
                Console.WriteLine("2 - Listar Produtos");
                Console.WriteLine("3 - Excluir Produto");
                Console.WriteLine("3 - ");
                Console.WriteLine("0 - Sair do Sistema");
                Console.WriteLine("********************************************\n");
                Console.Write("Digite a opção desejada: ");
                opcao = int.Parse(Console.ReadLine());

                if (opcao == 0)
                {
                    Console.WriteLine("Saindo...");
                    // GravarDadosNoArquivo(produtos, quant);
                    break;
                }
                Console.Clear();
                switch (opcao)
                {
                    case 1:
                        CadastrarProduto(produtos, ref quant);
                        break;
                    case 2:
                        ListarProdutos(produtos, quant);
                        break;
                    case 3:
                        ExcluirProduto(produtos, ref quant);
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
            GravarDadosNoArquivo(produtos, quant);
        }
        static public void CadastrarProduto(dadosProduto[] produtos, ref int q) // Função para Cadastrar os produtos
        {
            Console.WriteLine("********************************************");
            Console.WriteLine("********** CADASTRO DE PRODUTOS ************");
            Console.WriteLine("--------------------------------------------");
            // int indice;
            Console.Write("Produto: ");
            produtos[q].descricaoProduto = Console.ReadLine().ToUpper(); ; // Nome do Produto
            Console.Write("Valor Unitário do Produto: ");
            produtos[q].valorUnitario = float.Parse(Console.ReadLine()); // Valor Unitário
            Console.Write("Quantidade inicial do estoque: ");
            produtos[q].qtdEstoque = int.Parse(Console.ReadLine()); // Quantidade Estoque
            Console.Write("Data de Fabricação ex.: (22/11/23): ");
            produtos[q].dataFabricacao = Console.ReadLine(); // Data de Fabricação
            Console.Write("Prazo de Validade: ");
            produtos[q].prazoValidade = int.Parse(Console.ReadLine()); // Validade
            q++;
            Console.Clear();
        }

        static public void ListarProdutos(dadosProduto[] produtos, int q) // Função para Listar 
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

        static public void ExcluirProduto(dadosProduto[] produtos, ref int q) // Função para Excluir produto
        {
            Console.WriteLine("********************************************");
            Console.WriteLine("************* EXCLUIR PRODUTO **************");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Qual o produto: ");
            string desc = Console.ReadLine().ToUpper();
            int i = busca_bin(produtos, desc , q);
            if (i != -1)
            {
                Console.WriteLine("Produto: " + produtos[i].descricaoProduto);
                Console.WriteLine("Preço: " + produtos[i].valorUnitario);
                Console.WriteLine("Estoque: " + produtos[i].qtdEstoque);
                Console.Write("Deseja excluir? ('S' = Sim, 'N' = Não): ");
                string resp = Console.ReadLine().ToUpper();
                if (resp == "S") {
                    Console.WriteLine("Este produto foi excluído com sucesso!");
                    Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                    excluir(produtos, ref q, i);
                }
            }
            else
            {
                Console.WriteLine("Produto não cadastrado!!!");
                Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
            }
            Console.ReadKey();
            Console.Clear();
        }

        static void excluir(dadosProduto[] produtos, ref int tam, int x) // Função usada na função ExcluirProduto
        {
            for (int i = x; i < tam - 1; i++)
            {
                produtos[i] = produtos[i + 1];
            }
            tam--;
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
            IFormatter formatter = new BinaryFormatter(); 
            if (File.Exists("Produtos.bin"))
            {
                Stream rd = new FileStream("Produtos.bin", FileMode.Open, FileAccess.Read);
                dadosProduto produto;
                try 
                {
                    while (true)
                    {
                        produto = (dadosProduto)formatter.Deserialize(rd);
                        produtos[n] = produto;
                        n++;
                    }
                }
                catch (Exception err)
                {
                    Console.Write("Dados transferidos para o vetor!");
                }
                rd.Close();
            }
        }

        static void GravarDadosNoArquivo(dadosProduto[] produtos, int n)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream wr = new FileStream("Produtos.bin", FileMode.Create, FileAccess.Write);
            for (int i = 0; i < n; i++)
            {
                formatter.Serialize(wr, produtos[i]); 
            }
            wr.Close();
        }

        static int busca_bin(dadosProduto[] produtos, string chave, int tam)
        {
            int inf = 0, sup = tam, pos = -1, meio;
            while (inf <= sup)
            {
                meio = (inf + sup) / 2;
                if (produtos[meio].descricaoProduto == chave)
                {
                    pos = meio;
                    inf = sup + 1;
                }
                else if (produtos[meio].descricaoProduto.CompareTo(chave) < 0)
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