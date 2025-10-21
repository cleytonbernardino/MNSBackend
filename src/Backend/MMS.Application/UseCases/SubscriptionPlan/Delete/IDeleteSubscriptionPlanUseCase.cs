namespace MMS.Application.UseCases.SubscriptionPlan.Delete;

public interface IDeleteSubscriptionPlanUseCase
{
    Task Execute(short id);
}
