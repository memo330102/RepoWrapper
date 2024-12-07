using Grpc.Core;
using RepoWrapper.Application.Interfaces;
using RepoWrapper.Domain.DTOs.Github;
using RepoWrapper.GRPC;
namespace RepoWrapper.GRPC.Services
{
    public class GithubGrpcService : GithubWrapper.GithubWrapperBase
    {
        private readonly IGithubService _githubService;

        public GithubGrpcService(IGithubService githubService)
        {
            _githubService = githubService;
        }
        public override async Task<RepoResp> SearchRepos(RepoReq request, ServerCallContext context)
        {
            var response = await _githubService.SearchRepositoriesAsync(request.Querry);

            var result = MapToGrpcRepoResp(response);

            return result;
        }

        private RepoResp MapToGrpcRepoResp(GitHubSearchApiResponseDTO gitHubSearchApiResponse)
        {
            var repoResp = new RepoResp
            {
                TotalCount = gitHubSearchApiResponse.total_count
            };

            repoResp.Respos.AddRange(gitHubSearchApiResponse.items.Select(repo => new Repo
            {
                Name = repo.name,
                OwnerLogin = repo.owner.login,
            }));

            return repoResp;
        }
    }
}
