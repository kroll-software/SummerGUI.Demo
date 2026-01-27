using System;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using OpenTK.Input;
using KS.Foundation;
using SummerGUI;
using SummerGUI.DataGrid;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework; 

namespace SummerGUI.Demo
{
	public class ContactForm : ChildFormWindow
	{
		public Contact Contact { get; private set; }
		public TableLayoutContainer Table { get; private set; }
		public NotificationPanel Notifications { get; private set; }
		public ButtonContainer ButtonContainer { get; private set; }

		public ContactForm (SummerGUIWindow parent, Contact contact)
			: base ("ContactForm", contact == null ? "New Contact" : "Edit Contact", 
			       (520 * parent.ScaleFactor).Ceil (), (364 * parent.ScaleFactor).Ceil (), parent, true)
		{
			Notifications = AddChild (new NotificationPanel ("notifications"));
			Contact = contact;
			BackColor = Theme.Colors.Base2;

			InitButtons ();
			InitTable ();
			GetData ();
			Table.Children ["FirstName"].TabInto();
			RemoveBorders ();
		}		
			
		protected override void OnInitFonts ()
		{
			// We just need the AlphaNumeric glyphs from this font.
			// We're safe and add the OnDemand option, which allows later loading on demand.
			FontManager.Manager.GetConfig (CommonFontTags.Small).OnDemand = true;
			base.OnInitFonts ();
		}

		protected override void OnInitCursors ()
		{
			// don't init any new cursors
		}

		protected void RemoveBorders()
		{
			Table.Children.OfType<Widget> ().SelectMany (w => w.Styles)
				.Where(style => style != null).ForEach (style => style.Border = 0);
		}
			
		protected virtual void InitButtons()
		{
			// panel
			ButtonContainer = Controls.AddChild (new ButtonContainer("buttoncontainer"));
			ButtonContainer.BackColor = Theme.Colors.Base01;

			DefaultButton btnOK = ButtonContainer.AddChild (new DefaultButton ("okbutton", "&OK"));
			btnOK.MinSize = new SizeF(96, 0);
			btnOK.Click += (sender, eOK) => OnOK();
			btnOK.MakeDefaultButton ();	// handle Enter-Key even when not focused
			btnOK.Update ();			

			//DefaultButton btnCancel = ButtonContainer.AddChild (new DefaultButton ("cancelbutton", "&Cancel"));
			DefaultButton btnCancel = ButtonContainer.AddChild (new DefaultButton ("cancelbutton", "&Cancel"));
			btnCancel.MinSize = new SizeF(96, 0);
			btnCancel.Click += (sender, eCancel) => OnCancel();
			btnCancel.Update ();			

			//btnOK.Selected = true;	// make this the default button.
		}

		ScrollableContainer MainContainer;

