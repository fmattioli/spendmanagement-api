namespace SpendManagement.Application.Extensions
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
            catch (Exception ex)
            {
                throw new FluentValidation.ValidationException(ex.Message + @"\" +requestName);
            }
        }
    }
}
