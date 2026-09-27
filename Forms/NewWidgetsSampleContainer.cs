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
	/// just the new <see cref="ColorComboBox"/>.
	/// </summary>
	public class NewWidgetsSampleContainer : TableLayoutContainer
	{
		CaptionLabel m_Label1;
		TextLabel m_LblColor;
		ColorComboBox m_ColorComboBox1;
		TextLabel m_LblHatch;
		HatchStyleComboBox m_HatchStyleComboBox1;
		TextLabel m_LblDash;
		DashStyleComboBox m_DashStyleComboBox1;

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
		}
	}
}
