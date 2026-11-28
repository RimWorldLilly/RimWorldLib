using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001135 RID: 4405
	public class GameCondition_PsychicSuppression : GameCondition
	{
		// Token: 0x170012F3 RID: 4851
		// (get) Token: 0x06006E9F RID: 28319 RVA: 0x0026CD39 File Offset: 0x0026AF39
		public override string LetterText
		{
			get
			{
				return base.LetterText.Formatted(this.gender.GetLabel(false).ToLower());
			}
		}

		// Token: 0x170012F4 RID: 4852
		// (get) Token: 0x06006EA0 RID: 28320 RVA: 0x0026CD61 File Offset: 0x0026AF61
		public override string Description
		{
			get
			{
				return base.Description.Formatted(this.gender.GetLabel(false).ToLower());
			}
		}

		// Token: 0x06006EA1 RID: 28321 RVA: 0x0026CD89 File Offset: 0x0026AF89
		public override void Init()
		{
			base.Init();
		}

		// Token: 0x06006EA2 RID: 28322 RVA: 0x0026CD91 File Offset: 0x0026AF91
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
		}

		// Token: 0x06006EA3 RID: 28323 RVA: 0x0026CDAC File Offset: 0x0026AFAC
		public static void CheckPawn(Pawn pawn, Gender targetGender)
		{
			if (pawn.RaceProps.Humanlike && pawn.gender == targetGender && !pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicSuppression, false))
			{
				pawn.health.AddHediff(HediffDefOf.PsychicSuppression, null, null, null);
			}
		}

		// Token: 0x06006EA4 RID: 28324 RVA: 0x0026CE04 File Offset: 0x0026B004
		public override void GameConditionTick()
		{
			foreach (Map map in base.AffectedMaps)
			{
				List<Pawn> allPawns = map.mapPawns.AllPawns;
				for (int i = 0; i < allPawns.Count; i++)
				{
					GameCondition_PsychicSuppression.CheckPawn(allPawns[i], this.gender);
				}
			}
		}

		// Token: 0x06006EA5 RID: 28325 RVA: 0x0026CE80 File Offset: 0x0026B080
		public override void RandomizeSettings(float points, Map map, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.RandomizeSettings(points, map, outExtraDescriptionRules, outExtraDescriptionConstants);
			if (map.mapPawns.FreeColonistsCount > 0)
			{
				this.gender = map.mapPawns.FreeColonists.RandomElement<Pawn>().gender;
			}
			else
			{
				this.gender = Rand.Element<Gender>(Gender.Male, Gender.Female);
			}
			outExtraDescriptionRules.Add(new Rule_String("psychicSuppressorGender", this.gender.GetLabel(false)));
		}

		// Token: 0x040046CC RID: 18124
		public Gender gender;
	}
}
