using Grpc.Core;
using RepoWrapper.Application.Interfaces;
using RepoWrapper.GRPC.Helper;

namespace RepoWrapper.GRPC.Services
{
    public class GithubGrpcService : GithubWrapper.GithubWrapperBase
    {
        private readonly IGithubService _githubService;
        private readonly ILogger<GithubGrpcService> _logger;
        public GithubGrpcService(IGithubService githubService, ILogger<GithubGrpcService> logger)
        {
            _githubService = githubService;
            _logger = logger;
        }
        public override async Task<RepoResp> SearchRepos(RepoReq request, ServerCallContext context)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Querry))
                {
                    return GrpcErrorHandler.HandleError("Query parameter is required.", _logger, context, StatusCode.InvalidArgument);
                }

                var response = await _githubService.SearchRepositoriesAsync(request.Querry);

                if(response == null)
                {
                    return GrpcErrorHandler.HandleError("Search Repositories from github cannot be null.", _logger, context, StatusCode.Unknown);
                }

                var result = GrpcMapperHelper.MapToGrpcRepoResp(response);

                return result;
            }
            catch (RpcException ex)
            {
                return GrpcErrorHandler.HandleError($"GRPC error: {ex.Message}", _logger, context, StatusCode.Internal);
            }
            catch (ApplicationException ex)
            {
                return GrpcErrorHandler.HandleError($"Application error: {ex.Message}", _logger, context, StatusCode.Internal);
            }
            catch (Exception ex)
            {
                return GrpcErrorHandler.HandleError($"Unexpected error: {ex.Message}", _logger, context, StatusCode.Unknown);
            }
        }
    }
}
