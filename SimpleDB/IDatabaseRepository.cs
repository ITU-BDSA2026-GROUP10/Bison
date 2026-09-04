namespace SimpleDB;
interface IDatabaseRepository<T>
{
<<<<<<< HEAD
public IEnumerable<T> Read(int? limit = null);
public void Store(T record);
=======
    public IEnumerable<T> Read(int? limit = null);
    public void Store(T record);
>>>>>>> ab85ee2c874175ef3d65c669b9053b9c43e4db40
}