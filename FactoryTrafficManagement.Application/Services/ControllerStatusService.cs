using FactoryTrafficManagement.Application.Interfaces;

namespace FactoryTrafficManagement.Application.Services;

public class ControllerStatusService
    : IControllerStatusService
{
    private readonly IJunctionRepository _junctionRepository;

    public ControllerStatusService(
        IJunctionRepository junctionRepository)
    {
        _junctionRepository = junctionRepository;
    }

    public async Task SetOfflineAsync(
        string junctionId)
    {
        var junction =
            await _junctionRepository.GetByIdAsync(
                junctionId);

        if (junction == null)
        {
            throw new InvalidOperationException(
                $"Junction '{junctionId}' was not found.");
        }

        junction.SetDegraded();

        await _junctionRepository.SaveAsync(
            junction);
    }

    public async Task SetOnlineAsync(
        string junctionId)
    {
        var junction =
            await _junctionRepository.GetByIdAsync(
                junctionId);

        if (junction == null)
        {
            throw new InvalidOperationException(
                $"Junction '{junctionId}' was not found.");
        }

        junction.SetRecovered();

        await _junctionRepository.SaveAsync(
            junction);
    }
}