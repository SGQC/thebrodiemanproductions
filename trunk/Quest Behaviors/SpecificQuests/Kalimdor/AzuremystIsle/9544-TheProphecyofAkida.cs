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

namespace Honorbuddy.Quest_Behaviors.TheProphecyofAkida
{
	[CustomBehaviorFileName(@"9544-TheProphecyofAkida")]
	public class TheProphecyofAkida : CustomForcedBehavior
	{
		public TheProphecyofAkida(Dictionary<string, string> args)
			: base(args)
		{
			try
			{
				QuestId = 9544;//GetAttributeAsQuestId("QuestId", true, null) ?? 0;
			}
			catch
			{
				Logging.Write("Problem parsing a QuestId in behavior: The Key to Success");
			}
		}
		public int QuestId { get; set; }
		private bool _isBehaviorDone;
		public int MobIdCaptiveA = 17375;
		private Composite _root;
		public WoWPoint Location1 = new WoWPoint(-4504.114, -11618.4, 11.21694);
		public WoWPoint Location2 = new WoWPoint(-4599.796, -11638.13, 17.64635);
		public WoWPoint Location3 = new WoWPoint(-4629.48, -11523.7, 18.53337);
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
					new Decorator(ret => IsObjectiveComplete(1, (uint)QuestId), new Action(delegate
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
					new Decorator(ret => !IsObjectiveComplete(1, (uint)QuestId), new PrioritySelector(
						new Decorator(ret => CaptiveA.Count > 0, new Action(c =>
			{
				TreeRoot.StatusText = "Got a Captive, moving to them!";
				CaptiveA[0].Target();
				Navigator.MoveTo(CaptiveA[0].Location);

				if(CaptiveA[0].Location.Distance(Me.Location) < 10)
				{
					TreeRoot.StatusText = "Finished!";
					_isBehaviorDone = true;
					return RunStatus.Success;
				}
				return RunStatus.Success;
			})),
						new Decorator(ret => CaptiveA.Count == 0, new PrioritySelector(
							new Decorator(ret => Location1.Distance(Me.Location) > 20  && Me.CurrentTarget == null, new Action(c =>
				{
				TreeRoot.StatusText = "Moving to 1st location";
				Navigator.MoveTo(Location1);
				if(CaptiveA.Count > 0)
				{
					CaptiveA[0].Target();
				}
				return RunStatus.Success;
				}
			  )),

							new Decorator(ret => Location2.Distance(Me.Location) > 20 && Me.CurrentTarget == null, new Action(c =>
				{
				TreeRoot.StatusText = "Moving to 2nd location";
				Navigator.MoveTo(Location2);
				if(CaptiveA.Count > 0)
				{
					CaptiveA[0].Target();
				}
				return RunStatus.Success;
				}
			  )),

							new Decorator(ret => Location3.Distance(Me.Location) > 20 && Me.CurrentTarget == null, new Action(c =>
				{
				TreeRoot.StatusText = "Moving to 3rd location";
				Navigator.MoveTo(Location3);
				if(CaptiveA.Count > 0)
				{
					CaptiveA[0].Target();
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

