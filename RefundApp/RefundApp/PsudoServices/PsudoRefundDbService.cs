using RefundApp.Models;
using System.ComponentModel;

namespace RefundApp.PsudoServices
{
    public class PsudoRefundDbService
    {
        private static Dictionary<string, Dictionary<string, RefundModel>> refunds;
        private static PsudoRefundDbService instance;
        private static readonly object lockObj = new object();

        private PsudoRefundDbService()
        {
            refunds = new Dictionary<string, Dictionary<string, RefundModel>>();
        }

        public static PsudoRefundDbService Instance()
        {
            lock (lockObj)
            {
                if (instance == null)
                    instance = new PsudoRefundDbService();
                return instance;
            }
        }

        public Dictionary<string, Dictionary<string, RefundModel>> Get()
        {
            return refunds;
        }

        public Dictionary<string, RefundModel> GetByUserEmail(string u_mail)
        {
            if (refunds==null || !refunds.ContainsKey(u_mail))
                throw new KeyNotFoundException($"No refunds found with user mail: {u_mail}");
            return refunds[u_mail];
        }

        public RefundModel GetByOrderId(string u_mail,string order_id)
        {
            Dictionary<string, RefundModel> temp = GetByUserEmail(u_mail);
            if (!temp.ContainsKey(order_id))
                throw new KeyNotFoundException($"No refunds found withwith user mail: {u_mail} and order id: {order_id}");
            return temp[order_id];
        }

        public void Add(RefundModel refund)
        {
            if (refund != null)
            {
                if (!refunds.ContainsKey(refund.UEmail))
                    refunds[refund.UEmail] = new Dictionary<string, RefundModel>();
                refunds[refund.UEmail][refund.Id] = refund;
            }
            else
                throw new ArgumentNullException("User mail or refund cannot be null");

        }

        public void Remove(string u_mail,string order_id)
        {
            if (!refunds.ContainsKey(u_mail))
                throw new KeyNotFoundException($"No refund found with Email: {u_mail}");
            if (!refunds[u_mail].ContainsKey(order_id))
                throw new KeyNotFoundException($"No refund found with OrderId: {order_id}");
            refunds[u_mail].Remove(order_id);
        }
    }
}
