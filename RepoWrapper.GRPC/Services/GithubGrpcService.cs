using Grpc.Core;
using RepoWrapper.Application.Interfaces;
using RepoWrapper.GRPC.Mapping;

namespace RepoWrapper.GRPC.Services
{
    public class GithubGrpcService : GithubWrapper.GithubWrapperBase
    {
        private readonly IGithubService _githubService;
        private readonly IGrpcMapper _mapper;

        public GithubGrpcService(IGithubService githubService, IGrpcMapper mapper)
        {
            _githubService = githubService;
            _mapper = mapper;
        }
        public override async Task<RepoResp> SearchRepos(RepoReq request, ServerCallContext context)
        {
            var response = await _githubService.SearchRepositoriesAsync(request.Querry);

            return _mapper.MapToGrpcRepoResp(response);
        }
    }
}
