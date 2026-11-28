using System;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020004AB RID: 1195
	public class Hediff_Deathrest : HediffWithComps
	{
		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06002125 RID: 8485 RVA: 0x000C5BA0 File Offset: 0x000C3DA0
		private Gene_Deathrest DeathrestGene
		{
			get
			{
				Gene_Deathrest result;
				if ((result = this.cachedGene) == null)
				{
					Pawn_GeneTracker genes = this.pawn.genes;
					result = (this.cachedGene = ((genes != null) ? genes.GetFirstGeneOfType<Gene_Deathrest>() : null));
				}
				return result;
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06002126 RID: 8486 RVA: 0x000C5BD7 File Offset: 0x000C3DD7
		public bool Paused
		{
			get
			{
				if (this.lastPauseCheckTick < Find.TickManager.TicksGame + 120)
				{
					this.lastPauseCheckTick = Find.TickManager.TicksGame;
					this.cachedPaused = SanguophageUtility.ShouldBeDeathrestingOrInComaInsteadOfDead(this.pawn);
				}
				return this.cachedPaused;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06002127 RID: 8487 RVA: 0x000C5C15 File Offset: 0x000C3E15
		public override string LabelInBrackets
		{
			get
			{
				if (this.Paused)
				{
					return base.LabelInBrackets;
				}
				return this.DeathrestGene.DeathrestPercent.ToStringPercent("F0");
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06002128 RID: 8488 RVA: 0x000C5C3C File Offset: 0x000C3E3C
		public override string TipStringExtra
		{
			get
			{
				string text = base.TipStringExtra;
				if (this.Paused)
				{
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text += "PawnWillKeepDeathrestingLethalInjuries".Translate(this.pawn.Named("PAWN")).Colorize(ColorLibrary.RedReadable);
				}
				return text;
			}
		}

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x06002129 RID: 8489 RVA: 0x000C5C98 File Offset: 0x000C3E98
		public override bool ShouldRemove
		{
			get
			{
				return this.DeathrestGene == null || base.ShouldRemove;
			}
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x000C5CAA File Offset: 0x000C3EAA
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			Gene_Deathrest deathrestGene = this.DeathrestGene;
			if (deathrestGene == null)
			{
				return;
			}
			deathrestGene.Notify_DeathrestStarted();
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x000C5CC4 File Offset: 0x000C3EC4
		public override void PostRemoved()
		{
			base.PostRemoved();
			Gene_Deathrest deathrestGene = this.DeathrestGene;
			if (deathrestGene != null)
			{
				deathrestGene.Notify_DeathrestEnded();
			}
			if (this.pawn.Spawned && this.pawn.CurJobDef == JobDefOf.Deathrest)
			{
				this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x000C5D1C File Offset: 0x000C3F1C
		public override void PostTickInterval(int delta)
		{
			base.PostTickInterval(delta);
			bool paused = this.Paused;
			Gene_Deathrest deathrestGene = this.DeathrestGene;
			if (deathrestGene == null)
			{
				return;
			}
			deathrestGene.TickDeathresting(paused, delta);
		}

		// Token: 0x04001959 RID: 6489
		private Gene_Deathrest cachedGene;

		// Token: 0x0400195A RID: 6490
		private int lastPauseCheckTick = -1;

		// Token: 0x0400195B RID: 6491
		private bool cachedPaused;

		// Token: 0x0400195C RID: 6492
		private const int PauseCheckInterval = 120;
	}
}
