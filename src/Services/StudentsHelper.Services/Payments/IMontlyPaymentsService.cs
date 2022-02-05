namespace StudentsHelper.Services.Payments
{
    using System.Threading.Tasks;

    public interface IMontlyPaymentsService
    {
        Task PayMontlySalariesAsync();
    }
}
