using AutoMapper;
using BusinessLayer.Abstract;
using DTOLayer.DTOs.CommentDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project3_Travelin.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly ITourService _tourService;
        private readonly IMapper _mapper;

        public CommentController(ICommentService commentService, ITourService tourService, IMapper mapper)
        {
            _commentService = commentService;
            _tourService = tourService;
            _mapper = mapper;
        }

        public IActionResult CreateComment()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateComment(CreateCommentDTO createCommentDTO)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Eksik veya hatalı bilgi girdiniz." });
            }

            createCommentDTO.CommentDate = DateTime.Now;
            createCommentDTO.IsStatus = false;

            await _commentService.CreateCommentAsync(createCommentDTO);

            return Json(new { success = true, message = "Yorumunuz başarıyla alındı, onay sonrası görünecektir." });
        }

        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> CommentListByTourId(string id)
        {
            var values = await _commentService.GetCommentsByTourIdAsync(id);
            var mappedValues = _mapper.Map<List<ResultCommentListByTourIdDTO>>(values);

            // Tur adını ViewBag'e ekle
            var tour = await _tourService.GetTourByIdAsync(id);
            ViewBag.TourName = tour?.TourTitle ?? "Tur";
            ViewBag.TourId = id;

            return View(mappedValues);
        }
    }
}