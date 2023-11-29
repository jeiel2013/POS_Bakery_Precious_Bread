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
        public int qtdEstoqueInicial;
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
                Console.WriteLine("4 - Registrar Venda");
                Console.WriteLine("5 - Fechamento do Caixa");
                Console.WriteLine("6 - Reposição de Estoque");
                Console.WriteLine("7 - Gerar Relatório de Fluxo de Caixa");
                Console.WriteLine("8 - Gerar Relatório de Inventário");
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
                        CadastrarProduto(produtos, ref quant);
                        break;
                    case 2:
                        ListarProdutosOrdenados(produtos, quant);
                        break;
                    case 3:
                        ExcluirProduto(produtos, ref quant);
                        break;
                    case 4:
                        RegistrarVenda(produtos, ref quant);
                        break;
                    case 5:
                        FechamentoDoCaixa(produtos, quant);
                        break;
                    case 6:
                        ReposicaoDeEstoque(produtos, ref quant);
                        break;
                    case 7:
                        GerarRelatorioFluxoDeCaixa(produtos, quant);
                        break;
                    case 8:
                        GerarRelatorioInventario(produtos, quant);
                        break;
                    default:
                        Console.WriteLine("Opção inválida...");
                        break;
                }
            }
            GravarDadosNoArquivo(produtos, quant);
        }

        static void CadastrarProduto(dadosProduto[] produtos, ref int q)
        {
            Console.WriteLine("********************************************");
            Console.WriteLine("********** CADASTRO DE PRODUTOS ************");
            Console.WriteLine("--------------------------------------------");
            Console.Write("Produto: ");
            produtos[q].descricaoProduto = Console.ReadLine().ToUpper();
            Console.Write("Valor Unitário do Produto: ");
            produtos[q].valorUnitario = float.Parse(Console.ReadLine());
            Console.Write("Quantidade inicial do estoque: ");
            produtos[q].qtdEstoque = int.Parse(Console.ReadLine());
            produtos[q].qtdEstoqueInicial = produtos[q].qtdEstoque;
            Console.Write("Data de Fabricação ex.: (22/11/23): ");
            produtos[q].dataFabricacao = Console.ReadLine();
            Console.Write("Prazo de Validade: ");
            produtos[q].prazoValidade = int.Parse(Console.ReadLine());
            q++;
            Console.Clear();
        }

        static void ListarProdutosOrdenados(dadosProduto[] produtos, int q)
        {
            Console.WriteLine("********************************************");
            Console.WriteLine("********** LISTA DE PRODUTOS ***************");
            Console.WriteLine("--------------------------------------------");

            var produtosOrdenados = produtos
                .Where(p => !string.IsNullOrEmpty(p.descricaoProduto))
                .OrderBy(p => p.descricaoProduto)
                .ToList();

            foreach (var produto in produtosOrdenados)
            {
                Console.WriteLine($"Produto: {produto.descricaoProduto}");
                Console.WriteLine($"Valor Unitário: R${produto.valorUnitario}");
                Console.WriteLine($"Quantidade Estoque: {produto.qtdEstoque} unidades.");
                Console.WriteLine($"Data de Fabricação: {produto.dataFabricacao}");
                Console.WriteLine($"Validade: {produto.prazoValidade} dias.");
                Console.WriteLine("--------------------------------------------\n");
            }
        Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
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

        static public void listarProdutosOrdenados(dadosProduto[] produtos, int q)
        {
            var produtosOrdenados = produtos
                .Where(p => !string.IsNullOrEmpty(p.descricaoProduto))
                .OrderBy(p => p.descricaoProduto)
                .ToList();

            foreach (var produto in produtosOrdenados)
            {
                Console.WriteLine($"Produto: {produto.descricaoProduto}");
                Console.WriteLine($"Valor Unitário: R${produto.valorUnitario}");
                Console.WriteLine($"Quantidade Estoque: {produto.qtdEstoque} unidades.");
                Console.WriteLine($"Data de Fabricação: {produto.dataFabricacao}");
                Console.WriteLine($"Validade: {produto.prazoValidade} dias.");
                Console.WriteLine("--------------------------------------------\n");
            }
        }

        static void ExcluirProduto(dadosProduto[] produtos, ref int q)
        {
            Console.WriteLine("********************************************");
            Console.WriteLine("************* EXCLUIR PRODUTO **************");
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Qual o produto: ");
            string desc = Console.ReadLine().ToUpper();
            int i = busca_bin(produtos, desc, q);
            if (i != -1)
            {
                Console.WriteLine("Produto: " + produtos[i].descricaoProduto);
                Console.WriteLine("Preço: " + produtos[i].valorUnitario);
                Console.WriteLine("Estoque: " + produtos[i].qtdEstoque);
                Console.Write("Deseja excluir? ('S' = Sim, 'N' = Não): ");
                string resp = Console.ReadLine().ToUpper();
                if (resp == "S")
                {
                    Console.WriteLine("Este produto foi excluído com sucesso!");
                    Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                    excluir(produtos, ref q, i);
                }
                else
                {
                    Console.WriteLine("Produto não cadastrado!!!");
                    Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
                }
            }
            Console.ReadKey();
            Console.Clear();
        }

        static void excluir(dadosProduto[] produtos, ref int tam, int x)
        {
            for (int i = x; i < tam - 1; i++)
            {
                produtos[i] = produtos[i + 1];
            }
            tam--;
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

        static void RegistrarVenda(dadosProduto[] produtos, ref int q)
        {
            Console.WriteLine("********************************************");
            Console.WriteLine("************ REGRISTRAR VENDA **************");
            Console.WriteLine("--------------------------------------------");

            listarProdutosOrdenados(produtos, q);

            Console.Write("Escolha o produto para venda: ");
            string produtoSelecionado = Console.ReadLine().ToUpper();
            int indiceProduto = busca_bin(produtos, produtoSelecionado, q);

            if (indiceProduto != -1)
            {
                Console.Write("Quantidade para venda: ");
                int quantidadeVendida = int.Parse(Console.ReadLine());

                if (quantidadeVendida <= produtos[indiceProduto].qtdEstoque)
                {
                    float valorVenda = quantidadeVendida * produtos[indiceProduto].valorUnitario;
                    Console.WriteLine($"Total da venda: R${valorVenda}");

                    produtos[indiceProduto].qtdEstoque -= quantidadeVendida;

                    Console.WriteLine("Venda registrada com sucesso!");
                }
                else
                {
                    Console.WriteLine("Quantidade insuficiente em estoque!");
                }
            }
            else
            {
                Console.WriteLine("Produto não encontrado!");
            }

            Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            Console.Clear();
        }

        static void FechamentoDoCaixa(dadosProduto[] produtos, int q)
        {
            Console.WriteLine("********************************************");
            Console.WriteLine("*********** FECHAMENTO DO CAIXA ************");
            Console.WriteLine("--------------------------------------------");

            float totalVendas = 0;

            for (int i = 0; i < q; i++)
            {
                totalVendas += (produtos[i].qtdEstoqueInicial - produtos[i].qtdEstoque) * produtos[i].valorUnitario;
            }

            Console.WriteLine($"Total de Vendas: R${totalVendas}");

            Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            Console.Clear();
        }

        static void ReposicaoDeEstoque(dadosProduto[] produtos, ref int q)
        {
            Console.WriteLine("********************************************");
            Console.WriteLine("*********** REPOSIÇÃO DE ESTOQUE ***********");
            Console.WriteLine("--------------------------------------------");

            ListarProdutosOrdenados(produtos, q);

            Console.Write("Escolha o produto para reposição de estoque: ");
            string produtoSelecionado = Console.ReadLine().ToUpper();
            int indiceProduto = busca_bin(produtos, produtoSelecionado, q);

            if (indiceProduto != -1)
            {
                Console.Write("Quantidade para reposição: ");
                int quantidadeReposicao = int.Parse(Console.ReadLine());

                produtos[indiceProduto].qtdEstoque += quantidadeReposicao;

                Console.WriteLine($"Estoque de {produtos[indiceProduto].descricaoProduto} reposto com sucesso.");
            }
            else
            {
                Console.WriteLine("Produto não encontrado!");
            }

            Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            Console.Clear();
        }


        static void GerarRelatorioFluxoDeCaixa(dadosProduto[] produtos, int q)
        {
            StreamWriter relFluxoCaixa = new StreamWriter("FluxoCaixa.txt", false);
            relFluxoCaixa.WriteLine("Relatório de Fluxo de Caixa");

            float totalVendas = 0;

            for (int i = 0; i < q; i++)
            {
                totalVendas += (produtos[i].qtdEstoqueInicial - produtos[i].qtdEstoque) * produtos[i].valorUnitario;
            }

            relFluxoCaixa.WriteLine($"Total de Vendas: R${totalVendas}");

            relFluxoCaixa.Close();

            Console.WriteLine("Relatório de Fluxo de Caixa gerado com sucesso!");
            Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            Console.Clear();
        }

        static void GerarRelatorioInventario(dadosProduto[] produtos, int q)
        {
            StreamWriter relInventario = new StreamWriter("Inventario.txt", false);
            relInventario.WriteLine("Relatório de Inventário");

            for (int i = 0; i < q; i++)
            {
                relInventario.WriteLine($"Produto: {produtos[i].descricaoProduto} | Quantidade em Estoque: {produtos[i].qtdEstoque}");
            }

            relInventario.Close();

            Console.WriteLine("Relatório de Inventário gerado com sucesso!");
            Console.WriteLine("Pressione qualquer tecla para voltar ao menu...");
            Console.ReadKey();
            Console.Clear();
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
    }
}