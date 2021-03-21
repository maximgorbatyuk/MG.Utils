using Utils.Dates;

namespace Utils.Interfaces
{
    public interface IHasTimeRange : IHasFromToDates
    {
        TimeRange Range();
    }
}