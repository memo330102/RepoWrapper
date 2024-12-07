using RepoWrapper.Domain.DTOs.Github;

namespace RepoWrapper.GRPC.Helper
{
    public static class GrpcMapperHelper
    {
        public static RepoResp MapToGrpcRepoResp(GitHubSearchApiResponseDTO gitHubSearchApiResponse)
        {
            var repoResp = new RepoResp
            {
                TotalCount = gitHubSearchApiResponse.total_count
            };

            if (gitHubSearchApiResponse.items != null && gitHubSearchApiResponse.items.Any())
            {
                repoResp.Respos.AddRange(
                    gitHubSearchApiResponse.items.Select(repo => new Repo
                    {
                        Name = repo.name ?? string.Empty,
                        OwnerLogin = repo.owner?.login ?? string.Empty 
                    })
                );
            }

            return repoResp;
        }
    }
}
