using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using SummerGUI;
using KS.Foundation;

namespace SummerGUI.Demo
{
	public class CommonControlsSampleContainer : TableLayoutContainer
	{
		CaptionLabel m_Label1;
		CheckBox m_CheckBox1;
		CheckBox m_CheckBox3;
		ToggleCheckBox m_ToggleCheckBox;

		CaptionLabel m_Label2;
		RadioButton m_RadioButton1;
		RadioButton m_RadioButton2;
		RadioButton m_RadioButton3;

		CaptionLabel m_Label3;
		ProgressBar m_ProgressBar;

		TableLayoutContainer m_CircleSliderSubContainer;
		CircleSlider m_CircleSlider1;
		CircleSlider m_CircleSlider2;

		Button cmdDefaultButton;
		Button cmdShowInfo;
		Button cmdShowError;
		Button cmdShowWarning;
		Button cmdShowQuestion;

		TextBox m_TextBox1;
		CheckBox m_ShowPasswordChar;
		NumberTextBox m_NumberTextBox1;
		ButtonTextBox m_ButtonTextBox1;

		ComboListBox m_ComboListBox1;
		ComboBox m_ComboBox1;

		public CommonControlsSampleContainer ()
			: base("CommonControlsSampleContainer")
		{
			Padding = new Padding (16);
			CellPadding = new SizeF (16, 16);
			InitControls ();
			CollapsibleColumnsWidth = 420;
		}			
			
