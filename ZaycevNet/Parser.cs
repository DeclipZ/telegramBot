using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;

namespace ZaycevNet
{
    public class Parser
    {
        public Parser() { }

        public Song getSong(string songName)
        {
            string page = getHtmlPage("http://go.mail.ru/zaycev?q=" + songName);
 
            page = getInnerContent(page, "div", "zaycev__play");
            string url = getInnerOfAttribute(page, "href", "?autoplay");

            page = getHtmlPage(url);
      
            string performer = getInnerContent(page, "div", "musicset-track__artist");
            performer = getInnerContent(performer, "a");
            string title = getInnerContent(page, "div", "musicset-track__track-name");
            title = getInnerContent(title, "span");
            string durationString = getInnerContent(page, "div", "musicset-track__duration");
            string[] durationArgs = durationString.Split(':');
            int duration = Int32.Parse(durationArgs[0]) * 60 + Int32.Parse(durationArgs[1]);

            page = getInnerContent(page, "div", "audiotrack-button audiotrack-button_download");
            url = getInnerOfAttribute(page, "href", "\" ");

            WebRequest request = HttpWebRequest.Create(url);
            Stream stream = request.GetResponse().GetResponseStream();

            Song song = new Song(performer, title, stream, duration);
            return song;
        }


        public string getInnerContent(string webPage, string tagName, string className = null)
        {
           
            string openTag = String.Format("<{0}", tagName);
            if (className != null)
                openTag = String.Format("<{0} class=\"{1}", tagName, className);
            string closeTag = String.Format("</{0}>", tagName);

            int startIndex = webPage.IndexOf(openTag);
            int endIndex = webPage.IndexOf(">", startIndex);
            startIndex += endIndex - startIndex + 1;

            endIndex = webPage.IndexOf(closeTag, startIndex);
            int length = endIndex - startIndex;
            string innerContent = webPage.Substring(startIndex, length);
            return innerContent;
        }

        public string getInnerOfAttribute(string webPage, string attributeName, string attributeEnding)
        {
            int startIndex = webPage.IndexOf(attributeName);
            //Adding attributeName length to startIndex to prevent incorrect search
            int endIndex = webPage.IndexOf("\"", startIndex + attributeName.Length);

            startIndex += endIndex - startIndex + 1;
            endIndex = webPage.IndexOf(attributeEnding, startIndex);
            int length = endIndex - startIndex;
            string innerOfAttribute = webPage.Substring(startIndex, length);
            return innerOfAttribute;
        }

        public string getHtmlPage(string url)
        {
            WebClient webClient = new WebClient();
            using (Stream data = webClient.OpenRead(url))
            {
                using (StreamReader reader = new StreamReader(data))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
