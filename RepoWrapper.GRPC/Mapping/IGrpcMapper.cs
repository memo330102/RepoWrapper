using RepoWrapper.Domain.DTOs.Github;

namespace RepoWrapper.GRPC.Mapping
{
    public interface IGrpcMapper
    {
        RepoResp MapToGrpcRepoResp(GitHubSearchApiResponseDTO gitHubSearchApiResponse);
    }
}
