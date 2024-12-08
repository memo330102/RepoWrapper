using Grpc.Core;
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
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Query parameter is required."));
            }

            var response = await _githubServiceGrpcClient.SearchRepositoriesAsync(query);

            if (response == null || response.Content == null)
            {
                throw new RpcException(new Status(StatusCode.Unknown, "Search Repositories from GitHub can not be null."));
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new RpcException(new Status(StatusCode.Internal, response.Error.Message + " Something went wrong while fetching repositories from GitHub."));
            }

            return response.Content;
        }
    }
}
