using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02002325 RID: 8997
	public abstract class WorldDrawLayer_SingleTile : WorldDrawLayer
	{
		// Token: 0x170026F7 RID: 9975
		// (get) Token: 0x0600DE35 RID: 56885
		protected abstract PlanetTile Tile { get; }

		// Token: 0x170026F8 RID: 9976
		// (get) Token: 0x0600DE36 RID: 56886
		protected abstract Material Material { get; }

		// Token: 0x170026F9 RID: 9977
		// (get) Token: 0x0600DE37 RID: 56887 RVA: 0x0052A671 File Offset: 0x00528871
		public override bool ShouldRegenerate
		{
			get
			{
				return base.ShouldRegenerate || this.Tile != this.lastDrawnPlanetTile;
			}
		}

		// Token: 0x0600DE38 RID: 56888 RVA: 0x0052A68E File Offset: 0x0052888E
		public override IEnumerable Regenerate()
		{
			WorldDrawLayer_SingleTile.<Regenerate>d__8 <Regenerate>d__ = new WorldDrawLayer_SingleTile.<Regenerate>d__8(-2);
			<Regenerate>d__.<>4__this = this;
			return <Regenerate>d__;
		}

		// Token: 0x04008EA7 RID: 36519
		private PlanetTile lastDrawnPlanetTile = new PlanetTile(-1);

		// Token: 0x04008EA8 RID: 36520
		private List<Vector3> verts = new List<Vector3>();
	}
}
