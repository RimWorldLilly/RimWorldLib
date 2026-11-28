using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020012EC RID: 4844
	public class GenStep_ConditionCauser : GenStep_Scatterer
	{
		// Token: 0x17001487 RID: 5255
		// (get) Token: 0x06007832 RID: 30770 RVA: 0x002A7F40 File Offset: 0x002A6140
		public override int SeedPart
		{
			get
			{
				return 1068345639;
			}
		}

		// Token: 0x06007833 RID: 30771 RVA: 0x002A7F48 File Offset: 0x002A6148
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			CellRect rect = CellRect.CenteredOn(c, 10, 10);
			if (!rect.InBounds(map))
			{
				return false;
			}
			List<CellRect> list;
			if (MapGenerator.TryGetVar<List<CellRect>>("UsedRects", out list) && list.Any((CellRect x) => rect.Overlaps(x)))
			{
				return false;
			}
			foreach (IntVec3 c2 in rect.Cells)
			{
				if (!GenConstruct.CanBuildOnTerrain(this.currentParams.sitePart.conditionCauser.def, c2, map, Rot4.North, null, null))
				{
					return false;
				}
				using (List<Thing>.Enumerator enumerator2 = c2.GetThingList(map).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.def.building != null)
						{
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06007834 RID: 30772 RVA: 0x002A8064 File Offset: 0x002A6264
		public override void Generate(Map map, GenStepParams parms)
		{
			this.currentParams = parms;
			this.count = 1;
			base.Generate(map, parms);
		}

		// Token: 0x06007835 RID: 30773 RVA: 0x002A807C File Offset: 0x002A627C
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			Faction faction;
			if (map.ParentFaction == null || map.ParentFaction == Faction.OfPlayer)
			{
				faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			}
			else
			{
				faction = map.ParentFaction;
			}
			List<CellRect> orGenerateVar = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			CellRect cellRect = CellRect.CenteredOn(loc, 10, 10).ClipInsideMap(map);
			SitePart sitePart = this.currentParams.sitePart;
			sitePart.conditionCauserWasSpawned = true;
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.rect = cellRect;
			resolveParams.faction = faction;
			resolveParams.conditionCauser = sitePart.conditionCauser;
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("conditionCauserRoom", resolveParams, null);
			BaseGen.Generate();
			MapGenerator.SetVar<CellRect>("RectOfInterest", cellRect);
			orGenerateVar.Add(cellRect);
		}

		// Token: 0x04004B5B RID: 19291
		private const int Size = 10;

		// Token: 0x04004B5C RID: 19292
		private GenStepParams currentParams;
	}
}
