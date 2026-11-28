using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021B7 RID: 8631
	public class QuestNode_GetAnimalToHunt : QuestNode
	{
		// Token: 0x0600D701 RID: 55041 RVA: 0x004FCD3D File Offset: 0x004FAF3D
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate);
		}

		// Token: 0x0600D702 RID: 55042 RVA: 0x004FCD46 File Offset: 0x004FAF46
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600D703 RID: 55043 RVA: 0x004FCD54 File Offset: 0x004FAF54
		private bool DoWork(Slate slate)
		{
			Map map = slate.Get<Map>("map", null, false);
			if (map == null)
			{
				return false;
			}
			float x2 = slate.Get<float>("points", 0f, false);
			float animalDifficultyFromPoints = this.pointsToAnimalDifficultyCurve.GetValue(slate).Evaluate(x2);
			PawnKindDef pawnKindDef;
			if (!map.Biome.AllWildAnimals.Where(delegate(PawnKindDef x)
			{
				if (map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(x.race, 0f))
				{
					return map.listerThings.ThingsOfDef(x.race).Any((Thing p) => p.Faction == null);
				}
				return false;
			}).TryRandomElementByWeight((PawnKindDef x) => this.AnimalCommonalityByDifficulty(x, animalDifficultyFromPoints), out pawnKindDef))
			{
				return false;
			}
			int num = 0;
			for (int i = 0; i < map.mapPawns.AllPawnsSpawned.Count; i++)
			{
				Pawn pawn = map.mapPawns.AllPawnsSpawned[i];
				if (pawn.def == pawnKindDef.race && !pawn.IsQuestLodger() && pawn.Faction == null)
				{
					num++;
				}
			}
			SimpleCurve value = this.pointsToAnimalsToHuntCountCurve.GetValue(slate);
			float randomInRange = (this.animalsToHuntCountRandomFactorRange.GetValue(slate) ?? FloatRange.One).RandomInRange;
			int num2 = Mathf.RoundToInt(value.Evaluate(x2) * randomInRange);
			num2 = Mathf.Min(num2, num);
			num2 = Mathf.Max(num2, 1);
			slate.Set<ThingDef>(this.storeAnimalToHuntAs.GetValue(slate), pawnKindDef.race, false);
			slate.Set<int>(this.storeCountToHuntAs.GetValue(slate), num2, false);
			return true;
		}

		// Token: 0x0600D704 RID: 55044 RVA: 0x004FCEE0 File Offset: 0x004FB0E0
		private float AnimalCommonalityByDifficulty(PawnKindDef animalKind, float animalDifficultyFromPoints)
		{
			float num = Mathf.Abs(animalKind.GetAnimalPointsToHuntOrSlaughter() - animalDifficultyFromPoints);
			return 1f / num;
		}

		// Token: 0x0400885E RID: 34910
		[NoTranslate]
		public SlateRef<string> storeAnimalToHuntAs;

		// Token: 0x0400885F RID: 34911
		[NoTranslate]
		public SlateRef<string> storeCountToHuntAs;

		// Token: 0x04008860 RID: 34912
		public SlateRef<SimpleCurve> pointsToAnimalsToHuntCountCurve;

		// Token: 0x04008861 RID: 34913
		public SlateRef<SimpleCurve> pointsToAnimalDifficultyCurve;

		// Token: 0x04008862 RID: 34914
		public SlateRef<FloatRange?> animalsToHuntCountRandomFactorRange;
	}
}
