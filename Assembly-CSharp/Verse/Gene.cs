using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020003DD RID: 989
	public class Gene : IExposable, ILoadReferenceable
	{
		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001D16 RID: 7446 RVA: 0x000B2179 File Offset: 0x000B0379
		public virtual string Label
		{
			get
			{
				return this.def.label;
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001D17 RID: 7447 RVA: 0x000B2186 File Offset: 0x000B0386
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001D18 RID: 7448 RVA: 0x000B2193 File Offset: 0x000B0393
		public bool Overridden
		{
			get
			{
				return this.overriddenByGene != null;
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001D19 RID: 7449 RVA: 0x000B21A0 File Offset: 0x000B03A0
		public virtual bool Active
		{
			get
			{
				if (this.Overridden)
				{
					return false;
				}
				Pawn pawn = this.pawn;
				if (((pawn != null) ? pawn.ageTracker : null) != null && (float)this.pawn.ageTracker.AgeBiologicalYears < this.def.minAgeActive)
				{
					return false;
				}
				Pawn pawn2 = this.pawn;
				return ((pawn2 != null) ? pawn2.mutant : null) == null || !this.pawn.mutant.Def.disablesGenes.Contains(this.def);
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001D1A RID: 7450 RVA: 0x000B2224 File Offset: 0x000B0424
		public IEnumerable<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				Gene.<get_DisabledWorkTypes>d__14 <get_DisabledWorkTypes>d__ = new Gene.<get_DisabledWorkTypes>d__14(-2);
				<get_DisabledWorkTypes>d__.<>4__this = this;
				return <get_DisabledWorkTypes>d__;
			}
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x000B2234 File Offset: 0x000B0434
		public virtual void PostMake()
		{
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x000B2236 File Offset: 0x000B0436
		public virtual void PostAdd()
		{
			if (this.def.HasDefinedGraphicProperties)
			{
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x000B225A File Offset: 0x000B045A
		public virtual void PostRemove()
		{
			if (this.def.HasDefinedGraphicProperties)
			{
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x000B227E File Offset: 0x000B047E
		public virtual void Tick()
		{
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x000B2280 File Offset: 0x000B0480
		public virtual void TickInterval(int delta)
		{
			if (ModsConfig.BiotechActive && this.def.mentalBreakMtbDays > 0f && this.def.mentalBreakDef != null && this.pawn.Spawned && this.pawn.IsHashIntervalTick(60, delta) && !this.pawn.InMentalState && !this.pawn.Downed && Rand.MTBEventOccurs(this.def.mentalBreakMtbDays, 60000f, 60f) && this.def.mentalBreakDef.Worker.BreakCanOccur(this.pawn))
			{
				this.def.mentalBreakDef.Worker.TryStart(this.pawn, "MentalStateReason_Gene".Translate() + ": " + this.LabelCap, false);
			}
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x000B2376 File Offset: 0x000B0576
		public void OverrideBy(Gene overriddenBy)
		{
			if (ModsConfig.BiotechActive)
			{
				this.overriddenByGene = overriddenBy;
			}
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x000B2388 File Offset: 0x000B0588
		public Passion NewPassionForOnRemoval(SkillRecord skillRecord)
		{
			if (this.def.passionMod == null)
			{
				return skillRecord.passion;
			}
			PassionMod.PassionModType modType = this.def.passionMod.modType;
			if (modType != PassionMod.PassionModType.AddOneLevel)
			{
				if (modType == PassionMod.PassionModType.DropAll)
				{
					if (this.passionPreAdd != null)
					{
						return this.passionPreAdd.Value;
					}
					return skillRecord.passion;
				}
			}
			else
			{
				Passion passion = skillRecord.passion;
				if (passion == Passion.Minor)
				{
					return Passion.None;
				}
				if (passion == Passion.Major)
				{
					return Passion.Minor;
				}
			}
			return skillRecord.passion;
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x000B23FD File Offset: 0x000B05FD
		public virtual void Notify_IngestedThing(Thing thing, int numTaken)
		{
		}

		// Token: 0x06001D23 RID: 7459 RVA: 0x000B23FF File Offset: 0x000B05FF
		public virtual void Notify_NewColony()
		{
		}

		// Token: 0x06001D24 RID: 7460 RVA: 0x000B2401 File Offset: 0x000B0601
		public virtual void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			if (this.def.deathHistoryEvent != null)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(this.def.deathHistoryEvent), true);
			}
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x000B242B File Offset: 0x000B062B
		public virtual void Reset()
		{
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x000B242D File Offset: 0x000B062D
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			return null;
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x000B2430 File Offset: 0x000B0630
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			return null;
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x000B2433 File Offset: 0x000B0633
		public string GetUniqueLoadID()
		{
			return "Gene_" + this.loadID.ToString();
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x000B244C File Offset: 0x000B064C
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<GeneDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", true);
			Scribe_References.Look<Gene>(ref this.overriddenByGene, "overriddenByGene", false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<Passion?>(ref this.passionPreAdd, "passionPreAdd", null, false);
		}

		// Token: 0x04001767 RID: 5991
		public GeneDef def;

		// Token: 0x04001768 RID: 5992
		public Pawn pawn;

		// Token: 0x04001769 RID: 5993
		public int loadID;

		// Token: 0x0400176A RID: 5994
		public Gene overriddenByGene;

		// Token: 0x0400176B RID: 5995
		public Passion? passionPreAdd;
	}
}
