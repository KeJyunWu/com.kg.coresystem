public interface IPoolObject
{
    public void ProductInit(PoolProduct _product);
    public void RecoverBehaviour();
    public void ReuseBehaviour();
    public PoolProduct GetPorductType();
}
