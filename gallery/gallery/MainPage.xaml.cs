using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Reddit;
using Reddit.Inputs;
using Reddit.Inputs.LinksAndComments;
using Reddit.Inputs.Subreddits;
using Reddit.Inputs.Users;
using Reddit.Things;
using DLToolkit.Forms.Controls;
using Rg.Plugins.Popup;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Services;
using FFImageLoading;
using FFImageLoading.Forms;

namespace gallery
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        

        public async void TapGestureRecognizer_Tapped(Object sender, EventArgs args)
        {
            CachedImage image = sender as CachedImage;
            string url = image.Source.ToString().Substring(4);
            //DisplayAlert("alerte", url, "ok");
            
            await PopupNavigation.Instance.PushAsync(new popupPage(url));

        }

 
        public MainPage()
        {
            InitializeComponent();
            FlowListView.Init();

            var r = new RedditAPI("8fp-KAo4cqMTCA", "42551102-qKfgfzbeZW1MZaegec6AiOZ8z0w");
            var sub = r.Subreddit("aww");

            var getHotPosts = r.Models.Listings.Hot(new Reddit.Inputs.Listings.ListingsHotInput(limit:20), "aww").Data.Children;
            var getRisingPosts = r.Models.Listings.Rising(new CategorizedSrListingInput(limit: 15), "aww").Data.Children;
            string url = "";
            List<String> getUrlFromPosts = new List<string>();


            


            foreach (var item in getHotPosts)
            {
                if ((item.Data.URL.Contains("jpg") || item.Data.URL.Contains("png")))
                {
                    url = item.Data.URL;
                    getUrlFromPosts.Add(url);
                }
            }
            foreach (var item in getRisingPosts)
            {
                if ((item.Data.URL.Contains("jpg") || item.Data.URL.Contains("png")))
                {
                    url = item.Data.URL;
                    getUrlFromPosts.Add(url);
                }
            }

            gridaww.FlowItemsSource = getUrlFromPosts;

            gridaww.FlowColumnCount = 3;
            gridaww.FlowRowBackgroundColor = Color.White;
            gridaww.VerticalOptions = LayoutOptions.FillAndExpand;
            gridaww.HorizontalOptions = LayoutOptions.FillAndExpand;
            gridaww.FlowTappedBackgroundColor = Color.Gray;
            gridaww.RowHeight = 200;
            



        }

        
    }

    
}
