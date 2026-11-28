using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AC2 RID: 6850
	public class Building_NutrientPasteDispenser : Building
	{
		// Token: 0x17001F49 RID: 8009
		// (get) Token: 0x0600AE43 RID: 44611 RVA: 0x003D66B6 File Offset: 0x003D48B6
		public bool CanDispenseNow
		{
			get
			{
				return this.powerComp.PowerOn && this.HasEnoughFeedstockInHoppers();
			}
		}

		// Token: 0x17001F4A RID: 8010
		// (get) Token: 0x0600AE44 RID: 44612 RVA: 0x003D66CD File Offset: 0x003D48CD
		public List<IntVec3> AdjCellsCardinalInBounds
		{
			get
			{
				if (this.cachedAdjCellsCardinal == null)
				{
					this.cachedAdjCellsCardinal = (from c in GenAdj.CellsAdjacentCardinal(this)
					where c.InBounds(base.Map)
					select c).ToList<IntVec3>();
				}
				return this.cachedAdjCellsCardinal;
			}
		}

		// Token: 0x17001F4B RID: 8011
		// (get) Token: 0x0600AE45 RID: 44613 RVA: 0x003D66FF File Offset: 0x003D48FF
		public virtual ThingDef DispensableDef
		{
			get
			{
				return ThingDefOf.MealNutrientPaste;
			}
		}

		// Token: 0x17001F4C RID: 8012
		// (get) Token: 0x0600AE46 RID: 44614 RVA: 0x003D6706 File Offset: 0x003D4906
		public override Color DrawColor
		{
			get
			{
				if (!this.IsSociallyProper(null, false, false))
				{
					return Building_Bed.SheetColorForPrisoner;
				}
				return base.DrawColor;
			}
		}

		// Token: 0x0600AE47 RID: 44615 RVA: 0x003D671F File Offset: 0x003D491F
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
			this.cachedAdjCellsCardinal = null;
		}

		// Token: 0x0600AE48 RID: 44616 RVA: 0x003D673C File Offset: 0x003D493C
		public virtual Building AdjacentReachableHopper(Pawn reacher)
		{
			for (int i = 0; i < this.AdjCellsCardinalInBounds.Count; i++)
			{
				Building edifice = this.AdjCellsCardinalInBounds[i].GetEdifice(base.Map);
				if (edifice != null && edifice.IsHopper() && reacher.CanReach(edifice, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && !edifice.IsBurning())
				{
					return edifice;
				}
			}
			return null;
		}

		// Token: 0x0600AE49 RID: 44617 RVA: 0x003D67A0 File Offset: 0x003D49A0
		public virtual Thing TryDispenseFood()
		{
			if (!this.CanDispenseNow)
			{
				return null;
			}
			float num = this.def.building.nutritionCostPerDispense - 0.0001f;
			List<ThingDef> list = new List<ThingDef>();
			for (;;)
			{
				Thing thing = this.FindFeedInAnyHopper();
				if (thing == null)
				{
					break;
				}
				int num2 = Mathf.Min(thing.stackCount, Mathf.CeilToInt(num / thing.GetStatValue(StatDefOf.Nutrition, true, -1)));
				num -= (float)num2 * thing.GetStatValue(StatDefOf.Nutrition, true, -1);
				list.Add(thing.def);
				if (thing.stackCount == num2)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
				else
				{
					thing.SplitOff(num2);
				}
				if (num <= 0f)
				{
					goto Block_4;
				}
			}
			Log.Error("Did not find enough food in hoppers while trying to dispense.");
			return null;
			Block_4:
			this.def.building.soundDispense.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			Thing thing2 = ThingMaker.MakeThing(ThingDefOf.MealNutrientPaste, null);
			CompIngredients compIngredients = thing2.TryGetComp<CompIngredients>();
			for (int i = 0; i < list.Count; i++)
			{
				compIngredients.RegisterIngredient(list[i]);
			}
			return thing2;
		}

		// Token: 0x0600AE4A RID: 44618 RVA: 0x003D68B4 File Offset: 0x003D4AB4
		public virtual Thing FindFeedInAnyHopper()
		{
			for (int i = 0; i < this.AdjCellsCardinalInBounds.Count; i++)
			{
				Thing thing = null;
				Thing thing2 = null;
				List<Thing> thingList = this.AdjCellsCardinalInBounds[i].GetThingList(base.Map);
				for (int j = 0; j < thingList.Count; j++)
				{
					Thing thing3 = thingList[j];
					if (Building_NutrientPasteDispenser.IsAcceptableFeedstock(thing3.def))
					{
						thing = thing3;
					}
					if (thing3.IsHopper())
					{
						thing2 = thing3;
					}
				}
				if (thing != null && thing2 != null)
				{
					return thing;
				}
			}
			return null;
		}

		// Token: 0x0600AE4B RID: 44619 RVA: 0x003D6938 File Offset: 0x003D4B38
		public virtual bool HasEnoughFeedstockInHoppers()
		{
			float num = 0f;
			for (int i = 0; i < this.AdjCellsCardinalInBounds.Count; i++)
			{
				IntVec3 c = this.AdjCellsCardinalInBounds[i];
				Thing thing = null;
				Thing thing2 = null;
				List<Thing> thingList = c.GetThingList(base.Map);
				for (int j = 0; j < thingList.Count; j++)
				{
					Thing thing3 = thingList[j];
					if (Building_NutrientPasteDispenser.IsAcceptableFeedstock(thing3.def))
					{
						thing = thing3;
					}
					if (thing3.IsHopper())
					{
						thing2 = thing3;
					}
				}
				if (thing != null && thing2 != null)
				{
					num += (float)thing.stackCount * thing.GetStatValue(StatDefOf.Nutrition, true, -1);
				}
				if (num >= this.def.building.nutritionCostPerDispense)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600AE4C RID: 44620 RVA: 0x003D69F8 File Offset: 0x003D4BF8
		public static bool IsAcceptableFeedstock(ThingDef def)
		{
			return def.IsNutritionGivingIngestible && def.ingestible.preferability != FoodPreferability.Undefined && (def.ingestible.foodType & FoodTypeFlags.Plant) != FoodTypeFlags.Plant && (def.ingestible.foodType & FoodTypeFlags.Tree) != FoodTypeFlags.Tree;
		}

		// Token: 0x0600AE4D RID: 44621 RVA: 0x003D6A49 File Offset: 0x003D4C49
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_NutrientPasteDispenser.<GetGizmos>d__17 <GetGizmos>d__ = new Building_NutrientPasteDispenser.<GetGizmos>d__17(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AE4E RID: 44622 RVA: 0x003D6A5C File Offset: 0x003D4C5C
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.GetInspectString());
			if (!this.IsSociallyProper(null, false, false))
			{
				stringBuilder.AppendLine("InPrisonCell".Translate());
			}
			return stringBuilder.ToString().Trim();
		}

		// Token: 0x040064E1 RID: 25825
		public CompPowerTrader powerComp;

		// Token: 0x040064E2 RID: 25826
		private List<IntVec3> cachedAdjCellsCardinal;

		// Token: 0x040064E3 RID: 25827
		public static int CollectDuration = 50;
	}
}
