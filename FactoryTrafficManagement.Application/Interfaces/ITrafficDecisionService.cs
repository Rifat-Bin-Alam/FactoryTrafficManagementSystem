namespace FactoryTrafficManagement.Application.Interfaces;

public interface ITrafficDecisionService
{
    Task<string> GetNextDirectionAsync(string junctionId);
}