using System;
using System.Linq;
using System.Text.Json;
using Newtonsoft.Json;
using ZumRailsTest.Models.DTOs;
using ZumRailsTest.Services.Interface;

namespace ZumRailsTest.Services.Implementation
{
	public class BooksService : IBooksService
	{
        private const string baseUrl = "https://api.hatchways.io/assessment/blog/posts?tag=";

        private readonly List<string> validDirection = new() { "asc", "desc" };
        private readonly List<string> validSortBy = new() { "id", "reads", "likes", "popularity" };

        public BooksService()
		{

		}

        public (bool isValid, string errorMessage) ValidateInput(PostInputDto input)
        {
            var isValid = true;
            var errorMessage = "";

            if (string.IsNullOrEmpty(input.Tags?.Trim()))
            {
                isValid = false;
                errorMessage = "Tags parameter is required.";
            }

            if (!string.IsNullOrEmpty(input.SortBy?.Trim()) && !validSortBy.Contains(input.SortBy.Trim()))
            {
                isValid = false;
                errorMessage = "SortBy parameter is invalid.";
            }

            if (!string.IsNullOrEmpty(input.Direction?.Trim()) && !validDirection.Contains(input.Direction.Trim()))
            {
                isValid = false;
                errorMessage = "Direction parameter is invalid.";
            }

            return (isValid, errorMessage);
        }

        public async Task<PostListOutputDto> GetBooks(PostInputDto input)
        {
            if (string.IsNullOrEmpty(input.Tags))
            {
                throw new Exception("Tag is required.");
            }

            var postList = new List<PostOutputDto>();
            var listTags = input.Tags.Split(",").ToList();

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    foreach (var tag in listTags)
                    {
                        if (string.IsNullOrEmpty(tag))
                            continue;

                        var posts = await FetchPostsData(client, tag.Trim());
                        if (posts != null)
                            postList.AddRange(posts);
                    }
                }
                catch (HttpRequestException e)
                {
                    throw e;
                }
            }

            var result = new PostListOutputDto();

            postList = RemoveDuplicates(postList);
            postList = OrderPosts(postList, input.SortBy, input.Direction);

            result.Posts = postList;

            return result;
        }

        private List<PostOutputDto> RemoveDuplicates(List<PostOutputDto> inputList)
        {

            HashSet<int> uniquePostIds = new HashSet<int>();
            List<PostOutputDto> uniquePosts = new List<PostOutputDto>();

            foreach (var post in inputList)
            {
                if (uniquePostIds.Add(post.Id))
                {
                    uniquePosts.Add(post);
                }
            }

            return uniquePosts;
        }

        private List<PostOutputDto> OrderPosts(List<PostOutputDto> postList, string? sortBy, string? direction)
        {
            List<PostOutputDto> result;
            var isAscending = string.IsNullOrEmpty(direction) || direction == "asc";

            switch (sortBy)
            {
                case "reads":
                    result = isAscending ? postList.OrderBy(x => x.Reads).ToList() : postList.OrderByDescending(x => x.Reads).ToList();
                    break;
                case "likes":
                    result = isAscending ? postList.OrderBy(x => x.Likes).ToList() : postList.OrderByDescending(x => x.Likes).ToList();
                    break;
                case "popularity":
                    result = isAscending ? postList.OrderBy(x => x.Popularity).ToList() : postList.OrderByDescending(x => x.Popularity).ToList();
                    break;
                case "id":
                default:
                    result = isAscending ? postList.OrderBy(x => x.Id).ToList() : postList.OrderByDescending(x => x.Id).ToList();
                    break;
            }
            return result;
        }

        private async Task<List<PostOutputDto>?> FetchPostsData(HttpClient client, string tag)
        {
            PostListOutputDto? posts = new();
            HttpResponseMessage response;
            response = await client.GetAsync(baseUrl + tag);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Read response content as string
                string responseBody = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseBody))
                {
                    posts = JsonConvert.DeserializeObject<PostListOutputDto>(responseBody);
                }
            }
            else
            {
                throw new HttpRequestException($"Failed to make request. Status code: {response.StatusCode}");
            }

            return posts?.Posts;

        }
    }
}

