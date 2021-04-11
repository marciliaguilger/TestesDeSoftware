using Features;
using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Config;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests
{
    [TestCaseOrderer("Features.PriorityOrderer", "Features")]
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

        [Fact(DisplayName = "Realizar cadastro com sucesso"), TestPriority(1)]
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

        [Fact(DisplayName = "Realizar cadastro com senha fraca"), TestPriority(3)]
        [Trait("Categoria", "Integração Web - usuário")]
        public async Task Usuario_RealizarCadastroComSenhaFraca_DeveRetornarMensagemDeErro()
        {
            //arrange
            var initialResponse = await _testsFixture.Client.GetAsync("/Identity/Account/Register");
            initialResponse.EnsureSuccessStatusCode();
            var antiForgeryToken = _testsFixture.ObterAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());

            _testsFixture.GerarUserSenha();
            const string senhaFraca = "123456";

            var formData = new Dictionary<string, string>
            {
                {_testsFixture.AntiForgeryFiedlName, antiForgeryToken },
                {"Input.Email", _testsFixture.UsuarioEmail },
                {"Input.Password", senhaFraca},
                {"Input.ConfirmPassword", senhaFraca},
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
            Assert.Contains("Passwords must have at least one non alphanumeric character.", responseString);
            Assert.Contains("Passwords must have at least one lowercase (&#x27;a&#x27;-&#x27;z&#x27;).", responseString);
            Assert.Contains("Passwords must have at least one uppercase (&#x27;A&#x27;-&#x27;Z&#x27;).", responseString);

        }

        [Fact(DisplayName = "Realizar login com sucesso"), TestPriority(2)]
        [Trait("Categoria", "Integração Web - usuário")]
        public async Task Usuario_RealizarLogin_DeveExecutarComSucesso()
        {
            //arrange
            var initialResponse = await _testsFixture.Client.GetAsync("/Identity/Account/Login");
            initialResponse.EnsureSuccessStatusCode();

            var antiForgeryToken = _testsFixture.ObterAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());
            var formData = new Dictionary<string, string>
            {
                {_testsFixture.AntiForgeryFiedlName, antiForgeryToken },
                {"Input.Email", _testsFixture.UsuarioEmail },
                {"Input.Password", _testsFixture.UsuarioSenha}
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Login")
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
