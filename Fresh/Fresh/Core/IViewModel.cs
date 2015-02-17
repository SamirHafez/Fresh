using System;
using System.ComponentModel;

namespace Fresh
{
	public interface IViewModel : INotifyPropertyChanged
	{
		string Title { get; set; }

		void SetState<T>(Action<T> action) where T : class, IViewModel;
	}
}
