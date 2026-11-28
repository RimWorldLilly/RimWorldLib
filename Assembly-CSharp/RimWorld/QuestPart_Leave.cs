using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010CA RID: 4298
	public class QuestPart_Leave : QuestPart
	{
		// Token: 0x17001263 RID: 4707
		// (get) Token: 0x06006B6F RID: 27503 RVA: 0x0025AA92 File Offset: 0x00258C92
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_Leave.<get_QuestLookTargets>d__7 <get_QuestLookTargets>d__ = new QuestPart_Leave.<get_QuestLookTargets>d__7(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006B70 RID: 27504 RVA: 0x0025AAA4 File Offset: 0x00258CA4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			if (signal.tag == this.inSignal)
			{
				LeaveQuestPartUtility.MakePawnsLeave(this.pawns, this.sendStandardLetter, this.quest, this.wakeUp);
			}
		}

		// Token: 0x06006B71 RID: 27505 RVA: 0x0025AB2B File Offset: 0x00258D2B
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.leaveOnCleanup)
			{
				LeaveQuestPartUtility.MakePawnsLeave(this.pawns, this.sendStandardLetter, this.quest, this.wakeUp);
			}
		}

		// Token: 0x06006B72 RID: 27506 RVA: 0x0025AB58 File Offset: 0x00258D58
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
			Scribe_Values.Look<bool>(ref this.leaveOnCleanup, "leaveOnCleanup", false, false);
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			Scribe_Values.Look<bool>(ref this.wakeUp, "wakeUp", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06006B73 RID: 27507 RVA: 0x0025AC10 File Offset: 0x00258E10
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				Map randomPlayerHomeMap = Find.RandomPlayerHomeMap;
				if (randomPlayerHomeMap.mapPawns.FreeColonistsCount != 0)
				{
					this.pawns.Add(randomPlayerHomeMap.mapPawns.FreeColonists.First<Pawn>());
				}
			}
		}

		// Token: 0x06006B74 RID: 27508 RVA: 0x0025AC75 File Offset: 0x00258E75
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04004447 RID: 17479
		public string inSignal;

		// Token: 0x04004448 RID: 17480
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04004449 RID: 17481
		public bool sendStandardLetter = true;

		// Token: 0x0400444A RID: 17482
		public bool leaveOnCleanup = true;

		// Token: 0x0400444B RID: 17483
		public string inSignalRemovePawn;

		// Token: 0x0400444C RID: 17484
		public bool wakeUp;
	}
}
