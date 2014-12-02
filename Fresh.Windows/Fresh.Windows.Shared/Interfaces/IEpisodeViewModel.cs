using System;

namespace Fresh.Windows.Shared.Interfaces
{
    public interface IEpisodeViewModel
    {
        int Number { get; set; }
        int Season { get; set; }
        string Screen { get; set; }
        DateTime FirstAired { get; set; }
        string Overview { get; set; }

        string Link { get; set; }
    }
}
