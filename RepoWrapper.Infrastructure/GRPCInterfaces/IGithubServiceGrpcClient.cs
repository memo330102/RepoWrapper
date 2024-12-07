using Refit;
using RepoWrapper.Domain.DTOs.Github;

namespace RepoWrapper.Infrastructure.GRPCInterfaces
{
    public interface IGithubServiceGrpcClient
    {
        [Get("/search/repositories")]
        Task<ApiResponse<GitHubSearchApiResponseDTO>> SearchRepositoriesAsync([AliasAs("q")] string query);
    }
}
