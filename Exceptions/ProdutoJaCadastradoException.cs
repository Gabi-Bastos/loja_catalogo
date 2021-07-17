using System;

namespace CatalogoLoja.Exceptions
{
    public class ProdutoJaCadastradoException : Exception
    {
        public ProdutoJaCadastradoException()
            : base("Este Produto já está cadastrado")
        { }
    }
}
