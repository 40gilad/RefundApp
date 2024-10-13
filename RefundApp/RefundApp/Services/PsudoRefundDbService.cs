using RefundApp.Models;
using System.ComponentModel;

namespace RefundApp.Repositories
{
    // temp code file to imitate database
    public class PsudoRefundDbService
    {
        private static Dictionary<string, RefundModel> refunds;
        private static PsudoRefundDbService instance;
        private static readonly object lockObj = new object();

        public PsudoRefundDbService()
        {
        }

        public PsudoRefundDbService Instance()
        {
            lock (lockObj)
            {
                if (instance == null)
                    instance = new PsudoRefundDbService();
                return instance;
            }
        }

        public Dictionary<string, RefundModel> Get()
        {
            if (refunds == null)
                refunds = new Dictionary<string, RefundModel>();
            return refunds;
        }

        public RefundModel Get(string order_id)
        {
            if (!refunds.ContainsKey(order_id))
                throw new KeyNotFoundException($"No refund found with OrderId: {order_id}");
            return refunds[order_id];
        }

        public void Add(RefundModel refund)
        {
            if (refunds == null)
                refunds = new Dictionary<string, RefundModel>();

            if (refund != null)
                refunds.Add(refund.OrderId, refund);

        }

        public void Remove(string order_id)
        {
            if (!refunds.ContainsKey(order_id))
                throw new KeyNotFoundException($"No refund found with OrderId: {order_id}");
            refunds.Remove(order_id);
        }
    }
}
