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

namespace ProfileSwapper
{
    public partial class ProfileSwapperUI : Form
    {
        public ProfileSwapperUI()
        {
            InitializeComponent();
            CB1.Checked = ProfileSwapper.Settings.Active1;
            CB2.Checked = ProfileSwapper.Settings.Active2;
            CB3.Checked = ProfileSwapper.Settings.Active3;
            CB4.Checked = ProfileSwapper.Settings.Active4;
            CB5.Checked = ProfileSwapper.Settings.Active5;
            CB6.Checked = ProfileSwapper.Settings.Active6;
            CB7.Checked = ProfileSwapper.Settings.Active7;
            CB8.Checked = ProfileSwapper.Settings.Active8;
            CB9.Checked = ProfileSwapper.Settings.Active9;
            CB10.Checked = ProfileSwapper.Settings.Active10;
            CB11.Checked = ProfileSwapper.Settings.Active11;
            CB12.Checked = ProfileSwapper.Settings.Active12;
            this.CB1.Image = new Bitmap(Application.StartupPath + "\\Plugins\\ProfileSwapper\\Images\\achievement_faction_goldenlotus.jpg");
            this.CB2.Image = new Bitmap(Application.StartupPath + "\\Plugins\\ProfileSwapper\\Images\\achievement_faction_tillers.jpg");
            this.CB3.Image = new Bitmap(Application.StartupPath + "\\Plugins\\ProfileSwapper\\Images\\achievement_faction_serpentriders.jpg");
            this.CB4.Image = new Bitmap(Application.StartupPath + "\\Plugins\\ProfileSwapper\\Images\\achievement_faction_anglers.jpg");
            if (StyxWoW.Me.IsAlliance)
            {
                this.CB5.Image = new Bitmap(Application.StartupPath + "\\Plugins\\ProfileSwapper\\Images\\pvpcurrency-honor-alliance.jpg");
                this.CB8.Image = new Bitmap(Application.StartupPath + "\\Plugins\\ProfileSwapper\\Images\\achievement_reputation_kirintor_offensive.jpg");
                this.CB9.Image = new Bitmap(Application.StartupPath + "\\Plugins\\ProfileSwapper\\Images\\achievement_reputation_kirintor_offensive.jpg");
            }
            if (StyxWoW.Me.IsHorde)
            {
                this.CB5.Image = new Bitmap(Application.StartupPath + "\\Plugins\\ProfileSwapper\\Images\\pvpcurrency-honor-horde.jpg");
                this.CB8.Image = new Bitmap(Application.StartupPath + "\\Plugins\\ProfileSwapper\\Images\\achievement_faction_sunreaveronslaught.jpg");
                this.CB9.Image = new Bitmap(Application.StartupPath + "\\Plugins\\ProfileSwapper\\Images\\achievement_faction_sunreaveronslaught.jpg");
            }
            this.CB6.Image = new Bitmap(Application.StartupPath + "\\Plugins\\ProfileSwapper\\Images\\achievement_faction_klaxxi.jpg");
            this.CB7.Image = new Bitmap(Application.StartupPath + "\\Plugins\\ProfileSwapper\\Images\\achievement_faction_shadopan.jpg");
            this.CB10.Image = new Bitmap(Application.StartupPath + "\\Plugins\\ProfileSwapper\\Images\\achievement_faction_celestials.jpg");
            this.CB11.Image = new Bitmap(Application.StartupPath + "\\Plugins\\ProfileSwapper\\Images\\inv_helmet_50.jpg");
            this.CB12.Image = new Bitmap(Application.StartupPath + "\\Plugins\\ProfileSwapper\\Images\\trade_engineering.jpg");

            if (ProfileSwapper.Settings.DGProfile == "C:\\[Rep] Daily Grind [Brodie].xml")
            {
                Logging.Write(Colors.Red, "Profile Swapper: You must select select your [Rep] Daily Grind [Brodie].xml file. Commonly located in BrodieMan\\Profiles\\Reputation\\TMoPDE");
            }

            if (ProfileSwapper.Settings.DGProfile != "C:\\[Rep] Daily Grind [Brodie].xml")
            {
                CB1.Enabled = true;
                CB2.Enabled = true;
                CB3.Enabled = true;
                CB4.Enabled = true;
                CB5.Enabled = true;
                CB6.Enabled = true;
                CB7.Enabled = true;
                CB8.Enabled = true;
                CB9.Enabled = true;
                CB10.Enabled = true;
                CB11.Enabled = true;
                //CB12.Enabled = true;
            }

            DG.Text = ProfileSwapper.Settings.DGProfile;

            // Check for Alliance Shieldwall - Disable Dominance
            if (StyxWoW.Me.IsAlliance)
            {
                tb5.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1376));
                tb8.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1387));
                tb9.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1387));
            }
            // Check for Horde Dominance - Disable Shieldwall
            if (StyxWoW.Me.IsHorde)
            {
                tb5.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1375));
                tb8.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1388));
                tb9.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1388));
            }

            // Let's check Faction reps... why not make things easier? We are checking above too for Ally / Horde only.
            tb1.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1269));
            tb2.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1272));
            tb3.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1271));
            tb4.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1302));
            tb6.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1337));
            tb7.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1270));
            tb10.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1341));
            tb11.Text = Convert.ToString(StyxWoW.Me.GetReputationLevelWith(1358));
            tb12.Text = "Reserved";
        }

        private void BSave_Click(object sender, EventArgs e)
        {
            //----------------- Save Configfile and set Settings ---------------- 
            string Folder = "Settings\\";

            XmlDocument xml;
            XmlElement root;
            XmlElement element;
            XmlText text;
            XmlComment xmlComment;

            string sPath = Process.GetCurrentProcess().MainModule.FileName;
            sPath = Path.GetDirectoryName(sPath);

            ProfileSwapper.Settings.Active1 = CB1.Checked;
            ProfileSwapper.Settings.Active2 = CB2.Checked;
            ProfileSwapper.Settings.Active3 = CB3.Checked;
            ProfileSwapper.Settings.Active4 = CB4.Checked;
            ProfileSwapper.Settings.Active5 = CB5.Checked;
            ProfileSwapper.Settings.Active6 = CB6.Checked;
            ProfileSwapper.Settings.Active7 = CB7.Checked;
            ProfileSwapper.Settings.Active8 = CB8.Checked;
            ProfileSwapper.Settings.Active9 = CB9.Checked;
            ProfileSwapper.Settings.Active10 = CB10.Checked;
            ProfileSwapper.Settings.Active11 = CB11.Checked;
            ProfileSwapper.Settings.Active12 = CB12.Checked;

            ProfileSwapper.Settings.DGProfile = DG.Text;

            ProfileSwapper.Settings.dirName = Path.GetDirectoryName(ProfileSwapper.Settings.DGProfile);

            //Golden lotus, tillers, cloud serpent, anglers, landfall (DomOff or Shieldwall, one will choose other so you can load either), 
            //Klaxxi, Shadopan, isle of thunder (same as landfall load either) isle of thunder pvp (placeholder) August Celestials. I'm making the pvp this will make it super easy to swap.

            ProfileSwapper.Settings.Profile1 = Path.Combine(ProfileSwapper.Settings.dirName, "[Rep] Golden Lotus Dailies [Brodie].xml");
            ProfileSwapper.Settings.Profile2 = Path.Combine(ProfileSwapper.Settings.dirName, "[Rep] Tillers Dailies [Brodie].xml");
            ProfileSwapper.Settings.Profile3 = Path.Combine(ProfileSwapper.Settings.dirName, "[Rep] Cloud Serpent Dailies [Brodie].xml");
            ProfileSwapper.Settings.Profile4 = Path.Combine(ProfileSwapper.Settings.dirName, "[Rep] The Anglers Dailies [Brodie].xml");
            ProfileSwapper.Settings.Profile5 = Path.Combine(ProfileSwapper.Settings.dirName, "[Rep] Shieldwall [Brodie].xml");
            ProfileSwapper.Settings.Profile6 = Path.Combine(ProfileSwapper.Settings.dirName, "[Rep] The Klaxxi Dailies [Brodie].xml");
            ProfileSwapper.Settings.Profile7 = Path.Combine(ProfileSwapper.Settings.dirName, "[Rep] Shado Pan Dailies [Brodie].xml");
            if (StyxWoW.Me.IsAlliance)
            {
                ProfileSwapper.Settings.Profile8 = Path.Combine(ProfileSwapper.Settings.dirName, "[Rep] Isle of Thunder A [Brodie].xml");
                ProfileSwapper.Settings.Profile9 = Path.Combine(ProfileSwapper.Settings.dirName, "[Rep] Isle of Thunder PvP A [Brodie].xml");
            }
            if (StyxWoW.Me.IsHorde)
            {
                ProfileSwapper.Settings.Profile8 = Path.Combine(ProfileSwapper.Settings.dirName, "[Rep] Isle of Thunder H [Brodie].xml");
                ProfileSwapper.Settings.Profile9 = Path.Combine(ProfileSwapper.Settings.dirName, "[Rep] Isle of Thunder PvP H [Brodie].xml");
            }
            ProfileSwapper.Settings.Profile10 = Path.Combine(ProfileSwapper.Settings.dirName, "[Rep] August Celestials Dailies [Brodie].xml");
            ProfileSwapper.Settings.Profile11 = Path.Combine(ProfileSwapper.Settings.dirName, "[Rep] Nat Pagle Dailies [TBMP].xml");
            ProfileSwapper.Settings.Profile12 = Path.Combine(ProfileSwapper.Settings.dirName, "");

            ProfileSwapper.Settings.Profile11 = "";
            ProfileSwapper.Settings.Profile12 = "";

            // ---------- Save XML-Config-File ---------------------------- 
            sPath = Path.Combine(sPath, Folder);

            if (!Directory.Exists(sPath))
            {
                Logging.Write(Colors.LightSkyBlue, "Profile Swapper: Creating config directory");
                Directory.CreateDirectory(sPath);
            }

            sPath = Path.Combine(sPath, StyxWoW.Me.RealmName, StyxWoW.Me.Name, "ProfileSwapper.config");

            Logging.Write(Colors.LightSkyBlue, "Profile Swapper: Saving config file: {0}", sPath);
            Logging.Write(Colors.LightSkyBlue, "Profile Swapper: Settings Saved");
            xml = new XmlDocument();
            XmlDeclaration dc = xml.CreateXmlDeclaration("1.0", "utf-8", null);
            xml.AppendChild(dc);

            xmlComment = xml.CreateComment(
                "=======================================================================\n" +
                ".CONFIG  -  This is the Config File For ProfileSwapper\n\n" +
                "XML file containing settings to customize in the ProfileSwapper Plugin\n" +
                "It is STRONGLY recommended you use the Configuration UI to change this\n" +
                "file instead of direct changing it here.\n" +
                "========================================================================");

            //let's add the root element
            root = xml.CreateElement("ProfileSwapper");
            root.AppendChild(xmlComment);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active1");
            text = xml.CreateTextNode(CB1.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile1");
            text = xml.CreateTextNode(ProfileSwapper.Settings.Profile1.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active2");
            text = xml.CreateTextNode(CB2.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile2");
            text = xml.CreateTextNode(ProfileSwapper.Settings.Profile2.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active3");
            text = xml.CreateTextNode(CB3.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile3");
            text = xml.CreateTextNode(ProfileSwapper.Settings.Profile3.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active4");
            text = xml.CreateTextNode(CB4.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile4");
            text = xml.CreateTextNode(ProfileSwapper.Settings.Profile4.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active5");
            text = xml.CreateTextNode(CB5.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile5");
            text = xml.CreateTextNode(ProfileSwapper.Settings.Profile5.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active6");
            text = xml.CreateTextNode(CB6.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile6");
            text = xml.CreateTextNode(ProfileSwapper.Settings.Profile6.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active7");
            text = xml.CreateTextNode(CB7.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile7");
            text = xml.CreateTextNode(ProfileSwapper.Settings.Profile7.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active8");
            text = xml.CreateTextNode(CB8.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile8");
            text = xml.CreateTextNode(ProfileSwapper.Settings.Profile8.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active9");
            text = xml.CreateTextNode(CB9.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile9");
            text = xml.CreateTextNode(ProfileSwapper.Settings.Profile9.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active10");
            text = xml.CreateTextNode(CB10.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile10");
            text = xml.CreateTextNode(ProfileSwapper.Settings.Profile10.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active11");
            text = xml.CreateTextNode(CB11.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile11");
            text = xml.CreateTextNode(ProfileSwapper.Settings.Profile11.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Active12");
            text = xml.CreateTextNode(CB12.Checked.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile12");
            text = xml.CreateTextNode(ProfileSwapper.Settings.Profile12.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("Profile12");
            text = xml.CreateTextNode(ProfileSwapper.Settings.Profile12.ToString());
            element.AppendChild(text);
            root.AppendChild(element);

            //let's add another element (child of the root)
            element = xml.CreateElement("DGProfile");
            text = xml.CreateTextNode(ProfileSwapper.Settings.DGProfile.ToString());
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

        private void DGb_Click(object sender, EventArgs e)
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
                CB9.Enabled = true;
                CB10.Enabled = true;
                CB11.Enabled = true;
                //CB12.Enabled = true;
            }
        }
    }
}
