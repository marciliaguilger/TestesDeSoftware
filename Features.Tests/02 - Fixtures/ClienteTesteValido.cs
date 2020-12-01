using Xunit;
using static Features.Tests.ClienteTestsFixtures;

namespace Features.Tests
{
    [Collection(nameof(ClienteCollection))]
    public class ClienteTesteValido
    {
        readonly ClienteTestsFixtures _clienteTestsFixtures;
        public ClienteTesteValido(ClienteTestsFixtures clienteTestsFixtures)
        {
            _clienteTestsFixtures = clienteTestsFixtures;
        }

        [Fact(DisplayName = "Novo Cliente Válido")]
        [Trait("Categoria", "Cliente Trait Testes")]
        public void Cliente_NovoCliente_DeveEstarValido()
        {
            //Arrange
            var cliente = _clienteTestsFixtures.GerarClienteValido();

            //Act
            var result = cliente.EhValido();

            //Assert
            Assert.True(result);
            Assert.Equal(0, cliente.ValidationResult.Errors.Count);
        }
    }
}
