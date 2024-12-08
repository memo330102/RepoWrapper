using Moq;
using Refit;
using RepoWrapper.Application.Services;
using RepoWrapper.Domain.DTOs.Github;
using RepoWrapper.Infrastructure.GRPCInterfaces;

namespace RepoWrapper.Tests.UnitTests
{
    public class GithubServiceTests
    {
        private readonly Mock<IGithubServiceGrpcClient> _mockGithubServiceGrpcClient;
        private readonly GithubService _githubService;

        public GithubServiceTests()
        {
            _mockGithubServiceGrpcClient = new Mock<IGithubServiceGrpcClient>();
            _githubService = new GithubService(_mockGithubServiceGrpcClient.Object);
        }
    }
}
