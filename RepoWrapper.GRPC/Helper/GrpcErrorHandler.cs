using Grpc.Core;

namespace RepoWrapper.GRPC.Helper
{
    public static class GrpcErrorHandler
    {
        public static RepoResp HandleError( string errorMessage, ILogger logger, ServerCallContext context, StatusCode statusCode)
        {
            logger.LogError(errorMessage);
            context.Status = new Status(statusCode, errorMessage);
            return new RepoResp();
        }
    }
}
