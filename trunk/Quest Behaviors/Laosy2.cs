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



namespace Blastranaar
{
    public class Blastranaar : CustomForcedBehavior
    {
        public Blastranaar(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                QuestId = 31758;//GetAttributeAsQuestId("QuestId", true, null) ?? 0;
            }
            catch
            {
                Logging.Write("Problem parsing a QuestId in behavior: Laosy Scouting");
            }
        }
        public int QuestId { get; set; }
        private bool _isBehaviorDone;
        public int MobIdLao = 65868;
        private Composite _root;
        public Dictionary<string, WoWPoint> SearchLocation = new Dictionary<string, WoWPoint>();
        public QuestCompleteRequirement questCompleteRequirement = QuestCompleteRequirement.NotComplete;
        public QuestInLogRequirement questInLogRequirement = QuestInLogRequirement.InLog;
		static public bool InVehicle { get { return Lua.GetReturnVal<int>("if IsPossessBarVisible() or UnitInVehicle('player') or not(GetBonusBarOffset()==0) then return 1 else return 0 end", 0) == 1; } }
        public override bool IsDone
        {
            get
            {
                var ret = (_isBehaviorDone     // normal completion
                    || !UtilIsProgressRequirementsMet(QuestId, questInLogRequirement, questCompleteRequirement));

                if (ret)
                    _isBehaviorDone = true;
                return ret;
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
                SearchLocation.Clear();
                // add some search WoWPoint's 
                //               ( LocationName, WoWPoint )
                SearchLocation.Add("Location 1", new WoWPoint(1551.653, 1237.705, 490.3626));
                SearchLocation.Add("Location 2", new WoWPoint(1656.856, 1360.797, 471.681));
                SearchLocation.Add("Location 3", new WoWPoint(1580.121, 1487.525, 459.9365));


                PlayerQuest Quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);
                TreeRoot.GoalText = ((Quest != null) ? ("\"" + Quest.Name + "\"") : "In Progress");
            }
        }

        public WoWUnit Lao
        {
            get
            {
                return ObjectManager.GetObjectsOfType<WoWUnit>().Where(u => u.Entry == MobIdLao && !u.IsDead && u.Distance < 10000).OrderBy(u => u.Distance).FirstOrDefault();
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
                    new Decorator(ret => IsDone || SearchLocation.Keys.Count <= 0 || IsObjectiveComplete(1, (uint)QuestId), new Action(delegate
                    {
                        TreeRoot.StatusText = "Finished!";
                        _isBehaviorDone = true;
                        return RunStatus.Success;
                    }));

            }
        }

        public Composite LaoMove {
		get {
            return
                new Decorator(ret => !IsObjectiveComplete(1, (uint)QuestId), new PrioritySelector(

                    new Decorator(ret => Lao != null,
                        new PrioritySelector(

                            new Decorator(ret => Lao.Distance >= 10,
                                new Sequence(
                                    new ActionSetActivity("Got [" + Lao.Name + "], moving to him"),
                                    new Action(ret => Flightor.MoveTo(Lao.Location))
                                )),

                            new Decorator(ret => Lao.Distance < 10,
                                new Sequence(
                                    new ActionSetActivity("Finished!"),
                                    new Action(ret => _isBehaviorDone = true),
                                    new Action(ret => WoWMovement.MoveStop())
                                )))),


                    new Decorator(ret => Lao == null,
                        new PrioritySelector(

                            new Decorator(ret => SearchLocation.First().Value.Distance(StyxWoW.Me.Location) >= 5,
                                new Sequence(
                                    new ActionSetActivity((!string.IsNullOrEmpty(SearchLocation.First().Key)) ? "Flying to " + SearchLocation.First().Key : "Flying to " + SearchLocation.First().Value.ToString()),
                                    new Action(ret => Flightor.MoveTo(SearchLocation.First().Value))
                                )),

                            new Decorator(ret => SearchLocation.First().Value.Distance(StyxWoW.Me.Location) < 5,
                                new Action(ret => SearchLocation.Remove(SearchLocation.First().Key)))
                                ))));
		}
        }

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root = new Decorator(ret => !_isBehaviorDone, new PrioritySelector(DoneYet, LaoMove, new ActionAlwaysSucceed())));
        }
    }
}

