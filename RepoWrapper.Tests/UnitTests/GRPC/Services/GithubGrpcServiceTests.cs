using Google.Protobuf.WellKnownTypes;
using Moq;
using RepoWrapper.Application.Interfaces;
using RepoWrapper.Domain.DTOs.Github;
using RepoWrapper.GRPC;
using RepoWrapper.GRPC.Mapping;
using RepoWrapper.GRPC.Services;

namespace RepoWrapper.Tests.UnitTests.GRPC.Services
{
    public class GithubGrpcServiceTests
    {
        private readonly Mock<IGithubService> _mockGithubService;
        private readonly Mock<IGrpcMapper> _mockGrpcMapper;
        private readonly GithubGrpcService _githubGrpcService;

        public GithubGrpcServiceTests()
        {
            _mockGithubService = new Mock<IGithubService>();
            _mockGrpcMapper = new Mock<IGrpcMapper>();
            _githubGrpcService = new GithubGrpcService(_mockGithubService.Object, _mockGrpcMapper.Object);
        }

        [Fact]
        public async Task SearchRepos_ReturnsMappedResponse_When_GithubService_Succeeds()
        {
            var request = new RepoReq { Querry = "wrapper" };

            var githubResponse = CreateGitHubSearchApiResponse();

            var expectedGrpcResponse = CreateExpectedGrpcResponse();

            _mockGithubService
                .Setup(service => service.SearchRepositoriesAsync(It.IsAny<string>()))
                .ReturnsAsync(githubResponse);

            _mockGrpcMapper
                .Setup(mapper => mapper.MapToGrpcRepoResp(githubResponse))
                .Returns(expectedGrpcResponse);

            var response = await _githubGrpcService.SearchRepos(request, null);

            Assert.NotNull(response);
            Assert.Equal(expectedGrpcResponse, response);
        }

        [Fact]
        public async Task SearchRepos_Should_Handle_Empty_Query()
        {
            var request = new RepoReq { Querry = string.Empty };

            _mockGithubService
                .Setup(service => service.SearchRepositoriesAsync(""))
                .ThrowsAsync(new ArgumentException("Query parameter is required."));

            var expectedValue = "Query parameter is required.";

            var exception = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _githubGrpcService.SearchRepos(request, null));

            Assert.Equal(exception.Message, expectedValue);
        }

        [Fact]
        public async Task SearchRepos_Should_Handle_GithubService_Failure()
        {
            var request = new RepoReq { Querry = "wrapper" };

            _mockGithubService
                .Setup(service => service.SearchRepositoriesAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("GRPC Service failure"));

            var expectedValue = "GRPC Service failure";

            var exception = await Assert.ThrowsAsync<Exception>(async () =>
                await _githubGrpcService.SearchRepos(request, null));

            Assert.Equal(exception.Message, expectedValue);
        }

        [Fact]
        public async Task SearchRepos_ThrowsException_When_GithubService_Fails()
        {
            var request = new RepoReq { Querry = "wrapper" };

            _mockGithubService
                .Setup(service => service.SearchRepositoriesAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("GitHub API failure"));

            await Assert.ThrowsAsync<Exception>(() => _githubGrpcService.SearchRepos(request, null));
        }

        private GitHubSearchApiResponseDTO CreateGitHubSearchApiResponse()
        {
            return new GitHubSearchApiResponseDTO
            {
                total_count = 1,
                items = new List<GitHubSearchApiResponseItemDTO>
                {
                    new GitHubSearchApiResponseItemDTO
                    {
                        name = "wrapper",
                        owner = new GitHubSearchApiResponseItemOwnerDTO { login = "wrapper-owner" }
                    }
                }
            };
        }

        private RepoResp CreateExpectedGrpcResponse()
        {
            return new RepoResp
            {
                TotalCount = 1,
                Respos =
                {
                    new Repo { Name = "wrapper", OwnerLogin = "wrapper-owner" }
                }
            };
        }
    }
}
