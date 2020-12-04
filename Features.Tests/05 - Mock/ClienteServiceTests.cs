using Features.Clientes;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace Features.Tests
{
    [Collection(nameof(ClienteBogusCollection))]
    public class ClienteServiceTests
    {
        readonly ClienteTestsBogusFixture _clienteTestsBogus;

        public ClienteServiceTests(ClienteTestsBogusFixture clienteTestsFixtures)
        {
            _clienteTestsBogus = clienteTestsFixtures;
        }

        [Fact(DisplayName = "Adicionar cliente com sucesso")]
        [Trait("Categoria", "Cliente Service Mock Tests")]
        public void ClienteService_Adicionar_DeveExecutarComSucesso()
        {
            //Arrange
            var cliente = _clienteTestsBogus.GerarClienteValido();
            var clienteRepo = new Mock<IClienteRepository>();
            var mediatr = new Mock<IMediator>();

            
            var clienteService = new ClienteService(clienteRepo.Object, mediatr.Object);
            //Act
            clienteService.Adicionar(cliente);

            //Assert
            Assert.True(cliente.EhValido()); //não é obrigatorio
            //assert interno do moq que validar se um determinado método foi chamado para o objeto em questão, quantas vezes?
            clienteRepo.Verify(r => r.Adicionar(cliente), Times.Once);
            //se foi passado no publish qualquer classe que implemente a interface INotification
            mediatr.Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Adicionar cliente com falha")]
        [Trait("Categoria", "Cliente Service Mock Tests")]
        public void ClienteService_Adicionar_DeveFalharDevidoClienteInvalido()
        {
            //Arrange
            var cliente = _clienteTestsBogus.GerarClienteInvalido();
            var clienteRepo = new Mock<IClienteRepository>();
            var mediatr = new Mock<IMediator>();

            var clienteService = new ClienteService(clienteRepo.Object, mediatr.Object);
            //Act
            clienteService.Adicionar(cliente);

            //Assert
            Assert.False(cliente.EhValido()); //não é obrigatorio
            //assert interno do moq que validar se um determinado método foi chamado para o objeto em questão, quantas vezes?
            clienteRepo.Verify(r => r.Adicionar(cliente), Times.Never);
            //se foi passado no publish qualquer classe que implemente a interface INotification
            mediatr.Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }

        public void ClienteService_ObterTodosAtivos_DeveRetornarApenasClientesAtivos()
        {
            //Arrange
            var clienteRepo = new Mock<IClienteRepository>();
            var mediatr = new Mock<IMediator>();

            //fazendo setup do clienteRepo do Mock para o método obterTodos retornar o bogus! 
            clienteRepo.Setup(c => c.ObterTodos())
                .Returns(_clienteTestsBogus.ObterClientesVariados());

            var clienteService = new ClienteService(clienteRepo.Object, mediatr.Object);

            //Act
            var clientes = clienteService.ObterTodosAtivos();

            //Assert
            clienteRepo.Verify(r => r.ObterTodos(), Times.Once);
            Assert.True(clientes.Any());
            Assert.False(clientes.Count(c => !c.Ativo) > 0);

        }
    }
}
