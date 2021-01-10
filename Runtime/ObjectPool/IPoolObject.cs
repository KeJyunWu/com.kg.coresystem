public interface IPoolObject
{
    void ProductInit(PoolProduct _product);
    void RecoverBehaviour();
    void ReuseBehaviour();
    PoolProduct GetPorductType();
}
