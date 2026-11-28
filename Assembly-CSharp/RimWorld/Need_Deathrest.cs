using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200156F RID: 5487
	public class Need_Deathrest : Need
	{
		// Token: 0x170016DE RID: 5854
		// (get) Token: 0x06008694 RID: 34452 RVA: 0x0030A7A4 File Offset: 0x003089A4
		public bool Deathresting
		{
			get
			{
				return Find.TickManager.TicksGame <= this.lastDeathrestTick + this.pawn.UpdateRateTicks;
			}
		}

		// Token: 0x170016DF RID: 5855
		// (get) Token: 0x06008695 RID: 34453 RVA: 0x0030A7C7 File Offset: 0x003089C7
		private Gene_Deathrest DeathrestGene
		{
			get
			{
				if (this.cachedDeathrestGene == null)
				{
					Pawn_GeneTracker genes = this.pawn.genes;
					this.cachedDeathrestGene = ((genes != null) ? genes.GetFirstGeneOfType<Gene_Deathrest>() : null);
				}
				return this.cachedDeathrestGene;
			}
		}

		// Token: 0x170016E0 RID: 5856
		// (get) Token: 0x06008696 RID: 34454 RVA: 0x0030A7F4 File Offset: 0x003089F4
		public override int GUIChangeArrow
		{
			get
			{
				if (this.IsFrozen)
				{
					return 0;
				}
				if (!this.Deathresting)
				{
					return -1;
				}
				return 1;
			}
		}

		// Token: 0x06008697 RID: 34455 RVA: 0x0030A80B File Offset: 0x00308A0B
		public Need_Deathrest(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>
			{
				0.1f
			};
		}

		// Token: 0x06008698 RID: 34456 RVA: 0x0030A835 File Offset: 0x00308A35
		public override void SetInitialLevel()
		{
			this.CurLevel = 1f;
		}

		// Token: 0x06008699 RID: 34457 RVA: 0x0030A844 File Offset: 0x00308A44
		public override void NeedInterval()
		{
			if (this.IsFrozen)
			{
				return;
			}
			float curLevel = this.CurLevel;
			float num;
			if (!this.Deathresting)
			{
				num = -0.033333335f;
			}
			else
			{
				float num2 = 0.2f;
				Gene_Deathrest deathrestGene = this.DeathrestGene;
				num = num2 * ((deathrestGene != null) ? deathrestGene.DeathrestEfficiency : 1f);
			}
			this.CurLevel = curLevel + num / 400f;
			this.CheckForStateChange();
		}

		// Token: 0x0600869A RID: 34458 RVA: 0x0030A8A0 File Offset: 0x00308AA0
		private void CheckForStateChange()
		{
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DeathrestExhaustion, false);
			if (firstHediffOfDef != null && this.CurLevel > 0f)
			{
				firstHediffOfDef.Severity = 0f;
				return;
			}
			if (this.CurLevel == 0f)
			{
				Gene_Deathrest deathrestGene = this.DeathrestGene;
				if (deathrestGene != null)
				{
					deathrestGene.RemoveOldDeathrestBonuses();
				}
				this.pawn.health.AddHediff(HediffDefOf.DeathrestExhaustion, null, null, null);
			}
		}

		// Token: 0x0600869B RID: 34459 RVA: 0x0030A924 File Offset: 0x00308B24
		public override string GetTipString()
		{
			string str = (base.LabelCap + ": " + base.CurLevelPercentage.ToStringPercent()).Colorize(ColoredText.TipSectionTitleColor) + "\n";
			if (!this.Deathresting)
			{
				if (base.CurLevelPercentage > 0.1f)
				{
					float num = (base.CurLevelPercentage - 0.1f) / 0.033333335f;
					str += "NextDeathrestNeed".Translate(this.pawn.Named("PAWN"), "PeriodDays".Translate(num.ToString("F1")).Named("DURATION")).Resolve().CapitalizeFirst();
				}
				else
				{
					str += "PawnShouldDeathrestNow".Translate(this.pawn.Named("PAWN")).CapitalizeFirst().Colorize(ColorLibrary.RedReadable);
				}
				str += "\n\n";
			}
			return str + this.def.description;
		}

		// Token: 0x0600869C RID: 34460 RVA: 0x0030AA36 File Offset: 0x00308C36
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastDeathrestTick, "lastDeathrestTick", -999, false);
		}

		// Token: 0x040052F2 RID: 21234
		public int lastDeathrestTick = -999;

		// Token: 0x040052F3 RID: 21235
		[Unsaved(false)]
		private Gene_Deathrest cachedDeathrestGene;

		// Token: 0x040052F4 RID: 21236
		public const float LevelForAlert = 0.1f;

		// Token: 0x040052F5 RID: 21237
		public const float FallPerDay = 0.033333335f;

		// Token: 0x040052F6 RID: 21238
		public const float GainPerDayDeathresting = 0.2f;

		// Token: 0x040052F7 RID: 21239
		private const float Interval = 400f;

		// Token: 0x040052F8 RID: 21240
		public const float HemogenGainPerDayDeathrest = 0.08f;
	}
}
