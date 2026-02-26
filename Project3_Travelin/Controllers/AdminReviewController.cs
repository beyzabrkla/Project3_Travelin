using AutoMapper;
using BusinessLayer.Abstract;
using DTOLayer.DTOs.CommentDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project3_Travelin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminReviewController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly ITourService _tourService;
        private readonly IMapper _mapper;

        public AdminReviewController(ICommentService commentService, ITourService tourService, IMapper mapper)
        {
            _commentService = commentService;
            _tourService = tourService;
            _mapper = mapper;
        }

        public async Task<IActionResult> ReviewList(string q, string status, int score, int page = 1)
        {
            var allComments = await _commentService.GetAllCommentAsync();
            var allTours = await _tourService.GetAllTourAsync();

            // Tour adını DTO'ya eşleştir
            var tourDict = allTours.ToDictionary(t => t.TourId, t => t.TourTitle);
            foreach (var c in allComments)
            {
                if (!string.IsNullOrEmpty(c.TourId) && tourDict.ContainsKey(c.TourId))
                    c.TourName = tourDict[c.TourId];
            }

            // İstatistikler (filtresiz)
            ViewBag.TotalCount = allComments.Count;
            ViewBag.ApprovedCount = allComments.Count(x => x.IsStatus);
            ViewBag.PendingCount = allComments.Count(x => !x.IsStatus);
            ViewBag.AvgScore = allComments.Any() ? allComments.Average(x => x.Score) : 0.0;

            // DURUM FİLTRESİ
            if (!string.IsNullOrEmpty(status))
            {
                allComments = status switch
                {
                    "approved" => allComments.Where(x => x.IsStatus).ToList(),
                    "pending" => allComments.Where(x => !x.IsStatus).ToList(),
                    _ => allComments
                };
            }

            // ARAMA FİLTRESİ
            if (!string.IsNullOrEmpty(q))
            {
                allComments = allComments.Where(x =>
                    (x.Headline != null && x.Headline.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                    (x.TourName != null && x.TourName.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
                    (x.CommentDetail != null && x.CommentDetail.Contains(q, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            // PUAN FİLTRESİ
            if (score > 0)
                allComments = allComments.Where(x => x.Score == score).ToList();

            // SAYFALAMA
            int pageSize = 5;
            int totalCount = allComments.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            page = page < 1 ? 1 : page;

            var pagedData = allComments
                .OrderByDescending(x => x.CommentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.FilterCount = totalCount;
            ViewBag.PageSize = pageSize;
            ViewBag.ActiveStatus = status;
            ViewBag.ActiveScore = score;
            ViewBag.Q = q;

            return View(pagedData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveComment(string id, string returnUrl)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment != null)
            {
                var updateDto = new UpdateCommentDTO
                {
                    CommentId = comment.CommentId,
                    TourId = comment.TourId,
                    Headline = comment.Headline,
                    CommentDetail = comment.CommentDetail,
                    Score = comment.Score,
                    CommentDate = comment.CommentDate,
                    IsStatus = true   
                };
                await _commentService.UpdateCommentAsync(updateDto);
            }

            return Redirect(returnUrl ?? "/AdminReview/ReviewList");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectComment(string id, string returnUrl)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment != null)
            {
                var updateDto = new UpdateCommentDTO
                {
                    CommentId = comment.CommentId,
                    TourId = comment.TourId,
                    Headline = comment.Headline,
                    CommentDetail = comment.CommentDetail,
                    Score = comment.Score,
                    CommentDate = comment.CommentDate,
                    IsStatus = false 
                };
                await _commentService.UpdateCommentAsync(updateDto);
            }

            return Redirect(returnUrl ?? "/AdminReview/ReviewList");
        }

        public async Task<IActionResult> DeleteComment(string id)
        {
            await _commentService.DeleteCommentAsync(id);
            return RedirectToAction("ReviewList");
        }
    }
}