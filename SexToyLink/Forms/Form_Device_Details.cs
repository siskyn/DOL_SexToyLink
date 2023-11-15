using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Buttplug.Client;
using CefSharp.DevTools.Debugger;
using SexToyLink.Classes;

namespace SexToyLink.Forms
{
    public partial class Form_Device_Details : Form
    {
        Controller mycontroller;
        public Form_Device_Details(Controller mycontroller)
        {
            InitializeComponent();
            this.mycontroller = mycontroller;
        }

        private void Form_Device_Details_Load(object sender, EventArgs e)
        {
            populateDevices();
        }

        private void populateDevices()
        {// due to a current bug in buttplug API, DisplayName remains null so we'll temporarily use index instead

            listView_devicesAll.Items.Clear();
            listView_devicesOral.Items.Clear();
            listView_devicesBreasts.Items.Clear();
            listView_devicesGenital.Items.Clear();
            listView_devicesAnal.Items.Clear();

            foreach (var device in mycontroller.devicesAll)
            {
                if (device.Index != null)
                {
                    listView_devicesAll.Items.Add(new ListViewItem(device.Index.ToString()));
                }
                else
                {
                    listView_devicesAll.Items.Add(new ListViewItem("Unnamed \"" + device.Name + "\""));
                }
            }
            foreach (var device in mycontroller.devicesOral)
            {
                listView_devicesOral.Items.Add(new ListViewItem(device.Index.ToString()));
            }
            foreach (var device in mycontroller.devicesBreasts)
            {
                listView_devicesBreasts.Items.Add(new ListViewItem(device.Index.ToString()));
            }
            foreach (var device in mycontroller.devicesAnal)
            {
                listView_devicesAnal.Items.Add(new ListViewItem(device.Index.ToString()));
            }
            foreach (var device in mycontroller.devicesGenital)
            {
                listView_devicesGenital.Items.Add(new ListViewItem(device.Index.ToString()));
            }

            /*
            foreach (var device in mycontroller.devicesAll)
            {
                if (device.DisplayName != null)
                {
                    listView_devicesAll.Items.Add(new ListViewItem(device.DisplayName));
                }
                else
                {
                    listView_devicesAll.Items.Add(new ListViewItem("Unnamed \""+device.Name + "\""));
                }
            }
            foreach (var device in mycontroller.devicesOral)
            {
                listView_devicesOral.Items.Add(new ListViewItem(device.DisplayName));
            }
            foreach (var device in mycontroller.devicesBreasts)
            {
                listView_devicesBreasts.Items.Add(new ListViewItem(device.DisplayName));
            }
            foreach (var device in mycontroller.devicesAnal)
            {
                listView_devicesBreasts.Items.Add(new ListViewItem(device.DisplayName));
            }
            foreach (var device in mycontroller.devicesGenital)
            {
                listView_devicesGenital.Items.Add(new ListViewItem(device.DisplayName));
            }
            */
        }

        private void Form_Device_Details_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void DragAndDrop(ListView target, ListViewItem draggedItem)
        {

            if (draggedItem.Text.Contains("Unnamed"))
            {
                MessageBox.Show("Unnamed devices cannot be categorized. Name it in Intiface Central then reconnect.");
                return;
            }

            bool found = false;
            foreach (ListViewItem item in target.Items)
            {
                if(item.Text == draggedItem.Text)
                {
                    found = true; 
                    break; 
                }
            }
            if (!found)
            {
                target.Items.Add((ListViewItem)draggedItem.Clone());
            }


            // Remove the item from the source ListView
            //listView_devicesAll.Items.Remove(draggedItem);
        }

        private void listView_devicesAll_ItemDrag(object sender, ItemDragEventArgs e)
        {
            listView_devicesAll.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void listView_devicesAny_DragDrop(object sender, DragEventArgs e)
        {
            ListViewItem draggedItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
            DragAndDrop((ListView)sender, draggedItem);
        }

        private void listView_devicesAny_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            List<string> oralTextList = listView_devicesOral.Items
            .Cast<ListViewItem>()
            .Select(item => item.Text)
            .ToList();

            List<string> breastTextList = listView_devicesBreasts.Items
            .Cast<ListViewItem>()
            .Select(item => item.Text)
            .ToList();

            List<string> genitalTextList = listView_devicesGenital.Items
            .Cast<ListViewItem>()
            .Select(item => item.Text)
            .ToList();

            List<string> analTextList = listView_devicesAnal.Items
            .Cast<ListViewItem>()
            .Select(item => item.Text)
            .ToList();

            mycontroller.UpdateDeviceCategories(oralTextList, breastTextList, genitalTextList, analTextList);

            this.Close();
        }

        private void button_Delete_oral_Click(object sender, EventArgs e)
        {
            try
            {
                listView_devicesOral.Items.Remove(listView_devicesOral.SelectedItems[0]);
            }
            catch (Exception ex)
            {
            }
        }

        private void button_Delete_breast_Click(object sender, EventArgs e)
        {
            try
            {
                listView_devicesBreasts.Items.Remove(listView_devicesBreasts.SelectedItems[0]);
            }
            catch (Exception ex)
            {
            }
        }

        private void button_delete_genital_Click(object sender, EventArgs e)
        {
            try
            {
                listView_devicesGenital.Items.Remove(listView_devicesGenital.SelectedItems[0]);
            }
            catch (Exception ex)
            {
            }
        }

        private void button_Delete_anal_Click(object sender, EventArgs e)
        {
            try
            {
                    listView_devicesAnal.Items.Remove(listView_devicesAnal.SelectedItems[0]);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
