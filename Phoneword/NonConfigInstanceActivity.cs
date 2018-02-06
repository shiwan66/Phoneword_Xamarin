using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using JsonLib.Json;
using Newtonsoft.Json.Linq;

namespace Phoneword
{
    [Activity(Label = "NonConfigInstanceActivity")]
    public class NonConfigInstanceActivity : ListActivity
    {
        TweetListWrapper _savedInstance;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var tweetsWrapper = LastNonConfigurationInstance as TweetListWrapper;

            if (tweetsWrapper != null)
            {
                PopulateTweetList(tweetsWrapper.Tweets);
            }
            else
            {
                SearchTwitter("xamarin");
            }
        }

        public override Java.Lang.Object OnRetainNonConfigurationInstance()
        {
            base.OnRetainNonConfigurationInstance();
            return _savedInstance;
        }

        public void SearchTwitter(string text)
        {
            string searchUrl = String.Format("http://shop.troncell.com/search.json");

            var httpReq = (HttpWebRequest)HttpWebRequest.Create(new Uri(searchUrl));
            httpReq.BeginGetResponse(new AsyncCallback(ResponseCallback), httpReq);
        }

        void ResponseCallback(IAsyncResult ar)
        {
            var httpReq = (HttpWebRequest)ar.AsyncState;
            var httpRes = (HttpWebResponse)httpReq.EndGetResponse(ar);
            if (httpRes != null)
            {
                ParseResults(httpRes);
            }
        }

        void ParseResults(HttpWebResponse response)
        {
            var rawJson = new StreamReader(response.GetResponseStream()).ReadToEnd();
            var json = JObject.Parse(rawJson);
            var results = (from result in json["results"].ToArray() let jResult = result select jResult["text"].ToString()).ToArray();

            RunOnUiThread(() => {
                PopulateTweetList(results);
            });
        }

        void PopulateTweetList(string[] results)
        {
            ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, results);
            _savedInstance = new TweetListWrapper { Tweets = results };
        }
    }

    class TweetListWrapper: Java.Lang.Object
    {
        public string[] Tweets { get; set; }
    }
}