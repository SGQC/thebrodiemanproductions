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

namespace Honorbuddy.Quest_Behaviors.SpecificQuests.FranksAndBeans
{
	[CustomBehaviorFileName(@"26506-FranksAndBeans")]
	public class FranksAndBeans : CustomForcedBehavior
	{
		public FranksAndBeans(Dictionary<string, string> args)
			: base(args)
		{
			try
			{
				QuestId = 26506;//GetAttributeAsQuestId("QuestId", true, null) ?? 0;
				SpellIds = GetNumberedAttributesAsArray<int>("SpellId", 1, ConstrainAs.SpellId, null);
				//SpellId = GetAttributeAsNullable<int>("SpellId", false, ConstrainAs.SpellId, null) ?? 0;
				SpellId = SpellIds.FirstOrDefault(id => SpellManager.HasSpell(id));
			}
			catch
			{
				Logging.Write("Problem parsing a QuestId in behavior: Franks and Beans");
			}
		}
		public int QuestId { get; set; }
		private bool _isBehaviorDone;
		public int MobIdHawk = 428;
		public int[] SpellIds { get; private set; }
		public int SpellId { get; private set; }
		private Composite _root;
		public WoWPoint Location1 = new WoWPoint(-9632.205, -2009.591, 63.37442);
		public WoWPoint Location2 = new WoWPoint(-9519.535, -1928.543, 75.38841);
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

		public WoWSpell CurrentBehaviorSpell
		{
			get
			{
				return WoWSpell.FromId(SpellId);
			}
		}

		public List<WoWUnit> Hawk
		{
			get
			{
				return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == MobIdHawk && !u.IsDead && u.Distance < 50).OrderBy(u => u.Distance).ToList();
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
		
		public Composite HawkPull
		{
			get
			{
				return
					new Decorator(ret => !IsObjectiveComplete(2, (uint)QuestId), new PrioritySelector(
						new Decorator(ret => Hawk.Count > 0, new Action(c =>
						{
							TreeRoot.StatusText = "Pulling Dire Condor";
							Hawk[0].Target();
							if(Hawk[0].Location.Distance(Me.Location) > 30)
							{
								Navigator.MoveTo(Hawk[0].Location);
							}
							if(Hawk[0].Location.Distance(Me.Location) < 30)
							{
								Hawk[0].Face();
								Thread.Sleep(1000);
								SpellManager.Cast(SpellId);
								Thread.Sleep(1000);
								return RunStatus.Success;
							}
							TreeRoot.StatusText = "Finished!";
							_isBehaviorDone = true;
							return RunStatus.Success;
						})),
						new Decorator(ret => Hawk.Count == 0, new PrioritySelector(
							new Decorator(ret => Location1.Distance(Me.Location) > 30  && Me.CurrentTarget == null, new Action(c =>
							{
								TreeRoot.StatusText = "Moving to 1st location";
								Navigator.MoveTo(Location1);
								if(Hawk.Count > 0)
								{
									Hawk[0].Target();
									Hawk[0].Face();
									Thread.Sleep(1000);
									SpellManager.Cast(SpellId);
									Thread.Sleep(1000);
									return RunStatus.Success;
								}
								return RunStatus.Success;
							})),
							new Decorator(ret => Location2.Distance(Me.Location) > 50 && Me.CurrentTarget == null, new Action(c =>
							{
								TreeRoot.StatusText = "Moving to 2nd location";
								Navigator.MoveTo(Location2);
								if(Hawk.Count > 0)
								{
									Hawk[0].Target();
									Hawk[0].Face();
									Thread.Sleep(1000);
									SpellManager.Cast(SpellId);
									Thread.Sleep(1000);
									return RunStatus.Success;
								}
								return RunStatus.Success;
							}
							))))));
			}
		}
		
		protected override Composite CreateBehavior()
		{
			return _root ?? (_root = new Decorator(ret => !_isBehaviorDone, new PrioritySelector(DoneYet, HawkPull, new ActionAlwaysSucceed())));
		}
	}
}

