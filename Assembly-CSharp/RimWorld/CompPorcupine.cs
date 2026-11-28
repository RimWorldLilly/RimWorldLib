using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200193C RID: 6460
	public class CompPorcupine : ThingComp
	{
		// Token: 0x0600A203 RID: 41475 RVA: 0x0039E53C File Offset: 0x0039C73C
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			Pawn pawn = dinfo.Instigator as Pawn;
			if (pawn == null || !pawn.RaceProps.IsFlesh)
			{
				return;
			}
			if (dinfo.WeaponBodyPartGroup == null && dinfo.Weapon != null && !dinfo.Weapon.IsMeleeWeapon)
			{
				return;
			}
			BodyPartRecord corePart = pawn.RaceProps.body.corePart;
			pawn.health.AddHediff(HediffDefOf.PorcupineQuill, corePart, null, null);
		}
	}
}
