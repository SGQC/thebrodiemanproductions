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

namespace Honorbuddy.Quest_Behaviors.SpecificQuests.ItsAlive
{
	[CustomBehaviorFileName(@"26257-ItsAlive")]
	public class ItsAlive : CustomForcedBehavior
	{
		public ItsAlive(Dictionary<string, string> args)
			: base(args)
		{
			try
			{
				QuestId = 26257;//GetAttributeAsQuestId("QuestId", true, null) ?? 0;
			}
			catch
			{
				Logging.Write("Problem parsing a QuestId in behavior: Its Alive");
			}
		}
		public int QuestId { get; set; }
		private bool _isBehaviorDone;
		private Composite _root;
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
				if (TreeRoot.Current != null && TreeRoot.Current.Root != null && TreeRoot.Current.Root.LastStatus != RunStatus.Running)
				{
					var currentRoot = TreeRoot.Current.Root;
					if (currentRoot is GroupComposite)
					{
						var root = (GroupComposite)currentRoot;
						root.InsertChild(0, CreateBehavior());
					}
				}
				PlayerQuest Quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);
				TreeRoot.GoalText = ((Quest != null) ? ("\"" + Quest.Name + "\"") : "In Progress");
			}
		}

		public List<WoWUnit> Harvester
		{
			get
			{
				return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => (u.Entry == 42342) && !u.IsDead && u.Distance < 200).OrderBy(u => u.Distance).ToList();
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
					new Decorator(ret => IsQuestComplete(), new Action(delegate
					{
						TreeRoot.StatusText = "Finished!";
						_isBehaviorDone = true;
						return RunStatus.Success;
					}));
			}
		}

		public Composite HarvesterKill
		{
			get
			{
				return
					new Decorator(ret => !IsQuestComplete(), new Action(c =>
					{
						TreeRoot.StatusText = "Moving to Attack";
						var hostile =
							ObjectManager.GetObjectsOfType<WoWUnit>().Where(r => 
								r.Entry != 42601 && r.GotTarget && r.CurrentTarget == Me.CharmedUnit).OrderBy(r=>
								r.Distance).FirstOrDefault();
						WoWUnit tar;
						if (hostile != null)
						{
							tar = hostile;
						}
						else if (Harvester.Count > 0)
						{
							tar = Harvester.FirstOrDefault();
						}
						else
						{
							Logging.Write("No viable targets, waiting.");
							return RunStatus.Failure;
						}

						if (tar.Location.Distance(Me.Location) > 10)
						{
							tar.Target();
							tar.Face();
							Lua.DoString("CastPetAction(2)");
						}
						else
						{
							tar.Target();
							tar.Face();
							Lua.DoString("CastPetAction(1)");
						}
						return RunStatus.Failure;
					}));
			}
		}

		protected override Composite CreateBehavior()
		{
			return _root ?? (_root = new Decorator(ret => !_isBehaviorDone,
				new PrioritySelector(
					new Decorator(context => !InVehicle,
						new Action(context => { _isBehaviorDone = true; })),
					DoneYet,
					HarvesterKill,
					new ActionAlwaysSucceed())));
		}
	}
}