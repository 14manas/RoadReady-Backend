using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RoadReady.DTO;
using RoadReady.Models;

namespace RoadReady.Repositories
{
    public class PaymentDetailRepository : IPaymentDetailRepository
    {
        private readonly RoadReadyContext _context;
        private readonly IMapper _mapper;

        public PaymentDetailRepository(RoadReadyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PaymentDetailDTO>> GetAllAsync()
        {
            try
            {
                var paymentDetails = await _context.PaymentDetails.ToListAsync();
                return _mapper.Map<IEnumerable<PaymentDetailDTO>>(paymentDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PaymentDetailDTO?> GetByIdAsync(int id)
        {
            try
            {
                var paymentDetail = await _context.PaymentDetails.FindAsync(id);
                return _mapper.Map<PaymentDetailDTO>(paymentDetail);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PaymentDetailDTO> CreateAsync(PaymentDetailDTO paymentDetailDto)
        {
            try
            {
                var paymentDetail = _mapper.Map<PaymentDetail>(paymentDetailDto);
                _context.PaymentDetails.Add(paymentDetail);
                await _context.SaveChangesAsync();
                return _mapper.Map<PaymentDetailDTO>(paymentDetail);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PaymentDetailDTO> UpdateAsync(int id, PaymentDetailDTO paymentDetailDto)
        {
            try
            {
                var paymentDetail = await _context.PaymentDetails.FindAsync(id);
                if (paymentDetail == null) throw new KeyNotFoundException("PaymentDetail not found.");

                _mapper.Map(paymentDetailDto, paymentDetail);
                await _context.SaveChangesAsync();
                return _mapper.Map<PaymentDetailDTO>(paymentDetail);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var paymentDetail = await _context.PaymentDetails.FindAsync(id);
                if (paymentDetail == null) return false;

                _context.PaymentDetails.Remove(paymentDetail);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
