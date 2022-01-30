namespace SocialNetwork.Infrastructure.Database
{
    public interface IConnectionFactory
    {
        string GetMaster();
        string GetReplica();
    }
}