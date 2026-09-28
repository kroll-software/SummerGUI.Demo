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
		TextLabel m_LblFont;
		FontComboBox m_FontComboBox1;
		TextLabel m_LblMask;
		MaskedTextBox m_MaskedTextBox1;
		TextLabel m_LblMaskOpt;
		MaskedTextBox m_MaskedTextBox2;
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
