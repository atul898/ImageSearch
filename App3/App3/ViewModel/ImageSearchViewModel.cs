using ImageSearch.Common;
using ImageSearch.Model;
using ImageSearch.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSearch.ViewModel
{
    public class ImageSearchViewModel : BindableBase
    {
        public ImageSearchViewModel()
        {
            //var v = ImageRepository.Instance.GetResults("Random Girl");
            //Items = (v as RootObject).responseData.results;

            LoadData("");
        }

        private ObservableCollection<Result> items;

        //public List<Result> Items
        public ObservableCollection<Result> Items
        {
            get { return items; }
            set
            {
                items = value;
                OnPropertyChanged("Items");
            }
        }

        private string search = string.Empty;
        public string Search
        {
            get { return search; }
            set
            {
                if (search != value)
                {
                    search = value;
                    OnPropertyChanged("Search");
                    //var v = ImageRepository.Instance.GetResults(search);
                    //Items = (v as RootObject).responseData.results;
                    LoadData(search);
                }
            }
        }


        private async Task LoadData(string search)
        {
            if (!IsLoading)
            {
                IsLoading = true;
                try
                {
                    Items = await ImageRepository.Instance.GetSearchResultsAsync(search);
                    IsBannedByGoogle = false;
                }
                catch (Exception ex)
                {
                    IsBannedByGoogle = true;
                    BannedByGoogleMessage = ex.Message;
                }
            }
            IsLoading = false;
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            private set
            {
                SetProperty(ref _isLoading, value);
            }
        }

        private bool _isBannedByGoogle;
        public bool IsBannedByGoogle
        {
            get
            {
                return _isBannedByGoogle;
            }
            private set
            {
                SetProperty(ref _isBannedByGoogle, value);
            }
        }

        private string _bannedByGoogleMessage;
        public string BannedByGoogleMessage
        {
            get
            {
                return _bannedByGoogleMessage;
            }
            private set
            {
                SetProperty(ref _bannedByGoogleMessage, value);
            }
        }
    }

    public static class ObservableCollectionExtend
    {
        public static void AddRange<TSource>(this ObservableCollection<TSource> source, IEnumerable<TSource> items)
        {
            foreach (var item in items)
            {
                source.Add(item);
            }
        }
    }
}