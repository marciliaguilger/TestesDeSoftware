﻿using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NerdStore.Vendas.Domain
{

    public enum PedidoStatus
    {
        Rascunho = 0,
        Iniciado =1,
        Pago = 4,
        Entregue =5,
        Cancelado =6
    }
    public class Pedido
    {
        public static int MAX_UNIDADES_ITEM => 15;
        public static int MIN_UNIDADES_ITEM => 1;

        protected Pedido()
        {
            _pedidoItems = new List<PedidoItem>();
        }
        
        public decimal ValorTotal { get; private set; }
        public Guid ClienteId { get; private set; }
        public PedidoStatus PedidoStatus { get; private set; }

        
        private readonly List<PedidoItem> _pedidoItems;
        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;
        
        public void CalcularValorPedido()
        {
            ValorTotal = PedidoItems.Sum(i => i.CalcularValor());
        }

        public void AdicionarItem(PedidoItem pedidoItem)
        {
            if(pedidoItem.Quantidade > MAX_UNIDADES_ITEM) throw new DomainException($"Máximo de {MAX_UNIDADES_ITEM} unidades por produto");
            if(pedidoItem.Quantidade < MIN_UNIDADES_ITEM) throw new DomainException($"Mínima de {MIN_UNIDADES_ITEM} unidades por produto");
            
            if(_pedidoItems.Any(p=> p.ProdutoId == pedidoItem.ProdutoId))
            {
                var itemExistente = _pedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);
                itemExistente.AdicionarUnidades(pedidoItem.Quantidade);
                pedidoItem = itemExistente;

                _pedidoItems.Remove(itemExistente);
            }

            _pedidoItems.Add(pedidoItem);
            CalcularValorPedido();
        }

        public void TornarRascunho()
        {
            PedidoStatus = PedidoStatus.Rascunho;
        }

        public static class PedidoFactory
        {
            public static Pedido NovoPedidoRascunho(Guid clienteId)
            {
                var pedido = new Pedido
                {
                    ClienteId = clienteId,
                };

                pedido.TornarRascunho();
                return pedido;
            }
        }
    }
    
    public class PedidoItem
    {
        public PedidoItem(Guid produtoId, string produtoNome, int quantidade, decimal valorUnitario)
        {
            if (quantidade < Pedido.MIN_UNIDADES_ITEM) throw new DomainException($"Mínima de {Pedido.MIN_UNIDADES_ITEM} unidades por produto");


            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
        }

        public Guid ProdutoId { get;  private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }

        internal void AdicionarUnidades(int unidades)
        {
            Quantidade += unidades;
        }

        internal decimal CalcularValor()
        {
            return Quantidade * ValorUnitario;
        }
    }
}
