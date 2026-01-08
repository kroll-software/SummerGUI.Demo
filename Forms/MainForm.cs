using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework; 
using KS.Foundation;
using SummerGUI;
using SummerGUI.Scheduling;
using SummerGUI.Charting;

namespace SummerGUI.Demo
{
	public class MainForm : ApplicationWindow
	{						
		public CommonControlsSampleContainer m_CommonControlsSampleContainer;

		public DataGridEnsemble GridView { get; private set; }
		public new DemoController Controller 
		{ 
			get {
				return base.Controller as DemoController;
			}
		}

		public DayViewEnsamble m_Schedule { get; private set; }
		public TextEditorEnsemble m_Editor { get; private set; }
		public PlotterContainer m_GraphPlotter { get; private set; }
		public PerfChart m_SinusChart { get; private set; }
		public TableLayoutContainer m_SensorContainer { get; private set; }
		public List<PerfChart> m_SensorDisplays { get; private set; }

		IGuiMenuItem mnuShowPlotterData;
        ConsoleOutputWidget m_ConsoleOut;

		#pragma warning disable 219		// disable warnings about unused menu items

		/// <summary>
		/// Load all your widgets here,
		/// </summary>		
		public MainForm () : base("SummerGUI Demo", 800, 600)
		{			
			this.Title = "Summer GUI Demo - A lightweight X-Platform GUI Framework in C#";			

			// some more custom menu items

			var mnuView = MainMenu.FindItem ("View");

			mnuShowPlotterData = mnuView.InsertChild (2, "ShowPlotterData", "Plotter Data Table", (char)FontAwesomeIcons.fa_columns)
				.SetChecked (true).SetHotKey(Keys.F8);
						
			mnuShowPlotterData.CheckedChanged += delegate {
				m_GraphPlotter.Panel2Collapsed = !mnuShowPlotterData.Checked;
			};			

			mnuView.InsertSeparator (3);


			// ******

			this.TabMain.AdTabPage ("common", "Common Controls", Theme.Colors.White);
			this.TabMain.AdTabPage ("datagrid", "Data Grid");
			this.TabMain.AdTabPage ("schedule", "Schedule");
			this.TabMain.AdTabPage ("texteditor", "Editor", Color.Empty, (char)FontAwesomeIcons.fa_edit);
			this.TabMain.AdTabPage ("plotter", "Plotter");
			this.TabMain.AdTabPage ("sinus", "Sinus");
			this.TabMain.AdTabPage ("sensors", "Sensors");
            this.TabMain.AdTabPage("console", "Console");

            m_CommonControlsSampleContainer = this.TabMain.TabPages ["common"].AddChild (new CommonControlsSampleContainer ());
			this.TabMain.TabPages ["common"].ScrollBars = ScrollBars.Vertical;
			this.TabMain.TabPages ["common"].AutoScroll = true;

			GridView = this.TabMain.TabPages ["datagrid"].AddChild (new DataGridEnsemble ("samplegrid"));

			m_Schedule = this.TabMain.TabPages ["schedule"].AddChild (new DayViewEnsamble ("sampleschedule"));

			// insert a menu which the Schedule-Widget provides.., just before "Extras" menu
			var mnuExtras = MainMenu.FindItem ("Extras");
			MainMenu.Insert (MainMenu.IndexOf (mnuExtras), m_Schedule.Menu);


			m_Editor = this.TabMain.TabPages ["texteditor"].AddChild (new TextEditorEnsemble ("texteditor"));

            m_GraphPlotter = this.TabMain.TabPages ["plotter"].AddChild (new PlotterContainer ("graph2d"));

			// *** Charts
			
			// Single PerfChart "Sinus"
			m_SinusChart = this.TabMain.TabPages ["sinus"].AddChild (new PerfChart ("SinusChart"));
			m_SinusChart.Caption = "f(x) = sin(x)";
			m_SinusChart.DemoMode = PerfChart.DemoModes.Sinus;
			m_SinusChart.LineColor = MetroColors.Magenta;
			m_SinusChart.LineWidth = 2.5f;
			m_SinusChart.AverageLineColor = Color.FromArgb(128, Theme.Colors.Silver);
			m_SinusChart.AverageLineWidth = 2.5f;
			m_SinusChart.FlowDirection = PerfChart.FlowDirections.LeftToRight;
			m_SinusChart.GridScrolling = true;
			m_SinusChart.Click += delegate {
				if (m_SinusChart.FlowDirection == PerfChart.FlowDirections.LeftToRight)
					m_SinusChart.FlowDirection = PerfChart.FlowDirections.RightToLeft;
				else
					m_SinusChart.FlowDirection = PerfChart.FlowDirections.LeftToRight;
			};

			// Array of PerfCharts
			m_SensorContainer = this.TabMain.TabPages ["sensors"].AddChild (new TableLayoutContainer ("sensors"));
			m_SensorContainer.Padding = new Padding (2f);
			m_SensorContainer.CellPadding = new SizeF(1.5f, 1.5f);
			m_SensorContainer.Style.BackColorBrush.Color = Theme.Colors.Base01;

			//int maxSensors = 49;
			//int sensorColumns = 7;

			int maxSensors = 25;
			int sensorColumns = 5;
			m_SensorDisplays = new List<PerfChart> ();

			for (int k = 0; k < maxSensors; k++)
			{
				PerfChart display = m_SensorContainer.AddChild(new PerfChart("SensorDisplay" + (k + 1).ToString()), k / sensorColumns, k % sensorColumns);
				m_SensorDisplays.Add(display);

				if (k % 2 > 0)
					display.DemoMode = PerfChart.DemoModes.Random;
				else
					display.DemoMode = PerfChart.DemoModes.Sinus;

				display.DemoSpeed = (decimal)(ThreadSafeRandom.NextDouble () * 0.5);
				display.GridSpacing = 16;

				if (ThreadSafeRandom.NextBool()) {
					display.FlowDirection = PerfChart.FlowDirections.LeftToRight;
				} else {
					display.FlowDirection = PerfChart.FlowDirections.RightToLeft;
				}

				display.DisplayFrequency = (ThreadSafeRandom.NextDouble () * 100d).Ceil();

				if (ThreadSafeRandom.NextBool()) {
					display.GridScrolling = true;
					display.ValueSpacing = (float)(ThreadSafeRandom.NextDouble() * 4d + 1d);
				}

				display.AverageLineColor = MetroColors.Colors[ThreadSafeRandom.Next(MetroColors.Colors.Length)];
				display.LineColor = MetroColors.Colors[ThreadSafeRandom.Next(MetroColors.Colors.Length)];
			}
			m_SensorContainer.Rows.ForEach (row => row.SizeMode = TableSizeModes.None);

            m_ConsoleOut = new ConsoleOutputWidget("ConsoleOut");
            this.TabMain.TabPages["console"].AddChild(m_ConsoleOut);

            ShowStatus("Loading Contacts..", true);

			GridView.DataGrid.DataLoaded += delegate {				
				ShowStatus();
				bool LoadBook = true;
				if (LoadBook) {										
					string textEditorBook = (Strings.ApplicationPath (true) 
						// + "SampleData/Alice's Adventures in Wonderland.txt").FixPathForPlatform();
						+ "SampleData/Ulysses.txt").FixPathForPlatform();					 						
					ShowStatus (String.Format("Loading {0} into editor..", Strings.GetFilename(textEditorBook)), false);
					System.Threading.Tasks.Task.Factory.StartNew(() => {
                        m_Editor.Editor.RowManager.GroupParagraphs = true;  // just for the sample books
                        m_Editor.Text = TextFile.LoadTextFile(textEditorBook);
					}).ContinueWith(t => ShowStatus());
				}
			};				

			base.Controller = new DemoController (this);

            // Show Diagnostics in StatusBar
			StartDiagnostics ();
		}

