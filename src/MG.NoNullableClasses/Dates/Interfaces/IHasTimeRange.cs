namespace MG.Utils.Abstract.Dates.Interfaces
{
    public interface IHasTimeRange : IHasFromToDates
    {
        TimeRange Range();
    }
}