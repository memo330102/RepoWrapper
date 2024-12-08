using RepoWrapper.Domain.DTOs.Github;
using RepoWrapper.GRPC.Mapping;

namespace RepoWrapper.Tests.UnitTests.GRPC.Mapping
{
    public class GrpcMapperTests
    {
        private readonly GrpcMapper _mapper;
        public GrpcMapperTests()
        {
            _mapper = new GrpcMapper();
        }

        [Fact]
        public void MapToGrpcRepoResp_ReturnsValidResponse_When_Items_Exist()
        {
            var githubResponse = new GitHubSearchApiResponseDTO
            {
                total_count = 2,
                items = new List<GitHubSearchApiResponseItemDTO>
                {
                    new GitHubSearchApiResponseItemDTO
                    {
                        name = "wrapper1",
                        owner = new GitHubSearchApiResponseItemOwnerDTO { login = "wrapper1-owner" }
                    },
                    new GitHubSearchApiResponseItemDTO
                    {
                        name = "wrapper2",
                        owner = new GitHubSearchApiResponseItemOwnerDTO { login = "wrapper2-owner" }
                    }
                }
            };

            var response = _mapper.MapToGrpcRepoResp(githubResponse);

            Assert.NotNull(response);
            Assert.Equal(2, response.TotalCount);
            Assert.Equal(2, response.Respos.Count);
            Assert.Equal("wrapper1", response.Respos[0].Name);
            Assert.Equal("wrapper1-owner", response.Respos[0].OwnerLogin);
            Assert.Equal("wrapper2", response.Respos[1].Name);
            Assert.Equal("wrapper2-owner", response.Respos[1].OwnerLogin);
        }

        [Fact]
        public void MapToGrpcRepoResp_ReturnsValidResponse_When_Items_Does_Not_Exist()
        {
            var githubResponse = new GitHubSearchApiResponseDTO
            {
                total_count = 0,
                items = null
            };

            var response = _mapper.MapToGrpcRepoResp(githubResponse);

            Assert.NotNull(response);
            Assert.Equal(0, response.TotalCount);
            Assert.Empty(response.Respos);
        }
    }
}
