using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010D1 RID: 4305
	public class QuestPart_Message : QuestPart
	{
		// Token: 0x1700126B RID: 4715
		// (get) Token: 0x06006B9F RID: 27551 RVA: 0x0025C0D3 File Offset: 0x0025A2D3
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_Message.<get_QuestLookTargets>d__8 <get_QuestLookTargets>d__ = new QuestPart_Message.<get_QuestLookTargets>d__8(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006BA0 RID: 27552 RVA: 0x0025C0E4 File Offset: 0x0025A2E4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				LookTargets lookTargets = this.lookTargets;
				if (this.getLookTargetsFromSignal && !lookTargets.IsValid())
				{
					SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out lookTargets);
				}
				TaggedString formattedText = signal.args.GetFormattedText(this.message);
				if (!formattedText.NullOrEmpty())
				{
					Messages.Message(formattedText, lookTargets, this.messageType ?? MessageTypeDefOf.NeutralEvent, this.quest.hidden ? null : this.quest, this.historical);
				}
			}
		}

		// Token: 0x06006BA1 RID: 27553 RVA: 0x0025C190 File Offset: 0x0025A390
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.message, "message", null, false);
			Scribe_Defs.Look<MessageTypeDef>(ref this.messageType, "messageType");
			Scribe_Deep.Look<LookTargets>(ref this.lookTargets, "lookTargets", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.historical, "historical", true, false);
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", true, false);
			Scribe_Values.Look<bool>(ref this.addQuestLookTargets, "addQuestLookTargets", true, false);
		}

		// Token: 0x06006BA2 RID: 27554 RVA: 0x0025C224 File Offset: 0x0025A424
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.message = "DEV: Test";
			this.messageType = MessageTypeDefOf.PositiveEvent;
		}

		// Token: 0x0400446A RID: 17514
		public string inSignal;

		// Token: 0x0400446B RID: 17515
		public string message;

		// Token: 0x0400446C RID: 17516
		public MessageTypeDef messageType;

		// Token: 0x0400446D RID: 17517
		public LookTargets lookTargets;

		// Token: 0x0400446E RID: 17518
		public bool historical = true;

		// Token: 0x0400446F RID: 17519
		public bool getLookTargetsFromSignal = true;

		// Token: 0x04004470 RID: 17520
		public bool addQuestLookTargets = true;
	}
}
