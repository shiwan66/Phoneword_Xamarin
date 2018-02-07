using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Phoneword
{
    [Activity(Label = "CodeLayoutActivity")]
    public class CodeLayoutActivity : Activity
    {
        int c;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            this.SetContentView(Resource.Layout.SimpleStateView);

            var output = this.FindViewById<TextView>(Resource.Id.outputText);
            if(savedInstanceState != null)
            {
                c = savedInstanceState.GetInt("counter", -1);
            } else
            {
                c = -1;
            }

            output.Text = c.ToString();

            var incrementCounter = this.FindViewById<Button>(Resource.Id.incrementCounter);

            incrementCounter.Click += (s, e) =>
            {
                output.Text = (++c).ToString();
            };

        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt("counter", c);
            base.OnSaveInstanceState(outState);
        }
    }
}