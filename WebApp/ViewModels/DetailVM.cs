using WebApp.Models;

namespace WebApp.ViewModels
{
    public class DetailVM
    {
        public Article Article { get; set; }
        public List<Article> Suggestions { get; set; }
        public Article Befero { get; set; }
        public Article After { get; set; }
        public IEnumerable<Article> Videos { get; set; }
    }
}