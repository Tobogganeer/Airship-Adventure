
public interface ISaveable<T> where T : ISaveable<T>, new()
{
    
    public void Save(ByteBuffer buf);

    public void Load(ByteBuffer buf);
}
