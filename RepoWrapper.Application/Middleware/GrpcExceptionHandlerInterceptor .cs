using Serilog;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Serilog.Core;
using System.Text.RegularExpressions;

namespace RepoWrapper.Application.Middleware
{
    public class GrpcExceptionHandlerInterceptor : Interceptor
    {
        private readonly ILogger _logger;

        public GrpcExceptionHandlerInterceptor(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
               TRequest request,
               ServerCallContext context,
               UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                _logger.Information("Handling gRPC request for method {MethodName}", context.Method);
                return await continuation(request, context);
            }
            catch (RpcException rpcEx)
            {
                _logger.Error(rpcEx, "gRPC RPC Exception occurred for method {MethodName}", context.Method);
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An unexpected error occurred for method {MethodName}", context.Method);
                throw MapToRpcException(context, ex);
            }
        }

        private RpcException MapToRpcException(ServerCallContext context, Exception exception)
        {
            var (statusCode, message) = MapExceptionToStatus(exception);

            var trailers = new Metadata
        {
            { "error-message", exception.Message },
            { "stack-trace", exception.StackTrace ?? "No stack trace available" },
            { "grpc-method", context.Method }
        };

            return new RpcException(new Status(statusCode, message), trailers);
        }

        private (StatusCode StatusCode, string Message) MapExceptionToStatus(Exception exception)
        {
            return exception switch
            {
                ArgumentException _ => (StatusCode.InvalidArgument, "Invalid argument."),
                InvalidOperationException _ => (StatusCode.FailedPrecondition, "Invalid operation."),
                ApplicationException _ => (StatusCode.Internal, "An application error occurred."),
                RpcException _ => (StatusCode.Internal, "A gRPC-specific error occurred."),
                KeyNotFoundException _ => (StatusCode.NotFound, "The requested resource was not found."),
                _ => (StatusCode.Internal, "An internal server error occurred.")
            };
        }
    }
}
