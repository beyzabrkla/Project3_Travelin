using DTOLayer.DTOs.CommentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ICommentService
    {
        Task<List<ResultCommentDTO>> GetAllCommentAsync();
        Task CreateCommentAsync(CreateCommentDTO createCommentDTO);
        Task UpdateCommentAsync(UpdateCommentDTO updateCommentDTO);
        Task DeleteCommentAsync(string id);
        Task<GetCommentByIdDTO> GetCommentByIdAsync(string id);
        Task<List<ResultCommentDTO>> GetCommentsByTourIdAsync(string id);
    }
}
