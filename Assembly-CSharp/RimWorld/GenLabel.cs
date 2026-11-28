using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F1D RID: 7965
	public static class GenLabel
	{
		// Token: 0x0600CB25 RID: 52005 RVA: 0x004AA557 File Offset: 0x004A8757
		public static void ClearCache()
		{
			GenLabel.labelDictionary.Clear();
		}

		// Token: 0x0600CB26 RID: 52006 RVA: 0x004AA564 File Offset: 0x004A8764
		public static string ThingLabel(BuildableDef entDef, ThingDef stuffDef, int stackCount = 1)
		{
			GenLabel.LabelRequest key = default(GenLabel.LabelRequest);
			key.entDef = entDef;
			key.stuffDef = stuffDef;
			key.stackCount = stackCount;
			string text;
			if (!GenLabel.labelDictionary.TryGetValue(key, out text))
			{
				if (GenLabel.labelDictionary.Count > 2000)
				{
					GenLabel.labelDictionary.Clear();
				}
				text = GenLabel.NewThingLabel(entDef, stuffDef, stackCount);
				GenLabel.labelDictionary.Add(key, text);
			}
			return text;
		}

		// Token: 0x0600CB27 RID: 52007 RVA: 0x004AA5D4 File Offset: 0x004A87D4
		private static string NewThingLabel(BuildableDef entDef, ThingDef stuffDef, int stackCount)
		{
			string text;
			if (stuffDef == null)
			{
				text = entDef.label;
			}
			else
			{
				text = "ThingMadeOfStuffLabel".Translate(stuffDef.LabelAsStuff, entDef.label);
			}
			if (stackCount > 1)
			{
				text = text + " x" + stackCount.ToStringCached();
			}
			return text;
		}

		// Token: 0x0600CB28 RID: 52008 RVA: 0x004AA62C File Offset: 0x004A882C
		public static string ThingLabel(Thing t, int stackCount, bool includeHp = true, bool includeQuality = true)
		{
			GenLabel.LabelRequest key = default(GenLabel.LabelRequest);
			key.entDef = t.def;
			key.stuffDef = t.Stuff;
			key.styleDef = t.StyleDef;
			key.stackCount = stackCount;
			key.hasQuality = (includeQuality && t.TryGetQuality(out key.quality));
			key.includeHealth = includeHp;
			if (t.def.useHitPoints)
			{
				key.health = t.HitPoints;
				key.maxHealth = t.MaxHitPoints;
			}
			Apparel apparel = t as Apparel;
			if (apparel != null)
			{
				key.wornByCorpse = apparel.WornByCorpse;
			}
			string text;
			if (!GenLabel.labelDictionary.TryGetValue(key, out text))
			{
				if (GenLabel.labelDictionary.Count > 2000)
				{
					GenLabel.labelDictionary.Clear();
				}
				text = GenLabel.NewThingLabel(t, stackCount, includeHp, includeQuality);
				GenLabel.labelDictionary[key] = text;
			}
			return text;
		}

		// Token: 0x0600CB29 RID: 52009 RVA: 0x004AA714 File Offset: 0x004A8914
		private static string NewThingLabel(Thing t, int stackCount, bool includeHp, bool includeQuality)
		{
			ThingStyleDef styleDef = t.StyleDef;
			string text;
			if (styleDef != null && !styleDef.overrideLabel.NullOrEmpty())
			{
				text = styleDef.overrideLabel;
			}
			else
			{
				text = GenLabel.ThingLabel(t.def, t.Stuff, 1);
			}
			text += GenLabel.LabelExtras(t, includeHp, includeQuality);
			if (stackCount > 1)
			{
				text = text + " x" + stackCount.ToStringCached();
			}
			return text;
		}

		// Token: 0x0600CB2A RID: 52010 RVA: 0x004AA77C File Offset: 0x004A897C
		public static string LabelExtras(Thing t, bool includeHp, bool includeQuality)
		{
			string text = string.Empty;
			QualityCategory cat;
			bool flag = t.TryGetQuality(out cat) && includeQuality;
			int hitPoints = t.HitPoints;
			int maxHitPoints = t.MaxHitPoints;
			bool flag2 = t.def.useHitPoints && hitPoints < maxHitPoints && t.def.stackLimit == 1 && includeHp;
			Apparel apparel = t as Apparel;
			bool flag3 = apparel != null && apparel.WornByCorpse;
			if (flag || flag2 || flag3)
			{
				text += " (";
				if (flag)
				{
					text += cat.GetLabel();
				}
				if (flag2)
				{
					if (flag)
					{
						text += " ";
					}
					text += ((float)hitPoints / (float)maxHitPoints).ToStringPercent();
				}
				if (flag3)
				{
					if (flag || flag2)
					{
						text += " ";
					}
					text += "WornByCorpseChar".Translate();
				}
				text += ")";
			}
			return text;
		}

		// Token: 0x0600CB2B RID: 52011 RVA: 0x004AA86C File Offset: 0x004A8A6C
		public static string ThingsLabel(IEnumerable<Thing> things, string prefix = "  - ")
		{
			GenLabel.tmpThingCounts.Clear();
			IList<Thing> list = things as IList<Thing>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					GenLabel.tmpThingCounts.Add(new ThingCount(list[i], list[i].stackCount, false));
				}
			}
			else
			{
				foreach (Thing thing in things)
				{
					GenLabel.tmpThingCounts.Add(new ThingCount(thing, thing.stackCount, false));
				}
			}
			string result = GenLabel.ThingsLabel(GenLabel.tmpThingCounts, prefix, false);
			GenLabel.tmpThingCounts.Clear();
			return result;
		}

		// Token: 0x0600CB2C RID: 52012 RVA: 0x004AA924 File Offset: 0x004A8B24
		public static string ThingsLabel(List<ThingCount> things, string prefix = "  - ", bool ignoreStackLimit = false)
		{
			GenLabel.tmpThingsLabelElements.Clear();
			using (List<ThingCount>.Enumerator enumerator = things.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingCount thing = enumerator.Current;
					GenLabel.LabelElement labelElement = GenLabel.tmpThingsLabelElements.FirstOrDefault((GenLabel.LabelElement elem) => (thing.Thing.def.stackLimit > 1 | ignoreStackLimit) && elem.thingTemplate.def == thing.Thing.def && elem.thingTemplate.Stuff == thing.Thing.Stuff);
					if (labelElement != null)
					{
						labelElement.count += thing.Count;
					}
					else
					{
						GenLabel.tmpThingsLabelElements.Add(new GenLabel.LabelElement
						{
							thingTemplate = thing.Thing,
							count = thing.Count
						});
					}
				}
			}
			GenLabel.tmpThingsLabelElements.Sort(delegate(GenLabel.LabelElement lhs, GenLabel.LabelElement rhs)
			{
				int num = TransferableComparer_Category.Compare(lhs.thingTemplate.def, rhs.thingTemplate.def);
				if (num != 0)
				{
					return num;
				}
				return lhs.thingTemplate.MarketValue.CompareTo(rhs.thingTemplate.MarketValue);
			});
			StringBuilder stringBuilder = new StringBuilder();
			foreach (GenLabel.LabelElement labelElement2 in GenLabel.tmpThingsLabelElements)
			{
				string str = "";
				if (labelElement2.thingTemplate.ParentHolder is Pawn_ApparelTracker)
				{
					str = " (" + "WornBy".Translate(((Pawn)labelElement2.thingTemplate.ParentHolder.ParentHolder).LabelShort, (Pawn)labelElement2.thingTemplate.ParentHolder.ParentHolder) + ")";
				}
				else if (labelElement2.thingTemplate.ParentHolder is Pawn_EquipmentTracker)
				{
					str = " (" + "EquippedBy".Translate(((Pawn)labelElement2.thingTemplate.ParentHolder.ParentHolder).LabelShort, (Pawn)labelElement2.thingTemplate.ParentHolder.ParentHolder) + ")";
				}
				if (labelElement2.count == 1)
				{
					stringBuilder.AppendLine(prefix + labelElement2.thingTemplate.LabelCap + str);
				}
				else
				{
					stringBuilder.AppendLine(prefix + GenLabel.ThingLabel(labelElement2.thingTemplate.def, labelElement2.thingTemplate.Stuff, labelElement2.count).CapitalizeFirst() + str);
				}
			}
			GenLabel.tmpThingsLabelElements.Clear();
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600CB2D RID: 52013 RVA: 0x004AABF0 File Offset: 0x004A8DF0
		public static string BestKindLabel(Pawn pawn, bool mustNoteGender = false, bool mustNoteLifeStage = false, bool plural = false, int pluralCount = -1)
		{
			if (plural && pluralCount == 1)
			{
				plural = false;
			}
			bool flag = false;
			bool flag2 = false;
			string text = null;
			switch (pawn.gender)
			{
			case Gender.None:
				if (plural && !pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelPlural != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelPlural;
					flag2 = true;
				}
				else if (!pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.label != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.label;
					flag2 = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, pawn.gender, pluralCount);
					}
				}
				else
				{
					text = GenLabel.BestKindLabel(pawn.kindDef, Gender.None, out flag, plural, pluralCount);
				}
				break;
			case Gender.Male:
				if (plural && !pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelMalePlural != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelMalePlural;
					flag2 = true;
					flag = true;
				}
				else if (!pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelMale != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelMale;
					flag2 = true;
					flag = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, pawn.gender, pluralCount);
					}
				}
				else if (plural && !pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelPlural != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelPlural;
					flag2 = true;
				}
				else if (!pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.label != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.label;
					flag2 = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, pawn.gender, pluralCount);
					}
				}
				else
				{
					text = GenLabel.BestKindLabel(pawn.kindDef, Gender.Male, out flag, plural, pluralCount);
				}
				break;
			case Gender.Female:
				if (plural && !pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelFemalePlural != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelFemalePlural;
					flag2 = true;
					flag = true;
				}
				else if (!pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelFemale != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelFemale;
					flag2 = true;
					flag = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, pawn.gender, pluralCount);
					}
				}
				else if (plural && !pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.labelPlural != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.labelPlural;
					flag2 = true;
				}
				else if (!pawn.RaceProps.Humanlike && pawn.ageTracker.CurKindLifeStage.label != null)
				{
					text = pawn.ageTracker.CurKindLifeStage.label;
					flag2 = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, pawn.gender, pluralCount);
					}
				}
				else
				{
					text = GenLabel.BestKindLabel(pawn.kindDef, Gender.Female, out flag, plural, pluralCount);
				}
				break;
			}
			if (mustNoteGender && !flag && pawn.gender != Gender.None)
			{
				text = "PawnMainDescGendered".Translate(pawn.GetGenderLabel(), text, pawn.Named("PAWN"));
			}
			if (mustNoteLifeStage && !flag2 && pawn.ageTracker != null && pawn.ageTracker.CurLifeStage.visible)
			{
				text = "PawnMainDescLifestageWrap".Translate(text, pawn.ageTracker.CurLifeStage.Adjective, pawn);
			}
			return text;
		}

		// Token: 0x0600CB2E RID: 52014 RVA: 0x004AAFB0 File Offset: 0x004A91B0
		public static string BestKindLabel(PawnKindDef kindDef, Gender gender, bool plural = false, int pluralCount = -1)
		{
			bool flag;
			return GenLabel.BestKindLabel(kindDef, gender, out flag, plural, pluralCount);
		}

		// Token: 0x0600CB2F RID: 52015 RVA: 0x004AAFC8 File Offset: 0x004A91C8
		public static string BestKindLabel(PawnKindDef kindDef, Gender gender, out bool genderNoted, bool plural = false, int pluralCount = -1)
		{
			if (plural && pluralCount == 1)
			{
				plural = false;
			}
			string text = null;
			genderNoted = false;
			switch (gender)
			{
			case Gender.None:
				if (plural && kindDef.labelPlural != null)
				{
					text = kindDef.labelPlural;
				}
				else
				{
					text = kindDef.label;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, gender, pluralCount);
					}
				}
				break;
			case Gender.Male:
				if (plural && kindDef.labelMalePlural != null)
				{
					text = kindDef.labelMalePlural;
					genderNoted = true;
				}
				else if (kindDef.labelMale != null)
				{
					text = kindDef.labelMale;
					genderNoted = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, gender, pluralCount);
					}
				}
				else if (plural && kindDef.labelPlural != null)
				{
					text = kindDef.labelPlural;
				}
				else
				{
					text = kindDef.label;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, gender, pluralCount);
					}
				}
				break;
			case Gender.Female:
				if (plural && kindDef.labelFemalePlural != null)
				{
					text = kindDef.labelFemalePlural;
					genderNoted = true;
				}
				else if (kindDef.labelFemale != null)
				{
					text = kindDef.labelFemale;
					genderNoted = true;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, gender, pluralCount);
					}
				}
				else if (plural && kindDef.labelPlural != null)
				{
					text = kindDef.labelPlural;
				}
				else
				{
					text = kindDef.label;
					if (plural)
					{
						text = Find.ActiveLanguageWorker.Pluralize(text, gender, pluralCount);
					}
				}
				break;
			}
			return text;
		}

		// Token: 0x0600CB30 RID: 52016 RVA: 0x004AB117 File Offset: 0x004A9317
		public static bool MultipleItemsPerCellDrawn(this Thing t)
		{
			return t.def.category == ThingCategory.Item && t.Spawned && t.Position.GetItemCount(t.Map) >= 2;
		}

		// Token: 0x0600CB31 RID: 52017 RVA: 0x004AB148 File Offset: 0x004A9348
		public static string BestGroupLabel(List<Pawn> pawns, bool definite, out Pawn singlePawn)
		{
			singlePawn = null;
			if (!pawns.Any<Pawn>())
			{
				return "";
			}
			if (pawns.Count == 1)
			{
				singlePawn = pawns[0];
				if (!definite)
				{
					return pawns[0].LabelShort;
				}
				return pawns[0].LabelDefinite();
			}
			else
			{
				GenLabel.tmpHumanlikes.Clear();
				for (int i = 0; i < pawns.Count; i++)
				{
					if (!pawns[i].AnimalOrWildMan())
					{
						GenLabel.tmpHumanlikes.Add(pawns[i]);
					}
				}
				if (GenLabel.tmpHumanlikes.Any<Pawn>())
				{
					if (GenLabel.tmpHumanlikes.Count != 1)
					{
						GenLabel.tmpHumanlikeLabels.Clear();
						for (int j = 0; j < GenLabel.tmpHumanlikes.Count; j++)
						{
							if (GenLabel.tmpHumanlikes[j].Faction != null)
							{
								string text;
								if (definite)
								{
									text = Find.ActiveLanguageWorker.WithDefiniteArticle(GenLabel.tmpHumanlikes[j].Faction.def.pawnsPlural, LanguageDatabase.activeLanguage.ResolveGender(GenLabel.tmpHumanlikes[j].Faction.def.pawnsPlural, GenLabel.tmpHumanlikes[j].Faction.def.pawnSingular, Gender.Male), true, false);
								}
								else
								{
									text = GenLabel.tmpHumanlikes[j].Faction.def.pawnsPlural;
								}
								if (!GenLabel.tmpHumanlikeLabels.ContainsKey(text))
								{
									GenLabel.tmpHumanlikeLabels.Add(text, 1);
								}
								else
								{
									Dictionary<string, int> dictionary = GenLabel.tmpHumanlikeLabels;
									string key = text;
									int num = dictionary[key];
									dictionary[key] = num + 1;
								}
							}
							else
							{
								string text2;
								if (definite)
								{
									text2 = Find.ActiveLanguageWorker.WithDefiniteArticle(GenLabel.tmpHumanlikes[j].kindDef.GetLabelPlural(-1), LanguageDatabase.activeLanguage.ResolveGender(GenLabel.tmpHumanlikes[j].kindDef.GetLabelPlural(-1), GenLabel.tmpHumanlikes[j].kindDef.label, Gender.Male), true, false);
								}
								else
								{
									text2 = GenLabel.tmpHumanlikes[j].kindDef.GetLabelPlural(-1);
								}
								if (!GenLabel.tmpHumanlikeLabels.ContainsKey(text2))
								{
									GenLabel.tmpHumanlikeLabels.Add(text2, 1);
								}
								else
								{
									Dictionary<string, int> dictionary2 = GenLabel.tmpHumanlikeLabels;
									string key = text2;
									int num = dictionary2[key];
									dictionary2[key] = num + 1;
								}
							}
						}
						int num2 = -1;
						string result = null;
						foreach (KeyValuePair<string, int> keyValuePair in GenLabel.tmpHumanlikeLabels)
						{
							if (keyValuePair.Value > num2)
							{
								num2 = keyValuePair.Value;
								result = keyValuePair.Key;
							}
						}
						GenLabel.tmpHumanlikeLabels.Clear();
						GenLabel.tmpHumanlikes.Clear();
						return result;
					}
					singlePawn = GenLabel.tmpHumanlikes[0];
					if (!definite)
					{
						return GenLabel.tmpHumanlikes[0].LabelShort;
					}
					return GenLabel.tmpHumanlikes[0].LabelDefinite();
				}
				else
				{
					GenLabel.tmpLabels.Clear();
					for (int k = 0; k < pawns.Count; k++)
					{
						string text3;
						if (definite)
						{
							text3 = Find.ActiveLanguageWorker.WithDefiniteArticle(pawns[k].kindDef.GetLabelPlural(-1), LanguageDatabase.activeLanguage.ResolveGender(pawns[k].kindDef.GetLabelPlural(-1), pawns[k].kindDef.label, Gender.Male), true, false);
						}
						else
						{
							text3 = pawns[k].kindDef.GetLabelPlural(-1);
						}
						if (!GenLabel.tmpLabels.ContainsKey(text3))
						{
							GenLabel.tmpLabels.Add(text3, 1);
						}
						else
						{
							Dictionary<string, int> dictionary3 = GenLabel.tmpLabels;
							string key = text3;
							int num = dictionary3[key];
							dictionary3[key] = num + 1;
						}
					}
					int num3 = -1;
					string result2 = null;
					foreach (KeyValuePair<string, int> keyValuePair2 in GenLabel.tmpLabels)
					{
						if (keyValuePair2.Value > num3)
						{
							num3 = keyValuePair2.Value;
							result2 = keyValuePair2.Key;
						}
					}
					GenLabel.tmpLabels.Clear();
					GenLabel.tmpHumanlikes.Clear();
					if ((float)num3 / (float)pawns.Count >= 0.5f)
					{
						return result2;
					}
					if (definite)
					{
						return Find.ActiveLanguageWorker.WithDefiniteArticle("AnimalsLower".Translate(), true, false);
					}
					return "AnimalsLower".Translate();
				}
			}
		}

		// Token: 0x0400818F RID: 33167
		private static Dictionary<GenLabel.LabelRequest, string> labelDictionary = new Dictionary<GenLabel.LabelRequest, string>();

		// Token: 0x04008190 RID: 33168
		private const int LabelDictionaryMaxCount = 2000;

		// Token: 0x04008191 RID: 33169
		private static List<ThingCount> tmpThingCounts = new List<ThingCount>();

		// Token: 0x04008192 RID: 33170
		private static List<GenLabel.LabelElement> tmpThingsLabelElements = new List<GenLabel.LabelElement>();

		// Token: 0x04008193 RID: 33171
		private static List<Pawn> tmpHumanlikes = new List<Pawn>();

		// Token: 0x04008194 RID: 33172
		private static Dictionary<string, int> tmpHumanlikeLabels = new Dictionary<string, int>();

		// Token: 0x04008195 RID: 33173
		private static Dictionary<string, int> tmpLabels = new Dictionary<string, int>();

		// Token: 0x02003BB9 RID: 15289
		private class LabelElement
		{
			// Token: 0x0400ECA9 RID: 60585
			public Thing thingTemplate;

			// Token: 0x0400ECAA RID: 60586
			public int count;
		}

		// Token: 0x02003BBA RID: 15290
		private struct LabelRequest : IEquatable<GenLabel.LabelRequest>
		{
			// Token: 0x0601566A RID: 87658 RVA: 0x006C4E39 File Offset: 0x006C3039
			public static bool operator ==(GenLabel.LabelRequest lhs, GenLabel.LabelRequest rhs)
			{
				return lhs.Equals(rhs);
			}

			// Token: 0x0601566B RID: 87659 RVA: 0x006C4E43 File Offset: 0x006C3043
			public static bool operator !=(GenLabel.LabelRequest lhs, GenLabel.LabelRequest rhs)
			{
				return !(lhs == rhs);
			}

			// Token: 0x0601566C RID: 87660 RVA: 0x006C4E50 File Offset: 0x006C3050
			public override bool Equals(object obj)
			{
				if (obj is GenLabel.LabelRequest)
				{
					GenLabel.LabelRequest other = (GenLabel.LabelRequest)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x0601566D RID: 87661 RVA: 0x006C4E78 File Offset: 0x006C3078
			public bool Equals(GenLabel.LabelRequest other)
			{
				return this.entDef == other.entDef && this.stuffDef == other.stuffDef && this.styleDef == other.styleDef && this.stackCount == other.stackCount && this.quality == other.quality && this.hasQuality == other.hasQuality && this.health == other.health && this.maxHealth == other.maxHealth && this.includeHealth == other.includeHealth && this.wornByCorpse == other.wornByCorpse;
			}

			// Token: 0x0601566E RID: 87662 RVA: 0x006C4F14 File Offset: 0x006C3114
			public override int GetHashCode()
			{
				int num = 0;
				num = Gen.HashCombine<BuildableDef>(num, this.entDef);
				num = Gen.HashCombine<ThingDef>(num, this.stuffDef);
				ThingDef thingDef = this.entDef as ThingDef;
				if (thingDef != null)
				{
					num = Gen.HashCombine<ThingStyleDef>(num, this.styleDef);
					num = Gen.HashCombineInt(num, this.stackCount);
					num = Gen.HashCombineStruct<QualityCategory>(num, this.quality);
					if (thingDef.useHitPoints)
					{
						num = Gen.HashCombineInt(num, this.health);
						num = Gen.HashCombineInt(num, this.maxHealth);
					}
					num = Gen.HashCombineInt(num, (this.wornByCorpse > false) ? 1 : 0);
					num = Gen.HashCombineInt(num, (this.hasQuality > false) ? 1 : 0);
					num = Gen.HashCombineInt(num, (this.includeHealth > false) ? 1 : 0);
				}
				return num;
			}

			// Token: 0x0601566F RID: 87663 RVA: 0x006C4FC8 File Offset: 0x006C31C8
			public override string ToString()
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(106, 9);
				defaultInterpolatedStringHandler.AppendLiteral("entDef=");
				defaultInterpolatedStringHandler.AppendFormatted<BuildableDef>(this.entDef);
				defaultInterpolatedStringHandler.AppendLiteral(", stuffDef=");
				defaultInterpolatedStringHandler.AppendFormatted((this.stuffDef != null) ? this.stuffDef.defName : "null");
				defaultInterpolatedStringHandler.AppendLiteral(", stackCount=");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.stackCount);
				defaultInterpolatedStringHandler.AppendLiteral(", quality=");
				defaultInterpolatedStringHandler.AppendFormatted<QualityCategory>(this.quality);
				defaultInterpolatedStringHandler.AppendLiteral(", health=");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.health);
				defaultInterpolatedStringHandler.AppendLiteral(", maxHealth=");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.maxHealth);
				defaultInterpolatedStringHandler.AppendLiteral(", wornByCorpse=");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(this.wornByCorpse);
				defaultInterpolatedStringHandler.AppendLiteral(", hasQuality=");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(this.hasQuality);
				defaultInterpolatedStringHandler.AppendLiteral(", includeHealth=");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(this.includeHealth);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}

			// Token: 0x0400ECAB RID: 60587
			public BuildableDef entDef;

			// Token: 0x0400ECAC RID: 60588
			public ThingDef stuffDef;

			// Token: 0x0400ECAD RID: 60589
			public ThingStyleDef styleDef;

			// Token: 0x0400ECAE RID: 60590
			public int stackCount;

			// Token: 0x0400ECAF RID: 60591
			public QualityCategory quality;

			// Token: 0x0400ECB0 RID: 60592
			public int health;

			// Token: 0x0400ECB1 RID: 60593
			public int maxHealth;

			// Token: 0x0400ECB2 RID: 60594
			public bool wornByCorpse;

			// Token: 0x0400ECB3 RID: 60595
			public bool hasQuality;

			// Token: 0x0400ECB4 RID: 60596
			public bool includeHealth;
		}
	}
}
