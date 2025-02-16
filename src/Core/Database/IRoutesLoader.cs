using Core.RouteModels;

namespace Core.Database;

public interface IRoutesLoader
{
    public List<Route> LoadRoutes(string startFolder);
}