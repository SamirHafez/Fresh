namespace Fresh.Windows.Shared.Interfaces
{
    public interface IEpisodePageViewModel
    {
        int Number { get; set; }

        string Title { get; set; }

        string Overview { get; set; }

        string Link { get; set; }

        bool Watched { get; set; }

        string Screen { get; set; }
    }
}
