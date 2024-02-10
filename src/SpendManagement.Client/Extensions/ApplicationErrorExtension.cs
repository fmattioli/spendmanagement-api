using Web.Contracts.Exceptions;

namespace SpendManagement.Client.Extensions
{
    public static class ApplicationErrorExtension
    {
        public static async Task<T> HandleExceptions<T>(this Task<T> task, string requestName)
           where T : class
        {
            try
            {
                return await task;
            }
            catch (HttpRequestException e) when (e.Message.Contains("404"))
            {
                throw new NotFoundException(requestName + " " + e.Message, e.InnerException);
            }
            catch (HttpRequestException e) when (e.Message.Contains("500"))
            {
                throw new InternalServerErrorException(requestName + " " + e.Message, e.InnerException);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
