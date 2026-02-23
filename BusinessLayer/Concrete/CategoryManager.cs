using AutoMapper;
using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using DTOLayer.DTOs.CategoryDTOs;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ICategoryDal _categoryDal;

        public CategoryManager(IMapper mapper, ICategoryDal categoryDal)
        {
            _mapper = mapper;
            _categoryDal = categoryDal;
        }

        public async Task<List<ResultCategoryDTO>> GetAllCategoryAsync()
        {
            var values = await _categoryDal.GetAllAsync();
            var mappedValues = _mapper.Map<List<ResultCategoryDTO>>(values);
            return mappedValues;
        }

        public async Task CreateCategoryAsync(CreateCategoryDTO createCategoryDTO)
        {
            var value = _mapper.Map<Category>(createCategoryDTO);
            await _categoryDal.InsertAsync(value);
        }

        public async Task DeleteCategoryAsync(string id)
        {
            await _categoryDal.DeleteAsync(id);
        }

        public async Task<GetCategoryByIdDTO> GetCategoryByIdAsync(string id)
        {
            var value = await _categoryDal.GetByIdAsync(id);
            return _mapper.Map<GetCategoryByIdDTO>(value);
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDTO updateCategoryDTO)
        {
            var value = _mapper.Map<Category>(updateCategoryDTO);
            await _categoryDal.UpdateAsync(value);
        }
    }
}
