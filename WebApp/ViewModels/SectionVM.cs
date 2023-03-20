using WebApp.Models;

namespace WebApp.ViewModels
{
    public class SectionVM
    {
        public List<Article> Articles { get; set; }
        public List<Article> Videos { get; set; }
        public List<Article> PopularSection { get; set; }
    }
}
