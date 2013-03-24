using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CommonBehaviors.Actions;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.CommonBot.Profiles;
using Styx.Pathing;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace KeyToSuccess
{
    public class KeyToSuccess : CustomForcedBehavior
    {
        public KeyToSuccess(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                QuestId = 30300;//GetAttributeAsQuestId("QuestId", true, null) ?? 0;
            }
            catch
            {
                Logging.Write("Problem parsing a QuestId in behavior: The Key to Success");
            }
        }
        public int QuestId { get; set; }
        private bool _isBehaviorDone;
        public int MobIdCaptiveA = 63640;
        public int MobIdCaptiveB = 63654;
        public int MobIdCaptiveC = 63652;
        private Composite _root;
        public WoWPoint Location1 = new WoWPoint(1443.576, 1934.783, 313.7074);
        public WoWPoint Location2 = new WoWPoint(1453.100, 1843.182, 313.6932);
        public WoWPoint Location3 = new WoWPoint(1271.305, 1851.202, 363.7336);
        public WoWPoint Location4 = new WoWPoint(1405.447, 1691.386, 358.1148);
        public QuestCompleteRequirement questCompleteRequirement = QuestCompleteRequirement.NotComplete;
        public QuestInLogRequirement questInLogRequirement = QuestInLogRequirement.InLog;
		static public bool InVehicle { get { return Lua.GetReturnVal<int>("if IsPossessBarVisible() or UnitInVehicle('player') or not(GetBonusBarOffset()==0) then return 1 else return 0 end", 0) == 1; } }
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

        public List<WoWUnit> CaptiveA
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == MobIdCaptiveA && !u.IsDead && u.Distance < 10000).OrderBy(u => u.Distance).ToList();
            }
        }

        public List<WoWUnit> CaptiveB
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == MobIdCaptiveB && !u.IsDead && u.Distance < 10000).OrderBy(u => u.Distance).ToList();
            }
        }

        public List<WoWUnit> CaptiveC
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == MobIdCaptiveC && !u.IsDead && u.Distance < 10000).OrderBy(u => u.Distance).ToList();
            }
        }

	
        public bool IsQuestComplete()
        {
            var quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);
            return quest == null || quest.IsCompleted;
        }
        private bool IsObjectiveComplete(int objectiveId, uint questId)
        {
            if (Me.QuestLog.GetQuestById(questId) == null)
            {
                return false;
            }
            int returnVal = Lua.GetReturnVal<int>("return GetQuestLogIndexByID(" + questId + ")", 0);
            return
                Lua.GetReturnVal<bool>(
                    string.Concat(new object[] { "return GetQuestLogLeaderBoard(", objectiveId, ",", returnVal, ")" }), 2);
        }

        public Composite DoneYet
        {
            get
            {
                return
                    new Decorator(ret => IsObjectiveComplete(2, (uint)QuestId), new Action(delegate
                    {
                        TreeRoot.StatusText = "Finished!";
                        _isBehaviorDone = true;
                        return RunStatus.Success;
                    }));
            }
        }

        public Composite CaptiveMove
        {
            get
            {
                return
                    new Decorator(ret => !IsObjectiveComplete(2, (uint)QuestId), new PrioritySelector(
                    	new Decorator(ret => CaptiveA.Count > 0, new Action(c =>
			{
				TreeRoot.StatusText = "Got a Captive, moving to them!";
				CaptiveA[0].Target();
				Flightor.MoveTo(CaptiveA[0].Location);

				if(CaptiveA[0].Location.Distance(Me.Location) < 10)
				{
					TreeRoot.StatusText = "Finished!";
					_isBehaviorDone = true;
					return RunStatus.Success;
				}

				return RunStatus.Success;
			})),
                    	new Decorator(ret => CaptiveB.Count > 0, new Action(c =>
			{
				TreeRoot.StatusText = "Got a Captive, moving to them!";
				CaptiveB[0].Target();
				Flightor.MoveTo(CaptiveB[0].Location);

				if(CaptiveB[0].Location.Distance(Me.Location) < 10)
				{
					TreeRoot.StatusText = "Finished!";
					_isBehaviorDone = true;
					return RunStatus.Success;
				}

				return RunStatus.Success;
			})),
						new Decorator(ret => CaptiveC.Count > 0, new Action(c =>
			{
				TreeRoot.StatusText = "Got a Captive, moving to them!";
				CaptiveC[0].Target();
				Flightor.MoveTo(CaptiveC[0].Location);

				if(CaptiveC[0].Location.Distance(Me.Location) < 10)
				{
					TreeRoot.StatusText = "Finished!";
					_isBehaviorDone = true;
					return RunStatus.Success;
				}
				
				return RunStatus.Success;
			})),
						new Decorator(ret => CaptiveA.Count == 0 && CaptiveB.Count == 0 && CaptiveC.Count == 0, new PrioritySelector(
							new Decorator(ret => Location1.Distance(Me.Location) > 20  && Me.CurrentTarget == null, new Action(c =>
				{
				TreeRoot.StatusText = "Moving to 1st location";
				Flightor.MoveTo(Location1);
				if(CaptiveA.Count > 0)
				{
					CaptiveA[0].Target();
			    }
			    if(CaptiveB.Count > 0)
				{
					CaptiveB[0].Target();
				}
			    if(CaptiveC.Count > 0)
				{
					CaptiveC[0].Target();
				}
				return RunStatus.Success;

			  }

			  )),

							new Decorator(ret => Location2.Distance(Me.Location) > 20 && Me.CurrentTarget == null, new Action(c =>
				{
				TreeRoot.StatusText = "Moving to 2nd location";
				Flightor.MoveTo(Location2);
			    if(CaptiveA.Count > 0)
			    {
					CaptiveA[0].Target();
			    }
			    if(CaptiveB.Count > 0)
			    {
					CaptiveB[0].Target();
			    }
			    if(CaptiveC.Count > 0)
			    {
					CaptiveC[0].Target();
			    }
				return RunStatus.Success;

			  }

			  )),

							new Decorator(ret => Location3.Distance(Me.Location) > 20 && Me.CurrentTarget == null, new Action(c =>
				{
				TreeRoot.StatusText = "Moving to 3rd location";
				Flightor.MoveTo(Location3);
			    if(CaptiveA.Count > 0)
			    {
					CaptiveA[0].Target();
			    }
			    if(CaptiveB.Count > 0)
			    {
					CaptiveB[0].Target();
			    }
			    if(CaptiveC.Count > 0)
			    {
					CaptiveC[0].Target();
			    }
				return RunStatus.Success;
				}

			  )),
							new Decorator(ret => Location4.Distance(Me.Location) > 20 && Me.CurrentTarget == null, new Action(c =>
				{
				TreeRoot.StatusText = "Moving to 4th location";
				Flightor.MoveTo(Location4);
			    if(CaptiveA.Count > 0)
			    {
					CaptiveA[0].Target();
			    }
			    if(CaptiveB.Count > 0)
			    {
					CaptiveB[0].Target();
			    }
			    if(CaptiveC.Count > 0)
			    {
					CaptiveC[0].Target();
			    }
				return RunStatus.Success;
				}
			))))));
            }
        }
	
        protected override Composite CreateBehavior()
        {
            return _root ?? (_root = new Decorator(ret => !_isBehaviorDone, new PrioritySelector(DoneYet, CaptiveMove, new ActionAlwaysSucceed())));
        }
    }
}

