using MG.Utils.Dates;

namespace MG.Utils.Interfaces
{
    public interface IHasTimeRange : IHasFromToDates
    {
        TimeRange Range();
    }
}