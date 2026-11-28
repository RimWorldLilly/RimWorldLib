using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001889 RID: 6281
	public class CompFleshmass : ThingComp, ISizeReporter
	{
		// Token: 0x17001AE8 RID: 6888
		// (get) Token: 0x06009D30 RID: 40240 RVA: 0x0038850F File Offset: 0x0038670F
		public CompProperties_Fleshmass Props
		{
			get
			{
				return (CompProperties_Fleshmass)this.props;
			}
		}

		// Token: 0x06009D31 RID: 40241 RVA: 0x0038851C File Offset: 0x0038671C
		public float CurrentSize()
		{
			return 1f;
		}

		// Token: 0x06009D32 RID: 40242 RVA: 0x00388523 File Offset: 0x00386723
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_References.Look<Thing>(ref this.source, "source", false);
		}

		// Token: 0x06009D33 RID: 40243 RVA: 0x0038853C File Offset: 0x0038673C
		public override void CompTickRare()
		{
			if (this.sustainer == null && !this.parent.Position.Fogged(this.parent.Map))
			{
				SoundInfo info = SoundInfo.InMap(new TargetInfo(this.parent.Position, this.parent.Map, false), MaintenanceType.PerTickRare);
				this.sustainer = SustainerAggregatorUtility.AggregateOrSpawnSustainerFor(this, SoundDefOf.FleshmassAmbience, info);
			}
			Sustainer sustainer = this.sustainer;
			if (sustainer == null)
			{
				return;
			}
			sustainer.Maintain();
		}

		// Token: 0x06009D34 RID: 40244 RVA: 0x003885B4 File Offset: 0x003867B4
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.sustainer != null)
			{
				if (this.sustainer.externalParams.sizeAggregator == null)
				{
					this.sustainer.externalParams.sizeAggregator = new SoundSizeAggregator();
				}
				this.sustainer.externalParams.sizeAggregator.RemoveReporter(this);
			}
			this.sustainer = null;
		}

		// Token: 0x06009D35 RID: 40245 RVA: 0x00388610 File Offset: 0x00386810
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			bool flag;
			if (dinfo == null)
			{
				flag = (null != null);
			}
			else
			{
				Thing instigator = dinfo.GetValueOrDefault().Instigator;
				flag = (((instigator != null) ? instigator.Faction : null) != null);
			}
			if ((!flag || dinfo.Value.Instigator.Faction == Faction.OfPlayer) && this.source != null && this.source.Spawned)
			{
				CompGrowsFleshmassTendrils compGrowsFleshmassTendrils = this.source.TryGetComp<CompGrowsFleshmassTendrils>();
				if (compGrowsFleshmassTendrils == null)
				{
					return;
				}
				compGrowsFleshmassTendrils.Notify_FleshmassDestroyedByPlayer(this.parent);
			}
		}

		// Token: 0x06009D36 RID: 40246 RVA: 0x00388696 File Offset: 0x00386896
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			CompGrowsFleshmassTendrils compGrowsFleshmassTendrils = this.source.TryGetComp<CompGrowsFleshmassTendrils>();
			if (compGrowsFleshmassTendrils == null)
			{
				return;
			}
			compGrowsFleshmassTendrils.Notify_FleshmassDestroyed(this.parent);
		}

		// Token: 0x06009D37 RID: 40247 RVA: 0x003886B4 File Offset: 0x003868B4
		public override string CompInspectStringExtra()
		{
			Thing thing = this.source;
			CompGrowsFleshmassTendrils compGrowsFleshmassTendrils = (thing != null) ? thing.TryGetComp<CompGrowsFleshmassTendrils>() : null;
			if (this.parent.def == ThingDefOf.Fleshmass_Active && compGrowsFleshmassTendrils != null && compGrowsFleshmassTendrils.Props.fleshbeastBirthThresholdRange != null)
			{
				return "ActiveFleshmassInspect".Translate();
			}
			return "";
		}

		// Token: 0x04005D29 RID: 23849
		public Thing source;

		// Token: 0x04005D2A RID: 23850
		private Sustainer sustainer;
	}
}
