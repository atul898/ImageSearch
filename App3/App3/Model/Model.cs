using ImageSearch.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ImageSearch.Model
{
    public class Result : BindableBase
    {
        public virtual string GsearchResultClass { get; set; }
        public virtual string width { get; set; }
        public virtual string height { get; set; }
        public virtual string imageId { get; set; }
        public virtual string tbWidth { get; set; }
        public virtual string tbHeight { get; set; }
        public virtual string unescapedUrl { get; set; }
        public virtual string url { get; set; }
        public virtual string visibleUrl { get; set; }
        public virtual string title { get; set; }
        public virtual string titleNoFormatting { get; set; }
        public virtual string originalContextUrl { get; set; }
        public virtual string content { get; set; }
        public virtual string contentNoFormatting { get; set; }
        public virtual string tbUrl { get; set; }

        private ImageSource _workingImage = null;
        public ImageSource Image
        {
            get
            {
                if (this._workingImage == null)
                {
                    this._workingImage = new BitmapImage(new Uri(url));
                    //this._workingImage = new BitmapImage(new Uri(tbUrl));
                }
                return this._workingImage;
            }
        }

        public ImageSource tbImage
        {
            get
            {
                if (this._workingImage == null)
                {
                    this._workingImage = new BitmapImage(new Uri(tbUrl));
                }
                return this._workingImage;
            }
        }
    }

    public class Page : BindableBase
    {
        public virtual string start { get; set; }
        public int label { get; set; }
    }

    public class Cursor : BindableBase
    {
        public virtual string resultCount { get; set; }
        public virtual List<Page> pages { get; set; }
        public virtual string estimatedResultCount { get; set; }
        public virtual int currentPageIndex { get; set; }
        public virtual string moreResultsUrl { get; set; }
        public virtual string searchResultTime { get; set; }
    }

    public class ResponseData : BindableBase
    {

        public virtual ObservableCollection<Result> results { get; set; }
        public virtual Cursor cursor { get; set; }
    }

    public class RootObject : BindableBase
    {
        public virtual ResponseData responseData { get; set; }
        public virtual string responseDetails { get; set; }
        public virtual int responseStatus { get; set; }
    }
}
