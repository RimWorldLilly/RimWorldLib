using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001282 RID: 4738
	public class BiomeConditionMapComponent : MapComponent
	{
		// Token: 0x06007573 RID: 30067 RVA: 0x00293554 File Offset: 0x00291754
		public BiomeConditionMapComponent(Map map) : base(map)
		{
		}

		// Token: 0x06007574 RID: 30068 RVA: 0x00293560 File Offset: 0x00291760
		public override void MapGenerated()
		{
			if (this.map.Biome.biomeMapConditions.NullOrEmpty<GameConditionDef>())
			{
				return;
			}
			foreach (GameConditionDef def in this.map.Biome.biomeMapConditions)
			{
				this.map.GameConditionManager.RegisterCondition(GameConditionMaker.MakeConditionPermanent(def));
			}
		}
	}
}
