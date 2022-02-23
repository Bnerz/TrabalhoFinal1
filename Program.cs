using System;
using System.Collections.Generic;
using System.Linq;

namespace Projeto_Final_1___L_Programação
{
    class Program
    {
        static Funcionario funcionarioLogado;
        public static void Main(string[] args)
        {
            Console.Clear(); //limpa o buffer do console

            var opcao = "-1";

            while (opcao != "0")
            {
                Console.WriteLine(" ___________________________________________");
                Console.WriteLine("|                                           |");
                Console.WriteLine("|        Sistema de vendas Mercado APP      |");
                Console.WriteLine("|          1 - Entrar com login             |");
                Console.WriteLine("|      2 - Cadastrar novo funcionário       |");
                Console.WriteLine("|        Digite 0 para finalizar            |");
                Console.WriteLine("|___________________________________________|");
                opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        Logar();
                        break;
                    case "2":
                        NovoFuncionario();
                        break;
                    default:
                        Environment.Exit(0);
                        break;
                }
            }

        }

        static void Logar()
        {
            var credenciais = ObterCredenciais(false);

            funcionarioLogado = Funcionario.ValidarAcesso(credenciais.login, credenciais.senha);

            if (funcionarioLogado != null)
            {
                switch (funcionarioLogado.Cargo)
                {
                    case Perfil.Gerente:
                        MostrarMenuGerente();
                        break;
                    case Perfil.Caixa:
                        MostrarMenuCaixa();
                        break;
                    case Perfil.Repositor:
                        MostrarMenuRepositor();
                        break;
                    default:
                        Environment.Exit(0);
                        break;
                }
            } else
            {
                Console.WriteLine("Acesso negado");
            }
        }

        static void MostrarMenuRepositor()
        {
            //Forçar atualização do stock para novos produtos
            Stock.Init();

            Console.Clear();

            var opcao = "-1";

            while (opcao != "0")
            {
                Console.WriteLine(" ___________________________________________");
                Console.WriteLine("|                                           |");
                Console.WriteLine("|             MENU REPOSITOR                |");
                Console.WriteLine("|         1 - Exibir lista de produtos      |");
                Console.WriteLine("|          2 - Adicionar produto            |");
                Console.WriteLine("|          3 - Excluir produto              |");
                Console.WriteLine("|          4 - Alterar stock                |");
                Console.WriteLine("|          0 - Para finalizar               |");
                Console.WriteLine("|___________________________________________|");
                opcao = Console.ReadLine();

                Console.WriteLine(opcao);

                switch (opcao)
                {
                    case "1":
                        ExibirListaProdutos();
                        break;
                    case "2":
                        MostrarMenuAdicionarProduto();
                        break;
                    case "3":
                        MostrarMenuRemoverProduto();
                        break;
                    case "4":
                        MostrarMenuAlterarStock();
                        break;
                    default:
                        Console.Clear();
                        break;
                }
            }
        }

        private static void MostrarMenuAlterarStock()
        {
            Console.WriteLine(" ___________________________________________");
            Console.WriteLine("|                                           |");
            Console.WriteLine("|      MENU REPOSITOR > ALTERAR STOCK       |");
            Console.WriteLine("|            1 - Exibir stock               |");
            Console.WriteLine("|    2 - Alterar QT produto no stock        |");
            Console.WriteLine("|         0 -  Para finalizar               |");
            Console.WriteLine("|___________________________________________|");
            var opcao = Console.ReadLine();

            Console.WriteLine(opcao);

            switch (opcao)
            {
                case "1":
                    ExibirListaItemStock();
                    break;
                case "2":
                    AlterarStock();
                    break;
                default:
                    Console.Clear();
                    break;
            }
        }

