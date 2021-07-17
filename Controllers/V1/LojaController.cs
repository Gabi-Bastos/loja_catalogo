using CatalogoLoja.Exceptions;
using CatalogoLoja.InputModel;
using CatalogoLoja.Services;
using CatalogoLoja.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoLoja.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly LojaService _lojaService;

        public ProdutoController(LojaService lojaService)
        {
            _lojaService = lojaService;
        }

        /// <summary>
        /// Buscar todos os jogos de forma paginada
        /// </summary>
        /// <remarks>
        /// Não é possível retornar os jogos sem paginação
        /// </remarks>
        /// <param name="pagina">Indica qual página está sendo consultada. Mínimo 1</param>
        /// <param name="quantidade">Indica a quantidade de reistros por página. Mínimo 1 e máximo 50</param>
        /// <response code="200">Retorna a lista de jogos</response>
        /// <response code="204">Caso não haja jogos</response>   
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProdutoViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var Produtos = await _lojaService.Obter(pagina, quantidade);

            if (Produtos.Count() == 0)
                return NoContent();

            return Ok(Produtos);
        }

        /// <summary>
        /// Buscar um jogo pelo seu Id
        /// </summary>
        /// <param name="idJogo">Id do jogo buscado</param>
        /// <response code="200">Retorna o jogo filtrado</response>
        /// <response code="204">Caso não haja jogo com este id</response>   
        [HttpGet("{idProduto:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> Obter([FromRoute] Guid idProduto)
        {
            var produto = await _lojaService.Obter(idProduto);

            if (produto == null)
                return NoContent();

            return Ok(produto);
        }

        /// <summary>
        /// Inserir um jogo no catálogo
        /// </summary>
        /// <param name="jogoInputModel">Dados do jogo a ser inserido</param>
        /// <response code="200">Cao o jogo seja inserido com sucesso</response>
        /// <response code="422">Caso já exista um jogo com mesmo nome para a mesma produtora</response>   
        [HttpPost]
        public async Task<ActionResult<ProdutoViewModel>> InserirProduto([FromBody] ProdutoInputModel produtoInputModel)
        {
            try
            {
                var produto = await _lojaService.Inserir(produtoInputModel);

                return Ok(produto);
            }
            catch (ProdutoJaCadastradoException ex)
            {
                return UnprocessableEntity("Já existe um produto com este nome para esta marca");
            }
        }

        /// <summary>
        /// Atualizar um jogo no catálogo
        /// </summary>
        /// /// <param name="idJogo">Id do jogo a ser atualizado</param>
        /// <param name="jogoInputModel">Novos dados para atualizar o jogo indicado</param>
        /// <response code="200">Cao o jogo seja atualizado com sucesso</response>
        /// <response code="404">Caso não exista um jogo com este Id</response>   
        [HttpPut("{idProduto:guid}")]
        public async Task<ActionResult> AtualizarProduto([FromRoute] Guid idProduto, [FromBody] ProdutoInputModel produtoInputModel)
        {
            try
            {
                await _lojaService.Atualizar(idProduto, produtoInputModel);

                return Ok();
            }
            catch (ProdutoNaoCadastradoException ex)
            {
                return NotFound("Não existe este Produto");
            }
        }

        /// <summary>
        /// Atualizar o preço de um jogo
        /// </summary>
        /// /// <param name="idJogo">Id do jogo a ser atualizado</param>
        /// <param name="preco">Novo preço do jogo</param>
        /// <response code="200">Cao o preço seja atualizado com sucesso</response>
        /// <response code="404">Caso não exista um jogo com este Id</response>   
        [HttpPatch("{idProduto:guid}/preco/{preco:double}")]
        public async Task<ActionResult> AtualizarProduto([FromRoute] Guid idProduto, [FromRoute] double preco)
        {
            try
            {
                await _lojaService.Atualizar(idProduto, preco);

                return Ok();
            }
            catch (ProdutoNaoCadastradoException ex)
            {
                return NotFound("Não existe este produto");
            }
        }

        /// <summary>
        /// Excluir um jogo
        /// </summary>
        /// /// <param name="idJogo">Id do jogo a ser excluído</param>
        /// <response code="200">Cao o preço seja atualizado com sucesso</response>
        /// <response code="404">Caso não exista um jogo com este Id</response>   
        [HttpDelete("{idProduto:guid}")]
        public async Task<ActionResult> ApagarProduto([FromRoute] Guid idProduto)
        {
            try
            {
                await _lojaService.Remover(idProduto);

                return Ok();
            }
            catch (ProdutoNaoCadastradoException ex)
            {
                return NotFound("Não existe este produto");
            }
        }

    }
}
