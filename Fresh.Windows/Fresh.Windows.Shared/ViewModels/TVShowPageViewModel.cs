﻿using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Fresh.Windows.Core.Services.Interfaces;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Fresh.Windows.Shared.Interfaces;
using Fresh.Windows.Shared.Models;
using Fresh.Windows.Shared.Services.Interfaces;

namespace Fresh.Windows.ViewModels
{
    public class TVShowPageViewModel : ViewModel, ITVShowPageViewModel
    {
        private readonly ITraktService traktService;
        private readonly INavigationService navigationService;
        private readonly IStorageService storageService;

        private string showId;
        private string showTitle;

        public TVShowPageViewModel(ITraktService traktService, INavigationService navigationService, IStorageService storageService)
        {
            this.traktService = traktService;
            this.navigationService = navigationService;
            this.storageService = storageService;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            showId = navigationParameter as string;

            var dbShow = await storageService.GetShowAsync(showId);

            Update(dbShow);

            if (dbShow.Seasons.Count == 0)
            { 
                var fullShow = TVShow.FromTrakt(await traktService.GetShowAsync(showId, extended: true));

                await storageService.UpdateShowAsync(fullShow);

                dbShow = fullShow;
                Update(dbShow);
            } 
        }

        public DelegateCommand<Season> EnterSeasonCommand
        {
            get
            {
                return new DelegateCommand<Season>(EnterSeason);
            }
        }

        private void EnterSeason(Season season)
        {
            navigationService.Navigate(App.Experience.Season.ToString(), season.Id);
        }

        private void Update(TVShow fullShow)
        {
            Title = fullShow.Title;
            Year = fullShow.Year;
            Network = fullShow.Network;
            Overview = fullShow.Overview;
            Poster = fullShow.Poster;
            FirstAired = fullShow.FirstAired;

            Seasons = new ObservableCollection<Season>(fullShow.Seasons);
        }

        string title = default(string);
        public string Title { get { return title; } set { SetProperty(ref title, value); } }

        int year = default(int);
        public int Year { get { return year; } set { SetProperty(ref year, value); } }

        string url = default(string);
        public string Url { get { return url; } set { SetProperty(ref url, value); } }

        DateTime firstAired = default(DateTime);
        public DateTime FirstAired { get { return firstAired; } set { SetProperty(ref firstAired, value); } }

        string country = default(string);
        public string Country { get { return country; } set { SetProperty(ref country, value); } }

        string overview = default(string);
        public string Overview { get { return overview; } set { SetProperty(ref overview, value); } }

        TimeSpan runtime = default(TimeSpan);
        public TimeSpan Runtime { get { return runtime; } set { SetProperty(ref runtime, value); } }

        string status = default(string);
        public string Status { get { return status; } set { SetProperty(ref status, value); } }

        string network = default(string);
        public string Network { get { return network; } set { SetProperty(ref network, value); } }

        DayOfWeek airDay = default(DayOfWeek);
        public DayOfWeek AirDay { get { return airDay; } set { SetProperty(ref airDay, value); } }

        TimeSpan airTime = default(TimeSpan);
        public TimeSpan AirTime { get { return airTime; } set { SetProperty(ref airTime, value); } }

        DateTime lastUpdate = default(DateTime);
        public DateTime LastUpdate { get { return lastUpdate; } set { SetProperty(ref lastUpdate, value); } }

        string poster = default(string);
        public string Poster { get { return poster; } set { SetProperty(ref poster, value); } }

        ObservableCollection<Season> seasons = default(ObservableCollection<Season>);
        public ObservableCollection<Season> Seasons { get { return seasons; } set { SetProperty(ref seasons, value); } }

        ObservableCollection<string> genres = default(ObservableCollection<string>);
        public ObservableCollection<string> Genres { get { return genres; } set { SetProperty(ref genres, value); } }
    }
}
