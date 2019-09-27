using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Reddit;
using Reddit.Controllers;
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

            var getHotPosts = sub.Posts.GetHot(limit: 20);
            var getRisingPosts = sub.Posts.GetRising(limit: 15);

            List<string> getUrlFromPosts = new List<string>();
            for (int i = 0; i < Math.Max(getHotPosts.Count, getRisingPosts.Count); i++)
            {
                if (i < getHotPosts.Count 
                    && !getHotPosts[i].Listing.IsSelf)
                {
                    AddPostURL(r, (SelfPost)getHotPosts[i], ref getUrlFromPosts);
                }
                if (i < getRisingPosts.Count
                    && !getRisingPosts[i].Listing.IsSelf)
                {
                    AddPostURL(r, (SelfPost)getRisingPosts[i], ref getUrlFromPosts);
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

        private void AddPostURL(RedditAPI r, SelfPost post, ref List<string> getUrlFromPosts)
        {
            string url = (new LinkPost(r.Models, post)).URL;
            if (url.Contains("jpg", StringComparison.OrdinalIgnoreCase)
                || url.Contains("png", StringComparison.OrdinalIgnoreCase))
            {
                getUrlFromPosts.Add(url);
            }
        }
    }

    
}
