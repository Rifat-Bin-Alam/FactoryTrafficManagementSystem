namespace FactoryTrafficManagement.Application.Interfaces;

public interface ITrafficTransitionService
{
    Task ExecuteAsync(
        string junctionId);
}