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
    public partial class ProfileChangerUI : Form
    {
       
                
        public ProfileChangerUI()

        {
            InitializeComponent();
            CB1.Checked = ProfileChanger.Settings.Active1;
            CB2.Checked = ProfileChanger.Settings.Active2;
            CB3.Checked = ProfileChanger.Settings.Active3;
            CB4.Checked = ProfileChanger.Settings.Active4;
            CB5.Checked = ProfileChanger.Settings.Active5;
            CB6.Checked = ProfileChanger.Settings.Active6;
            CB7.Checked = ProfileChanger.Settings.Active7;
            CB8.Checked = ProfileChanger.Settings.Active8;
            CB9.Checked = ProfileChanger.Settings.Active9;
            CB10.Checked = ProfileChanger.Settings.Active10;
            CB11.Checked = ProfileChanger.Settings.Active11;
            CB12.Checked = ProfileChanger.Settings.Active12;

            if (ProfileChanger.Settings.DGProfile == "C:\\dailygrind.xml")
            {
                Logging.Write(Colors.Red, "Profile Changer: You must select select your DailyGrind [Brodie].xml file. Commonly located in BrodieMan\\Profiles\\Reputation\\TMoPDE");
            }

            if (ProfileChanger.Settings.DGProfile != "C:\\dailygrind.xml")
            {
                CB1.Enabled = true;
                CB2.Enabled = true;
                CB3.Enabled = true;
                CB4.Enabled = true;
                CB5.Enabled = true;
                CB6.Enabled = true;
                CB7.Enabled = true;
                CB8.Enabled = true;
                //CB9.Enabled = true;
                CB10.Enabled = true;
                //CB11.Enabled = true;
                //CB12.Enabled = true;
            }

            DG.Text = ProfileChanger.Settings.DGProfile;


            // Check for Alliance Shieldwall - Disable Dominance
            if (StyxWoW.Me.IsAlliance)
            {
                tb5.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1376));
                tb8.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1387));
            }
            // Check for Horde Dominance - Disable Shieldwall
            if (StyxWoW.Me.IsHorde)
            {
                tb5.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1375));
                tb8.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1388));
            }

            // Let's check Faction reps... why not make things easier? We are checking above too for Ally / Horde only.
            tb1.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1269));
            tb2.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1272));
            tb3.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1271));
            tb4.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1302));
            tb6.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1337));
            tb7.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1270));
            tb9.Text = "Soon to come!";
            tb10.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1341));
            tb11.Text = "N/A";
            tb12.Text = "N/A";

            // Rep checks can be handled by Profiles. Just a test to see if it would work. Might be useful in some cases.
            // Check for Rep for ShadoPan
            //if (StyxWoW.Me.GetReputationLevelWith(1269) < WoWUnitReaction.Revered)
            //{
            //    CB7.Enabled = false;
            //    CB7.Checked = false;
            //}

        }


        private void BSave_Click(object sender, EventArgs e)
        {
            //----------------- Save Configfile and set Settings ---------------- 
            string Folder = "Plugins\\ProfileChanger\\config\\";

            XmlDocument xml;
            XmlElement root;
            XmlElement element;
            XmlText text;
            XmlComment xmlComment;

            string sPath = Process.GetCurrentProcess().MainModule.FileName;
            sPath = Path.GetDirectoryName(sPath);


            ProfileChanger.Settings.Active1 = CB1.Checked;
            ProfileChanger.Settings.Active2 = CB2.Checked;
            ProfileChanger.Settings.Active3 = CB3.Checked;
            ProfileChanger.Settings.Active4 = CB4.Checked;
            ProfileChanger.Settings.Active5 = CB5.Checked;
            ProfileChanger.Settings.Active6 = CB6.Checked;
            ProfileChanger.Settings.Active7 = CB7.Checked;
            ProfileChanger.Settings.Active8 = CB8.Checked;
            ProfileChanger.Settings.Active9 = CB9.Checked;
            ProfileChanger.Settings.Active10 = CB10.Checked;
            ProfileChanger.Settings.Active11 = CB11.Checked;
            ProfileChanger.Settings.Active12 = CB12.Checked;

            ProfileChanger.Settings.DGProfile = DG.Text;

            ProfileChanger.Settings.dirName = Path.GetDirectoryName(ProfileChanger.Settings.DGProfile);

            //Golden lotus, tillers, cloud serpent, anglers, landfall (DomOff or Shieldwall, one will choose other so you can load either), 
            //Klaxxi, Shadopan, isle of thunder (same as landfall load either) isle of thunder pvp (placeholder) August Celestials. I'm making the pvp this will make it super easy to swap.

            
            ProfileChanger.Settings.Profile1 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] Golden Lotus Dailies [Brodie].xml");
            ProfileChanger.Settings.Profile2 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] Tillers Dailies [Brodie].xml");
            ProfileChanger.Settings.Profile3 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] Cloud Serpent Dailies [Brodie].xml");
            ProfileChanger.Settings.Profile4 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] The Anglers Dailies [Brodie].xml");
            ProfileChanger.Settings.Profile5 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] Shieldwall [Brodie].xml");
            ProfileChanger.Settings.Profile6 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] The Klaxxi Dailies [Brodie].xml");
            ProfileChanger.Settings.Profile7 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] Shado Pan Dailies [Brodie].xml");
            ProfileChanger.Settings.Profile8 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] Isle of Thunder A [Brodie].xml");
            ProfileChanger.Settings.Profile9 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] Isle of Thunder PvP.xml");
            ProfileChanger.Settings.Profile10 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] August Celestials Dailies [Brodie].xml");
            ProfileChanger.Settings.Profile11 = Path.Combine(ProfileChanger.Settings.dirName, "");
            ProfileChanger.Settings.Profile12 = Path.Combine(ProfileChanger.Settings.dirName, "");
            /*
            ProfileChanger.Settings.Profile1 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] Golden Lotus Dailies [Brodie].xml");
            ProfileChanger.Settings.Profile2 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] Tillers Dailies [Brodie].xml");
            ProfileChanger.Settings.Profile3 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] Cloud Serpent Dailies [Brodie].xml");
            ProfileChanger.Settings.Profile4 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] The Anglers Dailies [Brodie].xml");
            ProfileChanger.Settings.Profile5 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] The Klaxxi Dailies [Brodie].xml");
            ProfileChanger.Settings.Profile6 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] Shado Pan Dailies [Brodie].xml");
            ProfileChanger.Settings.Profile7 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] August Celestials Dailies [Brodie].xml");
            ProfileChanger.Settings.Profile8 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] Shieldwall [Brodie].xml");
            ProfileChanger.Settings.Profile9 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] Dominance [Brodie].xml");
             */
            // Auto load correct Isle of Thunder
            if (StyxWoW.Me.IsAlliance)
            {
                ProfileChanger.Settings.Profile10 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] Isle of Thunder A [Brodie].xml");
            }
            if (StyxWoW.Me.IsHorde)
            {
                ProfileChanger.Settings.Profile10 = Path.Combine(ProfileChanger.Settings.dirName, "[Rep] Isle of Thunder H [Brodie].xml");
            }
            ProfileChanger.Settings.Profile11 = "";
            ProfileChanger.Settings.Profile12 = "";

            // ---------- Save XML-Config-File ---------------------------- 
            sPath = Path.Combine(sPath, Folder);

            if (!Directory.Exists(sPath))
            {
                Logging.Write(Colors.LightSkyBlue, "Profile Changer: Creating config directory");
                Directory.CreateDirectory(sPath);
            }

            sPath = Path.Combine(sPath, "ProfileChanger.config");

            Logging.Write(Colors.LightSkyBlue, "Profile Changer: Saving config file: {0}", sPath);
            Logging.Write(Colors.LightSkyBlue, "Profile Changer: Settings Saved");
            xml = new XmlDocument();
            XmlDeclaration dc = xml.CreateXmlDeclaration("1.0", "utf-8", null);
            xml.AppendChild(dc);

            xmlComment = xml.CreateComment(
                "=======================================================================\n" +
                ".CONFIG  -  This is the Config File For ProfileChanger\n\n" +
                "XML file containing settings to customize in the ProfileChanger Plugin\n" +
                "It is STRONGLY recommended you use the Configuration UI to change this\n" +
                "file instead of direct changein it here.\n" +
                "========================================================================");

            //let's add the root element
            root = xml.CreateElement("ProfileChanger");
            root.AppendChild(xmlComment);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active1");
            text = xml.CreateTextNode(CB1.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile1");
            text = xml.CreateTextNode(ProfileChanger.Settings.Profile1.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active2");
            text = xml.CreateTextNode(CB2.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile2");
            text = xml.CreateTextNode(ProfileChanger.Settings.Profile2.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active3");
            text = xml.CreateTextNode(CB3.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile3");
            text = xml.CreateTextNode(ProfileChanger.Settings.Profile3.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active4");
            text = xml.CreateTextNode(CB4.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile4");
            text = xml.CreateTextNode(ProfileChanger.Settings.Profile4.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active5");
            text = xml.CreateTextNode(CB5.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile5");
            text = xml.CreateTextNode(ProfileChanger.Settings.Profile5.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active6");
            text = xml.CreateTextNode(CB6.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile6");
            text = xml.CreateTextNode(ProfileChanger.Settings.Profile6.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active7");
            text = xml.CreateTextNode(CB7.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile7");
            text = xml.CreateTextNode(ProfileChanger.Settings.Profile7.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active8");
            text = xml.CreateTextNode(CB8.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile8");
            text = xml.CreateTextNode(ProfileChanger.Settings.Profile8.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active9");
            text = xml.CreateTextNode(CB9.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile9");
            text = xml.CreateTextNode(ProfileChanger.Settings.Profile9.ToString());
            element.AppendChild(text);
            root.AppendChild(element);


            //let's add another element (child of the root)
            element = xml.CreateElement("Active10");
            text = xml.CreateTextNode(CB10.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile10");
            text = xml.CreateTextNode(ProfileChanger.Settings.Profile10.ToString());
            element.AppendChild(text);
            root.AppendChild(element);


            //let's add another element (child of the root)
            element = xml.CreateElement("Active11");
            text = xml.CreateTextNode(CB11.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile11");
            text = xml.CreateTextNode(ProfileChanger.Settings.Profile11.ToString());
            element.AppendChild(text);
            root.AppendChild(element);


            //let's add another element (child of the root)
            element = xml.CreateElement("Active12");
            text = xml.CreateTextNode(CB12.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile12");
            text = xml.CreateTextNode(ProfileChanger.Settings.Profile12.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile12");
            text = xml.CreateTextNode(ProfileChanger.Settings.Profile12.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("DGProfile");
            text = xml.CreateTextNode(ProfileChanger.Settings.DGProfile.ToString());
            element.AppendChild(text);
            root.AppendChild(element);



            xml.AppendChild(root);

            System.IO.FileStream fs = new System.IO.FileStream(@sPath, System.IO.FileMode.Create,
                                                               System.IO.FileAccess.Write);
            try
            {
                xml.Save(fs);
                fs.Close();
            }
            catch (Exception np)
            {
                Logging.Write(Colors.Red, np.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var loadDGProfile = new OpenFileDialog
            {
                Filter = "Xml files | *.xml",
                Title = "Select xml file to load"
            };

            if (loadDGProfile.ShowDialog() == DialogResult.OK)
            {
                string fileName1 = loadDGProfile.FileName;
                if (!string.IsNullOrEmpty(fileName1))
                {
                    DG.Text = fileName1;
                }
            }

            if (DG.Text.ToString() != "C:\\dailygrind.xml")
            {
                CB1.Enabled = true;
                CB2.Enabled = true;
                CB3.Enabled = true;
                CB4.Enabled = true;
                CB5.Enabled = true;
                CB6.Enabled = true;
                CB7.Enabled = true;
                CB8.Enabled = true;
                //CB9.Enabled = true;
                CB10.Enabled = true;
                //CB11.Enabled = true;
                //CB12.Enabled = true;
            }
        }

        private void ProfileChangerUI_Load(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }






    }
}
