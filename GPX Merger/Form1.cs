using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GPX_Merger
{
	public partial class Form1 : Form
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public Form1()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Load data to files list
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnLoadFiles_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				foreach (var fileName in openFileDialog1.FileNames)
				{
					listFiles.Items.Add(fileName);
				}
			}
		}

		/// <summary>
		/// Remove items from files list
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnRemove_Click(object sender, EventArgs e)
		{
			var selectedItems = new ListBox.SelectedObjectCollection(listFiles);
			selectedItems = listFiles.SelectedItems;

			if (SomethingIsSelected())
			{
				var totalItems = selectedItems.Count - 1;
				for (var i = totalItems; i >= 0; i--)
					listFiles.Items.Remove(selectedItems[i]);
			}
		}


		public void moveItems(bool toDown = false)
		{
			if (SomethingIsSelected())
			{
				var limit = toDown ? listFiles.Items.Count -1 : 0;
				var direction = toDown ? 1 : -1;

				var temp = string.Empty;
				var indices = new List<int>();

				foreach (var index in listFiles.SelectedIndices)
					indices.Add(Convert.ToInt32(index));

				var current = toDown ? listFiles.Items.Count + 1 : -1;

				foreach (var index in indices)
				{
					current = index;
					if ((current > limit && !toDown) | (current < limit && toDown))
					{
						temp = listFiles.Items[current + direction].ToString();
						listFiles.Items[current + direction] = listFiles.Items[current];
						listFiles.Items[current] = temp;
					}
				}

				listFiles.ClearSelected();
				foreach (var index in indices)
				{
					current = index;
					if ((current == limit && !toDown) || (current >= limit && toDown))
						current = toDown ? listFiles.Items.Count -2 : 1;
					listFiles.SetSelected(Convert.ToInt32(current) + direction, true);
				}
			}
		}

		private bool SomethingIsSelected()
		{
			return listFiles.SelectedItems.Count > 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnMerge_Click(object sender, EventArgs e)
		{
			if (listFiles.Items.Count == 0)
			{
				MessageBox.Show("Nothing to merge", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				var messageBoxIcon = MessageBoxIcon.Error;

				var items = new List<string>();
				foreach (var item in listFiles.Items)
					items.Add(item.ToString());

				var message = classes.ClassXml.createXmlDoc(items,
					saveFileDialog1.FileName,
					chkMinify.Checked);

				if (string.IsNullOrEmpty(message))
				{
					message = "Merge Complete";
					messageBoxIcon = MessageBoxIcon.Information;
				}
				else
				{

				}
				MessageBox.Show(message, Text, MessageBoxButtons.OK, messageBoxIcon);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			txtPath.Text = saveFileDialog1.FileName;
		}

		private void btnDown_Click(object sender, EventArgs e)
		{
			moveItems(true);
		}

		private void btnUp_Click(object sender, EventArgs e)
		{
			moveItems();
		}

	}
}