using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200203B RID: 8251
	public class PlaceWorker_ShowTurretRadius : PlaceWorker
	{
		// Token: 0x0600D0CC RID: 53452 RVA: 0x004CCC78 File Offset: 0x004CAE78
		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			VerbProperties verbProperties = ((ThingDef)checkingDef).building.turretGunDef.Verbs.Find((VerbProperties v) => v.verbClass == typeof(Verb_Shoot) || typeof(Verb_Spray).IsAssignableFrom(v.verbClass));
			if (verbProperties.range > 0f)
			{
				GenDraw.DrawRadiusRing(loc, verbProperties.range);
			}
			if (verbProperties.minRange > 0f)
			{
				GenDraw.DrawRadiusRing(loc, verbProperties.minRange);
			}
			return true;
		}
	}
}
