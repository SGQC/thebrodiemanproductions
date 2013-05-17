using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

using CommonBehaviors.Actions;
using Styx;
using Styx.Common;
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


namespace Honorbuddy.Quest_Behaviors.SpecificQuests.PompfruitPickup
{
	[CustomBehaviorFileName(@"SpecificQuests\30231-VOEB-PompfruitPickup")]
	public class Pomfruit : CustomForcedBehavior
	{
		public Pomfruit(Dictionary<string, string> args)
			: base(args)
		{
			try
			{
				QuestId = 30231;//GetAttributeAsQuestId("QuestId", true, null) ?? 0;
			}
			catch
			{
				Logging.Write("Problem parsing a QuestId in behavior: Pompfruits Pickup");
			}
		}
		public int QuestId { get; set; }
		private bool _isBehaviorDone;
		public int MobIdPomfruit = 58767;
		public int PomharvestFireworkId = 79344;
		private Composite _root;
		public WoWPoint Location2 = new WoWPoint(1574.712, 1428.84, 484.7786);
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

		public List<WoWUnit> Fruit
		{
			get
			{
				return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == MobIdPomfruit && !u.IsDead && u.Distance < 10000).OrderBy(u => u.Distance).ToList();
			}
		}

		public WoWItem PomharvestFirework { get { return (StyxWoW.Me.CarriedItems.FirstOrDefault(i => i.Entry == PomharvestFireworkId)); } }
	
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

		protected override Composite CreateBehavior()
		{
			return _root ?? (_root = new Decorator(ret => !_isBehaviorDone, new PrioritySelector(
				DoneYet,
				new Decorator(ret => !IsObjectiveComplete(1, (uint)QuestId), 
					new Sequence(
						new DecoratorContinue(ret => Fruit[0].Location.Distance(Me.Location) > 3,
							new Sequence(
								new DecoratorContinue(ret => PomharvestFirework.Cooldown == 0,
									new Action(ret => PomharvestFirework.UseContainerItem())),
								new Action(ret => Flightor.MoveTo(Fruit[0].Location)))),
						new DecoratorContinue(ret => Fruit[0].Location.Distance(Me.Location) <= 3,
							new Sequence(
								new Action(r => WoWMovement.MoveStop()),
								new Action(r => Fruit[0].Interact()))))),
				new ActionAlwaysSucceed())));
		}
	}
}
