namespace BookLibrary.Data.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IAuthorsRepository Authors { get; }
        IBooksRepository Books { get; }
    }
}
