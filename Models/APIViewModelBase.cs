namespace CarseerWebAPP.Models
{
    public abstract class APIViewModelBase
    {
        public int Count { get; set; }
        public string Message { get; set; }
        public string SearchCriteria { get; set; }
    }
}
