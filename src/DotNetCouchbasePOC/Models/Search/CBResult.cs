namespace DotNetCouchbasePOC.Models
{
    public class CBResult<T>
    {
        public Meta meta { get; set; }
        public T json { get; set; }
    }
}