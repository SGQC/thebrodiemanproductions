// Behavior originally contributed by BarryDurex
// [Quest Behavior] KillVicejaw - v1.0.0
//
// DOCUMENTATION: http://www.thebuddyforum.com/honorbuddy-forum/developer-forum/100747-quest-behavior-killvicejaw.html
//    
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Styx;
using Styx.CommonBot;
using Styx.CommonBot.Profiles;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Pathing;
using Styx.Plugins;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;
using Styx.WoWInternals.World;
using System.Windows.Media;


namespace Styx.Bot.Quest_Behaviors.KillVicejaw
{
	[CustomBehaviorFileName(@"KillVicejaw")]
    class KillVicejaw : CustomForcedBehavior
    {
        public KillVicejaw(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                isTest = GetAttributeAsNullable<bool>("Test", false, null, null) ?? false;
                QuestId = GetAttributeAsNullable<int>("QuestId", false, ConstrainAs.QuestId(this), null) ?? 30234;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
            }

            catch (Exception except)
            {
                // Maintenance problems occur for a number of reasons.  The primary two are...
                // * Changes were made to the behavior, and boundary conditions weren't properly tested.
                // * The Honorbuddy core was changed, and the behavior wasn't adjusted for the new changes.
                // In any case, we pinpoint the source of the problem area here, and hopefully it
                // can be quickly resolved.
                LogMessage("error", "BEHAVIOR MAINTENANCE PROBLEM: " + except.Message
                                    + "\nFROM HERE:\n"
                                    + except.StackTrace + "\n");
                IsAttributeProblem = true;
            }
        }


        // Attributes provided by caller
        public int QuestId { get; private set; }
        public QuestCompleteRequirement QuestRequirementComplete { get; private set; }
        public QuestInLogRequirement QuestRequirementInLog { get; private set; }
        public bool isTest { get; private set; }

        // Private variables for internal state
        private ConfigMemento _configMemento;
        private bool _isDisposed;
        private Composite _root;
        private bool _IsDone = false;
        private WoWUnit waitUnit;
        private WoWPoint waitPoint = WoWPoint.Empty;

        private bool wlog(WoWObject obj)
        { Styx.Common.Logging.WriteDiagnostic("found Paleblade Flesheater - dis: {0}", obj.Distance); return true; }
        
        private List<WoWUnit> PalebladeFlesheaters
        {
            get
            {
                ObjectManager.Update();
                return (from obj in ObjectManager.GetObjectsOfType<WoWUnit>()
                        orderby obj.Distance ascending
                        where obj.Entry == 64567
                        where obj.IsValid
                        where obj.Location.Distance(Vicejaw.Location) > 15
                        where obj.Auras.Values.FirstOrDefault(a => a.SpellId == 119073) != null
                        where obj.Auras.Values.FirstOrDefault(a => a.SpellId == 126131) == null // Chomp
                        //where wlog(obj)
                        select obj).ToList();
            }
        }

        private WoWUnit Vicejaw
        {
            get
            {
                ObjectManager.Update();
                return (from obj in ObjectManager.GetObjectsOfType<WoWUnit>()
                        where obj.Entry == 58769
                        where obj.IsValid
                        where obj.IsAlive
                        select obj).FirstOrDefault();
            }
        }
        
        ~KillVicejaw()
        {
            Dispose(false);
        }


        public void Dispose(bool isExplicitlyInitiatedDispose)
        {
            if (!_isDisposed)
            {
                // NOTE: we should call any Dispose() method for any managed or unmanaged
                // resource, if that resource provides a Dispose() method.

                // Clean up managed resources, if explicit disposal...
                if (isExplicitlyInitiatedDispose)
                {
                    // empty, for now
                }

                // Clean up unmanaged resources (if any) here...
                if (_configMemento != null)
                { _configMemento.Dispose(); }

                _configMemento = null;

                BotEvents.OnBotStop -= BotEvents_OnBotStop;
                TreeRoot.GoalText = string.Empty;
                TreeRoot.StatusText = string.Empty;

                // Call parent Dispose() (if it exists) here ...
                base.Dispose();
            }

            _isDisposed = true;
        }


        public void BotEvents_OnBotStop(EventArgs args)
        {
            Dispose();
        }


        #region Overrides of CustomForcedBehavior
        
