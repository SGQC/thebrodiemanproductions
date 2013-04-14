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

namespace ProfileChanger
{
    class ProfileChanger : HBPlugin
    {
        public override string Name { get { return "Profile Changer"; } }
        public override string Author { get { return "Obliv"; } }
        private readonly Version _version = new Version(1, 2);
        public override Version Version { get { return _version; } }
        public override string ButtonText { get { return "Settings"; } }
        public override bool WantButton { get { return true; } }

        public static ProfileChangerSettings Settings = new ProfileChangerSettings();
        public static LocalPlayer Me = StyxWoW.Me;

        

        bool hasItBeenInitialized = false;

        public ProfileChanger()
        {
            Logging.Write(Colors.LightSkyBlue, "Profile Changer loaded");
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
                    MyForm = new ProfileChangerUI();

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
			
            if (!hasItBeenInitialized && Settings.Active1)
            {

                ChangeProfile(Settings.Profile1);
                hasItBeenInitialized = true;
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Init Done");
            }

            // Check to see the first active profile
            // Probably not the best way to do this...
            if (!hasItBeenInitialized && !Settings.Active1 && Settings.Active2)
            {
                ChangeProfile(Settings.Profile2);
                hasItBeenInitialized = true;
            }

            if (!hasItBeenInitialized && !Settings.Active1 && !Settings.Active2 && Settings.Active3)
            {
                ChangeProfile(Settings.Profile3);
                hasItBeenInitialized = true;
            }

            if (!hasItBeenInitialized && !Settings.Active1 && !Settings.Active2 && !Settings.Active3 && Settings.Active4)
            {
                ChangeProfile(Settings.Profile4);
                hasItBeenInitialized = true;
            }

            if (!hasItBeenInitialized && !Settings.Active1 && !Settings.Active2 && !Settings.Active3 && !Settings.Active4 && Settings.Active5)
            {
                ChangeProfile(Settings.Profile5);
                hasItBeenInitialized = true;
            }

            if (!hasItBeenInitialized && !Settings.Active1 && !Settings.Active2 && !Settings.Active3 && !Settings.Active4 && !Settings.Active5 && Settings.Active6)
            {
                ChangeProfile(Settings.Profile6);
                hasItBeenInitialized = true;
            }

            if (!hasItBeenInitialized && !Settings.Active1 && !Settings.Active2 && !Settings.Active3 && !Settings.Active4 && !Settings.Active5 && !Settings.Active6 && Settings.Active7)
            {
                ChangeProfile(Settings.Profile7);
                hasItBeenInitialized = true;
            }

            if (!hasItBeenInitialized && !Settings.Active1 && !Settings.Active2 && !Settings.Active3 && !Settings.Active4 && !Settings.Active5 && !Settings.Active6 && !Settings.Active7 && Settings.Active8)
            {
                ChangeProfile(Settings.Profile8);
                hasItBeenInitialized = true;
            }

            if (!hasItBeenInitialized && !Settings.Active1 && !Settings.Active2 && !Settings.Active3 && !Settings.Active4 && !Settings.Active5 && !Settings.Active6 && !Settings.Active7 && !Settings.Active8 && Settings.Active9)
            {
                ChangeProfile(Settings.Profile9);
                hasItBeenInitialized = true;
            }

            if (!hasItBeenInitialized && !Settings.Active1 && !Settings.Active2 && !Settings.Active3 && !Settings.Active4 && !Settings.Active5 && !Settings.Active6 && !Settings.Active7 && !Settings.Active8 && !Settings.Active9 
                && Settings.Active10)
            {
                ChangeProfile(Settings.Profile10);
                hasItBeenInitialized = true;
            }

            if (!hasItBeenInitialized && !Settings.Active1 && !Settings.Active2 && !Settings.Active3 && !Settings.Active4 && !Settings.Active5 && !Settings.Active6 && !Settings.Active7 && !Settings.Active8 && !Settings.Active9
                && !Settings.Active10 && Settings.Active11)
            {
                ChangeProfile(Settings.Profile11);
                hasItBeenInitialized = true;
            }

            if (!hasItBeenInitialized && !Settings.Active1 && !Settings.Active2 && !Settings.Active3 && !Settings.Active4 && !Settings.Active5 && !Settings.Active6 && !Settings.Active7 && !Settings.Active8 && !Settings.Active9
                && !Settings.Active10 && !Settings.Active11 && Settings.Active12)
            {
                ChangeProfile(Settings.Profile12);
                hasItBeenInitialized = true;
            }

            // Skip profiles if they're not active:
            if (Settings.Active1 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
			{
				    Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
                    Settings.Active1 = false;

                    if (Settings.Active2 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{					
						SpellManager.StopCasting();	
						ChangeProfile(Settings.Profile2);
					}
                    if (Settings.Active3 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
						ChangeProfile(Settings.Profile3);
					}
                    if (Settings.Active4 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile4);
					}
                    if (Settings.Active5 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();		
					    ChangeProfile(Settings.Profile5);
					}
                    if (Settings.Active6 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile6);
					}
                    if (Settings.Active7 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile7);
					}
                    if (Settings.Active8 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile8);
					}
                    if (Settings.Active9 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile9);
					}
                    if (Settings.Active10 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile10);
					}
                    if (Settings.Active11 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile11);
					}
                    if (Settings.Active12 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile12);
					}
					else
					{
					// Stop Bot
					}
            }
				
			if (Settings.Active2 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
				{
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
                Settings.Active2 = false;

                    if (Settings.Active3 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674))) 
					{
						SpellManager.StopCasting();	
						ChangeProfile(Settings.Profile3);
					}
					if (Settings.Active4 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile4);
					}
                    if (Settings.Active5 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();		
					    ChangeProfile(Settings.Profile5);
					}
                    if (Settings.Active6 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile6);
					}
                    if (Settings.Active7 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile7);
					}
                    if (Settings.Active8 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile8);
					}
                    if (Settings.Active9 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile9);
					}
                    if (Settings.Active10 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile10);
					}
                    if (Settings.Active11 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile11);
					}
                    if (Settings.Active12 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile12);
					}
					else
					{
					// Stop Bot
					}
                }
				
            if (Settings.Active3 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
				{
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
                Settings.Active3 = false;

					if (Settings.Active4 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile4);
					}
                    if (Settings.Active5 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();		
					    ChangeProfile(Settings.Profile5);
					}
                    if (Settings.Active6 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile6);
					}
                    if (Settings.Active7 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile7);
					}
                    if (Settings.Active8 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile8);
					}
                    if (Settings.Active9 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile9);
					}
                    if (Settings.Active10 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile10);
					}
                    if (Settings.Active11 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile11);
					}
                    if (Settings.Active12 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile12);
					}
					else
					{
					// Stop Bot
					}
                }
				
            if (Settings.Active4 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
				{
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
                Settings.Active4 = false;

					if (Settings.Active5 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();		
					    ChangeProfile(Settings.Profile5);
					}
					if (Settings.Active6 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile6);
					}
					if (Settings.Active7 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile7);
					}
					if (Settings.Active8 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile8);
					}
					if (Settings.Active9 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile9);
					}
					if (Settings.Active10 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile10);
					}
					if (Settings.Active11 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile11);
					}
					if (Settings.Active12 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile12);
					}
					else
					{
					// Stop Bot
					}
                }
				            
			if (Settings.Active5 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
				{
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
                Settings.Active5 = false;

					if (Settings.Active6 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile6);
					}
					if (Settings.Active7 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile7);
					}
					if (Settings.Active8 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile8);
					}
					if (Settings.Active9 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile9);
					}
					if (Settings.Active10 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile10);
					}
					if (Settings.Active11 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile11);
					}
					if (Settings.Active12 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile12);
					}
					else
					{
					// Stop Bot
					}
                }

			if (Settings.Active6 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
				{
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
                Settings.Active6 = false;

					if (Settings.Active7 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile7);
					}
					if (Settings.Active8 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile8);
					}
					if (Settings.Active9 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile9);
					}
					if (Settings.Active10 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile10);
					}
					if (Settings.Active11 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile11);
					}
					if (Settings.Active12 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile12);
					}
					else
					{
					// Stop Bot
					}
                }
				
			if (Settings.Active7 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
				{
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
                Settings.Active7 = false;

					if (Settings.Active8 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile8);
					}
					if (Settings.Active9 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile9);
					}
					if (Settings.Active10 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile10);
					}
					if (Settings.Active11 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile11);
					}
					if (Settings.Active12 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile12);
					}
					else
					{
					// Stop Bot
					}
                }
				
			if (Settings.Active8 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
				{
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
                Settings.Active8 = false;

					if (Settings.Active10 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile10);
					}
					if (Settings.Active11 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile11);
					}
					if (Settings.Active12 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile12);
					}
					else
					{
					// Stop Bot
					}
                }
				
			if (Settings.Active9 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
				{
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
                Settings.Active9 = false;

					if (Settings.Active10 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile10);
					}
					if (Settings.Active11 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile11);
					}
					if (Settings.Active12 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile12);
					}
					else
					{
					// Stop Bot
					}
                }
				
			if (Settings.Active10 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
				{
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
                Settings.Active10 = false;

					if (Settings.Active11 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile11);
					}
					if (Settings.Active12 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile12);
					}
					else
					{
					// Stop Bot
					}
                }
				
			if (Settings.Active11 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
				{
				Logging.Write(Colors.LightSkyBlue, "Profile Changer: Checking to see if we should change profiles");
                Settings.Active11 = false;

					if (Settings.Active12 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
					{
						SpellManager.StopCasting();	
					    ChangeProfile(Settings.Profile12);
					}
					else
					{
					// Stop Bot
					}
                }

            if (Settings.Active12 && (StyxWoW.Me.IsCasting && (StyxWoW.Me.CastingSpellId == 8690 || StyxWoW.Me.CastingSpellId == 94719 || StyxWoW.Me.CastingSpellId == 136508 || StyxWoW.Me.CastingSpellId == 75136 || StyxWoW.Me.CastingSpellId == 82674)))
                {
                    Logging.Write(Colors.LightSkyBlue, "End of the line. Thank you, come again!");
                }
        }

        static public void ChangeProfile(string Profile)
        {
            Logging.Write(Colors.LightSkyBlue, "Profile Changer: Load Profile: {0}", Profile);
            WoWMovement.MoveStop();
            Thread.Sleep(1000);
            ProfileManager.LoadNew(Profile);
            Thread.Sleep(1000);
            Logging.Write(Colors.LightSkyBlue, "Profile Changer: Load Profile: {0} done", Profile);
        }

        static public bool inCombat
        {
            get
            {
                if (Me.Combat || Me.IsDead || Me.IsGhost) return true;
                return false;
            }
        }
    }
}
