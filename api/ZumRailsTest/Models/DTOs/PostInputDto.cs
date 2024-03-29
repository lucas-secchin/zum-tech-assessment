using System.ComponentModel.DataAnnotations;

namespace ZumRailsTest.Models.DTOs
{
	public class PostInputDto
	{
        public string? Tags { get; set; } = string.Empty;

        public string? SortBy { get; set; } = string.Empty;

        public string? Direction { get; set; } = string.Empty;
    }
}

