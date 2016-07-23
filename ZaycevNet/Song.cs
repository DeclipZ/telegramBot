using System.IO;

namespace ZaycevNet
{
    public class Song
    {
        public Song(string Performer, string Title, Stream Audio, int Duration)
        {
            this.Performer = Performer;
            this.Title = Title;
            this.Audio = Audio;
            this.Duration = Duration;
        }

        public string Performer;
        public string Title;
        public Stream Audio;
        public int Duration;
    }
}
