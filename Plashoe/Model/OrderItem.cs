﻿namespace Plashoe.Model
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal? TotalPrice { get; set; }
        public int Quantity { get; set; }
        public virtual Product? Product { get; set; }

        public virtual Order? Order { get; set; }
    }
}