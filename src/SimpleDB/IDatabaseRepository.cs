using System.Reflection.Metadata;

namespace SimpleDB;

public interface IDatabaseRepository<T>
{
    public IEnumerable<T> ReadObservation(string path, int? limit = null);

    public IEnumerable<T> ReadDiscussion(string path, long observationId, int? limit = null);

    public IEnumerable<T> ReadProposals(string path, long observationId, int? limit = null);

    public void Store(T record, string path, string location);

    public void StoreComment(T record, string observePath, string commentPath, long ObservationID);

    public void StoreProposal(T record, string proposalPath, string observePath, long ObservationID);

    public long GetNumberOfLinesInAFile(string path);

}