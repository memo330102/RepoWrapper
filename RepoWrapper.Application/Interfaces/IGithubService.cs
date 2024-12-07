using RepoWrapper.Domain.DTOs.Github;

namespace RepoWrapper.Application.Interfaces
{
    public interface IGithubService
    {
        Task<GitHubSearchApiResponseDTO> SearchRepositoriesAsync(string query);
    }
}
