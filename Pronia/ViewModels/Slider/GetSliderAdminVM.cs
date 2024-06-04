namespace Pronia.ViewModels.Slider
{
    public class GetSliderAdminVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public int Discount { get; set; }
        public string ImageUrl { get; set; }
        public bool IsDeleted { get; set; }
        public string CreateTime { get; set; }
        public string UpdateTime { get; set; }
    }
}
