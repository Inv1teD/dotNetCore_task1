namespace DotNetMentorship.TestAPI
{
    public interface IUnitOfWork
    {
        IUkrainianRepository Ukrainians { get; }
        void Save();
    }
}
