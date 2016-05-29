using BAMApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BAMApp.Views
{
    public partial class MasterPage : ContentPage
    {
        public ListView ListView
        {
            get { return listView; }
        }
        public MasterPage()
        {
            InitializeComponent();

            var masterPageItems = new List<MasterPageItem>();
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Survey List",
                IconSource = "contacts.png",
                TargetType = typeof(SurveyListPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Discounts Earned",
                IconSource = "todo.png",
                //TargetType = typeof(TodoListPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Account",
                IconSource = "reminders.png",
                //TargetType = typeof(ReminderPage)
            });

            listView.ItemsSource = masterPageItems;
        }
    }
}
