using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.Pedidos
{
    public class PedidoCommandHandlerTests
    {
        //intenção de adicionar um item ao carrinho
        [Fact(DisplayName = "Adicionar Item Novo Pedido com Sucesso")]
        [Trait("Categoria", "Vendas - Pedido Command Handler")]
        public async Task AdicionarItem_NovoPedido_DeveExecutarComSucesso()
        {
            //Arrange
            var pedidoCommand = new AdicionarItemPedidoCommand(Guid.NewGuid(),
                Guid.NewGuid(), "Produto Teste", 2, 100);
            
            //implementação de CQRS via commandHandler -> recebe injeções de dependencia (repositorio e mediator-entrega do comando para o comand handler)
            var mocker = new AutoMocker();
            var pedidoHandler = mocker.CreateInstance<PedidoCommandHandler>();

            //Act
            var result = await pedidoHandler.Handle(pedidoCommand, CancellationToken.None);

            //Assert
            Assert.True(result);
            mocker.GetMock<IPedidoRepository>().Verify(r => r.Adicionar(It.IsAny<Pedido>()), Times.Once);
            mocker.GetMock<IMediator>().Verify(r => r.Publish(It.IsAny<INotification>(),CancellationToken.None), Times.Once);



        }
    }
}
