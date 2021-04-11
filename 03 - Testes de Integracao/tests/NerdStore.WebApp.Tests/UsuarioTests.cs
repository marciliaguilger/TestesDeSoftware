using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests
{
    //Para injetar automaticamente as dependencias
    [Collection(nameof(IntegrationWebTestsFixtureCollection))]
    public class UsuarioTests
    {
        private readonly IntegrationTestsFixture<StartupWebTests> _testsFixture;
        public UsuarioTests(IntegrationTestsFixture<StartupWebTests> testsFixture)
        {
            //a fixture não é recriada a cada teste! ela mantém o estado.
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Realizar cadastro com sucesso")]
        [Trait("Categoria", "Integração Web - usuário")]
        public async Task Usuario_RealizarCadastro_DeveExecutarComSucesso()
        {
            var initialResponse = await _testsFixture.Client.GetAsync("/Identity/Account/Register");
            initialResponse.EnsureSuccessStatusCode();
        }

    }
}
