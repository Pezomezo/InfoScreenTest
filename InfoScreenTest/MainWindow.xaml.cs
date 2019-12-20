using CefSharp;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;
using TableDependency.SqlClient.Base.Delegates;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace InfoScreenTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<InfoScreenPC> InfoScreenPCs { get; private set; }

        public MainWindow(){

            InitializeComponent();
            
            Console.WriteLine("Progran started");

            this.Closing += OnClosing;

            var mapper = new ModelToTableMapper<InfoScreenPC>();
            mapper.AddMapping(model => model.InfoScreenPCID, "InfoScreenPCID");
            mapper.AddMapping(model => model.InfoScreenPCName, "InfoScreenPCName");
            mapper.AddMapping(model => model.PowerState, "PowerState");

            string connectionString = "data source=PC0175557;initial catalog=InfoScreenDB;persist security info=False;user ID=pezoka;password=Pezomezo#1;connect timeout=60;encrypt=True;TrustServerCertificate=True;";
            var tableDependency = new SqlTableDependency<InfoScreenPC>(connectionString, "InfoScreenPC", mapper: mapper);
            
            Console.WriteLine("Inside the using");
            
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            Console.WriteLine("\nAfter events have been created");
            tableDependency.Start();
        }
        private void TableDependency_OnError(object sender, ErrorEventArgs e)
        {
            Exception ex = e.Error;
            throw ex;
        }

        private void TableDependency_OnChanged(object sender, RecordChangedEventArgs<InfoScreenPC> e)
        {
            Console.WriteLine("\nInside the debendency changed");

            if (e.ChangeType != ChangeType.None)
            {
                var changedEntity = e.Entity;
                InfoScreenPCs = new List<InfoScreenPC>();
                InfoScreenPCs.Add(new InfoScreenPC() { InfoScreenPCID = changedEntity.InfoScreenPCID,
                                                       InfoScreenPCName = changedEntity.InfoScreenPCName.ToString(),
                                                       PowerState = changedEntity.PowerState
                });
                InfoScreenDataGrid.ItemsSource = InfoScreenPCs;
                Console.WriteLine( "\n" + e.ChangeType.ToString());
                Console.WriteLine("\t" + changedEntity.InfoScreenPCID.ToString());
                Console.WriteLine("\t" + changedEntity.InfoScreenPCName.ToString());
                Console.WriteLine("\t" + changedEntity.PowerState.ToString());
            }
        }

        private void ADD_Auto_login()
        {
            //// Create an HTTP request 
            //WebRequest request = WebRequest.Create(new Uri("https://danfossdrives.visualstudio.com/Autotest/_dashboards/dashboard/9fb29e36-df7a-41ae-8879-34c4c778c5e6"));

            //// Create Auth Context for AAD (common or tenant-specific endpoint):
            //AuthenticationContext authContext = new AuthenticationContext("https://login.microsoftonline.com/common/oauth2/nativeclient");

            //// Acquire application token for Kusto:
            //ClientCredential applicationCredentials = new ClientCredential("b4b54bd2-2f6a-4892-b833-b567b7bf66e5", "RgW8iSL.7_n-MWbfMY8BXEGaE@LDWd7k");
            //AuthenticationResult result =
            //        authContext.AcquireTokenAsync("https://danfossdrives.visualstudio.com/Autotest/_dashboards/dashboard/9fb29e36-df7a-41ae-8879-34c4c778c5e6", applicationCredentials).GetAwaiter().GetResult();

            //// Extract Bearer access token and set the Authorization header on your request:
            //string bearerToken = result.AccessToken;
            //request.Headers.Set(HttpRequestHeader.Authorization, string.Format(CultureInfo.InvariantCulture, "{0} {1}", "Bearer", bearerToken));
        }

        private void chromeBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            //Dispatcher.BeginInvoke((Action)(async () => {
            //    HtmlTextBlock.Text = await chromeBrowser.GetSourceAsync();
            //}));
            Console.WriteLine("Page Loaded");
        }

        private void SetHTML_Click(object sender, RoutedEventArgs e)
        {
            //chromeBrowser.LoadHtml(HtmlTextBlock.Text);
            Console.WriteLine("Clicked");
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
