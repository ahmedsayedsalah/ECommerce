using ECommerce.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Specifications.Order_Specs
{
    public class OrderSpecification: BaseSpecification<Order>
    {
        // Constructor: to Endepint GetOrdersForUser
        public OrderSpecification(string buyerEmail)
            :base(o=> o.BuyerEmail== buyerEmail)
        {
            Includes.Add(o=> o.DeliveryMethod);
            Includes.Add(o=> o.Items);

            AddOrderByDesc(o=> o.OrderDate);
        }
        // Constructor: to Endepint GetOrderForUserAsync
        public OrderSpecification(int id,string buyerEmail)
            :base(o=>
                   o.Id== id &&
                   o.BuyerEmail == buyerEmail
                 )
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
        }
    }
}