		private void InitControls()
		{	
			int tableRow = 0;
			int tableColumn = 0;

			/***
			FontAlignmentTestWidget test = new FontAlignmentTestWidget ();
			this.AddChild (test, tableRow++, tableColumn);
			***/

			m_Label1 = new CaptionLabel ("label1");
			//m_Label1.Styles.GetStyle (WidgetStates.Default).BackColorBrush.Color = SolarizedColors.Base2;
			m_Label1.Style.BackColorBrush.Color = Theme.Colors.Base2;
			m_Label1.Dock = Docking.Fill;
			m_Label1.Text = "Check Boxes".ToUpper();
			this.AddChild (m_Label1, tableRow++, tableColumn);

			m_CheckBox1 = new CheckBox ("checkbox1", "CheckBox 1");
			this.AddChild (m_CheckBox1, tableRow++, tableColumn);

			m_CheckBox3 = new CheckBox ("checkbox3", "CheckBox 3 (disabled)");
			m_CheckBox3.Enabled = false;
			this.AddChild (m_CheckBox3, tableRow++, tableColumn);

			m_ToggleCheckBox = new ToggleCheckBox ("togglecheckbox", "Option 1");
			m_ToggleCheckBox.Checked = true;
			this.AddChild (m_ToggleCheckBox, tableRow++, tableColumn);

			m_Label2 = new CaptionLabel ("label2");
			m_Label2.Style.BackColorBrush.Color = Theme.Colors.Base2;
			m_Label2.Dock = Docking.Fill;
			m_Label2.Text = "Radio Buttons".ToUpper();
			this.AddChild (m_Label2, tableRow++, tableColumn);

			m_RadioButton1 = new RadioButton ("radiobutton1", "RadioButton 1");
			m_RadioButton1.Checked = true;
			this.AddChild (m_RadioButton1, tableRow++, tableColumn);

			m_RadioButton2 = new RadioButton ("radiobutton2", "RadioButton 2");
			this.AddChild (m_RadioButton2, tableRow++, tableColumn);

			m_RadioButton3 = new RadioButton ("radiobutton3", "RadioButton 3");
			this.AddChild (m_RadioButton3, tableRow++, tableColumn);

			m_Label3 = new CaptionLabel ("label3");
			m_Label3.Style.BackColorBrush.Color = Theme.Colors.Base2;
			m_Label3.Dock = Docking.Fill;
			m_Label3.Text = "Progress Bars".ToUpper();
			this.AddChild (m_Label3, tableRow++, tableColumn);

			m_ProgressBar = new ProgressBar ("ProgressBar1");
			m_ProgressBar.Value = 0.625f;
			m_ProgressBar.Tooltip = "Click to animate..";
			m_ProgressBar.Click += delegate {
				ParentWindow.Animator.AddAnimation(m_ProgressBar, "Value", 0, 1, 5);
			};
			m_ProgressBar.AnimationCompleted += delegate {
				m_ProgressBar.Value = 0.625f;
				(ParentWindow as ApplicationWindow).ShowNotification("Progressbar animation completed, state was reset to it's former value.", ColorContexts.Information);
			};
			this.AddChild (m_ProgressBar, tableRow++, tableColumn);


			/*** ***/
			// Circle Sliders in a Sub-Container
			m_CircleSliderSubContainer = new TableLayoutContainer ("m_CircleSliderSubContainer");
			m_CircleSliderSubContainer.Margin = new Padding (0, 0, 0, 16);

			m_CircleSlider1 = new CircleSlider ("CircleSlider1", ColorContexts.Information);
			m_CircleSlider1.Value = 0.75f;
			m_CircleSlider1.Tooltip = "Click to animate..";
			m_CircleSliderSubContainer.AddChild (m_CircleSlider1, 0, 0);
			m_CircleSlider1.Click += delegate {
				ParentWindow.Animator.AddAnimation(m_CircleSlider1, "Value", 0, 1, 5);
			};
			m_CircleSlider1.AnimationCompleted += delegate {
				m_CircleSlider1.Value = 0.75f;
				(ParentWindow as ApplicationWindow).ShowNotification("The animation was successfully completed.", ColorContexts.Success);
			};

			m_CircleSlider2 = new CircleSlider ("CircleSlider1", ColorContexts.Information);
			m_CircleSlider2.Value = 0.333f;
			m_CircleSlider2.CustomColor = Theme.Colors.Magenta;
			m_CircleSlider2.Tooltip = "Drag up and down\nto change the value.";
			m_CircleSliderSubContainer.AddChild (m_CircleSlider2, 0, 1);

			this.AddChild(m_CircleSliderSubContainer, tableRow++, tableColumn);


			// >>> New Column >>>

			tableRow = 0;
			tableColumn = 1;

			cmdDefaultButton = new Button ("cmdDefaultButton", "Default Button", ColorContexts.Default);
			cmdDefaultButton.Click += delegate {
				ParentWindow.ShowInfo("You pressed the default button. Great.");	
			};				
			this.AddChild (cmdDefaultButton, tableRow++, tableColumn);

			cmdShowInfo = new Button ("cmdShowInfo", "Info MessageBox", (char)FontAwesomeIcons.fa_info_circle, ColorContexts.Information);
			cmdShowInfo.Click += delegate {
				ParentWindow.ShowInfo("This is an info.");
			};				
			this.AddChild (cmdShowInfo, tableRow++, tableColumn);

			cmdShowWarning = new Button ("cmdShowWarning", "Warning MessageBox", (char)FontAwesomeIcons.fa_warning, ColorContexts.Warning);
			cmdShowWarning.Click += delegate {
				ParentWindow.ShowWarning("This is a warning.");	
			};				
			this.AddChild (cmdShowWarning, tableRow++, tableColumn);

			cmdShowError = new Button ("cmdShowError", "Error MessageBox", (char)FontAwesomeIcons.fa_times_circle, ColorContexts.Danger);
			cmdShowError.Click += delegate {
				try {
					throw new Exception ("This is a sample error.");
				} catch (Exception ex) {
					string errMsg = ex.Message + "\n" + Concurrency.GetStackTrace ();
					ParentWindow.ShowError (errMsg);
				}
			};				
			this.AddChild (cmdShowError, tableRow++, tableColumn);

			cmdShowQuestion = new Button ("cmdShowQuestion", "Question MessageBox", (char)FontAwesomeIcons.fa_question_circle, ColorContexts.Question);
			cmdShowQuestion.Click += delegate {
				ParentWindow.ShowQuestion("This is a question. Are you sure ?");
			};				
			this.AddChild (cmdShowQuestion, tableRow++, tableColumn);

			m_TextBox1 = new TextBox ("TextBox1");
			m_TextBox1.Text = "Abcd Efg Hijk";
			this.AddChild (m_TextBox1, tableRow++, tableColumn);

			m_ShowPasswordChar = new CheckBox ("ShowPasswordChar", "Password visible");
			m_ShowPasswordChar.Checked = true;
			m_ShowPasswordChar.CheckedChanged += (object sender, EventArgs eCheckedChanged) =>
				m_TextBox1.PasswordChar = m_ShowPasswordChar.Checked ? (char)0 : TextBox.DefaultPasswortChar;
			this.AddChild (m_ShowPasswordChar, tableRow++, tableColumn);

			m_NumberTextBox1 = new NumberTextBox ("NumberTextBox1");
			m_NumberTextBox1.Value = 123.45m;
			this.AddChild (m_NumberTextBox1, tableRow++, tableColumn);

			m_ButtonTextBox1 = new ButtonTextBox ("ButtonTextBox1", (char)FontAwesomeIcons.fa_send);
			m_ButtonTextBox1.Button.Click += delegate {
				(ParentWindow as ApplicationWindow).ShowNotification("Your email has been sent.", ColorContexts.Success);
			};
			this.AddChild (m_ButtonTextBox1, tableRow++, tableColumn);

			m_ComboListBox1 = new ComboListBox ("ComboListBox1");
			m_ComboListBox1.Items.Add ("Apple", 1);
			m_ComboListBox1.Items.Add ("Orange", 2);
			m_ComboListBox1.Items.Add ("Banana", 3);
			m_ComboListBox1.Items.Add ("Cherry", 4);
			m_ComboListBox1.Items.Add ("Pineapple", 5);
			m_ComboListBox1.SelectedIndex = 0;
			this.AddChild (m_ComboListBox1, tableRow++, tableColumn);

			m_ComboBox1 = new ComboBox ("ComboBox1");
			m_ComboBox1.Items.Add ("Apple", 1);
			m_ComboBox1.Items.Add ("Orange", 2);
			m_ComboBox1.Items.Add ("Banana", 3);
			m_ComboBox1.Items.Add ("Cherry", 4);
			m_ComboBox1.Items.Add ("Pineapple", 5);
			m_ComboBox1.SelectedIndex = 0;
			this.AddChild (m_ComboBox1, tableRow++, tableColumn);
		}
	}
}

