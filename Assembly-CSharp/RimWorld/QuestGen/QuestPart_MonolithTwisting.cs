using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020022AA RID: 8874
	public class QuestPart_MonolithTwisting : QuestPart_Alert_StructuresArriving
	{
		// Token: 0x0600DB19 RID: 56089 RVA: 0x00519AFF File Offset: 0x00517CFF
		public QuestPart_MonolithTwisting()
		{
		}

		// Token: 0x0600DB1A RID: 56090 RVA: 0x00519B07 File Offset: 0x00517D07
		public QuestPart_MonolithTwisting(string label, string explanation, string inSignalEnable, string inSignalDisable, int delay, Building_VoidMonolith monolith, int intensifiedDelay) : base(label, explanation, inSignalEnable, inSignalDisable, delay, monolith)
		{
			this.monolith = monolith;
			this.intensifiedDelay = intensifiedDelay;
		}

		// Token: 0x0600DB1B RID: 56091 RVA: 0x00519B2D File Offset: 0x00517D2D
		protected override void Enable(SignalArgs receivedArgs)
		{
			this.intensifyTick = Find.TickManager.TicksGame + this.intensifiedDelay;
			base.Enable(receivedArgs);
		}

		// Token: 0x0600DB1C RID: 56092 RVA: 0x00519B50 File Offset: 0x00517D50
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.effecter == null)
			{
				this.effecter = EffecterDefOf.MonolithTwisting.Spawn();
			}
			if (Find.TickManager.TicksGame >= this.intensifyTick && this.intenseEffecter == null)
			{
				this.intenseEffecter = EffecterDefOf.MonolithTwistingIntense.SpawnMaintained(this.monolith, this.monolith.MapHeld, 1f);
			}
			this.effecter.EffectTick(this.monolith, this.monolith);
		}

		// Token: 0x0600DB1D RID: 56093 RVA: 0x00519BDC File Offset: 0x00517DDC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Building_VoidMonolith>(ref this.monolith, "monolith", false);
			Scribe_Values.Look<int>(ref this.intensifyTick, "intensifyTick", 0, false);
		}

		// Token: 0x04008C3F RID: 35903
		private Building_VoidMonolith monolith;

		// Token: 0x04008C40 RID: 35904
		private int intensifyTick;

		// Token: 0x04008C41 RID: 35905
		private Effecter effecter;

		// Token: 0x04008C42 RID: 35906
		private Effecter intenseEffecter;

		// Token: 0x04008C43 RID: 35907
		private int intensifiedDelay;
	}
}
