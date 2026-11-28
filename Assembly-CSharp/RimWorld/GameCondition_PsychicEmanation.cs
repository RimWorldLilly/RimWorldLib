using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200114E RID: 4430
	public class GameCondition_PsychicEmanation : GameCondition
	{
		// Token: 0x1700130F RID: 4879
		// (get) Token: 0x06006F39 RID: 28473 RVA: 0x0026EDAC File Offset: 0x0026CFAC
		public override string Label
		{
			get
			{
				if (this.level == PsychicDroneLevel.GoodMedium)
				{
					return this.def.label + ": " + this.gender.GetLabel(false).CapitalizeFirst();
				}
				if (this.gender != Gender.None)
				{
					return string.Concat(new string[]
					{
						this.def.label,
						": ",
						this.level.GetLabel().CapitalizeFirst(),
						" (",
						this.gender.GetLabel(false).ToLower(),
						")"
					});
				}
				return this.def.label + ": " + this.level.GetLabel().CapitalizeFirst();
			}
		}

		// Token: 0x17001310 RID: 4880
		// (get) Token: 0x06006F3A RID: 28474 RVA: 0x0026EE70 File Offset: 0x0026D070
		public override string LetterText
		{
			get
			{
				if (this.level == PsychicDroneLevel.GoodMedium)
				{
					return this.def.letterText.Formatted(this.gender.GetLabel(false).ToLower());
				}
				return this.def.letterText.Formatted(this.gender.GetLabel(false).ToLower(), this.level.GetLabel());
			}
		}

		// Token: 0x17001311 RID: 4881
		// (get) Token: 0x06006F3B RID: 28475 RVA: 0x0026EEED File Offset: 0x0026D0ED
		public override string Description
		{
			get
			{
				return base.Description.Formatted(this.gender.GetLabel(false).ToLower());
			}
		}

		// Token: 0x06006F3C RID: 28476 RVA: 0x0026EF15 File Offset: 0x0026D115
		public override void PostMake()
		{
			base.PostMake();
			this.level = this.def.defaultDroneLevel;
		}

		// Token: 0x06006F3D RID: 28477 RVA: 0x0026EF30 File Offset: 0x0026D130
		public override void RandomizeSettings(float points, Map map, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			if (this.def.defaultDroneLevel == PsychicDroneLevel.GoodMedium)
			{
				this.level = PsychicDroneLevel.GoodMedium;
			}
			else if (points < 800f)
			{
				this.level = PsychicDroneLevel.BadLow;
			}
			else if (points < 2000f)
			{
				this.level = PsychicDroneLevel.BadMedium;
			}
			else
			{
				this.level = PsychicDroneLevel.BadHigh;
			}
			if (map.mapPawns.FreeColonistsCount > 0)
			{
				this.gender = map.mapPawns.FreeColonists.RandomElement<Pawn>().gender;
			}
			else
			{
				this.gender = Rand.Element<Gender>(Gender.Male, Gender.Female);
			}
			outExtraDescriptionRules.Add(new Rule_String("psychicDroneLevel", this.level.GetLabel()));
			outExtraDescriptionRules.Add(new Rule_String("psychicDroneGender", this.gender.GetLabel(false)));
		}

		// Token: 0x06006F3E RID: 28478 RVA: 0x0026EFEC File Offset: 0x0026D1EC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<PsychicDroneLevel>(ref this.level, "level", PsychicDroneLevel.None, false);
		}

		// Token: 0x04004719 RID: 18201
		public Gender gender;

		// Token: 0x0400471A RID: 18202
		public PsychicDroneLevel level = PsychicDroneLevel.BadMedium;

		// Token: 0x0400471B RID: 18203
		public const float MaxPointsDroneLow = 800f;

		// Token: 0x0400471C RID: 18204
		public const float MaxPointsDroneMedium = 2000f;
	}
}
