using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B1B RID: 6939
	public class Genepack : GeneSetHolderBase
	{
		// Token: 0x17001FFA RID: 8186
		// (get) Token: 0x0600B187 RID: 45447 RVA: 0x003E904D File Offset: 0x003E724D
		public override string LabelNoCount
		{
			get
			{
				if (this.geneSet != null && this.geneSet.Label != "ERR")
				{
					return this.geneSet.Label;
				}
				return base.LabelNoCount;
			}
		}

		// Token: 0x17001FFB RID: 8187
		// (get) Token: 0x0600B188 RID: 45448 RVA: 0x003E9080 File Offset: 0x003E7280
		private CompGenepackContainer ParentContainer
		{
			get
			{
				IThingHolder parentHolder = base.ParentHolder;
				if (parentHolder != null)
				{
					CompGenepackContainer compGenepackContainer = parentHolder as CompGenepackContainer;
					if (compGenepackContainer != null)
					{
						return compGenepackContainer;
					}
				}
				return null;
			}
		}

		// Token: 0x17001FFC RID: 8188
		// (get) Token: 0x0600B189 RID: 45449 RVA: 0x003E90A4 File Offset: 0x003E72A4
		public bool Deteriorating
		{
			get
			{
				CompGenepackContainer parentContainer = this.ParentContainer;
				return parentContainer == null || !parentContainer.PowerOn;
			}
		}

		// Token: 0x17001FFD RID: 8189
		// (get) Token: 0x0600B18A RID: 45450 RVA: 0x003E90C6 File Offset: 0x003E72C6
		public bool AutoLoad
		{
			get
			{
				return this.autoLoad;
			}
		}

		// Token: 0x0600B18B RID: 45451 RVA: 0x003E90D0 File Offset: 0x003E72D0
		public void Initialize(List<GeneDef> genes)
		{
			this.geneSet = new GeneSet();
			foreach (GeneDef gene in genes)
			{
				this.geneSet.AddGene(gene);
			}
		}

		// Token: 0x0600B18C RID: 45452 RVA: 0x003E9130 File Offset: 0x003E7330
		public override void PostMake()
		{
			if (!ModLister.CheckBiotech("genepack"))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostMake();
			this.geneSet = GeneUtility.GenerateGeneSet(null);
		}

		// Token: 0x0600B18D RID: 45453 RVA: 0x003E916C File Offset: 0x003E736C
		public override void TickRare()
		{
			base.TickRare();
			if (this.ParentContainer != null)
			{
				if (this.Deteriorating)
				{
					float statValue = this.GetStatValue(StatDefOf.DeteriorationRate, true, -1);
					if (statValue > 0.001f && base.ParentHolder != null)
					{
						this.deteriorationPct += statValue * 250f / 60000f;
						if (this.deteriorationPct >= 1f)
						{
							this.deteriorationPct -= 1f;
							SteadyEnvironmentEffects.DoDeteriorationDamage(this, base.PositionHeld, base.MapHeld, true);
							return;
						}
					}
				}
				else if (this.HitPoints < base.MaxHitPoints)
				{
					this.hpRecoveryPct += 0.004166667f;
					if (this.hpRecoveryPct >= 1f)
					{
						this.hpRecoveryPct -= 1f;
						this.HitPoints++;
						if (this.HitPoints == base.MaxHitPoints)
						{
							this.hpRecoveryPct = 0f;
							return;
						}
					}
				}
			}
			else if (base.Spawned && !this.autoLoad && this.targetContainer != null)
			{
				this.targetContainer = null;
			}
		}

		// Token: 0x0600B18E RID: 45454 RVA: 0x003E928C File Offset: 0x003E748C
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.targetContainer != null && this.targetContainer.Map == base.Map)
			{
				GenDraw.DrawLineBetween(this.DrawPos, this.targetContainer.DrawPos);
			}
		}

		// Token: 0x0600B18F RID: 45455 RVA: 0x003E92C5 File Offset: 0x003E74C5
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Genepack.<GetGizmos>d__18 <GetGizmos>d__ = new Genepack.<GetGizmos>d__18(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600B190 RID: 45456 RVA: 0x003E92D8 File Offset: 0x003E74D8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.targetContainer, "targetContainer", false);
			Scribe_Values.Look<float>(ref this.hpRecoveryPct, "hpRecoveryPct", 0f, false);
			Scribe_Values.Look<float>(ref this.deteriorationPct, "deteriorationPct", 0f, false);
			Scribe_Values.Look<bool>(ref this.autoLoad, "autoLoad", true, false);
		}

		// Token: 0x04006694 RID: 26260
		public Thing targetContainer;

		// Token: 0x04006695 RID: 26261
		private float hpRecoveryPct;

		// Token: 0x04006696 RID: 26262
		private float deteriorationPct;

		// Token: 0x04006697 RID: 26263
		private bool autoLoad = true;

		// Token: 0x04006698 RID: 26264
		private const int HitPointRecoveryPerDayInGeneBankPerDay = 1;

		// Token: 0x04006699 RID: 26265
		private static readonly CachedTexture IconTex = new CachedTexture("UI/Gizmos/AutoLoadGenepack");
	}
}
