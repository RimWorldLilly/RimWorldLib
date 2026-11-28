using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021CF RID: 8655
	public class QuestNode_GetPawnKind : QuestNode
	{
		// Token: 0x0600D76D RID: 55149 RVA: 0x004FF7EA File Offset: 0x004FD9EA
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return !this.failIfNotFound || this.GetKindDef(slate) != null;
		}

		// Token: 0x0600D76E RID: 55150 RVA: 0x004FF807 File Offset: 0x004FDA07
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600D76F RID: 55151 RVA: 0x004FF814 File Offset: 0x004FDA14
		private void SetVars(Slate slate)
		{
			slate.Set<PawnKindDef>(this.storeAs.GetValue(slate), this.GetKindDef(slate), false);
		}

		// Token: 0x0600D770 RID: 55152 RVA: 0x004FF830 File Offset: 0x004FDA30
		private PawnKindDef GetKindDef(Slate slate)
		{
			QuestNode_GetPawnKind.Option option = this.options.GetValue(slate).RandomElementByWeight((QuestNode_GetPawnKind.Option x) => x.weight);
			PawnKindDef result;
			if (option.kindDef != null)
			{
				result = option.kindDef;
			}
			else if (option.anyAnimal)
			{
				int highestAnimalSkill = option.mustBeAbleToHandleAnimal ? 0 : int.MaxValue;
				if (option.mustBeAbleToHandleAnimal)
				{
					Map map = slate.Get<Map>("map", null, false);
					if (map != null)
					{
						using (List<Pawn>.Enumerator enumerator = map.mapPawns.FreeColonistsSpawned.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Pawn pawn = enumerator.Current;
								highestAnimalSkill = Mathf.Max(highestAnimalSkill, pawn.skills.GetSkill(SkillDefOf.Animals).Level);
							}
							goto IL_158;
						}
					}
					foreach (Pawn pawn2 in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists)
					{
						highestAnimalSkill = Mathf.Max(highestAnimalSkill, pawn2.skills.GetSkill(SkillDefOf.Animals).Level);
					}
				}
				IL_158:
				result = (from x in DefDatabase<PawnKindDef>.AllDefs
				where x.RaceProps.Animal && !x.RaceProps.neverIncludeInQuests && !x.RaceProps.Dryad && (option.onlyAllowedFleshType == null || x.RaceProps.FleshType == option.onlyAllowedFleshType) && base.<GetKindDef>g__CanHandle|1(x)
				select x).RandomElementWithFallback(null);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x040088D6 RID: 35030
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040088D7 RID: 35031
		public SlateRef<List<QuestNode_GetPawnKind.Option>> options;

		// Token: 0x040088D8 RID: 35032
		public bool failIfNotFound;

		// Token: 0x02003D27 RID: 15655
		public class Option
		{
			// Token: 0x0400F171 RID: 61809
			public PawnKindDef kindDef;

			// Token: 0x0400F172 RID: 61810
			public float weight;

			// Token: 0x0400F173 RID: 61811
			public bool anyAnimal;

			// Token: 0x0400F174 RID: 61812
			public bool mustBeAbleToHandleAnimal;

			// Token: 0x0400F175 RID: 61813
			public FleshTypeDef onlyAllowedFleshType;
		}
	}
}
