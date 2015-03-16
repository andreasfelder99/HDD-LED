using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Management.Instrumentation;
using System.Collections.Specialized;
using System.Threading;
namespace HDDLED
{
    public partial class Form1 : Form
    {
        NotifyIcon hddLedIcon;
        Icon aktivIcon;
        Icon passivIcon;
        Thread hddLedWorker;
        Thread hddLedWorker2;

        #region  Ablauf
        public Form1()
        {
            InitializeComponent();
            //Lade Icons in die Objekte
            aktivIcon = new Icon("HDD_Busy.ico");
            passivIcon = new Icon("HDD_Idle.ico");

            //Notify erstellen
            hddLedIcon = new NotifyIcon();
            hddLedIcon.Icon = passivIcon;
            hddLedIcon.Visible = true;

            //Menu Items erstellen
            MenuItem info = new MenuItem("HDD LED by Andi");
            MenuItem hdd1 = new MenuItem("HDD 1");
            MenuItem hdd2 = new MenuItem("HDD 2");
            MenuItem quit = new MenuItem("Schliessen");
            ContextMenu contextMenu = new ContextMenu();
            //Zum kontext hinzufügen
            contextMenu.MenuItems.Add(info);
            contextMenu.MenuItems.Add(quit);
            contextMenu.MenuItems.Add(hdd1);
            contextMenu.MenuItems.Add(hdd2);
            //Zum Symbol hinzufügen
            hddLedIcon.ContextMenu = contextMenu;
            //Aktion Erstellen
            quit.Click += quit_Click;
            hdd1.Click += hdd1_Click;
            hdd2.Click += hdd2_Click;


            //Verstecke Form
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            //Starte Thread
           
        }

        void hdd2_Click(object sender, EventArgs e)
        {
            hddLedWorker2 = new Thread(new ThreadStart(HDDActivityThread2));
            hddLedWorker2.Start();
        }

        void hdd1_Click(object sender, EventArgs e)
        {
            hddLedWorker = new Thread(new ThreadStart(HDDActivityThread));
            hddLedWorker.Start();
        }




        //Beenden funktion
        void quit_Click(object sender, EventArgs e)
        {
            hddLedIcon.Dispose();
            this.Close();
        }
        #endregion
        #region Threads
        public void HDDActivityThread()
        {
            ManagementClass driveDataClass = new ManagementClass("Win32_PerfFormattedData_PerfDisk_PhysicalDisk");
            try
            {
                

                while (true)
                {
                    ManagementObjectCollection driveDataClassCollection = driveDataClass.GetInstances();
                    foreach( ManagementObject obj in driveDataClassCollection)
                    {
                        if( obj["Name"].ToString() == "0 C:")
                        {
                            if ( Convert.ToInt64(obj["DiskBytesPersec"]) > 0)
                            {
                                //Zeig beschäftigt icon
                                hddLedIcon.Icon = aktivIcon;
                            }
                            else
                            {
                               //Zeig idle icon
                                hddLedIcon.Icon = passivIcon;
                            }
                        }
                        
                    }

                    Thread.Sleep(100);
                }
            }
            catch (ThreadAbortException tbe)
            {
                driveDataClass.Dispose();
            }
        }

        //thread 2
        public void HDDActivityThread2()
        {
            ManagementClass driveDataClass = new ManagementClass("Win32_PerfFormattedData_PerfDisk_PhysicalDisk");
            try
            {


                while (true)
                {
                    ManagementObjectCollection driveDataClassCollection = driveDataClass.GetInstances();
                    foreach (ManagementObject obj in driveDataClassCollection)
                    {
                        if (obj["Name"].ToString() == "1 D:")
                        {
                            if (Convert.ToInt64(obj["DiskBytesPersec"]) > 0)
                            {
                                //Zeig beschäftigt icon
                                hddLedIcon.Icon = aktivIcon;
                            }
                            else
                            {
                                //Zeig idle icon
                                hddLedIcon.Icon = passivIcon;
                            }
                        }

                    }

                    Thread.Sleep(100);
                }
            }
            catch (ThreadAbortException tbe)
            {
                driveDataClass.Dispose();
            }
        }

        #endregion

    }
}
