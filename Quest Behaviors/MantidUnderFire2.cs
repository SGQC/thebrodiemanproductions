using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommonBehaviors.Actions;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Profiles;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace MantidUnderFire
{
    public class MantidUnderFire : CustomForcedBehavior
    {
        public MantidUnderFire(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                QuestId = 30243;//GetAttributeAsQuestId("QuestId", true, null) ?? 0;
            }
            catch
            {
                Logging.Write("Problem parsing a QuestId in behavior: Mantid Under Fire");
            }
        }
        public int QuestId { get; set; }
        private bool _isBehaviorDone;
        public int MobIdMantid = 63972;
        private Composite _root;
        public QuestCompleteRequirement questCompleteRequirement = QuestCompleteRequirement.NotComplete;
        public QuestInLogRequirement questInLogRequirement = QuestInLogRequirement.InLog;
        public override bool IsDone
        {
            get
            {
                return _isBehaviorDone;
            }
        }
        private LocalPlayer Me
        {
            get { return (StyxWoW.Me); }
        }

        public override void OnStart()
        {
            OnStart_HandleAttributeProblem();
            if (!IsDone)
            {
                PlayerQuest Quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);
                TreeRoot.GoalText = ((Quest != null) ? ("\"" + Quest.Name + "\"") : "In Progress");
            }
        }

        public List<WoWUnit> Mantid
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == MobIdMantid && !u.IsDead && u.Distance < 10000).OrderBy(u => u.Distance).ToList();
            }
        }

        public WoWObject Vehicle
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWObject>(true).Where(o => o.Entry == 64336).
                    OrderBy(o => o.Distance).FirstOrDefault();
            }
        }

        private static bool IsInVehicle
        {
            get { return Lua.GetReturnVal<bool>("return UnitInVehicle('player')", 0); }
        }
	
        public bool IsQuestComplete()
        {
            var quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);
            return quest == null || quest.IsCompleted;
        }

        public Composite DoneYet
        {
            get
            {
                return
                    new Decorator(ret => IsQuestComplete(), new Action(delegate
                    {
			Lua.DoString("CastPetAction(12)");
                        TreeRoot.StatusText = "Finished!";
                        _isBehaviorDone = true;
                        return RunStatus.Success;
                    }));
            }
        }

        public Composite KillOne
        {
            get
            {
                return new Decorator(r => !IsQuestComplete(), new Action(r =>
                {
			Lua.DoString("CastPetAction(2)");
			SpellManager.ClickRemoteLocation(Mantid[10].Location);
			Thread.Sleep(500);
			Lua.DoString("CastPetAction(1)");
			SpellManager.ClickRemoteLocation(Mantid[10].Location);
			Thread.Sleep(8000);
		}));
            }
        }
		
        protected override Composite CreateBehavior()
        {
            return _root ?? (_root = 
		new Decorator(ret => !_isBehaviorDone,
			new PrioritySelector(DoneYet, KillOne, new ActionAlwaysSucceed())
		));
        }
    }
}
