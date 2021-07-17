using System;

namespace CatalogoLoja.Exceptions
{
    public class ProdutoNaoCadastradoException: Exception
    {
        public ProdutoNaoCadastradoException()
            :base("Este produto não está cadastrado")
        {}
    }
}
