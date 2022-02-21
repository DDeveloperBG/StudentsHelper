namespace StudentsHelper.Web.Hubs.Contracts
{
    using System.Threading.Tasks;

    using StudentsHelper.Web.ViewModels.Chat;

    public interface IChatClient
    {
        Task ReceiveMessage(MessageViewModel input);
    }
}
