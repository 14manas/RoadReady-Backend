using RoadReady.DTO;

namespace RoadReady.Repositories
{
    public interface IPaymentDetailRepository
    {
        Task<IEnumerable<PaymentDetailDTO>> GetAllAsync();
        Task<PaymentDetailDTO?> GetByIdAsync(int id);
        Task<PaymentDetailDTO> CreateAsync(PaymentDetailDTO paymentDetailDto);
        Task<PaymentDetailDTO> UpdateAsync(int id, PaymentDetailDTO paymentDetailDto);
        Task<bool> DeleteAsync(int id);
    }
}
