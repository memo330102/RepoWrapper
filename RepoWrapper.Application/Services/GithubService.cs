using RepoWrapper.Application.Interfaces;
using RepoWrapper.Domain.DTOs.Github;
using RepoWrapper.Infrastructure.GRPCInterfaces;

namespace RepoWrapper.Application.Services
{
    public class GithubService : IGithubService
    {
        private readonly IGithubServiceGrpcClient _githubServiceGrpcClient;

        public GithubService(IGithubServiceGrpcClient githubServiceGrpcClient)
        {
            _githubServiceGrpcClient = githubServiceGrpcClient;
        }
        public async Task<GitHubSearchApiResponseDTO> SearchRepositoriesAsync(string query)
        {
            var response = await _githubServiceGrpcClient.SearchRepositoriesAsync(query);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Error.Message + " Something went wrong while fetching repositories from GitHub.");
            }

            return response.Content;
        }
    }
}
