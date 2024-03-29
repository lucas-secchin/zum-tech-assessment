using System;
using ZumRailsTest.Models.DTOs;

namespace ZumRailsTest.Services.Interface
{
	public interface IBooksService
	{
        public (bool isValid, string errorMessage) ValidateInput(PostInputDto input);
        public Task<PostListOutputDto> GetBooks(PostInputDto input);
	}
}

