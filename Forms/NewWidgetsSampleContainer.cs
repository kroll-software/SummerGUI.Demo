using System;
using System.Drawing;
using SummerGUI;
using KS.Foundation;

namespace SummerGUI.Demo
{
	/// <summary>
	/// Sample container showcasing the newest widgets of the SummerGUI family.
	/// Mirrors the structure of <see cref="CommonControlsSampleContainer"/>:
	/// a TableLayout with a caption label plus the widgets, here currently
	/// the new <see cref="ColorComboBox"/> and the new <see cref="ListView"/>.
	/// </summary>
	public class NewWidgetsSampleContainer : TableLayoutContainer
	{
		CaptionLabel m_Label1;
		TreeView m_TreeView1;
		TextLabel m_LblTree;
		TextLabel m_TreeStatus;
		TextLabel m_LblLvLI;
		ListView m_ListViewLI;
		TextLabel m_LvLIStatus;
		TextLabel m_LblListView;
		ListView m_ListView1;
		TextLabel m_ListViewStatus;
		TextLabel m_LblColor;
		ColorComboBox m_ColorComboBox1;
		TextLabel m_LblHatch;
		HatchStyleComboBox m_HatchStyleComboBox1;
		TextLabel m_LblDash;
		DashStyleComboBox m_DashStyleComboBox1;
		TextLabel m_LblFont;
		FontComboBox m_FontComboBox1;
		TextLabel m_LblMask;
		MaskedTextBox m_MaskedTextBox1;
		TextLabel m_LblMaskOpt;
		MaskedTextBox m_MaskedTextBox2;
		TextLabel m_LblBlockEdit;
		SummerGUI.BlockEditTextBox m_BlockEdit1;
		TextLabel m_LblDateTime;
		DateTimePicker m_DateTimePicker1;
		TextLabel m_LblTime;
		TimePicker m_TimePicker1;

		public NewWidgetsSampleContainer()
			: base("NewWidgetsSampleContainer")
		{
			Padding = new Padding(16);
			CellPadding = new SizeF(16, 16);
			InitControls();
			CollapsibleColumnsWidth = 420;
		}