        protected override TreeSharp.Composite CreateBehavior()
        {
            return (_root ?? (_root = new Decorator(ret => !IsDone,
                new PrioritySelector(

                new Decorator(ret => StyxWoW.Me.IsDead || StyxWoW.Me.IsGhost,
                    Bots.Grind.LevelBot.CreateDeathBehavior()),

                new Decorator(ret => Vicejaw == null,
                    new Wait(5, ret => Vicejaw != null, new CommonBehaviors.Actions.ActionAlwaysSucceed())),

                new Decorator(ret => StyxWoW.Me.CurrentTarget != Vicejaw,
                    new Action(ret => { Vicejaw.Face(); Vicejaw.Target(); })),

                new Decorator(ret => !StyxWoW.Me.Combat,
                    new PrioritySelector(
                        RoutineManager.Current.PullBuffBehavior,
                        RoutineManager.Current.CombatBuffBehavior,
                        RoutineManager.Current.PullBehavior,
                        RoutineManager.Current.CombatBehavior,
                        new Action(ret => RoutineManager.Current.Pull())
                        )),

                new Decorator(ret => StyxWoW.Me.Combat,
                    new PrioritySelector(

                        new Decorator(ret => isVicejawSafeAttackable,
                            new PrioritySelector(

                                new Decorator(ret => needMove(6),
                                    new Action(ret => MoveNow(6))),

                                RoutineManager.Current.HealBehavior,
                                RoutineManager.Current.CombatBuffBehavior,
                                RoutineManager.Current.CombatBehavior
                                    )
                            ),

                        new Decorator(ret => (!isVicejawSafeAttackable) && PalebladeFlesheaters != null && PalebladeFlesheaters.Count > 0,
                            new PrioritySelector(

                                new Decorator(ret => (waitUnit == null || !waitUnit.IsValid || waitUnit.HasAura(126131)),
                                    new Action(ret => { waitUnit = PalebladeFlesheaters[0]; waitPoint = WoWPoint.Empty; })),

                                    new Decorator(ret => waitUnit != null && waitUnit.IsValid && !waitUnit.HasAura(126131) && (!isVicejawSafeAttackable),
                                        new PrioritySelector(

                                            new Decorator(ret => waitPoint == WoWPoint.Empty && (!isVicejawSafeAttackable),
                                                new PrioritySelector(

                                                    new Decorator(ret => waitUnit.Distance > 0.5f,
                                                        new Action(ret => Navigator.MoveTo(waitUnit.Location))),

                                                    new Decorator(ret => waitUnit.Distance <= 0.5f && (!isVicejawSafeAttackable),
                                                        new Action(ret => {
                                                            WoWMovement.MoveStop();
                                                            StyxWoW.Me.SetFacing(Vicejaw.Location);
                                                            Thread.Sleep(400);
                                                            WoWMovement.Move(WoWMovement.MovementDirection.Backwards, TimeSpan.FromMilliseconds(1500));
                                                            Thread.Sleep(1500);
                                                            waitPoint = StyxWoW.Me.Location;
                                                        }
                                                    ))
                                            )),

                                            new Decorator(ret => waitPoint != WoWPoint.Empty && (!isVicejawSafeAttackable),
                                                new PrioritySelector(

                                                    new Decorator(ret => waitPoint.Distance(StyxWoW.Me.Location) >= 0.3f,
                                                        new Action(ret => Navigator.MoveTo(waitPoint))),

                                                    new Decorator(ret => waitPoint.Distance(StyxWoW.Me.Location) < 0.3f && (!isVicejawSafeAttackable),
                                                        new PrioritySelector(

                                                            new Decorator(ret => StyxWoW.Me.SpecType == SpecType.RangedDps && (!isVicejawSafeAttackable),
                                                                new PrioritySelector(
                                                                    RoutineManager.Current.HealBehavior,
                                                                    RoutineManager.Current.CombatBuffBehavior,
                                                                    RoutineManager.Current.CombatBehavior,
                                                                    new CommonBehaviors.Actions.ActionAlwaysSucceed()
                                                                    ))
                                                    ))
                                            ))
                                        ))
                                    ))
                                ))
                            //new CommonBehaviors.Actions.ActionAlwaysSucceed()
                            ))));
        }

        private bool needBackwards { get { return StyxWoW.Me.Location.Distance(Vicejaw.Location) <= 15; } }
        private bool isVicejawSafeAttackable { get { return (Vicejaw.IsCasting && (Vicejaw.CastingSpellId == 126126 || Vicejaw.CastingSpellId == 126187)) || Vicejaw.Stunned || StyxWoW.Me.HasAura("Vice Jaws") || !Vicejaw.IsSafelyFacing(StyxWoW.Me); } }

