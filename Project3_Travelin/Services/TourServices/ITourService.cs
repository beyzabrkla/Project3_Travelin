using Project3_Travelin.DTOs.TourDTOs;

namespace Project3_Travelin.Services.TourServices
{
    public interface ITourService
    {
        Task<List<ResultTourDTO>> GetAllTourAsync(); //turları listeleme metodu
        Task CreateTourAsync(CreateTourDTO createTourDTO); //tur oluşturma
        Task UpdateTourAsync(UpdateTourDTO updateTourDTO); //tur güncelleme
        Task DeleteTourAsync(string id); //tur silme
        Task<GetTourByIdDTO>GetTourByIdAsync(string id); //id ye göre tur getirme
        Task<List<ResultTourDTO>> GetToursByGuideIdAsync(string guideId); //rehberin tüm turları
    }
}
