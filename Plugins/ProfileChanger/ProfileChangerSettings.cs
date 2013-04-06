using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.Windows.Media;
using System.Net;
using System.Globalization;

using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Plugins;
using Styx.Pathing;

namespace ProfileChanger
{
    class ProfileChangerSettings
    {

        public LocalPlayer Me = StyxWoW.Me;
		string File = "Plugins\\ProfileChanger\\config\\";
        public ProfileChangerSettings()
        {
            if (Me != null)
                try
                {
                    Load();
                }
                catch (Exception e)
                {
                    Logging.Write(Colors.Red, e.Message);
                }
        }

		public bool Active1 = false;
        public bool Active2 = false;
        public bool Active3 = false;
        public bool Active4 = false;
        public bool Active5 = false;
        public bool Active6 = false;
        public bool Active7 = false;
        public bool Active8 = false;
        public bool Active9 = false;
        public bool Active10 = false;
        public bool Active11 = false;
        public bool Active12 = false;

        public string Profile1 = "";
        public string Profile2 = "";
        public string Profile3 = "";
        public string Profile4 = "";
        public string Profile5 = "";
        public string Profile6 = "";
        public string Profile7 = "";
        public string Profile8 = "";
        public string Profile9 = "";

        public string Profile10 = "";
        public string Profile11 = "";
        public string Profile12 = "";

        public string dirName = "C:\\";

        public string DGProfile = "C:\\dailygrind.xml";

        
	
		// -------------- Load ConfigFile ---------------
        public void Load()
        {

            //    XmlTextReader reader;
            XmlDocument xml = new XmlDocument();
            XmlNode xvar;

            string sPath = Process.GetCurrentProcess().MainModule.FileName;
            sPath = Path.GetDirectoryName(sPath);
            sPath = Path.Combine(sPath, File);

            if (!Directory.Exists(sPath))
            {
                Logging.Write(Colors.LightSkyBlue, "Profile Changer: Creating config directory");
                Directory.CreateDirectory(sPath);
            }

            sPath = Path.Combine(sPath, "ProfileChanger.config");

            //Logging.Write(Colors.LightSkyBlue, "Profile Changer: Loading config file: {0}", sPath);
            System.IO.FileStream fs = new System.IO.FileStream(@sPath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite);
            try
            {
                xml.Load(fs);
                fs.Close();
            }
            catch (Exception e)
            {
                Logging.Write(Colors.Red, e.Message);
                Logging.Write(Colors.Red, "Profile Changer: Continuing with Default Config Values");
                fs.Close();
                return;
            }

            //            xml = new XmlDocument();

            try
            {
                //                xml.Load(reader);
                if (xml == null)
                    return;
                // Load Variables
                xvar = xml.SelectSingleNode("//ProfileChanger/Active1");
                if (xvar != null)
                {
                    Active1 = Convert.ToBoolean(xvar.InnerText);
                    //Logging.WriteDebug("Profile Changer Load: " + xvar.Name + "=" + Active1.ToString());
                }
				
				xvar = xml.SelectSingleNode("//ProfileChanger/Active2");
                if (xvar != null)
                {
                    Active2 = Convert.ToBoolean(xvar.InnerText);
                    //Logging.WriteDebug("Profile Changer Load: " + xvar.Name + "=" + Active2.ToString());
                }

				xvar = xml.SelectSingleNode("//ProfileChanger/Active3");
                if (xvar != null)
                {
                    Active3 = Convert.ToBoolean(xvar.InnerText);
                    //Logging.WriteDebug("Profile Changer Load: " + xvar.Name + "=" + Active3.ToString());
                }
				
				xvar = xml.SelectSingleNode("//ProfileChanger/Active4");
                if (xvar != null)
                {
                    Active4 = Convert.ToBoolean(xvar.InnerText);
                    //Logging.WriteDebug("Profile Changer Load: " + xvar.Name + "=" + Active4.ToString());
                }

				xvar = xml.SelectSingleNode("//ProfileChanger/Active5");
                if (xvar != null)
                {
                    Active5 = Convert.ToBoolean(xvar.InnerText);
                    //Logging.WriteDebug("Profile Changer Load: " + xvar.Name + "=" + Active5.ToString());
                }
				
				xvar = xml.SelectSingleNode("//ProfileChanger/Active6");
                if (xvar != null)
                {
                    Active6 = Convert.ToBoolean(xvar.InnerText);
                    //Logging.WriteDebug("Profile Changer Load: " + xvar.Name + "=" + Active6.ToString());
                }

                xvar = xml.SelectSingleNode("//ProfileChanger/Active7");
                if (xvar != null)
                {
                    Active7 = Convert.ToBoolean(xvar.InnerText);
                    //Logging.WriteDebug("Profile Changer Load: " + xvar.Name + "=" + Active7.ToString());
                }

                xvar = xml.SelectSingleNode("//ProfileChanger/Active8");
                if (xvar != null)
                {
                    Active8 = Convert.ToBoolean(xvar.InnerText);
                    //Logging.WriteDebug("Profile Changer Load: " + xvar.Name + "=" + Active8.ToString());
                }

                xvar = xml.SelectSingleNode("//ProfileChanger/Active9");
                if (xvar != null)
                {
                    Active9 = Convert.ToBoolean(xvar.InnerText);
                    //Logging.WriteDebug("Profile Changer Load: " + xvar.Name + "=" + Active9.ToString());
                }

                xvar = xml.SelectSingleNode("//ProfileChanger/Active10");
                if (xvar != null)
                {
                    Active10 = Convert.ToBoolean(xvar.InnerText);
                    //Logging.WriteDebug("Profile Changer Load: " + xvar.Name + "=" + Active10.ToString());
                }

                xvar = xml.SelectSingleNode("//ProfileChanger/Active11");
                if (xvar != null)
                {
                    Active11 = Convert.ToBoolean(xvar.InnerText);
                    //Logging.WriteDebug("Profile Changer Load: " + xvar.Name + "=" + Active11.ToString());
                }

                xvar = xml.SelectSingleNode("//ProfileChanger/Active12");
                if (xvar != null)
                {
                    Active12 = Convert.ToBoolean(xvar.InnerText);
                    //Logging.WriteDebug("Profile Changer Load: " + xvar.Name + "=" + Active12.ToString());
                }

                xvar = xml.SelectSingleNode("//ProfileChanger/DGProfile");
                if (xvar != null)
                {
                    DGProfile = Convert.ToString(xvar.InnerText);
                    //Logging.WriteDebug("Profile Changer Load: " + xvar.Name + "=" + Profile1.ToString());
                }

			}
            catch (Exception e)
            {
                Logging.Write(Colors.Red, "Profile Changer: PROJECTE EXCEPTION, STACK=" + e.StackTrace);
                Logging.Write(Colors.Red, "Profile Changer: PROJECTE EXCEPTION, SRC=" + e.Source);
                Logging.Write(Colors.Red, "Profile Changer: PROJECTE : " + e.Message);
            }
		}
    }
}
