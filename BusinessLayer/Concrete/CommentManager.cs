using AutoMapper;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DTOLayer.DTOs.CommentDTOs;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class CommentManager : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly ICommentDal _commentDal;

        public CommentManager(IMapper mapper, ICommentDal commentDal)
        {
            _mapper = mapper;
            _commentDal = commentDal;
        }

        public async Task<List<ResultCommentDTO>> GetAllCommentAsync()
        {
            var values = await _commentDal.GetAllAsync();
            return _mapper.Map<List<ResultCommentDTO>>(values);
        }

        public async Task CreateCommentAsync(CreateCommentDTO createCommentDTO)
        {
            var value = _mapper.Map<Comment>(createCommentDTO);
            await _commentDal.InsertAsync(value);
        }

        public async Task DeleteCommentAsync(string id)
        {
            await _commentDal.DeleteAsync(id);
        }

        public async Task UpdateCommentAsync(UpdateCommentDTO updateCommentDTO)
        {
            var value = _mapper.Map<Comment>(updateCommentDTO);
            await _commentDal.UpdateAsync(value);
        }

        public async Task<GetCommentByIdDTO> GetCommentByIdAsync(string id)
        {
            var value = await _commentDal.GetByIdAsync(id);
            return _mapper.Map<GetCommentByIdDTO>(value);
        }

        public async Task<List<ResultCommentDTO>> GetCommentsByTourIdAsync(string id)
        {
            var values = await _commentDal.GetByFilterAsync(x => x.TourId == id);
            return _mapper.Map<List<ResultCommentDTO>>(values);
        }
    }
}
