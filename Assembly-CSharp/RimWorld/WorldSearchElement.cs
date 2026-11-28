using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CCB RID: 7371
	public class WorldSearchElement
	{
		// Token: 0x17002282 RID: 8834
		// (get) Token: 0x0600BD95 RID: 48533 RVA: 0x00441A9C File Offset: 0x0043FC9C
		public string DisplayLabel
		{
			get
			{
				if (this.worldObject != null)
				{
					return this.worldObject.LabelCap;
				}
				if (ModsConfig.OdysseyActive)
				{
					if (this.landmark != null)
					{
						return this.landmark.name.CapitalizeFirst();
					}
					if (!this.mutators.NullOrEmpty<TileMutatorDef>())
					{
						return this.mutators[0].label.CapitalizeFirst();
					}
				}
				return null;
			}
		}

		// Token: 0x04006C84 RID: 27780
		public PlanetTile tile = PlanetTile.Invalid;

		// Token: 0x04006C85 RID: 27781
		public WorldObject worldObject;

		// Token: 0x04006C86 RID: 27782
		public Landmark landmark;

		// Token: 0x04006C87 RID: 27783
		public List<TileMutatorDef> mutators;
	}
}
