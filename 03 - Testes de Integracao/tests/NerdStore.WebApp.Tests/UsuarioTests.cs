using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Config;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
            //arrange
            var initialResponse = await _testsFixture.Client.GetAsync("/Identity/Account/Register");
            initialResponse.EnsureSuccessStatusCode();
            var antiForgeryToken = _testsFixture.ObterAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());

            _testsFixture.GerarUserSenha();


            var formData = new Dictionary<string, string>
            {
                {_testsFixture.AntiForgeryFiedlName, antiForgeryToken },
                {"Input.Email", _testsFixture.UsuarioEmail },
                {"Input.Password", _testsFixture.UsuarioSenha},
                {"Input.ConfirmPassword", _testsFixture.UsuarioSenha},
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Register")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            //act
            var postResponse = await _testsFixture.Client.SendAsync(postRequest);

            //assert
            var responseString = await postResponse.Content.ReadAsStringAsync();
            
            postResponse.EnsureSuccessStatusCode();
            Assert.Contains($"Hello {_testsFixture.UsuarioEmail}!", responseString);

        }

    }
}
