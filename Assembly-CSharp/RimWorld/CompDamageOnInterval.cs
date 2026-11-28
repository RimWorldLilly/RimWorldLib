using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001856 RID: 6230
	public class CompDamageOnInterval : ThingComp
	{
		// Token: 0x17001AA0 RID: 6816
		// (get) Token: 0x06009BEB RID: 39915 RVA: 0x00382C37 File Offset: 0x00380E37
		public CompProperties_DamageOnInterval Props
		{
			get
			{
				return (CompProperties_DamageOnInterval)this.props;
			}
		}

		// Token: 0x06009BEC RID: 39916 RVA: 0x00382C44 File Offset: 0x00380E44
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.ticksToDamage = this.Props.ticksBetweenDamage;
		}

		// Token: 0x06009BED RID: 39917 RVA: 0x00382C5E File Offset: 0x00380E5E
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksToDamage, "ticksToPlantHarm", 0, false);
		}

		// Token: 0x06009BEE RID: 39918 RVA: 0x00382C78 File Offset: 0x00380E78
		public override void CompTick()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.Props.startHitPointsPercent > 0f && (float)this.parent.HitPoints > this.Props.startHitPointsPercent * (float)this.parent.MaxHitPoints)
			{
				return;
			}
			this.ticksToDamage--;
			if (this.ticksToDamage <= 0)
			{
				this.ticksToDamage = this.Props.ticksBetweenDamage;
				this.Damage();
			}
			EffecterDef effecterDef = null;
			foreach (DamageEffectStage damageEffectStage in this.Props.effectStages)
			{
				if (damageEffectStage.minHitPointsPercent * (float)this.parent.MaxHitPoints > (float)this.parent.HitPoints)
				{
					effecterDef = damageEffectStage.effecterDef;
				}
			}
			if (effecterDef != null)
			{
				EffecterDef effecterDef2 = effecterDef;
				Effecter effecter = this.effecter;
				if (effecterDef2 != ((effecter != null) ? effecter.def : null))
				{
					Effecter effecter2 = this.effecter;
					if (effecter2 != null)
					{
						effecter2.Cleanup();
					}
					this.effecter = effecterDef.SpawnAttached(this.parent, this.parent.MapHeld, 1f);
				}
			}
			Effecter effecter3 = this.effecter;
			if (effecter3 == null)
			{
				return;
			}
			effecter3.EffectTick(this.parent, this.parent);
		}

		// Token: 0x06009BEF RID: 39919 RVA: 0x00382DDC File Offset: 0x00380FDC
		public void Damage()
		{
			this.parent.TakeDamage(new DamageInfo(this.Props.damageDef ?? DamageDefOf.Deterioration, this.Props.damage, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
		}

		// Token: 0x06009BF0 RID: 39920 RVA: 0x00382E2D File Offset: 0x0038102D
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompDamageOnInterval.<CompGetGizmosExtra>d__8 <CompGetGizmosExtra>d__ = new CompDamageOnInterval.<CompGetGizmosExtra>d__8(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x04005CB7 RID: 23735
		private int ticksToDamage;

		// Token: 0x04005CB8 RID: 23736
		private Effecter effecter;
	}
}
