using Microsoft.AspNetCore.Http;

namespace RefundApp.Models
{
    public class RefundModel
    {
        public string UEmail { get; set; }
        public string OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime RefundDate { get; set; }
        public int Amount { get; set; }
        public string Reason { get; set; }

        public bool IsResturantFault { get; set; }

        public RefundModel() { }
        public RefundModel(string u_email,string order_id, string customer_name, DateTime refund_date,int amount, string reason, bool is_rest_fault)
        {
            if (string.IsNullOrEmpty(u_email))
                throw new ArgumentNullException(nameof(u_email));
            if (order_id == null)
                throw new ArgumentNullException(nameof(order_id));
            if (refund_date == default)
                throw new ArgumentNullException(nameof(refund_date));
            if (amount < 0)
                    throw new ArgumentNullException(nameof(amount));
            if (is_rest_fault == default)
                throw new ArgumentNullException(nameof(amount));
            UEmail = u_email;
            OrderId = order_id;
            CustomerName = customer_name;
            RefundDate = refund_date;
            Amount = amount;
            Reason = reason;
            IsResturantFault = is_rest_fault;
        }

        public RefundModel(string u_email, string order_id, string customer_name, string refund_date, string amount, string reason, string is_rest_fault)
        {
            DateTime temp_date = default;
            int temp_amount = -1;
            bool temp_is_rest_fault=default;
            try
            {
                temp_date = DateTime.Parse(refund_date);
                temp_amount = int.Parse(amount);
                temp_is_rest_fault = bool.Parse(is_rest_fault);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to parse '{0}'", refund_date);
            }

            if (string.IsNullOrEmpty(u_email))
                throw new ArgumentNullException(nameof(u_email));
            if (order_id == null)
                throw new ArgumentNullException(nameof(order_id));
            if (temp_date == default)
                throw new ArgumentNullException(nameof(refund_date));
            if (temp_amount < 0)
                throw new ArgumentNullException(nameof(amount));
            UEmail = u_email;
            OrderId = order_id;
            CustomerName = customer_name;
            RefundDate = temp_date;
            Amount = temp_amount;
            Reason = reason;
            IsResturantFault = temp_is_rest_fault;
        }

        public RefundModel(RefundModel other)
        {
            UEmail = other.UEmail;
            OrderId = other.OrderId;
            CustomerName = other.CustomerName;
            RefundDate = other.RefundDate;
            Amount = other.Amount;
            Reason = other.Reason;
            IsResturantFault = other.IsResturantFault;
        }

        public override string ToString()
        {
            return $"{{\nUEmail: {UEmail},\nOrderId: {OrderId},\n CustomerName: {CustomerName},\n RefundDate: {RefundDate:yyyy-MM-dd},\n Amount: {Amount},\n Reason: {Reason},\n IsResturantFault: {IsResturantFault}\n}}";
        }

    }

}


/*
{
"UEmail": "abc@gmail.com",
"OrderId": "123456",
"CustomerName": "John Doe",
"RefundDate": "2024-10-13T00:00:00Z",
"Amount": 100,
"Reason": "Product defect"
}
 */
