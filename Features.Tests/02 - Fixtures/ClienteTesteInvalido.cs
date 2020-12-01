using Features.Clientes;
using System;
using Xunit;
using static Features.Tests.ClienteTestsFixtures;

namespace Features.Tests
{
    [Collection(nameof(ClienteCollection))]
    public class ClienteTesteInvalido
    {
        readonly ClienteTestsFixtures _clienteTestsFixtures;
        public ClienteTesteInvalido(ClienteTestsFixtures clienteTestsFixtures)
        {
            _clienteTestsFixtures = clienteTestsFixtures;
        }

        [Fact(DisplayName = "Novo Cliente inválido")]
        [Trait("Categoria", "Cliente Trait Testes")]
        public void Cliente_NovoCliente_DeveEstarInvalido()
        {
            //Arrange
            var cliente = _clienteTestsFixtures.GerarClienteInvalido();

            //Act
            var result = cliente.EhValido();

            //Assert
            Assert.False(result);
            Assert.NotEqual(0, cliente.ValidationResult.Errors.Count);
        }
    }
}
