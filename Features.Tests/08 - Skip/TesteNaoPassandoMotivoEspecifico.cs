using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Features.Tests
{
    public class TesteNaoPassandoMotivoEspecifico
    {
        [Fact(DisplayName = "Novo cliente 2.0", Skip ="Nova Versão 2.0 quebrando")]
        [Trait("Categoria", "Escapando dos testes")]
        public void Teste_NaoEstaPassando_VersaoNovaNaoCompativel()
        {
            Assert.True(false);
        }

    }
}
