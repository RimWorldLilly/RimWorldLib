using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200137D RID: 4989
	public class RoomRoleWorker_Kitchen : RoomRoleWorker
	{
		// Token: 0x06007B9E RID: 31646 RVA: 0x002C01A0 File Offset: 0x002BE3A0
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.def.designationCategory == DesignationCategoryDefOf.Production)
				{
					for (int j = 0; j < thing.def.AllRecipes.Count; j++)
					{
						RecipeDef recipeDef = thing.def.AllRecipes[j];
						for (int k = 0; k < recipeDef.products.Count; k++)
						{
							ThingDef thingDef = recipeDef.products[k].thingDef;
							if (thingDef.IsNutritionGivingIngestible && thingDef.ingestible.HumanEdible)
							{
								num++;
								goto IL_AD;
							}
						}
					}
				}
				IL_AD:;
			}
			return (float)num * 28f;
		}

		// Token: 0x06007B9F RID: 31647 RVA: 0x002C0274 File Offset: 0x002BE474
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (buildingDef.designationCategory != DesignationCategoryDefOf.Production)
			{
				return 0f;
			}
			for (int i = 0; i < buildingDef.AllRecipes.Count; i++)
			{
				RecipeDef recipeDef = buildingDef.AllRecipes[i];
				for (int j = 0; j < recipeDef.products.Count; j++)
				{
					ThingDef thingDef = recipeDef.products[j].thingDef;
					if (thingDef.IsNutritionGivingIngestible && thingDef.ingestible.HumanEdible)
					{
						return 28f;
					}
				}
			}
			return 0f;
		}
	}
}
