using Cirrious.MvvmCross.ViewModels;
using Fresh.Core.Models;
using Fresh.Core.Services.Interfaces;
using System.Collections.ObjectModel;

namespace Fresh.Core.ViewModels
{
	public class MainViewModel : MvxViewModel
	{
		ITraktService traktService;

		ObservableCollection<TVShow> popular = default(ObservableCollection<TVShow>);
		public ObservableCollection<TVShow> Popular
		{
			get { return popular; }
			set { popular = value; RaisePropertyChanged(() => Popular); }
		}

		public MainViewModel(ITraktService traktService)
		{
			this.traktService = traktService;
		}

		protected override async void InitFromBundle(IMvxBundle parameters)
		{
			base.InitFromBundle(parameters);

			var popularShows = await traktService.GetPopularShowsAsync(Services.TraktExtendEnum.IMAGES);

			Popular = new ObservableCollection<TVShow>(popularShows);
		}
	}
}
