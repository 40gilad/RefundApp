namespace RefundApp.Models
{
    public class RefundModel
    {
        public int Id { get; set; }
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime RefundDate { get; set; }
        public int Amount { get; set; }
        public string Reason { get; set; }
    }
}
