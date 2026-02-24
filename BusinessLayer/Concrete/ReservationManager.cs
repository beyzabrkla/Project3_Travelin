using AutoMapper;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DTOLayer.DTOs.ReservationDTOs;
using EntityLayer;

namespace BusinessLayer.Concrete
{
    public class ReservationManager : IReservationService
    {
        private readonly IMapper _mapper;
        private readonly IReservationDal _reservationDal;

        public ReservationManager(IMapper mapper, IReservationDal reservationDal)
        {
            _mapper = mapper;
            _reservationDal = reservationDal;
        }

        public async Task CreateReservationAsync(CreateReservationDTO createReservationDTO)
        {
            var value = _mapper.Map<Reservation>(createReservationDTO);
            value.ReservationDate = DateTime.Now;
            value.Status = "pending";
            await _reservationDal.InsertAsync(value);
        }

        public async Task DeleteReservationAsync(string id)
        {
            await _reservationDal.DeleteAsync(id);
        }

        public async Task<List<ResultReservationDTO>> GetAllReservationAsync()
        {
            var values = await _reservationDal.GetAllAsync();
            return _mapper.Map<List<ResultReservationDTO>>(values);
        }

        public async Task<ResultReservationDTO> GetReservationByIdAsync(string id)
        {
            var value = await _reservationDal.GetByIdAsync(id);
            return _mapper.Map<ResultReservationDTO>(value);
        }

        public async Task<List<ResultReservationDTO>> GetReservationsByTourIdAsync(string tourId)
        {
            var values = await _reservationDal.GetByFilterAsync(x => x.TourId == tourId);
            return _mapper.Map<List<ResultReservationDTO>>(values);
        }

        public async Task<List<ResultReservationDTO>> GetReservationsByStatusAsync(string status)
        {
            var values = await _reservationDal.GetByFilterAsync(x => x.Status == status);
            return _mapper.Map<List<ResultReservationDTO>>(values);
        }

        public async Task UpdateReservationStatusAsync(UpdateReservationStatusDTO updateReservationStatusDTO)
        {
            var existing = await _reservationDal.GetByIdAsync(updateReservationStatusDTO.ReservationId);
            existing.Status = updateReservationStatusDTO.Status;
            existing.AdminNote = updateReservationStatusDTO.AdminNote;
            await _reservationDal.UpdateAsync(existing);
        }
    }
}
