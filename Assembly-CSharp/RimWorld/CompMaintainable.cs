using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018E4 RID: 6372
	public class CompMaintainable : ThingComp
	{
		// Token: 0x17001B81 RID: 7041
		// (get) Token: 0x06009FB2 RID: 40882 RVA: 0x00393E5B File Offset: 0x0039205B
		public CompProperties_Maintainable Props
		{
			get
			{
				return (CompProperties_Maintainable)this.props;
			}
		}

		// Token: 0x17001B82 RID: 7042
		// (get) Token: 0x06009FB3 RID: 40883 RVA: 0x00393E68 File Offset: 0x00392068
		public MaintainableStage CurStage
		{
			get
			{
				if (this.ticksSinceMaintain < this.Props.ticksHealthy)
				{
					return MaintainableStage.Healthy;
				}
				if (this.ticksSinceMaintain < this.Props.ticksHealthy + this.Props.ticksNeedsMaintenance)
				{
					return MaintainableStage.NeedsMaintenance;
				}
				return MaintainableStage.Damaging;
			}
		}

		// Token: 0x17001B83 RID: 7043
		// (get) Token: 0x06009FB4 RID: 40884 RVA: 0x00393EA4 File Offset: 0x003920A4
		private bool Active
		{
			get
			{
				Hive hive = this.parent as Hive;
				return hive == null || hive.CompDormant.Awake;
			}
		}

		// Token: 0x06009FB5 RID: 40885 RVA: 0x00393ECD File Offset: 0x003920CD
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksSinceMaintain, "ticksSinceMaintain", 0, false);
		}

		// Token: 0x06009FB6 RID: 40886 RVA: 0x00393EE1 File Offset: 0x003920E1
		public override void CompTick()
		{
			base.CompTick();
			if (!this.Active)
			{
				return;
			}
			this.ticksSinceMaintain++;
			if (this.parent.IsHashIntervalTick(250))
			{
				this.CheckTakeDamage();
			}
		}

		// Token: 0x06009FB7 RID: 40887 RVA: 0x00393F18 File Offset: 0x00392118
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (!this.Active)
			{
				return;
			}
			this.ticksSinceMaintain += 250;
			this.CheckTakeDamage();
		}

		// Token: 0x06009FB8 RID: 40888 RVA: 0x00393F44 File Offset: 0x00392144
		private void CheckTakeDamage()
		{
			if (this.CurStage == MaintainableStage.Damaging)
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
		}

		// Token: 0x06009FB9 RID: 40889 RVA: 0x00393F90 File Offset: 0x00392190
		public void Maintained()
		{
			this.ticksSinceMaintain = 0;
		}

		// Token: 0x06009FBA RID: 40890 RVA: 0x00393F9C File Offset: 0x0039219C
		public override string CompInspectStringExtra()
		{
			MaintainableStage curStage = this.CurStage;
			if (curStage == MaintainableStage.NeedsMaintenance)
			{
				return "DueForMaintenance".Translate();
			}
			if (curStage != MaintainableStage.Damaging)
			{
				return null;
			}
			return "DeterioratingDueToLackOfMaintenance".Translate();
		}

		// Token: 0x04005E74 RID: 24180
		public int ticksSinceMaintain;
	}
}
