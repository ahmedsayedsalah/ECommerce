using ECommerce.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Specifications.Order_Specs
{
    public class OrderWithPaymentIntentSpecification: BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpecification(string paymentIntentId)
            :base(o=> o.PaymentIntentId==paymentIntentId)
        {
            
        }
    }
}
