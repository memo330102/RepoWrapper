using Grpc.Core;
using RepoWrapper.Application.Interfaces;
using RepoWrapper.GRPC.Helper;
using RepoWrapper.GRPC.Mapping;
using Serilog;

namespace RepoWrapper.GRPC.Services
{
    public class GithubGrpcService : GithubWrapper.GithubWrapperBase
    {
        private readonly IGithubService _githubService;
        private readonly ILogger<GithubGrpcService> _logger;
        private readonly IGrpcMapper _mapper;

        public GithubGrpcService(IGithubService githubService, ILogger<GithubGrpcService> logger, IGrpcMapper mapper)
        {
            _githubService = githubService;
            _logger = logger;
            _mapper = mapper;
        }
        public override async Task<RepoResp> SearchRepos(RepoReq request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.Querry))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Query parameter is required."));
            }

            var response = await _githubService.SearchRepositoriesAsync(request.Querry);

            if (response == null)
            {
                throw new RpcException(new Status(StatusCode.Unknown, "Search Repositories from GitHub cannot be null."));
            }

            return _mapper.MapToGrpcRepoResp(response);
        }
    }
}
