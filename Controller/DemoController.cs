using System;
using System.Threading;
using System.Drawing;
using KS.Foundation;

namespace SummerGUI.Demo
{
	public class DemoController : RootController
	{
		public MainForm Context { get; private set; }
		public SampleDataProvider DataProvider { get; private set; }

		public DemoController (MainForm context)
		{
			Context = context;
			DataGridView grid = Context.GridView.DataGrid;

			DataProvider = AddSubController (new SampleDataProvider (this, grid));

			DataProvider.InitializeColumns ();
			grid.SetDataProvider (DataProvider);
			Context.GridView.ToolBar.SetRowManager (DataProvider.RowManager);

			/**** ***/
			grid.ItemSelected += delegate {
				int rowIndex = DataProvider.RowManager.CurrentRowIndex;
				Contact contact = DataProvider.Contacts[rowIndex];
				ContactForm Dlg = new ContactForm(Context, contact);
				Dlg.ShowDialog(Context);
				Dlg.Dispose();
				Context.MakeCurrent();	// set the OpenGL Context to the active window
				grid.Focus();
			};
				
			Context.GridView.ToolBar.CmdNew.Click += delegate {	
				ContactForm Dlg = new ContactForm(Context, null);
				Dlg.ShowDialog(Context);
				if (Dlg.Result == DialogResults.OK) {
					DataProvider.AddContact(Dlg.Contact);
				}
				Dlg.Dispose();
				grid.Focus();
			};

			grid.DeleteItem += delegate {				
				if (Context.ShowQuestion("Do you really want to delete the selected contact ?") == DialogResults.Yes) {
					DataProvider.DeleteContact(grid.RowIndex);
				}	
			};
		}

		public void OnApplicationRunning ()
		{			
			DataProvider.InitializeRows ();
		}

		ContactForm DlgTest;
		public void TestChildWindowMemoryLeaks()
		{
			int i = 0;
			while (i++ < 1000) {
				Context.GridView.ToolBar.CmdEdit.OnClick ();

				if (DlgTest != null) {				
					DlgTest.Dispose ();
					DlgTest = null;
				}

				//await System.Threading.Tasks.Task.Delay (100);
				//Thread.Sleep(10);
				Console.WriteLine ("Window opend and closed: {0}", i.ToString("n0"));
			}
		}
	}
}

