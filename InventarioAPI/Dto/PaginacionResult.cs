namespace InventarioAPI.Dto
{
    public class PaginacionResult<TEntity>
    {
        public List<TEntity> Result { get; set; }

        public int Total { get; set; }
    }
}