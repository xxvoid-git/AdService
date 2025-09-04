namespace AdService.Services;

public interface ILocationIndex
{
    void Load(IEnumerable<string> lines);
    IEnumerable<string> Search(string location);
}
