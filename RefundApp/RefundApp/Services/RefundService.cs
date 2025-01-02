using Microsoft.EntityFrameworkCore;
using RefundApp.Data;
using RefundApp.Models;
using RefundApp.Utils;

namespace RefundApp.Services
{
    public class RefundService
    {
        private readonly RefundAppDbContext _context;
        public RefundService(RefundAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RefundModel>?> Get()
        {
            return await _context.Refunds.ToListAsync();
        }

        public async Task<RefundModel?> Get(int id)
        {
            return await _context.Refunds.FindAsync(id);
        }


        public async Task Add(RefundModel refund)
        {
            if (refund != null)
            {
                _context.Refunds.Add(refund);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> Delete(int id)
        {
            RefundModel? u = await Get(id);
            if (u is null)
                return false;
            return await Delete(u);
        }

        public async Task<bool> Delete(RefundModel refund)
        {
            try
            {
                _context.Refunds.Remove(refund);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<bool> Update(RefundModel refund)
        {
            try
            {
                _context.Refunds.Update(refund);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
