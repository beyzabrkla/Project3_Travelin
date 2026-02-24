using DTOLayer.DTOs.ReservationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface IReservationService
    {
        Task<List<ResultReservationDTO>> GetAllReservationAsync();
        Task CreateReservationAsync(CreateReservationDTO createReservationDTO);
        Task UpdateReservationStatusAsync(UpdateReservationStatusDTO updateReservationStatusDTO);
        Task DeleteReservationAsync(string id);
        Task<ResultReservationDTO> GetReservationByIdAsync(string id);
        Task<List<ResultReservationDTO>> GetReservationsByTourIdAsync(string tourId);
        Task<List<ResultReservationDTO>> GetReservationsByStatusAsync(string status);

    }
}
