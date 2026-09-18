using System.Reflection.Metadata;

namespace SimpleDB;
interface IDatabaseRepository<T>
{
    public IEnumerable<T> Read(string path, int? limit = null);

    public void Store(T record);

    public void StoreComment(T record, string observePath, long ObservationID);
}