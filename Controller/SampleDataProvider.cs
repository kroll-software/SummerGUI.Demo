using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using KS.Foundation;
using SummerGUI;
using SummerGUI.DataGrid;


namespace SummerGUI.Demo
{
	public class Contact : IComparable<Contact>
	{
		public int ID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Company { get; set; }
		public string Zip { get; set; }
		public string City { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public string Web { get; set; }

		public string DisplayName
		{
			get{
				return String.Format ("{0} {1}", FirstName, LastName).Trim();
			}
		}

		public int CompareTo(Contact other)
		{
			return ID.CompareTo (other.ID);
		}

		public override string ToString ()
		{
			return string.Format ("[Contact: {0}, {1}]", LastName, FirstName);
		}
	}

	public class SampleDataProvider : DataProvider
	{

		BalancedOrderStatisticTree<Contact> m_Contacts;
		public BalancedOrderStatisticTree<Contact> Contacts 
		{ 
			get {
				return m_Contacts;
			}
		}
		
		public SampleDataProvider (IController parent, IDataProviderOwner owner)
			: base(parent, owner)
		{			
		}			

		public override void InitializeColumns ()
		{
			this.ColumnManager.Columns.Add ("ID", "ID", 80);
			this.ColumnManager.Columns.Add ("FirstName", "First Name", 120);
			this.ColumnManager.Columns.Add ("LastName", "Last Name", 120);
			this.ColumnManager.Columns.Add ("Company", "Company", 250);
			this.ColumnManager.Columns.Add ("Zip", "Zip Code", 80);
			this.ColumnManager.Columns.Add ("City", "City", 160);
			this.ColumnManager.Columns.Add ("Address", "Address", 250);
			this.ColumnManager.Columns.Add ("Phone", "Phone", 140);
			this.ColumnManager.Columns.Add ("Email", "Email", 200);
			this.ColumnManager.Columns.Add ("Web", "Web", 250);

			this.ColumnManager.Columns.First ().TextAlignment = Alignment.Far;
			this.ColumnManager.InitializeColumns ();
		}

		public override void InitializeRows ()
		{
			// set the generic comparer to out data-list, so that it can be sorted 
			// by the user, by clicking on a column-header

			var GenericComparer = new GenericSortComparer<Contact> (this.ColumnManager.Columns.ToArray(), "DisplayName");
			
			m_Contacts = new BalancedOrderStatisticTree<Contact>(GenericComparer);

			// *** Open CSV file..

			string csvPath = (Strings.ApplicationPath (true) + "SampleData/50000.csv").FixPathForPlatform();

			CSVReader.RowQueue.Start ();
			System.Threading.Tasks.Task.Factory.StartNew (() => {
				//CSVReader.LoadFile (csvPath, ";", true, "\"", null, System.Text.Encoding.UTF8, true);
				CSVReader.LoadFile (csvPath, null, true, null, null, null, true);
			});

			System.Threading.Tasks.Task.Factory.StartNew (() => {
				DataRow row = null;
				int id = 1;
				do {
					if (CSVReader.RowQueue.Dequeue (out row) && row != null) {						
						Contacts.Add (new Contact {
							ID = id++,
							FirstName = row ["FirstName"].SafeString (),
							LastName = row ["LastName"].SafeString (),
							Company = row ["Company"].SafeString (),
							Address = row ["Address"].SafeString (),
							Zip = row ["ZIP"].SafeString (),
							City = row ["City"].SafeString (),
							Phone = row ["Phone"].SafeString (),
							Email = row ["Email"].SafeString (),
							Web = row ["Web"].SafeString (),
						});
					}
				} while (row != null || CSVReader.RowQueue.IsStarted);
			}).ContinueWith ((t) => {
				this.RowManager.RowCount = Contacts.Count;
				OnDataLoaded();
				//ApplySort();
			});
		}

		public void AddContact(Contact contact)
		{
			if (contact == null)
				return;
			Contacts.Add (contact);
			contact.ID = Contacts.Count;
			RowManager.RowCount = Contacts.Count;
			RowManager.MoveLast ();
		}

		public void DeleteContact (int index)
		{
			if (index < 0 || index >= Contacts.Count) {
				this.LogWarning ("DeleteContact: Index out of bounds");
				return;
			}

			if (Contacts.Remove (Contacts [index])) {
				RowManager.RowCount = Contacts.Count;
			}
		}

		public override object GetValue (int row, int col)
		{
			if (row < 0 || row > Contacts.Count -1)
				return null;
			var contact = Contacts [row];
			if (contact == null)
				return null;

			switch (col) {
			case 0:
				return contact.ID;
			case 1:
				return contact.FirstName;
			case 2:
				return contact.LastName;
			case 3:
				return contact.Company;
			case 4:
				return contact.Zip;
			case 5:
				return contact.City;
			case 6:
				return contact.Address;
			case 7:
				return contact.Phone;
			case 8:
				return contact.Email;
			case 9:
				return contact.Web;
			default:
				return null;
			}
		}


		public CancellationTokenSource TokenSource { get; private set; }
		public void CancelSort()
		{
			if (TokenSource != null) {
				try {
					if (!TokenSource.IsCancellationRequested) {
						TokenSource.Cancel (true);
						//Root.SendMessage (this, "ClearStatus");
					}
					TokenSource.Dispose ();	
				} catch (Exception ex) {
					ex.LogWarning ();
				}
			}
		}

		public override void ApplySort()
		{
			CancelSort();
			TokenSource = new CancellationTokenSource();
			Root.SendMessage(this, "ShowStatus", false, "Sorting Contacts..", true);

			try {
				Task<BalancedOrderStatisticTree<Contact>>.Factory.StartNew(() => 					
					new BalancedOrderStatisticTree<Contact>(Contacts, Contacts.Comparer)
				, TokenSource.Token, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default)
					.ContinueWith((t) => {						
						if (t.Status == TaskStatus.RanToCompletion && t.Result != null) {
							Concurrency.LockFreeUpdate(ref m_Contacts, t.Result);
						}
						Root.SendMessage (this, "ClearStatus");
					});

			} catch (Exception ex) {
				ex.LogWarning ();
				Root.SendMessage (this, "ClearStatus");			
			}
		}

		protected override void CleanupManagedResources ()
		{
			CancelSort ();
			base.CleanupManagedResources ();
		}
	}
}

