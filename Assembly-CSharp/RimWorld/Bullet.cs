using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001B42 RID: 6978
	public class Bullet : Projectile
	{
		// Token: 0x1700206E RID: 8302
		// (get) Token: 0x0600B33C RID: 45884 RVA: 0x003F2A2B File Offset: 0x003F0C2B
		public override bool AnimalsFleeImpact
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600B33D RID: 45885 RVA: 0x003F2A30 File Offset: 0x003F0C30
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			Map map = base.Map;
			IntVec3 position = base.Position;
			base.Impact(hitThing, blockedByShield);
			BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(this.launcher, hitThing, this.intendedTarget.Thing, this.equipmentDef, this.def, this.targetCoverDef);
			Find.BattleLog.Add(battleLogEntry_RangedImpact);
			this.NotifyImpact(hitThing, map, position);
			if (hitThing != null)
			{
				Pawn pawn = this.launcher as Pawn;
				bool instigatorGuilty = pawn == null || !pawn.Drafted;
				DamageInfo dinfo = new DamageInfo(base.DamageDef, (float)this.DamageAmount, this.ArmorPenetration, this.ExactRotation.eulerAngles.y, this.launcher, null, this.equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown, this.intendedTarget.Thing, instigatorGuilty, true, QualityCategory.Normal, true, false);
				dinfo.SetWeaponQuality(this.equipmentQuality);
				hitThing.TakeDamage(dinfo).AssociateWithLog(battleLogEntry_RangedImpact);
				Pawn pawn2 = hitThing as Pawn;
				if (pawn2 != null)
				{
					Pawn_StanceTracker stances = pawn2.stances;
					if (stances != null)
					{
						stances.stagger.Notify_BulletImpact(this);
					}
				}
				if (base.ExtraDamages == null)
				{
					return;
				}
				using (IEnumerator<ExtraDamage> enumerator = base.ExtraDamages.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ExtraDamage extraDamage = enumerator.Current;
						if (Rand.Chance(extraDamage.chance))
						{
							DamageInfo dinfo2 = new DamageInfo(extraDamage.def, extraDamage.amount, extraDamage.AdjustedArmorPenetration(), this.ExactRotation.eulerAngles.y, this.launcher, null, this.equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown, this.intendedTarget.Thing, instigatorGuilty, true, QualityCategory.Normal, true, false);
							hitThing.TakeDamage(dinfo2).AssociateWithLog(battleLogEntry_RangedImpact);
						}
					}
					return;
				}
			}
			if (!blockedByShield)
			{
				SoundDefOf.BulletImpact_Ground.PlayOneShot(new TargetInfo(base.Position, map, false));
				if (base.Position.GetTerrain(map).takeSplashes)
				{
					FleckMaker.WaterSplash(this.ExactPosition, map, Mathf.Sqrt((float)this.DamageAmount) * 1f, 4f);
				}
				else
				{
					FleckMaker.Static(this.ExactPosition, map, FleckDefOf.ShotHit_Dirt, 1f);
				}
			}
			if (Rand.Chance(base.DamageDef.igniteCellChance))
			{
				FireUtility.TryStartFireIn(base.Position, map, Rand.Range(0.55f, 0.85f), this.launcher, null);
			}
		}

		// Token: 0x0600B33E RID: 45886 RVA: 0x003F2C98 File Offset: 0x003F0E98
		private void NotifyImpact(Thing hitThing, Map map, IntVec3 position)
		{
			BulletImpactData impactData = new BulletImpactData
			{
				bullet = this,
				hitThing = hitThing,
				impactPosition = position
			};
			if (hitThing != null)
			{
				hitThing.Notify_BulletImpactNearby(impactData);
			}
			int num = 9;
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = position + GenRadial.RadialPattern[i];
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						if (thingList[j] != hitThing)
						{
							thingList[j].Notify_BulletImpactNearby(impactData);
						}
					}
				}
			}
		}
	}
}