		public override void OnApplicationRunning ()
		{
			base.OnApplicationRunning ();
			Controller.OnApplicationRunning ();
		}

		public override void OnLoadSettings ()
		{			
			base.OnLoadSettings ();

			if (WindowState != WindowState.Minimized) {				
				ConfigurationService.Instance.ConfigFile.Do (cfg => {					
					m_GraphPlotter.Panel2Collapsed = cfg.GetSetting(Name, "GraphPlotter.Panel2Collapsed", m_GraphPlotter.Panel2Collapsed).SafeBool();
					m_GraphPlotter.Splitter.Distance = cfg.GetSetting(Name, "GraphPlotter.Splitter", m_GraphPlotter.Splitter.Distance).SafeFloat();
				});
				mnuShowPlotterData.Checked = !m_GraphPlotter.Panel2Collapsed;			

				m_Schedule.LoadSettings (Name);				
			}
		}

		public override void OnSaveSettings ()
		{
			base.OnSaveSettings ();

			if (WindowState != WindowState.Minimized) {				
				ConfigurationService.Instance.ConfigFile.Do (cfg => {										
					cfg.SetSetting (Name, "GraphPlotter.Panel2Collapsed", m_GraphPlotter.Panel2Collapsed.ToLowerString());
					cfg.SetSetting (Name, "GraphPlotter.Splitter", m_GraphPlotter.Splitter.Distance / ScaleFactor);
				});				

				m_Schedule.SaveSettings(Name);
			}
		}

		protected override void OnInitFonts ()
		{
			SummerGUI.Scheduling.Theme.InitTheme (this);
			base.OnInitFonts ();
		}
			
		protected override void OnLoad (EventArgs e)
		{			
			base.OnLoad (e);
		}

        protected override void Dispose(bool manual)
        {
            if (manual)
            {
                // Dispose your objects here
            }
			GC.SuppressFinalize(this);
            base.Dispose(manual);
        }
    }
}

