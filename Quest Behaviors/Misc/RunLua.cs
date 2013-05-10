﻿// Behavior originally contributed by HighVoltz.
//
// DOCUMENTATION:
//     
//
using System;
using System.Collections.Generic;
using CommonBehaviors.Actions;
using Styx;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.CommonBot.Profiles;
using Styx.TreeSharp;
using Styx.WoWInternals;

using Action = Styx.TreeSharp.Action;


namespace Honorbuddy.Quest_Behaviors.RunLua
{
    /// <summary>
    /// Runs the lua script x amount of times waiting x milliseconds inbetween
    /// ##Syntax##           
    /// Lua: the lua script to run
    /// NumOfTimes: (Optional) - The number of times to execute this script. default:1
    /// QuestId: (Optional) - the quest to perform this action on
    /// WaitTime: (Optional) - The time in milliseconds to wait before executing the next. default: 0ms
    ///                         This is a Post-LUA delay.
    /// </summary>
    [CustomBehaviorFileName(@"Misc\RunLua")]
    public class RunLua : CustomForcedBehavior
    {
        public RunLua(Dictionary<string, string> args)
            : base(args)
        {
            try
            {
                // QuestRequirement* attributes are explained here...
                //    http://www.thebuddyforum.com/mediawiki/index.php?title=Honorbuddy_Programming_Cookbook:_QuestId_for_Custom_Behaviors
                // ...and also used for IsDone processing.
                LuaCommand = GetAttributeAs<string>("Lua", true, ConstrainAs.StringNonEmpty, null) ?? string.Empty;
                NumOfTimes = GetAttributeAsNullable<int>("NumOfTimes", false, ConstrainAs.RepeatCount, null) ?? 1;
                QuestId = GetAttributeAsNullable<int>("QuestId", false, ConstrainAs.QuestId(this), null) ?? 0;
                QuestRequirementComplete = GetAttributeAsNullable<QuestCompleteRequirement>("QuestCompleteRequirement", false, null, null) ?? QuestCompleteRequirement.NotComplete;
                QuestRequirementInLog = GetAttributeAsNullable<QuestInLogRequirement>("QuestInLogRequirement", false, null, null) ?? QuestInLogRequirement.InLog;
                WaitTime = GetAttributeAsNullable<int>("WaitTime", false, ConstrainAs.Milliseconds, null) ?? 0;
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
        private string LuaCommand { get; set; }
        private int NumOfTimes { get; set; }
        private int QuestId { get; set; }
        private QuestCompleteRequirement QuestRequirementComplete { get; set; }
        private QuestInLogRequirement QuestRequirementInLog { get; set; }
        private int WaitTime { get; set; }

        // Private variables for internal state
        private int _counter;
        private bool _isBehaviorDone;
        private bool _isDisposed;
        private Composite _root;
        private readonly WaitTimer _waitTimer = new WaitTimer(TimeSpan.Zero);

        // DON'T EDIT THESE--they are auto-populated by Subversion
        public override string SubversionId { get { return ("$Id: RunLua.cs 369 2013-03-18 09:05:58Z chinajade $"); } }
        public override string SubversionRevision { get { return ("$Revision: 369 $"); } }


        ~RunLua()
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
                TreeRoot.GoalText = string.Empty;
                TreeRoot.StatusText = string.Empty;

                // Call parent Dispose() (if it exists) here ...
                base.Dispose();
            }

            _isDisposed = true;
        }


        #region Overrides of CustomForcedBehavior

        protected override Composite CreateBehavior()
        {
            return _root ?? (_root = new PrioritySelector(
                // Wait for post-LUA timer to expire...
                new Decorator(context => !_waitTimer.IsFinished,
                    new ActionAlwaysSucceed()),

                // If we've met our completion count, we're done...
                new Decorator(context => _counter >= NumOfTimes,
                    new Action(context => { _isBehaviorDone = true; })),

                // Run the LUA command...
                new Action(c =>
                {
                    Lua.DoString(LuaCommand);
                    _counter++;
                    _waitTimer.WaitTime = TimeSpan.FromMilliseconds(WaitTime);
                    _waitTimer.Reset();
                }))
            );
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
                return (_isBehaviorDone     // normal completion
                        || !UtilIsProgressRequirementsMet(QuestId, QuestRequirementInLog, QuestRequirementComplete));
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
                PlayerQuest quest = StyxWoW.Me.QuestLog.GetQuestById((uint)QuestId);

                TreeRoot.GoalText = GetType().Name + ": " + ((quest != null) ? ("\"" + quest.Name + "\"") : "In Progress");
                TreeRoot.StatusText = string.Format("{0}: {1} {2} number of times while waiting {3} inbetween",
                                                    GetType().Name, LuaCommand, NumOfTimes, WaitTime);

                // NB: The _waitTimer is initialzed to zero, so there will be no 'initial delay'.
                // This is what the user expects.
            }
        }

        #endregion
    }
}
