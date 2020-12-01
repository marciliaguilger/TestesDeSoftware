using Bogus;
using Bogus.DataSets;
using Features.Clientes;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Features.Tests
{
    [CollectionDefinition(nameof(ClienteCollection))] //implementação da definição de coleção
    public class ClienteCollection : ICollectionFixture<ClienteTestsFixtures>
    {
    }

    public class ClienteTestsFixtures : IDisposable
    {
        
        
        public Cliente GerarClienteValido()
        {
            var genero = new Faker().PickRandom<Name.Gender>();

            //exemplos
            //var email = new Faker().Internet.Email();
            //var clientefaker = new Faker<Cliente>();
            //clientefaker.RuleFor(c => c.Nome, (f, c) => f.Name.FirstName());

            //var cliente = new Faker<Cliente>("pt_BR")
            //    .CustomInstantiator(f => new Cliente(
            //        Guid.NewGuid(),
            //        f.Name.FirstName(genero),
            //        f.Name.LastName(genero),
            //        f.Date.Past(80, DateTime.Now.AddYears(-18)),
            //        "",
            //        true,
            //        DateTime.Now))
            //    .RuleFor(c => c.Email, (f, c) => 
            //        f.Internet.Email(c.Nome.ToLower(), c.Sobrenome.ToLower()));

            var cliente = new Cliente(
                Guid.NewGuid(),
                "Marcilia",
                "G.",
                DateTime.Now.AddYears(-30),
                "ma@gmail.com",
                true,
                DateTime.Now);

            return cliente;
        }
        
        public Cliente GerarClienteInvalido()
        {
            var cliente = new Cliente(
                Guid.NewGuid(),
                "",
                "",
                DateTime.Now,
                "ma@gmail.com",
                true,
                DateTime.Now);
            
            return cliente;
        }

        public void Dispose()
        {
        }
    }
}
