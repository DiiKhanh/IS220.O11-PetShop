namespace PetShop.DTOs
{
    public class CommentDto
    {
        public string User_id { get; set; }
        public int Product_id { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public string Username { get; set; }
    }

    public class Comment1Dto
    {
        public bool IsAccept { get; set; }
    }
    public class Comment2Dto
    {
        public string Type { get; set; }
    }
}
