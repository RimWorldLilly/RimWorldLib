using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018B5 RID: 6325
	public class CompHarbingerTreeConsumable : ThingComp
	{
		// Token: 0x17001B2B RID: 6955
		// (get) Token: 0x06009E87 RID: 40583 RVA: 0x0038E754 File Offset: 0x0038C954
		public bool BeingConsumed
		{
			get
			{
				return !this.tmpTreesInRange.Empty<HarbingerTree>();
			}
		}

		// Token: 0x17001B2C RID: 6956
		// (get) Token: 0x06009E88 RID: 40584 RVA: 0x0038E764 File Offset: 0x0038C964
		private Corpse Corpse
		{
			get
			{
				return this.parent as Corpse;
			}
		}

		// Token: 0x17001B2D RID: 6957
		// (get) Token: 0x06009E89 RID: 40585 RVA: 0x0038E774 File Offset: 0x0038C974
		public bool CanBeConsumed
		{
			get
			{
				if (!ModsConfig.AnomalyActive)
				{
					return false;
				}
				if (!this.parent.Spawned)
				{
					return false;
				}
				SlotGroup slotGroup = this.parent.GetSlotGroup();
				return slotGroup == null || slotGroup.parent.Isnt<Building>();
			}
		}

		// Token: 0x06009E8A RID: 40586 RVA: 0x0038E7B5 File Offset: 0x0038C9B5
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x06009E8B RID: 40587 RVA: 0x0038E7C9 File Offset: 0x0038C9C9
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			LongEventHandler.ExecuteWhenFinished(new Action(this.ConsumedRootsLazy));
		}

		// Token: 0x06009E8C RID: 40588 RVA: 0x0038E7DC File Offset: 0x0038C9DC
		private void ConsumedRootsLazy()
		{
			if (!this.CanBeConsumed)
			{
				return;
			}
			this.tmpTreesInRange.Clear();
			foreach (Thing thing in this.parent.Map.listerThings.ThingsOfDef(ThingDefOf.Plant_TreeHarbinger))
			{
				HarbingerTree harbingerTree = thing as HarbingerTree;
				if (harbingerTree.Position.InHorDistOf(this.parent.Position, harbingerTree.ConsumeRadius))
				{
					this.tmpTreesInRange.Add(harbingerTree);
				}
			}
		}

		// Token: 0x06009E8D RID: 40589 RVA: 0x0038E884 File Offset: 0x0038CA84
		public override void CompTick()
		{
			if (this.parent.IsHashIntervalTick(250))
			{
				this.CompTickRare();
			}
		}

		// Token: 0x06009E8E RID: 40590 RVA: 0x0038E8A0 File Offset: 0x0038CAA0
		public override void CompTickRare()
		{
			if (!this.CanBeConsumed)
			{
				return;
			}
			this.ConsumedRootsLazy();
			if (this.BeingConsumed)
			{
				this.ticksPassed += 250;
				if (this.AvailableNutrition(false) > 0f)
				{
					EffecterDefOf.HarbingerTreeWillConsume.Spawn(this.parent.Position, this.parent.Map, 1f);
				}
				if (this.ticksPassed >= 5000)
				{
					Map map = this.parent.Map;
					if (this.TryConsume())
					{
						EffecterDefOf.HarbingerTreeConsume.Spawn(this.parent.Position, map, 1f);
					}
					this.ticksPassed -= 5000;
				}
			}
		}

		// Token: 0x06009E8F RID: 40591 RVA: 0x0038E95C File Offset: 0x0038CB5C
		public float AvailableNutrition(bool applyDigestion)
		{
			if (this.Corpse != null && !this.Corpse.InnerPawn.RaceProps.IsFlesh)
			{
				return 0f;
			}
			float result;
			if (this.Corpse != null)
			{
				result = CompHarbingerTreeConsumable.GetNutritionFromCorpse(this.Corpse, applyDigestion);
			}
			else if (this.parent.def.category == ThingCategory.Item)
			{
				result = CompHarbingerTreeConsumable.GetNutritionFromItemStack(this.parent, applyDigestion);
			}
			else
			{
				result = 1f;
			}
			return result;
		}

		// Token: 0x06009E90 RID: 40592 RVA: 0x0038E9D4 File Offset: 0x0038CBD4
		private bool TryConsume()
		{
			float num = this.AvailableNutrition(true);
			if (num == 0f)
			{
				return false;
			}
			foreach (HarbingerTree harbingerTree in this.tmpTreesInRange)
			{
				harbingerTree.AddNutrition(num / (float)this.tmpTreesInRange.Count);
			}
			return true;
		}

		// Token: 0x06009E91 RID: 40593 RVA: 0x0038EA48 File Offset: 0x0038CC48
		private static float GetNutritionFromCorpse(Corpse corpse, bool applyDigestion)
		{
			BodyPartRecord bodyPartRecord;
			(from x in corpse.InnerPawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where x.depth == BodyPartDepth.Outside && !x.def.conceptual && x != corpse.InnerPawn.RaceProps.body.corePart
			select x).TryRandomElement(out bodyPartRecord);
			float bodyPartNutrition;
			if (bodyPartRecord == null)
			{
				bodyPartNutrition = FoodUtility.GetBodyPartNutrition(corpse, corpse.InnerPawn.RaceProps.body.corePart);
				if (applyDigestion)
				{
					corpse.Destroy(DestroyMode.Vanish);
				}
			}
			else
			{
				bodyPartNutrition = FoodUtility.GetBodyPartNutrition(corpse, bodyPartRecord);
				if (applyDigestion)
				{
					Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, corpse.InnerPawn, bodyPartRecord);
					hediff_MissingPart.IsFresh = true;
					hediff_MissingPart.lastInjury = HediffDefOf.Digested;
					corpse.InnerPawn.health.AddHediff(hediff_MissingPart, null, null, null);
				}
			}
			return bodyPartNutrition;
		}

		// Token: 0x06009E92 RID: 40594 RVA: 0x0038EB34 File Offset: 0x0038CD34
		private static float GetNutritionFromItemStack(ThingWithComps item, bool applyDigestion)
		{
			int num = Mathf.Min(item.stackCount, CompHarbingerTreeConsumable.ItemConsumeCountRange.RandomInRange);
			float result;
			if (applyDigestion)
			{
				Thing thing = item.SplitOff(num);
				result = thing.GetStatValue(StatDefOf.Nutrition, true, -1) * (float)num;
				thing.Destroy(DestroyMode.Vanish);
			}
			else
			{
				result = item.GetStatValue(StatDefOf.Nutrition, true, -1) * ((float)num / (float)item.stackCount);
			}
			return result;
		}

		// Token: 0x06009E93 RID: 40595 RVA: 0x0038EB9D File Offset: 0x0038CD9D
		public override string CompInspectStringExtra()
		{
			if (this.BeingConsumed)
			{
				return "ConsumedByHarbingerTree".Translate();
			}
			return "";
		}

		// Token: 0x04005DDC RID: 24028
		private const int ConsumeInterval = 5000;

		// Token: 0x04005DDD RID: 24029
		private static readonly IntRange ItemConsumeCountRange = new IntRange(4, 12);

		// Token: 0x04005DDE RID: 24030
		private int ticksPassed;

		// Token: 0x04005DDF RID: 24031
		private readonly List<HarbingerTree> tmpTreesInRange = new List<HarbingerTree>();
	}
}
