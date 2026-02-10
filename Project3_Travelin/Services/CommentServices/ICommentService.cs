using Project3_Travelin.DTOs.CommentDTOs;

namespace Project3_Travelin.Services.CommentServices
{
    public interface ICommentService
    {
        Task<List<ResultCommentDTO>> GetAllCommentAsync(); 
        Task CreateCommentAsync(CreateCommentDTO createCommentDTO); 
        Task UpdateCommentAsync(UpdateCommentDTO updateCommentDTO); 
        Task DeleteCommentAsync(string id); 
        Task<GetCommentByIdDTO> GetCommentByIdAsync(string id); 
    }
}