        static void MostrarMenuCaixa()
        {
            Console.Clear();

            var opcao = "-1";

            while (opcao != "0")
            {
                Console.WriteLine(" ___________________________________________");
                Console.WriteLine("|                                           |");
                Console.WriteLine("|                MENU CAIXA                 |");
                Console.WriteLine("|      1 - Exibir lista de vendas           |");
                Console.WriteLine("|           2 - Realizar venda              |");
                Console.WriteLine("|           0 - Para finalizar              |");
                Console.WriteLine("|___________________________________________|");
                opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        ExibirListaVendas();
                        break;
                    case "2":
                        MostrarMenuRealizarVenda();
                        break;
                    default:
                        Console.Clear();
                        break;
                }
            }
        }

        private static void MostrarMenuRealizarVenda()
        {
            Console.Clear();

            var opcao = "-1";

            while (opcao != "0")
            {
                Console.WriteLine(" ___________________________________________");
                Console.WriteLine("|                                           |");
                Console.WriteLine("|       MENU CAIXA > REALIZAR VENDA         |");
                Console.WriteLine("|            1 - Exibir stock               |");
                Console.WriteLine("|           2 - Realizar venda              |");
                Console.WriteLine("|           0 - Para finalizar              |");
                Console.WriteLine("|___________________________________________|");
                opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        ExibirListaItemStock();
                        break;
                    case "2":
                        RealizarVenda();
                        break;
                    default:
                        Console.Clear();
                        break;
                }
            }
        }

        private static void RealizarVenda()
        {
            var opcao = "-1";
            var venda = new Venda();
            venda.ListaProdutos = new List<Produto>();
            venda.NomeFuncionario = funcionarioLogado.Login;

            Console.WriteLine("Digite o nome do cliente");
            venda.NomeCliente = Console.ReadLine();

            while (opcao != "0")
            {
                Console.WriteLine("Insira o ID do produto");
                var idProduto = Console.ReadLine();

                Console.WriteLine("Insira a quantidade");
                var quantidade = Console.ReadLine();

                var lista = IncrementarVenda(Convert.ToInt32(idProduto), Convert.ToInt32(quantidade));
                foreach (var item in lista)
                {
                    venda.ListaProdutos.Add(item);
                }

                Console.WriteLine("0 Para finalizar - Qualquer tecla para continuar");
                opcao = Console.ReadLine();

                if (opcao == "0")
                {
                    FinalizarVenda(venda);
                }
            }
        }

        private static void FinalizarVenda(Venda venda)
        {

            if (venda.ListaProdutos != null && venda.ListaProdutos.Count > 0)
            {
                Venda.CriarVenda(venda);
            }
        }

        private static List<Produto> IncrementarVenda(int idProduto, int quantidade)
        {
            var listaProduto = new List<Produto>();
            var ListaGenerica = Produto.ObterListaProdutos();
            if (ValidarEstoque(idProduto, quantidade))
            {

                for (int i = 0; i < quantidade; i++)
                {
                    listaProduto.Add(ListaGenerica.Single(x => x.Id == idProduto));
                }
            } else
            {
                Console.WriteLine("Produto inválido ou quantidade insuficiente");
            }

            foreach (var item in listaProduto)
            {
                Console.WriteLine(item.ToString());
            }

            return listaProduto;
        }

        private static bool ValidarEstoque(int idProduto, int quantidade)
        {
            var listaStock = Stock.ObterListaItemStock();
            return listaStock.Any(x => x.Prod.Id == idProduto && x.Quantidade >= quantidade);
        }

        private static void ExibirListaVendas()
        {
            foreach (var item in Venda.ObterListaVenda())
            {
                Console.WriteLine(item.ToString());
            }

        }

        static void MostrarMenuGerente()
        {
            Console.Clear();

            var opcao = "-1";

            while (opcao != "0")
            {
                Console.WriteLine(" ___________________________________________");
                Console.WriteLine("|                                           |");
                Console.WriteLine("|                MENU GERENTE               |");
                Console.WriteLine("|     1 - Exibir lista de funcionários      |");
                Console.WriteLine("|        2 - Excluir funcionário            |");
                Console.WriteLine("|        3 - Exibir opções de vendas        |");
                Console.WriteLine("|            0 - Para finalizar             |");
                Console.WriteLine("|___________________________________________|");
                opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        ExibirListaFuncionarios();
                        break;
                    case "2":
                        MostrarMenuRemoverFuncionario();
                        break;
                    case "3":
                        MostrarMenuCaixa();
                        break;
                    default:
                        Console.Clear();
                        break;
                }
            }
        }

        static void MostrarMenuRemoverFuncionario()
        {
            Console.WriteLine("Digite o ID do funcioário a ser removido");
            var idFuncionario = Console.ReadLine();
            var retorno = Gerente.RemoverFuncionario(Convert.ToInt32(idFuncionario));

            if (retorno)
            {
                Console.WriteLine("usuário removido com sucesso!");
            } else
            {
                Console.WriteLine("Não foi possível remover o usuário");
            }

        }

        static void MostrarMenuRemoverProduto()
        {
            Console.WriteLine("Digite o ID do produto a ser removido");
            var idProduto = Console.ReadLine();
            var retorno = Repositor.RemoverProduto(Convert.ToInt32(idProduto));

            if (retorno)
            {
                Console.WriteLine("produto removido com sucesso!");
            } else
            {
                Console.WriteLine("Não foi possível remover o produto");
            }
        }

        static void MostrarMenuAdicionarProduto()
        {
            Console.WriteLine("Digite o nome do produto");
            var nome = Console.ReadLine();

            Console.WriteLine("Digite o preço do produto");
            var preco = Console.ReadLine();

            Console.WriteLine("Digite a categoria do produto");
            Console.WriteLine("1 - Congelado");
            Console.WriteLine("2 - Pateleira");
            Console.WriteLine("3 - Enlatado");
            Console.WriteLine("");
            var categoria = Console.ReadLine();


            var produto = new Produto() { Nome = nome, Preco = decimal.Parse(preco) };

            switch (categoria)
            {
                case "1":
                    produto.Categoria = Categoria.Congelado;
                    break;
                case "2":
                    produto.Categoria = Categoria.Patreleira;
                    break;
                case "3":
                    produto.Categoria = Categoria.Enlatado;
                    break;
                default:
                    break;
            }

            Repositor.CriarProduto(produto);
            Stock.Init();
        }

        static void AlterarStock()
        {
            Console.WriteLine("Digite o ID do produto qpara alterar a quantidade");
            var idProduto = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Digite a quantidade");
            var quantidade = Convert.ToInt32(Console.ReadLine());

            Repositor.AlterarQuantidadeStock(idProduto, quantidade);
        }

        static void ExibirListaFuncionarios()
        {
            foreach (var item in Funcionario.ObterListaFuncionarios())
            {
                Console.WriteLine(item.ToString());
            }
        }

        static void ExibirListaProdutos()
        {
            foreach (var item in Produto.ObterListaProdutos())
            {
                Console.WriteLine(item.ToString());
            }
        }

        static void ExibirListaItemStock()
        {
            foreach (var item in Stock.ObterListaItemStock())
            {
                Console.WriteLine(item.ToString());
            }
        }

        static void NovoFuncionario()
        {
            var credenciais = ObterCredenciais(true);
            Funcionario func;

            switch (credenciais.cargo)
            {
                case Perfil.Gerente:
                    func = new Gerente() { Login = credenciais.login, Nome = credenciais.login, Senha = credenciais.senha, Cargo = credenciais.cargo };
                    break;
                case Perfil.Caixa:
                    func = new Caixa() { Login = credenciais.login, Nome = credenciais.login, Senha = credenciais.senha, Cargo = credenciais.cargo };
                    break;
                case Perfil.Repositor:
                    func = new Repositor() { Login = credenciais.login, Nome = credenciais.login, Senha = credenciais.senha, Cargo = credenciais.cargo };
                    break;
                default:
                    func = new Funcionario() { Login = credenciais.login, Nome = credenciais.login, Senha = credenciais.senha, Cargo = credenciais.cargo };
                    break;
            }

            Funcionario.CriarFuncionario(func);
        }

        static Credenciais ObterCredenciais(bool ehCadastro)
        {
            Console.WriteLine("Insira o login");
            var _login = Console.ReadLine();

            Console.WriteLine("Insira a senha");
            var _senha = Console.ReadLine();

            var credencial = new Credenciais() { login = _login, senha = _senha };

            if (ehCadastro)
            {
                Console.WriteLine(" ___________________________________________");
                Console.WriteLine("|                                           |");
                Console.WriteLine("|        Insira o perfil do funcionário     |");
                Console.WriteLine("|              1 - GERENTE                  |");
                Console.WriteLine("|               2 - CAIXA                   |");
                Console.WriteLine("|             3 - REPOSITOR                 |");
                Console.WriteLine("|___________________________________________|");
                var perfil = Console.ReadLine();

                switch (perfil)
                {
                    case "1":
                        credencial.cargo = Perfil.Gerente;
                        break;

                    case "2":
                        credencial.cargo = Perfil.Caixa;
                        break;

                    case "3":
                        credencial.cargo = Perfil.Repositor;
                        break;

                    default:
                        break;
                }
            }

            return credencial;
        }

        struct Credenciais
        {
            public string login;
            public string senha;
            public Perfil cargo;
        }



    }
}
