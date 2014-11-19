﻿using Fresh.Windows.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fresh.Windows.Core.Services.Interfaces
{
    public interface ITraktService
    {
        Task<dynamic> GetSettings(string username, string password);

        Task<IList<TVShow>> GetCollection(string username);
    }
}
