namespace JH_Challenge.Web.Models
{
    public class Tweet
    {
        public Data? Data { get; set; }
    }

    public class Data
    {
        public Entities? Entities { get; set; }
        public string? Id { get; set; }
        public string? Text { get; set; }
    }

    public class Entities
    {
        public List<Hashtag>? Hashtags { get; set; }
    }

    public class Hashtag
    {
        public string? Tag { get; set; }
    }
}