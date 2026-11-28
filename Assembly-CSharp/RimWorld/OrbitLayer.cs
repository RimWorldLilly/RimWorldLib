using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001765 RID: 5989
	public class OrbitLayer : PlanetLayer
	{
		// Token: 0x06009532 RID: 38194 RVA: 0x00364BC8 File Offset: 0x00362DC8
		public OrbitLayer()
		{
		}

		// Token: 0x06009533 RID: 38195 RVA: 0x00364BD0 File Offset: 0x00362DD0
		public OrbitLayer(int layerId, PlanetLayerDef def, float radius, Vector3 origin, Vector3 viewCenter, float viewAngle, int subdivisions, float extraCameraAltitude, float backgroundWorldCameraOffset, float backgroundWorldCameraParallaxDistancePer100Cells) : base(layerId, def, radius, origin, viewCenter, viewAngle, subdivisions, extraCameraAltitude, backgroundWorldCameraOffset, backgroundWorldCameraParallaxDistancePer100Cells)
		{
		}

		// Token: 0x06009534 RID: 38196 RVA: 0x00364BF4 File Offset: 0x00362DF4
		public override AcceptanceReport CanSelectLayer()
		{
			AcceptanceReport acceptanceReport = base.CanSelectLayer();
			if (!acceptanceReport)
			{
				return acceptanceReport;
			}
			if (!Find.WorldObjects.AnyWorldObjectOnLayer(this))
			{
				return "CannotSelectOrbitReason".Translate();
			}
			return true;
		}
	}
}
