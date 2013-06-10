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
using Styx.CommonBot.Profiles;

namespace ProfileSwapper
{
	class ProfileSwapper : HBPlugin
	{
		public override string Name { get { return "Brodieman's Profile Swapper"; } }
		public override string Author { get { return "Obliv & TheBrodieMan"; } }
		private readonly Version _version = new Version(2,0);
		public override Version Version { get { return _version; } }
		public override string ButtonText { get { return "Settings"; } }
		public override bool WantButton { get { return true; } }

		public static ProfileSwapperSettings Settings = new ProfileSwapperSettings();
		public static LocalPlayer Me = StyxWoW.Me;
		bool hasItBeenInitialized = false;

		public ProfileSwapper()
		{
			UpdatePlugin();
			Logging.Write(Colors.LightSkyBlue, "Profile Swapper loaded");
		}

		public override void OnButtonPress()
		{
			ConfigForm.ShowDialog();
		}

		private Form MyForm;
		public Form ConfigForm
		{
			get
			{
				if (MyForm == null)
					MyForm = new ProfileSwapperUI();
				return MyForm;
			}
		}

		public override void Pulse()
		{
// ------------ Deactivate if not in Game etc
			if (Me == null || !StyxWoW.IsInGame)
				return;

// ------------ Deactivate Plugin in BGs, Inis, while Casting and on Transport
			if (Battlegrounds.IsInsideBattleground || Me.IsInInstance || Me.IsOnTransport)
				return;
				
// ------------ Deactivate Plugin if in Combat, Dead or Ghost			
			if (inCombat)
				return;
			
			if (!hasItBeenInitialized)
			{
				if (Settings.Active1)
				{
					ChangeProfile(Settings.Profile1);
					hasItBeenInitialized = true;
				}
				else if (Settings.Active2)
				{
					ChangeProfile(Settings.Profile2);
					hasItBeenInitialized = true;
				}

				else if (Settings.Active3)
				{
					ChangeProfile(Settings.Profile3);
					hasItBeenInitialized = true;
				}

				else if (Settings.Active4)
				{
					ChangeProfile(Settings.Profile4);
					hasItBeenInitialized = true;
				}

				else if (Settings.Active5)
				{
					ChangeProfile(Settings.Profile5);
					hasItBeenInitialized = true;
				}

				else if (Settings.Active6)
				{
					ChangeProfile(Settings.Profile6);
					hasItBeenInitialized = true;
				}

				else if (Settings.Active7)
				{
					ChangeProfile(Settings.Profile7);
					hasItBeenInitialized = true;
				}

				else if (Settings.Active8)
				{
					ChangeProfile(Settings.Profile8);
					hasItBeenInitialized = true;
				}

				else if (Settings.Active9)
				{
					ChangeProfile(Settings.Profile9);
					hasItBeenInitialized = true;
				}

				else if (Settings.Active10)
				{
					ChangeProfile(Settings.Profile10);
					hasItBeenInitialized = true;
				}

				else if (Settings.Active11)
				{
					ChangeProfile(Settings.Profile11);
					hasItBeenInitialized = true;
				}

				else if (Settings.Active12)
				{
					ChangeProfile(Settings.Profile12);
					hasItBeenInitialized = true;
				}
				else
				{
					Logging.Write(Colors.Red, "Profile Swapper: Init failed - No profiles selected");
				}
				Logging.Write(Colors.LightSkyBlue, "Profile Swapper: Init Done");
			}

			// Skip profiles if they're not active:
			if (Settings.Active1 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 556 || StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
			{
				Logging.Write(Colors.LightSkyBlue, "Profile Swapper: Checking to see if we should change profiles");
				Settings.Active1 = false;

				if (Settings.Active2)
				{					
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile2);
				}
				else if (Settings.Active3)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile3);
				}
				else if (Settings.Active4)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile4);
				}
				else if (Settings.Active5)
				{
					SpellManager.StopCasting();		
					ChangeProfile(Settings.Profile5);
				}
				else if (Settings.Active6)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile6);
				}
				else if (Settings.Active7)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile7);
				}
				else if (Settings.Active8)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile8);
				}
				else if (Settings.Active9)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile9);
				}
				else if (Settings.Active10)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile10);
				}
				else if (Settings.Active11)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile11);
				}
				else if (Settings.Active12)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile12);
				}
				else
				{
				// Stop Bot
				}
			}
				
			if (Settings.Active2 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 556 || StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
			{
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
				Settings.Active2 = false;

				if (Settings.Active3)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile3);
				}
				else if (Settings.Active4)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile4);
				}
				else if (Settings.Active5)
				{
					SpellManager.StopCasting();		
					ChangeProfile(Settings.Profile5);
				}
				else if (Settings.Active6)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile6);
				}
				else if (Settings.Active7)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile7);
				}
				else if (Settings.Active8)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile8);
				}
				else if (Settings.Active9)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile9);
				}
				else if (Settings.Active10)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile10);
				}
				else if (Settings.Active11)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile11);
				}
				else if (Settings.Active12)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile12);
				}
				else
				{
				// Stop Bot
				}
			}
				
			if (Settings.Active3 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 556 || StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
			{
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
				Settings.Active3 = false;

				if (Settings.Active4)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile4);
				}
				else if (Settings.Active5)
				{
					SpellManager.StopCasting();		
					ChangeProfile(Settings.Profile5);
				}
				else if (Settings.Active6)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile6);
				}
				else if (Settings.Active7)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile7);
				}
				else if (Settings.Active8)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile8);
				}
				else if (Settings.Active9)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile9);
				}
				else if (Settings.Active10)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile10);
				}
				else if (Settings.Active11)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile11);
				}
				else if (Settings.Active12)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile12);
				}
				else
				{
				// Stop Bot
				}
			}
				
			if (Settings.Active4 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 556 || StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
			{
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
				Settings.Active4 = false;

				if (Settings.Active5)
				{
					SpellManager.StopCasting();		
					ChangeProfile(Settings.Profile5);
				}
				else if (Settings.Active6)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile6);
				}
				else if (Settings.Active7)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile7);
				}
				else if (Settings.Active8)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile8);
				}
				else if (Settings.Active9)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile9);
				}
				else if (Settings.Active10)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile10);
				}
				else if (Settings.Active11)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile11);
				}
				else if (Settings.Active12)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile12);
				}
				else
				{
				// Stop Bot
				}
			}
							
			if (Settings.Active5 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 556 || StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
			{
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
				Settings.Active5 = false;

				if (Settings.Active6)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile6);
				}
				else if (Settings.Active7)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile7);
				}
				else if (Settings.Active8)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile8);
				}
				else if (Settings.Active9)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile9);
				}
				else if (Settings.Active10)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile10);
				}
				else if (Settings.Active11)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile11);
				}
				else if (Settings.Active12)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile12);
				}
				else
				{
				// Stop Bot
				}
			}

			if (Settings.Active6 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 556 || StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
			{
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
				Settings.Active6 = false;

				if (Settings.Active7)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile7);
				}
				else if (Settings.Active8)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile8);
				}
				else if (Settings.Active9)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile9);
				}
				else if (Settings.Active10)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile10);
				}
				else if (Settings.Active11)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile11);
				}
				else if (Settings.Active12)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile12);
				}
				else
				{
				// Stop Bot
				}
			}
				
			if (Settings.Active7 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 556 || StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
			{
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
				Settings.Active7 = false;

				if (Settings.Active8)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile8);
				}
				else if (Settings.Active9)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile9);
				}
				else if (Settings.Active10)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile10);
				}
				else if (Settings.Active11)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile11);
				}
				else if (Settings.Active12)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile12);
				}
				else
				{
				// Stop Bot
				}
			}
				
			if (Settings.Active8 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 556 || StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
			{
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
				Settings.Active8 = false;

				if (Settings.Active9)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile9);
				}
				else if (Settings.Active10)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile10);
				}
				else if (Settings.Active11)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile11);
				}
				else if (Settings.Active12)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile12);
				}
				else
				{
				// Stop Bot
				}
			}
				
			if (Settings.Active9 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 556 || StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
			{
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
				Settings.Active9 = false;

				if (Settings.Active10)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile10);
				}
				else if (Settings.Active11)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile11);
				}
				else if (Settings.Active12)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile12);
				}
				else
				{
				// Stop Bot
				}
			}
				
			if (Settings.Active10 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 556 || StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
			{
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
				Settings.Active10 = false;

				if (Settings.Active11)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile11);
				}
				else if (Settings.Active12)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile12);
				}
				else
				{
				// Stop Bot
				}
			}
				
			if (Settings.Active11 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 556 || StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
			{
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
				Settings.Active11 = false;

				if (Settings.Active12)
				{
					SpellManager.StopCasting();	
					ChangeProfile(Settings.Profile12);
				}
				else
				{
				// Stop Bot
				}
			}

			if (Settings.Active12 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 556 || StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
				{
					Logging.WriteDiagnostic(Colors.LightSkyBlue, "Profile Swapper has run out of profiles to use. Thank you come again!");
				}
		}

		static public void ChangeProfile(string Profile)
		{
			Logging.Write(Colors.LightSkyBlue, "Profile Swapper: Load Profile: {0}", Profile);
			WoWMovement.MoveStop();
			Thread.Sleep(1000);
			ProfileManager.LoadNew(Profile);
			Thread.Sleep(1000);
			Logging.Write(Colors.LightSkyBlue, "Profile Swapper: Load Profile: {0} done", Profile);
		}
		
		static public string FolderPath
        {
            get
            {
                string sPath = Process.GetCurrentProcess().MainModule.FileName;
                sPath = Path.GetDirectoryName(sPath);
                sPath = Path.Combine(sPath, "Plugins\\ProfileSwapper\\");
                return sPath;
            }
        }
		
		static public void UpdatePlugin()
		{
			//Here you have to insert your Internetlocation for the SVN
			string Website = "http://thebrodiemanproductions.googlecode.com/svn/trunk";

			try
			{
				WebClient client = new WebClient();
				IFormatProvider culture = new CultureInfo("fr-FR", true);

				XDocument VersionLatest = XDocument.Load(Website + "/UpdaterProfileSwapper.xml");
				XDocument VersionCurrent = XDocument.Load(FolderPath + "\\Updater.xml");
				DateTime latestTime = DateTime.Parse(VersionLatest.Element("ProfileSwapperUpdater").Element("UpdateTime").Value, culture, DateTimeStyles.NoCurrentDateDefault);
				DateTime currentTime = DateTime.Parse(VersionCurrent.Element("Updater").Element("UpdateTime").Value, culture, DateTimeStyles.NoCurrentDateDefault);
				string Version = VersionLatest.Element("ProfileSwapperUpdater").Element("Version").Value;

				if (latestTime <= currentTime)
					return;
				Logging.Write(Colors.LightSkyBlue, "**********************************************");
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: New Version available {0}", Version);
				Logging.Write(Colors.LightSkyBlue, "Download it in the Pluginsection of Honorbuddy", Version);
				Logging.Write(Colors.LightSkyBlue, "Link or via SVN");
				Logging.Write(Colors.LightSkyBlue, "**********************************************");
			}
			catch (System.Threading.ThreadAbortException) { throw; }
			catch (Exception e)
			{
				Logging.WriteDiagnostic(Colors.Red, e.Message);
			}
		}

		static public bool inCombat
		{
			get
			{
				if (Me == null || !StyxWoW.IsInGame || Me.Combat || Me.IsDead || Me.IsGhost || InPetCombat()) return true;
				return false;
			}
		}
		
		static public bool InPetCombat()
        {
            List<string> cnt = Lua.GetReturnValues("dummy,reason=C_PetBattles.IsTrapAvailable() return dummy,reason");

            if (cnt != null) { if (cnt[1] != "0") return true; }
            return false;
        }
	}
}
