using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003EC RID: 1004
	public class DamageWorker_MiningBomb : DamageWorker_AddInjury
	{
		// Token: 0x06001D73 RID: 7539 RVA: 0x000B46FC File Offset: 0x000B28FC
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing thing)
		{
			if (!ModLister.CheckOdyssey("Mining explosives"))
			{
				return base.Apply(dinfo, thing);
			}
			Mineable mineable = thing as Mineable;
			if (mineable != null)
			{
				float num = dinfo.Amount;
				num *= dinfo.Def.buildingDamageFactor;
				if (thing.def.passability == Traversability.Impassable)
				{
					num *= dinfo.Def.buildingDamageFactorImpassable;
				}
				else
				{
					num *= dinfo.Def.buildingDamageFactorPassable;
				}
				int amount = Mathf.Min(thing.HitPoints, GenMath.RoundRandom(num));
				mineable.Notify_TookMiningDamage(amount, null);
			}
			return base.Apply(dinfo, thing);
		}
	}
}
