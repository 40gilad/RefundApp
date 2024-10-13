namespace RefundApp.Models
{
    public class RefundModel
    {
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime RefundDate { get; set; }
        public int Amount { get; set; }
        public string Reason { get; set; }

        public RefundModel() { }
        public RefundModel(string order_id, string customer_name, DateTime refund_date,int amount, string reason)
        {
            if (order_id == null)
                throw new ArgumentNullException(nameof(order_id));
            if (refund_date == default)
                throw new ArgumentNullException(nameof(refund_date));
            if (amount < 0)
                    throw new ArgumentNullException(nameof(amount));

            OrderId = order_id;
            CustomerName = customer_name;
            RefundDate = refund_date;
            Amount = amount;
            Reason = reason;

        }
        public RefundModel(RefundModel other)
        {
            OrderId = other.OrderId;
            CustomerName = other.CustomerName;
            RefundDate = other.RefundDate;
            Amount = other.Amount;
            Reason = other.Reason;
        }

    }

    /*
    {
    "OrderId": "123456",
    "CustomerName": "John Doe",
    "RefundDate": "2024-10-13T00:00:00Z",
    "Amount": 100,
    "Reason": "Product defect"
    }
     */
}
