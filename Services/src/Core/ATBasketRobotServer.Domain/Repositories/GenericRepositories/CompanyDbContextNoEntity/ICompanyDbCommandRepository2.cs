namespace ATBasketRobotServer.Domain.Repositories.GenericRepositories.CompanyDbContext;
public interface ICompanyDbCommandRepository2<T> : ICompanyDbRepository2<T>, ICommandGenericRepository2<T> where T : class
{
}