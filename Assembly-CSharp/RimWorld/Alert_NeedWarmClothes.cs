using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BCC RID: 7116
	public class Alert_NeedWarmClothes : Alert
	{
		// Token: 0x0600B658 RID: 46680 RVA: 0x00403510 File Offset: 0x00401710
		public Alert_NeedWarmClothes()
		{
			this.defaultLabel = "NeedWarmClothes".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x0600B659 RID: 46681 RVA: 0x00403578 File Offset: 0x00401778
		private bool AnyColonistsNeedWarmClothes(Map map)
		{
			this.colonistsWithoutWarmClothes.Clear();
			this.needWarmClothesCount = 0;
			this.colonistsWithWarmClothesCount = 0;
			this.missingWarmClothesCount = 0;
			if (this.lowestTemperatureComing < ThingDefOf.Human.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null))
			{
				foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
				{
					if (!pawn.DevelopmentalStage.Baby() && !pawn.IsGhoul && !pawn.IsShambler && !pawn.apparel.AnyApparelLocked)
					{
						Pawn_StoryTracker story = pawn.story;
						if ((((story != null) ? story.traits : null) == null || !pawn.story.traits.HasTrait(TraitDefOf.Nudist)) && (pawn.Ideo == null || !pawn.Ideo.IdeoPrefersNudity()))
						{
							this.needWarmClothesCount++;
							if (pawn.GetStatValue(StatDefOf.ComfyTemperatureMin, true, -1) <= this.lowestTemperatureComing)
							{
								this.colonistsWithWarmClothesCount++;
							}
							else
							{
								this.colonistsWithoutWarmClothes.Add(pawn);
							}
						}
					}
				}
				this.missingWarmClothesCount = Mathf.Max(this.needWarmClothesCount - this.colonistsWithWarmClothesCount - this.FreeWarmClothesSetsCount(map), 0);
				return this.missingWarmClothesCount > 0;
			}
			return false;
		}

		// Token: 0x0600B65A RID: 46682 RVA: 0x004036E8 File Offset: 0x004018E8
		private int FreeWarmClothesSetsCount(Map map)
		{
			this.jackets.Clear();
			this.shirts.Clear();
			this.pants.Clear();
			this.tmpApparelList.Clear();
			this.tmpApparelList.AddRange(map.listerThings.ThingsInGroup(ThingRequestGroup.Apparel));
			foreach (IHaulSource haulSource in map.haulDestinationManager.AllHaulSourcesListForReading)
			{
				foreach (Thing thing in ((IEnumerable<Thing>)haulSource.GetDirectlyHeldThings()))
				{
					Apparel apparel = thing as Apparel;
					if (apparel != null)
					{
						this.tmpApparelList.Add(apparel);
					}
				}
			}
			for (int i = 0; i < this.tmpApparelList.Count; i++)
			{
				if (this.tmpApparelList[i].IsInAnyStorage())
				{
					float statValue = this.tmpApparelList[i].GetStatValue(StatDefOf.Insulation_Cold, true, 120);
					if (statValue > 0f)
					{
						if (this.tmpApparelList[i].def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))
						{
							if (this.tmpApparelList[i].def.apparel.layers.Contains(ApparelLayerDefOf.OnSkin))
							{
								this.shirts.Enqueue(this.tmpApparelList[i], -statValue);
							}
							else
							{
								this.jackets.Enqueue(this.tmpApparelList[i], -statValue);
							}
						}
						if (this.tmpApparelList[i].def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Legs))
						{
							this.pants.Enqueue(this.tmpApparelList[i], -statValue);
						}
					}
				}
			}
			this.tmpApparelList.Clear();
			float num = ThingDefOf.Human.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) - this.lowestTemperatureComing;
			if (num <= 0f)
			{
				return GenMath.Max(this.jackets.Count, this.shirts.Count, this.pants.Count);
			}
			int num2 = 0;
			while (this.jackets.Count > 0 || this.shirts.Count > 0 || this.pants.Count > 0)
			{
				float num3 = 0f;
				Thing thing2;
				float num4;
				if (this.jackets.TryDequeue(out thing2, out num4))
				{
					num3 += num4;
				}
				float num5;
				if (this.shirts.Count > 0 && num3 < num && this.shirts.TryDequeue(out thing2, out num5))
				{
					num3 += num5;
				}
				float num6;
				if (this.pants.Count > 0 && num3 < num && this.pants.TryDequeue(out thing2, out num6))
				{
					num3 += num6;
				}
				if (num3 < num)
				{
					break;
				}
				num2++;
			}
			this.jackets.Clear();
			this.shirts.Clear();
			this.pants.Clear();
			return num2;
		}

		// Token: 0x0600B65B RID: 46683 RVA: 0x00403A1C File Offset: 0x00401C1C
		public override TaggedString GetExplanation()
		{
			if (this.missingWarmClothesCount == this.needWarmClothesCount)
			{
				return "NeedWarmClothesDesc1All".Translate() + "\n\n" + "NeedWarmClothesDesc2".Translate(this.lowestTemperatureComing.ToStringTemperature("F0"));
			}
			return "NeedWarmClothesDesc1".Translate(this.missingWarmClothesCount) + "\n\n" + "NeedWarmClothesDesc2".Translate(this.lowestTemperatureComing.ToStringTemperature("F0"));
		}

		// Token: 0x0600B65C RID: 46684 RVA: 0x00403AB3 File Offset: 0x00401CB3
		public override AlertReport GetReport()
		{
			if (this.MapWithMissingWarmClothes() == null)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.colonistsWithoutWarmClothes);
		}

		// Token: 0x0600B65D RID: 46685 RVA: 0x00403AD0 File Offset: 0x00401CD0
		private Map MapWithMissingWarmClothes()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome && !map.Biome.inVacuum)
				{
					this.lowestTemperatureComing = this.LowestTemperatureComing(map);
					if (this.lowestTemperatureComing < 5f && this.AnyColonistsNeedWarmClothes(map))
					{
						return map;
					}
				}
			}
			return null;
		}

		// Token: 0x0600B65E RID: 46686 RVA: 0x00403B38 File Offset: 0x00401D38
		private float LowestTemperatureComing(Map map)
		{
			Twelfth twelfth = GenLocalDate.Twelfth(map);
			float a = this.GetTemperature(twelfth, map);
			for (int i = 0; i < 3; i++)
			{
				twelfth = twelfth.NextTwelfth();
				a = Mathf.Min(a, this.GetTemperature(twelfth, map));
			}
			return Mathf.Min(a, map.mapTemperature.OutdoorTemp);
		}

		// Token: 0x0600B65F RID: 46687 RVA: 0x00403B88 File Offset: 0x00401D88
		private float GetTemperature(Twelfth twelfth, Map map)
		{
			return GenTemperature.AverageTemperatureAtTileForTwelfth(map.Tile, twelfth);
		}

		// Token: 0x040068BC RID: 26812
		private PriorityQueue<Thing, float> jackets = new PriorityQueue<Thing, float>();

		// Token: 0x040068BD RID: 26813
		private PriorityQueue<Thing, float> shirts = new PriorityQueue<Thing, float>();

		// Token: 0x040068BE RID: 26814
		private PriorityQueue<Thing, float> pants = new PriorityQueue<Thing, float>();

		// Token: 0x040068BF RID: 26815
		private List<Pawn> colonistsWithoutWarmClothes = new List<Pawn>();

		// Token: 0x040068C0 RID: 26816
		private int needWarmClothesCount;

		// Token: 0x040068C1 RID: 26817
		private int colonistsWithWarmClothesCount;

		// Token: 0x040068C2 RID: 26818
		private int missingWarmClothesCount;

		// Token: 0x040068C3 RID: 26819
		private float lowestTemperatureComing;

		// Token: 0x040068C4 RID: 26820
		private List<Thing> tmpApparelList = new List<Thing>();

		// Token: 0x040068C5 RID: 26821
		private const int CheckNextTwelfthsCount = 3;

		// Token: 0x040068C6 RID: 26822
		private const float CanShowAlertOnlyIfTempBelow = 5f;
	}
}
