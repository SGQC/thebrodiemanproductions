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
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.Plugins;
using Styx.Pathing;
using ObjectManager = Styx.WoWInternals.ObjectManager;

namespace ProfileSwapper
{
	class ProfileSwapperSettings
	{
		public static LocalPlayer Me = StyxWoW.Me;
		string FileFolder = "Settings\\";
		public ProfileSwapperSettings()
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

		public string DGProfile = "C:\\[Rep] Daily Grind [Brodie].xml";

		// -------------- Load ConfigFile ---------------
		public void Load()
		{
			//	XmlTextReader reader;
			XmlDocument xml = new XmlDocument();
			XmlNode xvar;

			string sPath = Process.GetCurrentProcess().MainModule.FileName;
			sPath = Path.GetDirectoryName(sPath);
			sPath = Path.Combine(sPath, FileFolder);

			if (!Directory.Exists(sPath))
			{
				Logging.WriteDiagnostic(Colors.LightSkyBlue, "Profile Swapper: Creating config directory");
				Directory.CreateDirectory(sPath);
			}

			sPath = Path.Combine(sPath, StyxWoW.Me.RealmName, StyxWoW.Me.Name, "ProfileSwapper.config");

			Logging.WriteDiagnostic(Colors.LightSkyBlue, "Profile Swapper: Loading config file");
			if (!File.Exists(sPath))
			{
				Logging.WriteDiagnostic("Profile Swapper: No Special Config - Continuing with Default Config Values");
				return;
			}
			System.IO.FileStream fs = new System.IO.FileStream(@sPath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite);
			try
			{
				xml.Load(fs);
				fs.Close();
			}
			catch (Exception e)
			{
				Logging.WriteDiagnostic(Colors.Red, e.Message);
				Logging.Write(Colors.Red, "Profile Swapper: Continuing with Default Config Values");
				fs.Close();
				return;
			}

			//			xml = new XmlDocument();

			try
			{
				//				xml.Load(reader);
				if (xml == null)
					return;
				// Load Variables
				xvar = xml.SelectSingleNode("//ProfileSwapper/Active1");
				if (xvar != null)
				{
					Active1 = Convert.ToBoolean(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Active1.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Profile1");
				if (xvar != null)
				{
					Profile1 = Convert.ToString(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Profile1.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Active2");
				if (xvar != null)
				{
					Active2 = Convert.ToBoolean(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Active2.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Profile2");
				if (xvar != null)
				{
					Profile2 = Convert.ToString(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Profile2.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Active3");
				if (xvar != null)
				{
					Active3 = Convert.ToBoolean(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Active3.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Profile3");
				if (xvar != null)
				{
					Profile3 = Convert.ToString(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Profile3.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Active4");
				if (xvar != null)
				{
					Active4 = Convert.ToBoolean(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Active4.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Profile4");
				if (xvar != null)
				{
					Profile4 = Convert.ToString(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Profile4.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Active5");
				if (xvar != null)
				{
					Active5 = Convert.ToBoolean(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Active5.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Profile5");
				if (xvar != null)
				{
					Profile5 = Convert.ToString(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Profile5.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Active6");
				if (xvar != null)
				{
					Active6 = Convert.ToBoolean(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Active6.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Profile6");
				if (xvar != null)
				{
					Profile6 = Convert.ToString(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Profile6.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Active7");
				if (xvar != null)
				{
					Active7 = Convert.ToBoolean(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Active7.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Profile7");
				if (xvar != null)
				{
					Profile7 = Convert.ToString(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Profile7.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Active8");
				if (xvar != null)
				{
					Active8 = Convert.ToBoolean(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Active8.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Profile8");
				if (xvar != null)
				{
					Profile8 = Convert.ToString(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Profile8.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Active9");
				if (xvar != null)
				{
					Active9 = Convert.ToBoolean(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Active9.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Profile9");
				if (xvar != null)
				{
					Profile9 = Convert.ToString(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Profile9.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Active10");
				if (xvar != null)
				{
					Active10 = Convert.ToBoolean(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Active10.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Profile10");
				if (xvar != null)
				{
					Profile10 = Convert.ToString(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Profile10.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Active11");
				if (xvar != null)
				{
					Active11 = Convert.ToBoolean(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Active11.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Profile11");
				if (xvar != null)
				{
					Profile11 = Convert.ToString(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Profile11.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Active12");
				if (xvar != null)
				{
					Active12 = Convert.ToBoolean(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Active12.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/Profile12");
				if (xvar != null)
				{
					Profile12 = Convert.ToString(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + Profile12.ToString());
				}
				xvar = xml.SelectSingleNode("//ProfileSwapper/DGProfile");
				if (xvar != null)
				{
					DGProfile = Convert.ToString(xvar.InnerText);
					Logging.WriteDiagnostic("Profile Swapper Load: " + xvar.Name + "=" + DGProfile.ToString());
				}
			}
			catch (Exception e)
			{
				Logging.WriteDiagnostic(Colors.Red, e.Message);
			}
		}
	}
}
