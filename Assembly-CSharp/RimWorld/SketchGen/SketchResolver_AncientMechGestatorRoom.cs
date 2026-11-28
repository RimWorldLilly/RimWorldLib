using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020020BE RID: 8382
	public class SketchResolver_AncientMechGestatorRoom : SketchResolver
	{
		// Token: 0x170025E1 RID: 9697
		// (get) Token: 0x0600D322 RID: 54050 RVA: 0x004DAF7B File Offset: 0x004D917B
		private static IEnumerable<ThingDef> CentralThings
		{
			get
			{
				return new SketchResolver_AncientMechGestatorRoom.<get_CentralThings>d__2(-2);
			}
		}

		// Token: 0x0600D323 RID: 54051 RVA: 0x004DAF84 File Offset: 0x004D9184
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return parms.rect != null && parms.sketch != null;
		}

		// Token: 0x0600D324 RID: 54052 RVA: 0x004DAFA0 File Offset: 0x004D91A0
		protected override void ResolveInt(SketchResolveParams parms)
		{
			if (!ModLister.CheckBiotech("Ancient mech gestator room"))
			{
				return;
			}
			SketchResolveParams parms2 = parms;
			parms2.cornerThing = ThingDefOf.AncientLamp;
			parms2.requireFloor = new bool?(true);
			SketchResolverDefOf.AddCornerThings.Resolve(parms2);
			foreach (ThingDef thingCentral in SketchResolver_AncientMechGestatorRoom.CentralThings)
			{
				SketchResolveParams parms3 = parms;
				parms3.thingCentral = thingCentral;
				parms3.requireFloor = new bool?(true);
				SketchResolverDefOf.AddThingsCentral.Resolve(parms3);
			}
			if (ModsConfig.IdeologyActive)
			{
				foreach (IntVec3 pos in parms.rect.Value.Cells)
				{
					if (Rand.Chance(0.05f) && !parms.sketch.ThingsAt(pos).Any<SketchThing>())
					{
						parms.sketch.AddThing(ThingDefOf.ChunkMechanoidSlag, pos, ThingDefOf.ChunkMechanoidSlag.defaultPlacingRot, null, 1, null, null, true, 2f);
					}
				}
			}
		}

		// Token: 0x040085D5 RID: 34261
		private const float MechanoidDebrisChance = 0.05f;
	}
}
