using System.Threading.Tasks;

namespace WebHost.Infrastructure.Contracts.Infrastructure
{
    public interface IView
    {
        Task<string> RenderAsync<T>(string view, T model);
    }
}