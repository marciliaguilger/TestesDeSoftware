using MediatR;
using NerdStore.Core.DomainObjects;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Commands
{
    public class PedidoCommandHandler : IRequestHandler<AdicionarItemPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMediator _mediator;
        public PedidoCommandHandler(IPedidoRepository pedidoRepository, IMediator mediator)
        {
            _pedidoRepository = pedidoRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;
            
            var pedido = await _pedidoRepository.ObterPedidoRascunhoPorClienteId(message.ClienteId);
            var pedidoItem = new PedidoItem(message.ProdutoId,
                                            message.Nome,
                                            message.Quantidade,
                                            message.ValorUnitario);

            if(pedido == null)
            {
                pedido = Pedido.PedidoFactory.NovoPedidoRascunho(message.ClienteId);
                pedido.AdicionarItem(pedidoItem);

                _pedidoRepository.Adicionar(Pedido.PedidoFactory.NovoPedidoRascunho(message.ClienteId));
            }
            else
            {
                var pedidoItemExistente = pedido.PedidoItemExistente(pedidoItem);
                pedido.AdicionarItem(pedidoItem);

                if (pedidoItemExistente)
                {
                    _pedidoRepository.AtualizarItem(pedido
                                                    .PedidoItems
                                                    .FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId));
                }
                else
                {
                    _pedidoRepository.AdicionarItem(pedidoItem);
                }


                _pedidoRepository.Atualizar(pedido);
            }

            pedido.AdicionarEvento(new PedidoItemAdicionadoEvent(
                                    pedido.ClienteId,
                                    pedido.Id,
                                    message.ProdutoId,
                                    message.Nome,
                                    message.ValorUnitario,
                                    message.Quantidade));

            return await _pedidoRepository.UnitOfWork.Commit();

            //minuto 15: 39
        }

        private bool ValidarComando(Command message)
        {
            //como esse comando é genérico e pode ser utilizado em outras classes, pode-se criar uma classe base com esse método que seja disponível
            //para todas as classes filhas
            if (message.EhValido()) return true;
            foreach (var erro in message.ValidationResult.Errors)
            {
                 _mediator.Publish(new DomainNotification(message.MessageType, erro.ErrorMessage));
                //pode-se adicionar numa lista scoped e mostrar na tela do usuário os problemas que foram relatados.
            }
            return false;
        }
        
    }
}
