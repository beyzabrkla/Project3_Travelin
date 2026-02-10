namespace Project3_Travelin.DTOs.CommentDTOs
{
    public class CreateCommentDTO
    {
        public string CommentDetail { get; set; }
        public int Score { get; set; }
        public DateTime CommentDate { get; set; }
        public bool IsStatus { get; set; }
    }
}
