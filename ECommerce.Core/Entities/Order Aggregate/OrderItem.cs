﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Entities.Order_Aggregate
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {
            
        }
        public OrderItem(ProductItemOrdered product, decimal price, int quantity)
        {
            Product = product;
            Price = price;
            Quantity = quantity;
        }

        //public int ProductId { get; set; }
        //public string ProductName { get; set; }
        //public string PictureUrl { get; set; }
        public ProductItemOrdered Product { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
