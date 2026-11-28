using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200185C RID: 6236
	public class CompDestroyAfterDelay : ThingComp
	{
		// Token: 0x17001AA5 RID: 6821
		// (get) Token: 0x06009C0F RID: 39951 RVA: 0x0038357E File Offset: 0x0038177E
		public int DestructionTick
		{
			get
			{
				return this.spawnTick + this.Props.delayTicks;
			}
		}

		// Token: 0x17001AA6 RID: 6822
		// (get) Token: 0x06009C10 RID: 39952 RVA: 0x00383592 File Offset: 0x00381792
		public int TicksLeft
		{
			get
			{
				return this.DestructionTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17001AA7 RID: 6823
		// (get) Token: 0x06009C11 RID: 39953 RVA: 0x003835A5 File Offset: 0x003817A5
		public CompProperties_DestroyAfterDelay Props
		{
			get
			{
				return (CompProperties_DestroyAfterDelay)this.props;
			}
		}

		// Token: 0x06009C12 RID: 39954 RVA: 0x003835B2 File Offset: 0x003817B2
		public override void CompTick()
		{
			base.CompTick();
			if (this.TicksLeft <= 0 && !this.parent.Destroyed)
			{
				this.parent.Destroy(this.Props.destroyMode);
			}
		}

		// Token: 0x06009C13 RID: 39955 RVA: 0x003835E8 File Offset: 0x003817E8
		public override string CompInspectStringExtra()
		{
			if (this.Props.countdownLabel.NullOrEmpty())
			{
				return "";
			}
			int numTicks = Mathf.Max(0, this.spawnTick + this.Props.delayTicks - Find.TickManager.TicksGame);
			return this.Props.countdownLabel + ": " + numTicks.ToStringSecondsFromTicks();
		}

		// Token: 0x06009C14 RID: 39956 RVA: 0x0038364C File Offset: 0x0038184C
		public override string TransformLabel(string label)
		{
			if (this.Props.displayCountdownOnLabel)
			{
				return base.TransformLabel(label) + " (" + this.TicksLeft.TicksToSeconds().ToString("F0") + "s)";
			}
			return base.TransformLabel(label);
		}

		// Token: 0x06009C15 RID: 39957 RVA: 0x0038369C File Offset: 0x0038189C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.spawnTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06009C16 RID: 39958 RVA: 0x003836C5 File Offset: 0x003818C5
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
		}

		// Token: 0x04005CC6 RID: 23750
		public int spawnTick;
	}
}
