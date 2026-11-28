using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001286 RID: 4742
	public class TileMutatorConditionMapComponent : MapComponent
	{
		// Token: 0x06007587 RID: 30087 RVA: 0x00293EAA File Offset: 0x002920AA
		public TileMutatorConditionMapComponent(Map map) : base(map)
		{
		}

		// Token: 0x06007588 RID: 30088 RVA: 0x00293EB4 File Offset: 0x002920B4
		public override void MapGenerated()
		{
			if (this.map.TileInfo.Mutators.EnumerableNullOrEmpty<TileMutatorDef>())
			{
				return;
			}
			foreach (TileMutatorDef tileMutatorDef in this.map.TileInfo.Mutators)
			{
				if (!tileMutatorDef.additionalGameConditions.NullOrEmpty<GameConditionDef>())
				{
					foreach (GameConditionDef def in tileMutatorDef.additionalGameConditions)
					{
						this.map.GameConditionManager.RegisterCondition(GameConditionMaker.MakeConditionPermanent(def));
					}
				}
			}
		}
	}
}
