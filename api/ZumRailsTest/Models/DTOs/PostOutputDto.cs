using System;
namespace ZumRailsTest.Models.DTOs
{
    public class PostOutputDto
    {

        public int Id { get; set; }
        public string Author { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public int Likes { get; set; }
        public decimal Popularity { get; set; }
        public int Reads { get; set; }
        public List<string> Tags { get; set; } = new();
    }

    public class PostListOutputDto
    {
        public List<PostOutputDto> Posts { get; set; } = new();
    }
}

