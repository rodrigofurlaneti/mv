using System;

namespace ApiInfox.Models
{
    public class TabloideModelView
    {
        public TabloideModelView()
        {
            
        }

        public TabloideModelView(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new Exception("URL não foi preenchida.");

            URL = url;
        }

        public string URL { get; set; }
    }
}