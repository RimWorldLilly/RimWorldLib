using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017AD RID: 6061
	public class ScenPart_MonolithGeneration : ScenPart_DisableMapGen
	{
		// Token: 0x06009709 RID: 38665 RVA: 0x0036C96E File Offset: 0x0036AB6E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<MonolithGenerationMethod>(ref this.method, "method", MonolithGenerationMethod.Disabled, false);
		}

		// Token: 0x0600970A RID: 38666 RVA: 0x0036C988 File Offset: 0x0036AB88
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.method.ToStringHuman(), true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (object obj in Enum.GetValues(typeof(MonolithGenerationMethod)))
				{
					MonolithGenerationMethod localM2 = (MonolithGenerationMethod)obj;
					MonolithGenerationMethod localM = localM2;
					list.Add(new FloatMenuOption(localM.ToStringHuman(), delegate()
					{
						this.method = localM;
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x0600970B RID: 38667 RVA: 0x0036CA70 File Offset: 0x0036AC70
		public override void PostMapGenerate(Map map)
		{
			if (this.method == MonolithGenerationMethod.Disabled)
			{
				return;
			}
			if (!Find.Anomaly.GenerateMonolith)
			{
				return;
			}
			if (Find.GameInfo.startingTile != map.Tile)
			{
				return;
			}
			IntVec3 loc;
			if (!LargeBuildingCellFinder.TryFindCell(out loc, map, ScenPart_MonolithGeneration.StructureSpawnParms.ForThing(ThingDefOf.VoidMonolith), null, (IntVec3 c) => ScattererValidator_AvoidSpecialThings.IsValid(c, map), true) && !LargeBuildingCellFinder.TryFindCell(out loc, map, ScenPart_MonolithGeneration.StructureSpawnParmsLoose.ForThing(ThingDefOf.VoidMonolith), null, (IntVec3 c) => ScattererValidator_AvoidSpecialThings.IsValid(c, map), true))
			{
				Log.Error("Failed to generate monolith.");
				return;
			}
			GenStep_Monolith.GenerateMonolith(loc, map);
		}

		// Token: 0x0600970C RID: 38668 RVA: 0x0036CB32 File Offset: 0x0036AD32
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.method.GetHashCode();
		}

		// Token: 0x04005A81 RID: 23169
		private MonolithGenerationMethod method = MonolithGenerationMethod.NearColonists;

		// Token: 0x04005A82 RID: 23170
		private static readonly LargeBuildingSpawnParms StructureSpawnParms = new LargeBuildingSpawnParms
		{
			minDistToEdge = 10,
			maxDistanceFromPlayerStartPosition = 20f,
			attemptSpawnLocationType = SpawnLocationType.Outdoors,
			attemptNotUnderBuildings = true,
			canSpawnOnImpassable = false
		};

		// Token: 0x04005A83 RID: 23171
		private static readonly LargeBuildingSpawnParms StructureSpawnParmsLoose = new LargeBuildingSpawnParms
		{
			minDistToEdge = 10,
			attemptSpawnLocationType = SpawnLocationType.Outdoors,
			attemptNotUnderBuildings = true,
			canSpawnOnImpassable = false
		};
	}
}
