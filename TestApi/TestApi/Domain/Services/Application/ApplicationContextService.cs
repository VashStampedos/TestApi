using TestApi.DB.Context;
using TestApi.Domain.Exceptions;

namespace TestApi.Domain.Services.Application
{
    public class ApplicationContextService
    {
        protected readonly ApplicationContext context;

        public ApplicationContextService(ApplicationContext context)
        {
            this.context = context;
        }

        protected async Task SaveChanges()
        {
            int result;
            try
            {
                result = await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ConflictException("Can not save changes to database", ex);
            }
            if(result <1)
            {
                throw new ConflictException("Changes don't saved");
            }
        }
    }
}