		protected virtual void InitTable()
		{
			MainContainer = Controls.AddChild (new ScrollableContainer ("main", Docking.Fill, new SilverPanelWidgetStyle()));
			MainContainer.AutoScroll = true;
			MainContainer.ScrollBars = ScrollBars.Vertical;

			Table = MainContainer.AddChild (new TableLayoutContainer ("table"));

			// this should do the same:
			Table.Padding = new Padding(12, 6);
			Table.CellPadding = new SizeF (12, 6);
			Table.CollapsibleColumnsWidth = 420;

			int row = 0;

			/***
			Table.AddChild (new TextLabel ("lblGender", "Gender"), row, 0, 1, 2);
			row++;
			Table.AddChild (new ComboListBox ("Gender"), row, 0, 1, 2).CastTo<ComboListBox>().Do (cbo => {
				cbo.Items.Add("Male", 1);
				cbo.Items.Add("Female", 2);
				cbo.Items.Add("Not sure", 0);
				cbo.SelectedIndex = 0;
			});
			row++;
			***/

			Table.AddChild (new TextLabel ("lblFirstName", "First Name"), row, 0, 1, 2);
			Table.AddChild (new TextBox ("FirstName"), row + 1, 0, 1, 2);

			Table.AddChild (new TextLabel ("lblLastName", "Last Name"), row, 2);
			Table.AddChild (new TextBox ("LastName"), row + 1, 2);

			row++;
			row++;
			Table.AddChild (new TextLabel ("lblCompany", "Company"), row, 0, 1, 3);
			row++;
			Table.AddChild (new TextBox ("Company"), row, 0, 1, 3);

			row++;
			Table.AddChild (new TextLabel ("lblZip", "Zip"), row, 0);
			Table.AddChild (new TextBox ("Zip"), row + 1, 0);

			Table.AddChild (new TextLabel ("lblCity", "City"), row, 1);
			Table.AddChild (new TextBox ("City"), row + 1, 1);

			Table.AddChild (new TextLabel ("lblAddress", "Street Address"), row, 2);
			Table.AddChild (new TextBox ("Address"), row + 1, 2);

			row++;
			row++;

			Table.AddChild (new TextLabel ("lblPhone", "Phone"), row, 0, 1, 2);
			Table.AddChild (new ButtonTextBox ("Phone", (char)FontAwesomeIcons.fa_phone, null, ColorContexts.Success), row + 1, 0, 1, 2);

			Table.AddChild (new TextLabel ("lblEmail", "Email"), row, 2);
			Table.AddChild (new ButtonTextBox ("Email", (char)FontAwesomeIcons.fa_envelope_o, null, ColorContexts.Success), row + 1, 2);

			row++;
			row++;
			Table.AddChild (new TextLabel ("lblWeb", "Web"), row, 0, 1, 3);
			row++;
			Table.AddChild (new ButtonTextBox ("Web", (char)FontAwesomeIcons.fa_chain, null, ColorContexts.Default), row, 0, 1, 3);

			LayoutDirtyFlag = true;
			Table.AfterLayout += (sender, e) => UpdateTableSize();
		}			

		private bool LayoutDirtyFlag = false;
		private void UpdateTableSize()
		{
			if (!LayoutDirtyFlag || Table == null)
				return;

			LayoutDirtyFlag = false;

			TextBox tbZip = Table.ChildByName("Zip") as TextBox;
			if (tbZip != null) {				
				Table.Columns [0].SizeMode = TableSizeModes.Fixed;
				float zipWidth = tbZip.Font.Measure (new string ('8', 8)).Width + tbZip.Padding.Width;
				Table.Columns [0].Width = zipWidth;

				Table.Columns [1].SizeMode = TableSizeModes.Fixed;
				Table.Columns [1].Width = ((this.Width - Table.Padding.Width) / 2) - zipWidth - (Table.CellPadding.Width * 2);
			}
		}

        protected override void OnResize(ResizeEventArgs e)
        {
			LayoutDirtyFlag = true;
            base.OnResize(e);
			Invalidate ();
        }		

		protected virtual void GetData()
		{
			if (Contact == null)
				return;
			Table.Children.ForEach (child => {
				if (ReflectionUtils.HasProperty(Contact.GetType (), child.Name))
					ReflectionUtils.SetPropertyValue(child, "Text", ReflectionUtils.GetPropertyValue(Contact, child.Name));
			});
		}

		protected virtual void UpdateData()
		{
			if (Contact == null)
				Contact = new Contact ();
			Table.Children.ForEach (child => {
				if (ReflectionUtils.HasProperty(Contact.GetType (), child.Name))
					ReflectionUtils.SetPropertyValue(Contact, child.Name, ReflectionUtils.GetPropertyValue(child, "Text"));
			});
		}			

		public bool Validate()
		{
			string firstName = (Table.Children ["FirstName"] as TextBox).Text;
			string lastName = (Table.Children ["LastName"] as TextBox).Text;

			if ((firstName + lastName + String.Empty).Trim().Length == 0) {
				Notifications.AddNotification ("Please provide at least a first or a last name.", ColorContexts.Warning);
				return false;
			}
			return true;
		}

		/**** ***/
		protected override void OnKeyDown (KeyboardKeyEventArgs e)
		{
			switch (e.Key) {
			case Keys.Escape:
				OnCancel ();
				return;
			case Keys.Enter:
				if (ButtonContainer.OnKeyDown (e))
					return;
				//else
				//	OnOK ();
				break;
			}				

			base.OnKeyDown (e);
		}

		public override void OnOK ()
		{			
			if (!Validate())
				return;

			UpdateData ();
			base.OnOK ();
		}
	}
}

