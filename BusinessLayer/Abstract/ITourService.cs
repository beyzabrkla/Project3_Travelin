using DTOLayer.DTOs.TourDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ITourService
    {
        Task<List<ResultTourDTO>> GetAllTourAsync(); //turları listeleme metodu
        Task CreateTourAsync(CreateTourDTO createTourDTO); //tur oluşturma
        Task UpdateTourAsync(UpdateTourDTO updateTourDTO); //tur güncelleme
        Task DeleteTourAsync(string id); //tur silme
        Task<GetTourByIdDTO> GetTourByIdAsync(string id); //id ye göre tur getirme
        Task<List<ResultTourDTO>> GetToursByGuideIdAsync(string guideId); //rehberin tüm turları

    }
}
