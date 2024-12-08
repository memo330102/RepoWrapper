using Moq;
using RepoWrapper.Application.Interfaces;
using RepoWrapper.Domain.DTOs.Github;
using RepoWrapper.GRPC;
using RepoWrapper.GRPC.Mapping;
using RepoWrapper.GRPC.Services;

namespace RepoWrapper.Tests.IntegrationTests.GRPC.Services
{
    public class GithubGrpcServiceIntegrationTests
    {
        private readonly GithubGrpcService _githubGrpcService;

        public GithubGrpcServiceIntegrationTests()
        {
            var mockGithubService = new Mock<IGithubService>();

            mockGithubService
                .Setup(service => service.SearchRepositoriesAsync(It.IsAny<string>()))
                .ReturnsAsync(new GitHubSearchApiResponseDTO
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
                });

            _githubGrpcService = new GithubGrpcService(mockGithubService.Object, new GrpcMapper());
        }

        [Fact]
        public async Task SearchRepos_IntegrationTest()
        {
            var request = new RepoReq { Querry = "wrapper" };

            var response = await _githubGrpcService.SearchRepos(request, null);

            Assert.NotNull(response);
            Assert.Equal(1, response.TotalCount);
            Assert.Single(response.Respos);
            Assert.Equal("wrapper", response.Respos[0].Name);
            Assert.Equal("wrapper-owner", response.Respos[0].OwnerLogin);
        }
    }
}
