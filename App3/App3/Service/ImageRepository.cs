using ImageSearch.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ImageSearch.Service
{
    public class ImageRepository
    {
        private static object syncRoot = new Object();
        private static ImageRepository instance;

        //cache
        Dictionary<string, object> cache = new Dictionary<string, object>();


        public static Windows.UI.Core.CoreDispatcher UIDispatcher;

        #region Constructor
        private ImageRepository() { }
        public static ImageRepository Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ImageRepository();
                            instance.Initialize();
                        }
                    }
                }

                return instance;
            }
        }

        private void Initialize()
        {

        }
        #endregion Constructor

        #region Project
        private object GetResults(string searchTerm)
        {
            RootObject rd = null;
            {
                using (var client = new WebBackendClient())
                {
                    rd = client.Download<RootObject>("&q=" + searchTerm);
                }
            }
            return rd;
        }

        private async Task<RootObject> GetResultsAsync(string searchTerm, int start = 0)
        {
            RootObject rd = null;
            using (var client = new WebBackendClient())
            {
                rd = await client.DownloadAsync<RootObject>("&q=" + searchTerm + "&start=" + start);
                if((rd.responseStatus == 403)
                        ||(rd.responseData == null))
                {
                    throw new Exception(rd.responseDetails);
                }
            }
            return rd;
        }

        public async Task<ObservableCollection<Result>> GetSearchResultsAsync(string search)
        {
            ObservableCollection<Result>  Items = null;
            if (cache.ContainsKey(CallerName() + search))
                Items = cache[CallerName() + search] as ObservableCollection<Result>;
            else
            {
                Items = new ObservableCollection<Result>((await ImageRepository.Instance.GetResultsAsync(search) as RootObject).responseData.results);
                Task.Factory.StartNew(async () =>
                {
                    int calls = 10;
                    for (int i = 1; i < calls; i++)
                    {
                        ObservableCollection<Result> moreItems = new ObservableCollection<Result>((await ImageRepository.Instance.GetResultsAsync(search, i * 8) as RootObject).responseData.results);
                        UIDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
                        {
                            Items.AddRange(moreItems);
                        });
                    }
                });
                cache.Add(CallerName() + search, Items);
            }
            return Items;
        }

        #endregion

        #region Helper
        string CallerName([CallerMemberName]string caller = "")
        {
            return caller;
        }
        #endregion
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

