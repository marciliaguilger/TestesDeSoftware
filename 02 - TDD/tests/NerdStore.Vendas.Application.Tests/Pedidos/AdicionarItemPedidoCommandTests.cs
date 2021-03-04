using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Domain;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class AdicionarItemPedidoCommandTests
    {
        [Fact(DisplayName = "Adicionar Item Command Válido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void AdicionarItemPedidoCommand_CommandoEstaValido_DevePassarNaValidacao()
        {
            //arange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.NewGuid(),
                Guid.NewGuid(), "Produto Teste", 2, 100);

            //act
            var result = pedidoCommand.EhValido();

            //assert
            Assert.True(result);

        }

        [Fact(DisplayName = "Adicionar Item Command Inválido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public  void AdicionarItemPedidoCommand_CommandoEstaValido_NaoDevePassarNaValidacao()
        {
            //arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.Empty,
                Guid.Empty, "", 0, 0);
            //act
            var result = pedidoCommand.EhValido();

            //assert
            Assert.False(result);
            Assert.Contains(AdicionarItemPedidoValidation.IdClienteErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AdicionarItemPedidoValidation.IdProdutoErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AdicionarItemPedidoValidation.NomeErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AdicionarItemPedidoValidation.QtdMinErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AdicionarItemPedidoValidation.ValorErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));

        }

        [Fact(DisplayName = "Adicionar Item Command Inválido")]
        [Trait("Categoria", "Vendas - Pedido Commands")]
        public void AdicionarItemPedidoCommand_QuantidadeUnidadesSuperiorAoPermitido_NaoDevePassarNaValidacao()
        {
            //arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.NewGuid(),
                Guid.NewGuid(), "Produto Teste", Pedido.MAX_UNIDADES_ITEM +1, 100);
            //act
            var result = pedidoCommand.EhValido();
            //assert
            Assert.False(result);
            Assert.Contains(AdicionarItemPedidoValidation.QtdMaxErroMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }
    }


}
