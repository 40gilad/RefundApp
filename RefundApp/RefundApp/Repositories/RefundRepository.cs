using RefundApp.Models;

namespace RefundApp.Repositories
{
    public class RefundRepository
    {
        private List<RefundModel> refunds;

        public List<RefundModel> GetAll()
        {
            return refunds;
        }

        public void Add(RefundModel refund)
        {
            if (refund != null)
                refunds.Add(refund);
        }
    }
}
