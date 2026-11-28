using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A73 RID: 6771
	public static class TurretGunUtility
	{
		// Token: 0x0600AA62 RID: 43618 RVA: 0x003C5637 File Offset: 0x003C3837
		public static bool NeedsShells(ThingDef turret)
		{
			return turret.category == ThingCategory.Building && turret.building.IsTurret && turret.building.turretGunDef.HasComp(typeof(CompChangeableProjectile));
		}

		// Token: 0x0600AA63 RID: 43619 RVA: 0x003C566C File Offset: 0x003C386C
		public static ThingDef TryFindRandomShellDef(ThingDef turret, bool allowEMP = true, bool allowToxGas = false, bool mustHarmHealth = true, TechLevel techLevel = TechLevel.Undefined, bool allowAntigrainWarhead = false, float maxMarketValue = -1f, Faction faction = null)
		{
			if (!TurretGunUtility.NeedsShells(turret))
			{
				return null;
			}
			ThingFilter fixedFilter = turret.building.turretGunDef.building.fixedStorageSettings.filter;
			if (ModsConfig.BiotechActive && faction != null && faction.def == FactionDefOf.PirateWaster && fixedFilter.Allows(ThingDefOf.Shell_Toxic) && Rand.Chance(0.5f))
			{
				return ThingDefOf.Shell_Toxic;
			}
			ThingDef result;
			if ((from x in DefDatabase<ThingDef>.AllDefsListForReading
			where fixedFilter.Allows(x) && (allowEMP || x.projectileWhenLoaded.projectile.damageDef != DamageDefOf.EMP) && (allowToxGas || !ModsConfig.BiotechActive || x.projectileWhenLoaded.projectile.damageDef != DamageDefOf.ToxGas) && (!mustHarmHealth || x.projectileWhenLoaded.projectile.damageDef.harmsHealth) && (techLevel == TechLevel.Undefined || x.techLevel <= techLevel) && (allowAntigrainWarhead || x != ThingDefOf.Shell_AntigrainWarhead) && (maxMarketValue < 0f || x.BaseMarketValue <= maxMarketValue)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x04006390 RID: 25488
		private const float WasterToxicShellChance = 0.5f;
	}
}
