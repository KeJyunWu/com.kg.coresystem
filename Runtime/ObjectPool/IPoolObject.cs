public interface IPoolObject
{
    void Init(PoolProduct _product);
    void RecoverBehaviour();
    void ReuseBehaviour();
    PoolProduct GetPorductType();
}
