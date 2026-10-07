using FactoryTrafficManagement.Application.DTOs;

namespace FactoryTrafficManagement.Application.Interfaces;

public interface IAutomaticTrafficService
{
    Task<TrafficTransitionDto> EvaluateAsync(
        string junctionId);
}