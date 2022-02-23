using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto_Final_1___L_Programação
{
    public class Repositor : Funcionario
    {
        public static void CriarProduto(Produto produto)
        {
            Produto.CriarProduto(produto);
        }

        public static void AlterarQuantidadeStock(int idProduto, int quantidade)
        {
            Stock.AlterarQuantidadeStock(idProduto, quantidade);
        }

        public static bool RemoverProduto(int idProduto)
        {
            return Produto.RemoverProduto(idProduto);
        }
    }
}