		private void InitControls()
		{
			int tableRow = 0;
			int tableColumn = 0;

			m_Label1 = new CaptionLabel("newwidgetslabel");
			m_Label1.Style.BackColorBrush.Color = Theme.Colors.Base2;
			m_Label1.Dock = Docking.Fill;
			m_Label1.Text = "New Widgets".ToUpper();
			this.AddChild(m_Label1, tableRow++, tableColumn);

			// =================================================================
			// TreeView (ZUERST — neue WinForms-Parität-Widget mit Ästen,
			// FontAwesome +/−-Icons, gestrichelten Treelines, V+H-Scrolling,
			// Keyboard-Navigation und CollapseAll/ExpandAll).
			// =================================================================
			m_LblTree = new TextLabel("lblTree", "TreeView (expand / collapse, +/−, dashed treelines)");
			this.AddChild(m_LblTree, tableRow++, tableColumn);

			m_TreeView1 = new TreeView("TreeView1");
			m_TreeView1.MinSize = new SizeF(420, 260);
			m_TreeView1.MaxSize = new SizeF(420, 260);

			// Sample tree — mimics a small project tree.
			char chFile = (char)FontAwesomeIcons.fa_file_o;
			char chFolder = (char)FontAwesomeIcons.fa_folder;

			var root1 = m_TreeView1.Add("SummerGUI", tag: "root", glyph: (char)FontAwesomeIcons.fa_dot_circle_o, isFolder: true);
			var src = root1.AddChild(new TreeViewItem("src", glyph: chFolder, isFolder: true));

			var drawing = src.AddChild(new TreeViewItem("drawing", glyph: chFolder, isFolder: true));
			drawing.AddChild(new TreeViewItem("textRendering.cs", glyph: chFile));
			drawing.AddChild(new TreeViewItem("pen.cs", glyph: chFile));
			drawing.AddChild(new TreeViewItem("brush.cs", glyph: chFile));
			drawing.AddChild(new TreeViewItem("dashEngine.cs", glyph: chFile));

			var forms = src.AddChild(new TreeViewItem("forms", glyph: chFolder, isFolder: true));
			forms.AddChild(new TreeViewItem("listView.cs", glyph: chFile));
			forms.AddChild(new TreeViewItem("comboBox.cs", glyph: chFile));
			var tree = forms.AddChild(new TreeViewItem("treeView.cs", glyph: chFile));
			tree.AddChild(new TreeViewItem("treeNode.cs", glyph: chFile));
			tree.AddChild(new TreeViewItem("treeViewWidgetStyles.cs", glyph: chFile));

			var tests = root1.AddChild(new TreeViewItem("tests", glyph: chFolder, isFolder: true));
			tests.AddChild(new TreeViewItem("drawingTests.cs", glyph: chFile));
			tests.AddChild(new TreeViewItem("formsTests.cs", glyph: chFile));
			tests.AddChild(new TreeViewItem("widgetStyleTests.cs", glyph: chFile));

			var docs = root1.AddChild(new TreeViewItem("docs", glyph: chFolder, isFolder: true));
			docs.AddChild(new TreeViewItem("readme.md", glyph: chFile));
			docs.AddChild(new TreeViewItem("contributing.md", glyph: chFile));
			docs.AddChild(new TreeViewItem("changes.md", glyph: chFile));

			// Start expanded (default) so the user sees the full tree immediately.
			m_TreeView1.ExpandAll ();

			this.AddChild(m_TreeView1, tableRow++, tableColumn);

			m_TreeStatus = new TextLabel("treeStatus", "Selected: (none)");
			this.AddChild(m_TreeStatus, tableRow++, tableColumn);

			m_TreeView1.SelectionChanged += (s, e) =>
			{
				var sel = m_TreeView1.SelectedItem;
				m_TreeStatus.Text = sel != null
					? ("Selected: " + sel.Text + (sel.HasChildren ? " [depth " + sel.Depth + "]" : ""))
					: "Selected: (none)";
			};

			// Doppelklick oder Enter → InfoMsgBox (wie bei der ListView ItemActivated).
			m_TreeView1.ItemDoubleClicked += (s, e) =>
			{
				var sel = e.Node ?? m_TreeView1.SelectedItem;
				string msg = (sel != null)
					? "TreeView-Item aktiviert:\n" + sel.Text
					: "(kein Item gewählt)";
				m_TreeView1.ParentWindow?.ShowInfo (msg);
			};

			m_TreeView1.NodeExpandStateChanged += (s, e) =>
			{
				// (Just for observation; no popup so as not to disrupt interaction.)
			};

			// Zwei Test-Buttons für ExpandAll / CollapseAll.
			// WICHTIG: Standard-Button feuert bei normalem Klick das "Click"-Event (via
			// OnClick/InvokeMouseUp), NICHT "Fire". "Fire" wird nur bei IsAutofire=true
			// (Spinner-Buttons) ausgelöst. Deshalb Click verwenden.
			var btnExpand = new Button("btnTvExpand", "Expand All");
			btnExpand.MinSize = new SizeF(160, 28);
			btnExpand.Click += (s, e) => m_TreeView1.ExpandAll ();
			this.AddChild(btnExpand, tableRow++, tableColumn);

			var btnCollapse = new Button("btnTvCollapse", "Collapse All");
			btnCollapse.MinSize = new SizeF(160, 28);
			btnCollapse.Click += (s, e) => m_TreeView1.CollapseAll ();
			this.AddChild(btnCollapse, tableRow++, tableColumn);

			// =================================================================
			// ListViewen ZUERST (damit beim Testen nicht gescrollt werden muss).
			// =================================================================

			// ---------------------------------------------
			// ListView – LARGE ICON (View.LargeIcon):
			// große Icons, 2-zeiliger Text, Tooltips bei gekürztem Text,
			// Cursor-Tasten-Navigation (↑↓←→), aktives Item hervorgehoben.
			// ---------------------------------------------
			m_LblLvLI = new TextLabel("lblLvLI", "ListView – LargeIcon");
			this.AddChild(m_LblLvLI, tableRow++, tableColumn);

			m_ListViewLI = new ListView("ListViewLI");
			m_ListViewLI.MinSize = new SizeF(380, 300);
			m_ListViewLI.MaxSize = new SizeF(380, 300);
			m_ListViewLI.View = SummerGUI.ListViewView.LargeIcon;
			m_ListViewLI.LargeIconSize = 48f;

			// Items mit (teils langen) Namen → Tooltip bei gekürztem 2-zeiligen Text.
			string[] namesLI = {
				"Readme", "Logo", "Audio Track", "Report Q2 2026",
				"Backup", "Projects", "Screenshot 2026", "Config",
				"License", "Notes", "Q22026QuarterlyFinancialReportSummary", "Archive"
			};
			char[] iconsLI = {
				(char)FontAwesomeIcons.fa_file_text_o,
				(char)FontAwesomeIcons.fa_file_image_o,
				(char)FontAwesomeIcons.fa_file,
				(char)FontAwesomeIcons.fa_file_excel_o,
				(char)FontAwesomeIcons.fa_file_archive_o,
				(char)FontAwesomeIcons.fa_list,
				(char)FontAwesomeIcons.fa_camera,
				(char)FontAwesomeIcons.fa_cog,
				(char)FontAwesomeIcons.fa_file_o,
				(char)FontAwesomeIcons.fa_file_o,
				(char)FontAwesomeIcons.fa_image,
				(char)FontAwesomeIcons.fa_archive
			};
			for (int i = 0; i < namesLI.Length; i++)
				m_ListViewLI.Items.AddLast (new ListViewItem (namesLI [i], iconsLI [i], "", ""));

			this.AddChild(m_ListViewLI, tableRow++, tableColumn);

			m_LvLIStatus = new TextLabel("lvLIStatus", "LargeIcon: -");
			this.AddChild(m_LvLIStatus, tableRow++, tableColumn);
			m_ListViewLI.SelectionChanged += (s, e) =>
			{
				var sel = m_ListViewLI.SelectedItem;
				m_LvLIStatus.Text = sel != null
					? "LargeIcon: [" + m_ListViewLI.SelectedIndex + "] " + sel.Text
					: "LargeIcon: -";
			};
			m_ListViewLI.ItemActivated += (s, e) =>
			{
				var sel = m_ListViewLI.SelectedItem;
				string msg = (sel != null)
					? "Item [" + m_ListViewLI.SelectedIndex + "] aktiviert:\n" + sel.Text
					: "(kein Item gewählt)";
				m_ListViewLI.ParentWindow?.ShowInfo(msg);
			};
			m_ListViewLI.SelectedIndex = 2;

			// ---------------------------------------------
			// ListView – Details (Spalten, Sub-Items, Scrollbars).
			// ---------------------------------------------
			m_LblListView = new TextLabel("lblListView", "ListView (Details)");
			this.AddChild(m_LblListView, tableRow++, tableColumn);

			m_ListView1 = new ListView("ListView1");
			m_ListView1.MinSize = new SizeF(380, 220);
			m_ListView1.MaxSize = new SizeF(380, 220);

			// Columns – breiter als die List-Breite (380), damit eine horizontale Scrollbar entsteht
			m_ListView1.Columns.Add(new SummerGUI.ListViewColumn("Name", 200f));
			m_ListView1.Columns.Add(new SummerGUI.ListViewColumn("Type", 200f));
			m_ListView1.Columns.Add(new SummerGUI.ListViewColumn("Size", 120f));

			string[] types = { "Text file", "Image", "Audio", "Spreadsheet", "Archive", "Folder" };
			char[]   icons = {
				(char)FontAwesomeIcons.fa_file_text_o,
				(char)FontAwesomeIcons.fa_file_image_o,
				(char)FontAwesomeIcons.fa_file,
				(char)FontAwesomeIcons.fa_file_excel_o,
				(char)FontAwesomeIcons.fa_file_archive_o,
				(char)FontAwesomeIcons.fa_list
			};
			string[] names = { "Readme", "Logo", "Track", "Report", "Backup", "Projects" };

			for (int i = 1; i <= 40; i++)
			{
				int j = (i - 1) % icons.Length;
				var it = new ListViewItem(names[j] + " " + i.ToString(),
					icons[j],
					types[j], (i * 3).ToString("N0") + " KB");
				m_ListView1.Items.AddLast(it);
			}

			m_ListView1.SelectedIndex = 2;
			this.AddChild(m_ListView1, tableRow++, tableColumn);

			m_ListViewStatus = new TextLabel("lvStatus", "Selected: -");
			this.AddChild(m_ListViewStatus, tableRow++, tableColumn);
			m_ListView1.SelectionChanged += (s, e) =>
			{
				var sel = m_ListView1.SelectedItem;
				m_ListViewStatus.Text = sel != null
					? "Selected: [" + m_ListView1.SelectedIndex + "] " + m_ListView1.GetRowText(m_ListView1.SelectedIndex)
					: "Selected: -";
			};
			m_ListView1.ItemActivated += (s, e) =>
			{
				var sel = m_ListView1.SelectedItem;
				string msg = (sel != null)
					? "Zeile [" + m_ListView1.SelectedIndex + "] aktiviert:\n" + m_ListView1.GetRowText(m_ListView1.SelectedIndex)
					: "(keine Zeile gewählt)";
				m_ListView1.ParentWindow?.ShowInfo(msg);
			};

			// =================================================================
			// Übrige Widgets (unter den ListViewen).
			// =================================================================
			m_LblColor = new TextLabel("lblColor", "Color");
			this.AddChild(m_LblColor, tableRow++, tableColumn);
			m_ColorComboBox1 = new ColorComboBox("ColorComboBox1");
			// Demo: select the last item — proves EnsureIndexVisible scrolls
			// the open drop-down list to the selected entry.
			m_ColorComboBox1.SelectedIndex = m_ColorComboBox1.Count - 1;
			this.AddChild(m_ColorComboBox1, tableRow++, tableColumn);

			m_LblHatch = new TextLabel("lblHatch", "Hatch Style");
			this.AddChild(m_LblHatch, tableRow++, tableColumn);
			m_HatchStyleComboBox1 = new HatchStyleComboBox("HatchStyleComboBox1");
			m_HatchStyleComboBox1.SetSelectedHatchStyle(HatchStyle.DiagonalCross);
			this.AddChild(m_HatchStyleComboBox1, tableRow++, tableColumn);

			m_LblDash = new TextLabel("lblDash", "Dash Style");
			this.AddChild(m_LblDash, tableRow++, tableColumn);
			m_DashStyleComboBox1 = new DashStyleComboBox("DashStyleComboBox1");
			m_DashStyleComboBox1.SetSelectedDashStyle(DashStyle.DashDot);
			this.AddChild(m_DashStyleComboBox1, tableRow++, tableColumn);

			m_LblFont = new TextLabel("lblFont", "Font");
			this.AddChild(m_LblFont, tableRow++, tableColumn);
			m_FontComboBox1 = new FontComboBox("FontComboBox1");
			// Demo: pick a non-default font to prove the on-demand preview path.
			m_FontComboBox1.SetSelectedFont("Fonts/Lato-Regular.ttf".FixedExpandedPath());
			this.AddChild(m_FontComboBox1, tableRow++, tableColumn);

			m_LblMask = new TextLabel("lblMask", "Mask (Phone / Date)");
			this.AddChild(m_LblMask, tableRow++, tableColumn);
			// A classic phone mask: '(ddd) ddd-dddd'. Literals are pre-filled;
			// '0' = required digit.
			m_MaskedTextBox1 = new MaskedTextBox("MaskedTextBox1", "(000) 000-0000");
			this.AddChild(m_MaskedTextBox1, tableRow++, tableColumn);

			m_LblMaskOpt = new TextLabel("lblMaskOpt", "Mask (Optional, ddmmyy)");
			this.AddChild(m_LblMaskOpt, tableRow++, tableColumn);
			// A date mask with optional day and month: '99/99/00'.
			// '9' = optional digit, '*' = any char is accepted.
			m_MaskedTextBox2 = new MaskedTextBox("MaskedTextBox2", "99/99/00");
			this.AddChild(m_MaskedTextBox2, tableRow++, tableColumn);

			m_LblBlockEdit = new TextLabel("lblBlockEdit", "BlockEditTextBox (Uhr 00:00)");
			this.AddChild(m_LblBlockEdit, tableRow++, tableColumn);
			var b1 = new SummerGUI.BlockEditTextBox.Block { Len = 2, Max = 59, Value = 8 };
			var b2 = new SummerGUI.BlockEditTextBox.Block { Len = 2, Max = 59, Value = 45 };
			m_BlockEdit1 = new SummerGUI.BlockEditTextBox("BlockEditTextBox1", new SummerGUI.BlockEditTextBox.Block[] { b1, b2 }, ':');
			this.AddChild(m_BlockEdit1, tableRow++, tableColumn);

			m_LblDateTime = new TextLabel("lblDateTime", "Date (DateTimePicker)");
			this.AddChild(m_LblDateTime, tableRow++, tableColumn);
			// Masked date box + calendar icon button that opens the MonthCalendar overlay.
			m_DateTimePicker1 = new DateTimePicker("DateTimePicker1");
			m_DateTimePicker1.Value = DateTime.Today;
			this.AddChild(m_DateTimePicker1, tableRow++, tableColumn);

			m_LblTime = new TextLabel("lblTime", "Time (TimePicker)");
			this.AddChild(m_LblTime, tableRow++, tableColumn);
			// Masked time box + up/down spin buttons for hour and minute.
			m_TimePicker1 = new TimePicker("TimePicker1");
			m_TimePicker1.Value = DateTime.Today.AddHours(14).AddMinutes(30);
			this.AddChild(m_TimePicker1, tableRow++, tableColumn);
		}
	}
}