        private bool updateBlacklist(WoWObject obj)
        {
            if (!Blacklist.Contains(obj, BlacklistFlags.All) && obj.Location.Distance2D(Vicejaw.Location) <= 8)
            { Blacklist.Add(obj, BlacklistFlags.All, TimeSpan.FromHours(1)); LogMessage("debug", "{0}({1}) was eaten", obj.Name, obj.Guid); Thread.Sleep(80); }
            return true;
        }

        private void MoveNow(int AttackRadius)
        {
            Vicejaw.Face();
            Thread.Sleep(200);

            float VicejawRotation = getPositive(Vicejaw.RotationDegrees);
            float MeRotation = getPositive(StyxWoW.Me.RotationDegrees);

            float invertRotation = getInvert(VicejawRotation);
            float minRotation = (invertRotation - (AttackRadius / 2));
            float maxRotation = (invertRotation + (AttackRadius / 2));

            if (MeRotation > minRotation && MeRotation < maxRotation)
            {
                if (MeRotation < invertRotation)
                    while (getPositive(StyxWoW.Me.RotationDegrees) < invertRotation && Vicejaw.Distance2D <= 15)
                    {
                        WoWMovement.Move(WoWMovement.MovementDirection.StrafeRight, TimeSpan.FromMilliseconds(300));
                        Vicejaw.Face();
                    }
                else
                    while (getPositive(StyxWoW.Me.RotationDegrees) >= invertRotation && Vicejaw.Distance2D <= 15)
                    {
                        WoWMovement.Move(WoWMovement.MovementDirection.StrafeLeft, TimeSpan.FromMilliseconds(300));
                        Vicejaw.Face();
                    }
            }
        }


        private bool needMove(float AttackRadius)
        {
            return Vicejaw.IsSafelyFacing(StyxWoW.Me, AttackRadius) && !StyxWoW.Me.HasAura("Vice Jaws");
        }

        private float getInvert(float f)
        {
            if (f < 180)
                return (f + 180);
            //else if (f >= 180)
            return (f - 180);
        }

        private float getPositive(float f)
        {
            if (f < 0)
                return (f + 360);
            return f;
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        
        public override bool IsDone
        {
            get
            {
                bool ret = _IsDone || (!UtilIsProgressRequirementsMet(QuestId, QuestRequirementInLog, QuestRequirementComplete));
                if (ret) _IsDone = true;
                if (isTest) ret = false;
                return ret;
            }
        }


        public override void OnStart()
        {
            // This reports problems, and stops BT processing if there was a problem with attributes...
            // We had to defer this action, as the 'profile line number' is not available during the element's
            // constructor call.
            OnStart_HandleAttributeProblem();

            // If the quest is complete, this behavior is already done...
            // So we don't want to falsely inform the user of things that will be skipped.
            if (!IsDone)
            {
                // The ConfigMemento() class captures the user's existing configuration.
                // After its captured, we can change the configuration however needed.
                // When the memento is dispose'd, the user's original configuration is restored.
                // More info about how the ConfigMemento applies to saving and restoring user configuration
                // can be found here...
                //     http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_Saving_and_Restoring_User_Configuration
                _configMemento = new ConfigMemento();

                BotEvents.OnBotStop += BotEvents_OnBotStop;

                // Disable any settings that may cause us to dismount --
                // When we mount for travel via FlyTo, we don't want to be distracted by other things.
                // We also set PullDistance to its minimum value.  If we don't do this, HB will try
                // to dismount and engage a mob if it is within its normal PullDistance.
                // NOTE: these settings are restored to their normal values when the behavior completes
                // or the bot is stopped.
                CharacterSettings.Instance.HarvestHerbs = false;
                CharacterSettings.Instance.HarvestMinerals = false;
                CharacterSettings.Instance.LootChests = false;
                CharacterSettings.Instance.LootMobs = false;
                CharacterSettings.Instance.NinjaSkin = false;
                CharacterSettings.Instance.SkinMobs = false;

                TreeRoot.GoalText = "Kill Vicejaw for Quest: " + QuestId;

                if (TreeRoot.Current != null && TreeRoot.Current.Root != null && TreeRoot.Current.Root.LastStatus != RunStatus.Running)
                {
                    var currentRoot = TreeRoot.Current.Root;
                    if (currentRoot is GroupComposite)
                    {
                        var root = (GroupComposite)currentRoot;
                        root.InsertChild(0, CreateBehavior());
                    }
                }

                LogMessage("debug", "Kill Vicejaw for Quest '{0}'", QuestId);
            }
        }

        #endregion
    }
}
