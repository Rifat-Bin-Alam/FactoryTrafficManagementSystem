namespace FactoryTrafficManagement.Application.Interfaces;

public interface IControllerStatusService
{
    Task SetOfflineAsync(string junctionId);

    Task SetOnlineAsync(string junctionId);
}