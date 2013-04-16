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

namespace Honorbuddy.Quest_Behaviors.SpecificQuests.ThunderingSkies
{
	[CustomBehaviorFileName(@"SpecificQuests\30310-VOEB-ThunderingSkies")]
    public class ThunderingSkies : CustomForcedBehavior
    {
        public ThunderingSkies(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                QuestId = 30310;//GetAttributeAsQuestId("QuestId", true, null) ?? 0;
                SpellIds = GetNumberedAttributesAsArray<int>("SpellId", 1, ConstrainAs.SpellId, null);
                //SpellId = GetAttributeAsNullable<int>("SpellId", false, ConstrainAs.SpellId, null) ?? 0;
                SpellId = SpellIds.FirstOrDefault(id => SpellManager.HasSpell(id));
            }
            catch
            {
                Logging.Write("Problem parsing a QuestId in behavior: Thundering Skies");
            }
        }
        public int QuestId { get; set; }
        private bool _isBehaviorDone;
        public int MobIdSerpent = 59158;
        public int[] SpellIds { get; private set; }
        public int SpellId { get; private set; }
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

        public WoWSpell CurrentBehaviorSpell
        {
            get
            {
                return WoWSpell.FromId(SpellId);
            }
        }

        public List<WoWUnit> Serpent
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == MobIdSerpent && !u.IsDead && u.Distance < 10000).OrderBy(u => u.Distance).ToList();
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

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root = new Decorator(ret => !_isBehaviorDone, new PrioritySelector(
			DoneYet,

			new DecoratorContinue(ret => !IsObjectiveComplete(1, (uint)QuestId),
				new Sequence(  
					new DecoratorContinue(ret => Serpent[0].Location.Distance(Me.Location) > 30,
						new Sequence(
							new Action(ret => Navigator.MoveTo(Serpent[0].Location))
						)
					),
					new DecoratorContinue(ret => Serpent[0].Location.Distance(Me.Location) <= 30,
						new Sequence(
							new Action(r => WoWMovement.MoveStop()),
							new Action(r => Serpent[0].Face()),
							new Action(r => SpellManager.Cast(SpellId))
						)
					)
				)
			),
			new ActionAlwaysSucceed()
			)));
        }
    }
}
