using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FC7 RID: 8135
	public class StatWorker_MeleeDamageAmountTrap : StatWorker_MeleeDamageAmount
	{
		// Token: 0x0600CE8A RID: 52874 RVA: 0x004C088C File Offset: 0x004BEA8C
		public override bool ShouldShowFor(StatRequest req)
		{
			if (req.HasThing)
			{
				Building_Trap building_Trap = req.Thing as Building_Trap;
				if (building_Trap != null && !building_Trap.ShouldShowTrapDamageStat)
				{
					return false;
				}
			}
			ThingDef thingDef = req.Def as ThingDef;
			return thingDef != null && thingDef.category == ThingCategory.Building && thingDef.building.isTrap;
		}

		// Token: 0x0600CE8B RID: 52875 RVA: 0x004C08E2 File Offset: 0x004BEAE2
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			return base.GetValueUnfinalized(req, applyPostProcess) / 5f;
		}

		// Token: 0x0600CE8C RID: 52876 RVA: 0x004C08F4 File Offset: 0x004BEAF4
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			float num = 5f;
			return base.GetExplanationUnfinalized(req, numberSense) + "Stat_TrapDamageHitCount".Translate(num) + ": x" + (1f / num).ToStringPercent();
		}

		// Token: 0x0600CE8D RID: 52877 RVA: 0x004C0944 File Offset: 0x004BEB44
		protected override DamageArmorCategoryDef CategoryOfDamage(ThingDef def)
		{
			return def.building.trapDamageCategory;
		}
	}
}
