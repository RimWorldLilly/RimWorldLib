using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022A9 RID: 8873
	public class QuestPart_Alert_StructuresArriving : QuestPart_Alert
	{
		// Token: 0x1700262D RID: 9773
		// (get) Token: 0x0600DB11 RID: 56081 RVA: 0x005199DB File Offset: 0x00517BDB
		private int TicksRemaining
		{
			get
			{
				return this.arriveTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x1700262E RID: 9774
		// (get) Token: 0x0600DB12 RID: 56082 RVA: 0x005199EE File Offset: 0x00517BEE
		public override string AlertLabel
		{
			get
			{
				return base.AlertLabel + ": " + this.TicksRemaining.ToStringTicksToPeriod(false, false, true, true, false);
			}
		}

		// Token: 0x0600DB13 RID: 56083 RVA: 0x00519A10 File Offset: 0x00517C10
		public QuestPart_Alert_StructuresArriving()
		{
		}

		// Token: 0x0600DB14 RID: 56084 RVA: 0x00519A18 File Offset: 0x00517C18
		public QuestPart_Alert_StructuresArriving(string label, string explanation, string inSignalEnable, string inSignalDisable, int delay, LookTargets lookTargets = null)
		{
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				this.label = x;
			}, QuestGenUtility.MergeRules(null, label, "root"));
			QuestGen.AddTextRequest("root", delegate(string x)
			{
				this.explanation = x;
			}, QuestGenUtility.MergeRules(null, explanation, "root"));
			this.inSignalEnable = inSignalEnable;
			this.inSignalDisable = inSignalDisable;
			this.delay = delay;
			this.lookTargets = lookTargets;
		}

		// Token: 0x0600DB15 RID: 56085 RVA: 0x00519A8E File Offset: 0x00517C8E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.arriveTick, "arriveTick", 0, false);
			Scribe_Values.Look<int>(ref this.delay, "delay", 0, false);
		}

		// Token: 0x0600DB16 RID: 56086 RVA: 0x00519ABA File Offset: 0x00517CBA
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignalEnable)
			{
				this.arriveTick = Find.TickManager.TicksGame + this.delay;
			}
			base.Notify_QuestSignalReceived(signal);
		}

		// Token: 0x04008C3D RID: 35901
		private int arriveTick;

		// Token: 0x04008C3E RID: 35902
		private int delay;
	}
}
