using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02000EB0 RID: 3760
	public class BookOutcomeDoerGainSkillExp : BookOutcomeDoer
	{
		// Token: 0x170010DB RID: 4315
		// (get) Token: 0x060062F9 RID: 25337 RVA: 0x00239CD2 File Offset: 0x00237ED2
		public new BookOutcomeProperties_GainSkillExp Props
		{
			get
			{
				return (BookOutcomeProperties_GainSkillExp)this.props;
			}
		}

		// Token: 0x170010DC RID: 4316
		// (get) Token: 0x060062FA RID: 25338 RVA: 0x00239CDF File Offset: 0x00237EDF
		public IReadOnlyDictionary<SkillDef, float> Values
		{
			get
			{
				return this.values;
			}
		}

		// Token: 0x060062FB RID: 25339 RVA: 0x00239CE8 File Offset: 0x00237EE8
		public override bool DoesProvidesOutcome(Pawn reader)
		{
			foreach (KeyValuePair<SkillDef, float> keyValuePair in this.values)
			{
				SkillDef skillDef;
				float num;
				keyValuePair.Deconstruct(out skillDef, out num);
				SkillDef skill = skillDef;
				if (BookOutcomeDoerGainSkillExp.CanProgressSkill(reader, skill, base.Quality))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060062FC RID: 25340 RVA: 0x00239D5C File Offset: 0x00237F5C
		public override void OnBookGenerated(Pawn author = null)
		{
			List<SkillDef> availableSkills = this.GetAvailableSkills(null);
			int count = Rand.Chance(0.25f) ? 2 : 1;
			List<SkillDef> list = availableSkills.TakeRandomDistinct(count);
			float num = BookUtility.GetSkillExpForQuality(base.Quality);
			if (list.Count > 1)
			{
				num *= 1.25f;
			}
			float value = num / (float)list.Count;
			for (int i = 0; i < list.Count; i++)
			{
				this.values[list[i]] = value;
			}
		}

		// Token: 0x060062FD RID: 25341 RVA: 0x00239DD8 File Offset: 0x00237FD8
		protected virtual List<SkillDef> GetAvailableSkills(Pawn author = null)
		{
			List<SkillDef> list = new List<SkillDef>();
			if (this.Props.skills.Count > 0)
			{
				using (List<BookOutcomeProperties_GainSkillExp.BookStatReward>.Enumerator enumerator = this.Props.skills.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BookOutcomeProperties_GainSkillExp.BookStatReward bookStatReward = enumerator.Current;
						list.Add(bookStatReward.skill);
					}
					return list;
				}
			}
			list = DefDatabase<SkillDef>.AllDefsListForReading;
			return list;
		}

		// Token: 0x060062FE RID: 25342 RVA: 0x00239E58 File Offset: 0x00238058
		public override void Reset()
		{
			this.values.Clear();
		}

		// Token: 0x060062FF RID: 25343 RVA: 0x00239E68 File Offset: 0x00238068
		public override void OnReadingTick(Pawn reader, float factor)
		{
			foreach (KeyValuePair<SkillDef, float> keyValuePair in this.values)
			{
				SkillDef skillDef;
				float num;
				keyValuePair.Deconstruct(out skillDef, out num);
				SkillDef skillDef2 = skillDef;
				float num2 = num;
				if (BookOutcomeDoerGainSkillExp.CanProgressSkill(reader, skillDef2, base.Quality))
				{
					reader.skills.GetSkill(skillDef2).Learn(num2 * factor, false, false);
				}
			}
		}

		// Token: 0x06006300 RID: 25344 RVA: 0x00239EEC File Offset: 0x002380EC
		public override string GetBenefitsString(Pawn reader = null)
		{
			if (this.values.Count == 0)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<SkillDef, float> keyValuePair in this.values)
			{
				SkillDef skillDef;
				float num;
				keyValuePair.Deconstruct(out skillDef, out num);
				SkillDef skillDef2 = skillDef;
				float num2 = num;
				float num3 = 1f;
				if (reader != null)
				{
					num3 = reader.skills.GetSkill(skillDef2).LearnRateFactor(false);
					num2 *= num3;
				}
				float f = num2 * 2500f;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(skillDef2.LabelCap);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("XpPerHour".Translate(f.ToStringDecimalIfSmall()));
				string text = defaultInterpolatedStringHandler.ToStringAndClear();
				int maxSkillLevel = BookOutcomeDoerGainSkillExp.GetMaxSkillLevel(base.Quality);
				string str = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(3, 1);
				defaultInterpolatedStringHandler2.AppendLiteral(" (");
				defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("BookMaxLevel".Translate(maxSkillLevel));
				defaultInterpolatedStringHandler2.AppendLiteral(")");
				text = str + defaultInterpolatedStringHandler2.ToStringAndClear();
				if (!Mathf.Approximately(num3, 1f))
				{
					string str2 = text;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(5, 2);
					defaultInterpolatedStringHandler3.AppendLiteral(" (x");
					defaultInterpolatedStringHandler3.AppendFormatted(num3.ToStringPercent("0"));
					defaultInterpolatedStringHandler3.AppendLiteral(" ");
					defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("BookLearningModifier".Translate());
					defaultInterpolatedStringHandler3.AppendLiteral(")");
					text = str2 + defaultInterpolatedStringHandler3.ToStringAndClear();
				}
				stringBuilder.AppendLine(" - " + text);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06006301 RID: 25345 RVA: 0x0023A0C8 File Offset: 0x002382C8
		public override IEnumerable<RulePack> GetTopicRulePacks()
		{
			return from x in this.values.Keys
			select x.generalRules;
		}

		// Token: 0x06006302 RID: 25346 RVA: 0x0023A0F9 File Offset: 0x002382F9
		public override void PostExposeData()
		{
			Scribe_Collections.Look<SkillDef, float>(ref this.values, "values", LookMode.Undefined, LookMode.Undefined);
		}

		// Token: 0x06006303 RID: 25347 RVA: 0x0023A10D File Offset: 0x0023830D
		private static bool CanProgressSkill(Pawn pawn, SkillDef skill, QualityCategory quality)
		{
			return !pawn.skills.GetSkill(skill).TotallyDisabled && pawn.skills.GetSkill(skill).GetLevel(false) < BookOutcomeDoerGainSkillExp.GetMaxSkillLevel(quality);
		}

		// Token: 0x06006304 RID: 25348 RVA: 0x0023A13E File Offset: 0x0023833E
		private static int GetMaxSkillLevel(QualityCategory quality)
		{
			return Mathf.RoundToInt(BookOutcomeDoerGainSkillExp.QualityMaxLevel.Evaluate((float)quality));
		}

		// Token: 0x04003696 RID: 13974
		private Dictionary<SkillDef, float> values = new Dictionary<SkillDef, float>();

		// Token: 0x04003697 RID: 13975
		private const float MultipleSkillFactor = 1.25f;

		// Token: 0x04003698 RID: 13976
		private static readonly SimpleCurve QualityMaxLevel = new SimpleCurve
		{
			{
				new CurvePoint(0f, 2f),
				true
			},
			{
				new CurvePoint(1f, 5f),
				true
			},
			{
				new CurvePoint(2f, 8f),
				true
			},
			{
				new CurvePoint(3f, 10f),
				true
			},
			{
				new CurvePoint(4f, 12f),
				true
			},
			{
				new CurvePoint(5f, 14f),
				true
			},
			{
				new CurvePoint(6f, 16f),
				true
			}
		};
	}
}
