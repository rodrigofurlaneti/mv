using System;

namespace Api.Models
{
    public class TutorialModelView
    {
        public TutorialModelView()
        {
            
        }

        public TutorialModelView(int id, string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new Exception("URL não foi preenchida.");

            Id = id;
            URL = url;
        }

        public int Id { get; set; }
        public string URL { get; set; }
    }
}