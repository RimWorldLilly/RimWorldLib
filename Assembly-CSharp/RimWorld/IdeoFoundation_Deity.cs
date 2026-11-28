using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Grammar;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200161F RID: 5663
	public class IdeoFoundation_Deity : IdeoFoundation
	{
		// Token: 0x1700186B RID: 6251
		// (get) Token: 0x06008D29 RID: 36137 RVA: 0x0033690C File Offset: 0x00334B0C
		public List<IdeoFoundation_Deity.Deity> DeitiesListForReading
		{
			get
			{
				return this.deities;
			}
		}

		// Token: 0x06008D2A RID: 36138 RVA: 0x00336914 File Offset: 0x00334B14
		public override void Init(IdeoGenerationParms parms)
		{
			if (!ModLister.CheckIdeology("Ideoligion"))
			{
				return;
			}
			this.ideo.classicExtraMode = parms.classicExtra;
			this.RandomizeCulture(parms);
			this.RandomizePlace();
			this.RandomizeMemes(parms);
			this.GenerateDeities();
			this.GenerateTextSymbols();
			this.GenerateLeaderTitle();
			this.RandomizeIcon();
			this.RandomizePrecepts(true, parms);
			this.ideo.RegenerateDescription(true);
			this.RandomizeStyles();
		}

		// Token: 0x06008D2B RID: 36139 RVA: 0x00336985 File Offset: 0x00334B85
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IdeoFoundation_Deity.Deity>(ref this.deities, "deities", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06008D2C RID: 36140 RVA: 0x003369A4 File Offset: 0x00334BA4
		public override void DoInfo(ref float curY, float width, IdeoEditMode editMode)
		{
			if (this.deities.Count == 0 && this.ideo.DeityCountRange.max <= 0)
			{
				return;
			}
			curY += 4f;
			Widgets.Label((width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f, ref curY, width, "Deities".Translate(), default(TipSignal));
			if (editMode != IdeoEditMode.None)
			{
				float num = width - (width - IdeoUIUtility.PreceptBoxSize.x * 3f - 16f) / 2f;
				Rect rect = new Rect(num - IdeoUIUtility.ButtonSize.x, curY - IdeoUIUtility.ButtonSize.y, IdeoUIUtility.ButtonSize.x, IdeoUIUtility.ButtonSize.y);
				Rect rect2 = rect;
				rect2.x = rect.xMin - rect.width - 10f;
				bool flag = this.deities.Count < this.ideo.DeityCountRange.max;
				if (Widgets.ButtonText(flag ? rect2 : rect, "RandomizeDeities".Translate(), true, true, true, null) && IdeoUIUtility.TutorAllowsInteraction(editMode))
				{
					this.GenerateDeities();
					this.ideo.RegenerateAllPreceptNames();
					this.ideo.RegenerateDescription(false);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				if (flag && Widgets.ButtonText(rect, "AddDeity".Translate(), true, true, true, null) && IdeoUIUtility.TutorAllowsInteraction(editMode))
				{
					this.deities.Add(this.GenerateNewDeity());
					this.ideo.RegenerateAllPreceptNames();
					this.ideo.RegenerateDescription(false);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
			}
			curY += 4f;
			for (int i = 0; i < this.deities.Count; i++)
			{
				IdeoFoundation_Deity.Deity curDeity = this.deities[i];
				int num2 = i / 3;
				int num3 = i % 3;
				int num4 = (i >= this.deities.Count - this.deities.Count % 3) ? (this.deities.Count % 3) : 3;
				float num5 = (width - (float)num4 * IdeoFoundation_Deity.DeityBoxSize.x - (float)((num4 - 1) * 8)) / 2f;
				Rect rect3 = new Rect(num5 + (float)num3 * IdeoFoundation_Deity.DeityBoxSize.x + (float)(num3 * 8), curY + (float)num2 * IdeoFoundation_Deity.DeityBoxSize.y + (float)(num2 * 8), IdeoFoundation_Deity.DeityBoxSize.x, IdeoFoundation_Deity.DeityBoxSize.y);
				Widgets.DrawLightHighlight(rect3);
				if (Mouse.IsOver(rect3))
				{
					Widgets.DrawHighlight(rect3);
					string text = curDeity.name.Colorize(ColoredText.TipSectionTitleColor) + "\n" + curDeity.type;
					if (curDeity.relatedMeme != null)
					{
						text = string.Concat(new string[]
						{
							text,
							"\n\n",
							"RelatedToMeme".Translate().Colorize(ColoredText.TipSectionTitleColor),
							": ",
							curDeity.relatedMeme.LabelCap.Resolve()
						});
					}
					if (editMode != IdeoEditMode.None)
					{
						text = text + "\n\n" + IdeoUIUtility.ClickToEdit;
					}
					TooltipHandler.TipRegion(rect3, text);
				}
				if (Widgets.ButtonInvisible(rect3, true) && editMode != IdeoEditMode.None && IdeoUIUtility.TutorAllowsInteraction(editMode))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					Action action = null;
					TaggedString taggedString = "Remove".Translate().CapitalizeFirst();
					int min = this.ideo.DeityCountRange.min;
					if (this.deities.Count > min)
					{
						action = delegate()
						{
							this.deities.Remove(curDeity);
							this.ideo.RegenerateDescription(false);
						};
					}
					else
					{
						string arg = (min <= 1) ? "Deity".Translate() : Find.ActiveLanguageWorker.Pluralize("Deity".Translate(), min);
						taggedString += " (" + "DeitiesRequired".Translate(min, arg.Named("DEITYNOUN")) + ")";
					}
					list.Add(new FloatMenuOption(taggedString, action, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					list.Add(new FloatMenuOption("Regenerate".Translate().CapitalizeFirst(), delegate()
					{
						this.FillDeity(curDeity);
						this.ideo.RegenerateDescription(false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					list.Add(new FloatMenuOption("EditDeity".Translate().CapitalizeFirst(), delegate()
					{
						Find.WindowStack.Add(new Dialog_EditDeity(curDeity, this.ideo));
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					Find.WindowStack.Add(new FloatMenu(list));
				}
				Rect position = new Rect(rect3.x + (rect3.height - 50f) / 2f, rect3.y + (rect3.height - 50f) / 2f, 50f, 50f);
				GUI.DrawTexture(position, this.deities[i].Icon);
				Rect rect4 = new Rect(position.xMax + 10f, rect3.y + 3f, rect3.xMax - position.xMax - 10f, rect3.height / 2f);
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(rect4, this.deities[i].name);
				GenUI.ResetLabelAlign();
				Rect rect5 = new Rect(position.xMax + 10f, rect3.y + rect3.height / 2f - 3f, rect3.xMax - position.xMax - 10f, rect3.height / 2f);
				Text.Anchor = TextAnchor.MiddleLeft;
				GUI.color = new Color(0.8f, 0.8f, 0.8f);
				Widgets.Label(rect5, this.deities[i].type);
				GUI.color = Color.white;
				GenUI.ResetLabelAlign();
				GUI.DrawTexture(new Rect(rect3.xMax - 20f - 4f, rect3.y + 4f, 20f, 20f), this.deities[i].gender.GetIcon());
			}
			int num6 = Mathf.CeilToInt((float)this.deities.Count / 3f);
			curY += (float)num6 * IdeoFoundation_Deity.DeityBoxSize.y + (float)((num6 - 1) * 8);
		}

		// Token: 0x06008D2D RID: 36141 RVA: 0x00337088 File Offset: 0x00335288
		public override void GenerateTextSymbols()
		{
			IdeoFoundation_Deity.<>c__DisplayClass11_0 CS$<>8__locals1 = new IdeoFoundation_Deity.<>c__DisplayClass11_0();
			CS$<>8__locals1.<>4__this = this;
			if (this.ideo.culture == null)
			{
				return;
			}
			this.ideo.usedSymbols.Clear();
			this.ideo.usedSymbolPacks.Clear();
			CS$<>8__locals1.usedMemes = new List<MemeDef>();
			CS$<>8__locals1.request = default(GrammarRequest);
			CS$<>8__locals1.request.Includes.Add(this.ideo.culture.ideoNameMaker);
			base.AddPlaceRules(ref CS$<>8__locals1.request);
			this.AddDeityRules(ref CS$<>8__locals1.request);
			List<IdeoFoundation_Deity.SymbolSource> list = new List<IdeoFoundation_Deity.SymbolSource>();
			if (this.ideo.memes.Any((MemeDef m) => !m.symbolPacks.NullOrEmpty<IdeoSymbolPack>()))
			{
				list.Add(IdeoFoundation_Deity.SymbolSource.Pack);
			}
			if (this.deities.Count >= 1)
			{
				if (!this.ideo.memes.Any((MemeDef m) => !m.allowSymbolsFromDeity))
				{
					list.Add(IdeoFoundation_Deity.SymbolSource.Deity);
				}
			}
			if (list.Count == 0)
			{
				Log.Error("No way to generate ideo symbols. Memes: " + (from m in this.ideo.memes
				select m.defName).ToCommaList(false, false));
				this.ideo.name = "Errorism";
				this.ideo.adjective = "Errorist";
				this.ideo.memberName = "Errorist";
				return;
			}
			IdeoFoundation_Deity.SymbolSource symbolSource = list.RandomElementByWeight(delegate(IdeoFoundation_Deity.SymbolSource s)
			{
				if (s == IdeoFoundation_Deity.SymbolSource.Pack)
				{
					return 1f;
				}
				if (s == IdeoFoundation_Deity.SymbolSource.Deity)
				{
					return 0.5f;
				}
				throw new NotImplementedException();
			});
			if (symbolSource != IdeoFoundation_Deity.SymbolSource.Pack)
			{
				if (symbolSource == IdeoFoundation_Deity.SymbolSource.Deity)
				{
					CS$<>8__locals1.<GenerateTextSymbols>g__SetupFromDeity|2();
				}
			}
			else
			{
				CS$<>8__locals1.<GenerateTextSymbols>g__SetupFromSymbolPack|1();
			}
			this.ideo.name = this.GetResolvedText("r_ideoName", CS$<>8__locals1.request, true);
			this.ideo.name = GenText.CapitalizeAsTitle(this.ideo.name);
			this.ideo.adjective = this.GetResolvedText("r_ideoAdjective", CS$<>8__locals1.request, false);
			this.ideo.memberName = this.GetResolvedText("r_memberName", CS$<>8__locals1.request, true);
		}

		// Token: 0x06008D2E RID: 36142 RVA: 0x003372D0 File Offset: 0x003354D0
		public void AddDeityRules(ref GrammarRequest request)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			this.AddDeityRules(dictionary, ref request);
			request.Rules.AddRange(from kv in dictionary
			select new Rule_String(kv.Key, kv.Value));
		}

		// Token: 0x06008D2F RID: 36143 RVA: 0x0033731C File Offset: 0x0033551C
		public void AddDeityRules(Dictionary<string, string> tokens, ref GrammarRequest request)
		{
			for (int i = 0; i < this.deities.Count; i++)
			{
				IdeoFoundation_Deity.Deity deity = this.deities[i];
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 1);
				defaultInterpolatedStringHandler.AppendLiteral("deity");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				defaultInterpolatedStringHandler.AppendLiteral("_");
				string str = defaultInterpolatedStringHandler.ToStringAndClear();
				tokens.AddDistinct(str + "name", deity.name.ApplyTag(TagType.Name, null).Resolve());
				tokens.AddDistinct(str + "pronoun", deity.gender.GetPronoun());
				tokens.AddDistinct(str + "objective", deity.gender.GetObjective());
				tokens.AddDistinct(str + "possessive", deity.gender.GetPossessive());
				tokens.AddDistinct(str + "type", deity.type);
				request.Constants.SetOrAdd(str + "gender", deity.gender.ToString());
			}
		}

		// Token: 0x06008D30 RID: 36144 RVA: 0x0033743C File Offset: 0x0033563C
		public void SetDeities(List<IdeoFoundation_Deity.Deity> deities)
		{
			this.deities = deities;
		}

		// Token: 0x06008D31 RID: 36145 RVA: 0x00337448 File Offset: 0x00335648
		private bool CanUseSymbolPack(IdeoSymbolPack pack)
		{
			if (Find.World == null || Find.IdeoManager == null)
			{
				return true;
			}
			using (List<Ideo>.Enumerator enumerator = Find.IdeoManager.IdeosListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.usedSymbolPacks.Contains(pack.PrimarySymbol))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06008D32 RID: 36146 RVA: 0x003374C0 File Offset: 0x003356C0
		private bool CanUseSymbol(string symbol)
		{
			if (Find.World == null || Find.IdeoManager == null)
			{
				return true;
			}
			using (List<Ideo>.Enumerator enumerator = Find.IdeoManager.IdeosListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.usedSymbols.Contains(symbol))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06008D33 RID: 36147 RVA: 0x00337534 File Offset: 0x00335734
		private string GetResolvedText(string key, GrammarRequest request, bool capitalizeFirstSentence = true)
		{
			string text = GrammarResolver.Resolve(key, request, null, false, null, null, null, capitalizeFirstSentence);
			for (int i = 0; i < 10; i++)
			{
				if (this.CanUseSymbol(text))
				{
					this.ideo.usedSymbols.Add(text);
					return text;
				}
				text = GrammarResolver.Resolve(key, request, null, false, null, null, null, capitalizeFirstSentence);
			}
			this.ideo.usedSymbols.Add(text);
			return text;
		}

		// Token: 0x06008D34 RID: 36148 RVA: 0x00337598 File Offset: 0x00335798
		public void GenerateDeities()
		{
			this.deities.Clear();
			int num;
			if (Rand.Chance(0.5f))
			{
				num = 0;
			}
			else if (Rand.Chance(0.5f))
			{
				num = 1;
			}
			else
			{
				num = Rand.RangeInclusive(2, 4);
			}
			IntRange deityCountRange = this.ideo.DeityCountRange;
			num = Mathf.Clamp(num, deityCountRange.min, deityCountRange.max);
			for (int i = 0; i < num; i++)
			{
				IdeoFoundation_Deity.Deity item = this.GenerateNewDeity();
				this.deities.Add(item);
			}
		}

		// Token: 0x06008D35 RID: 36149 RVA: 0x00337618 File Offset: 0x00335818
		private void FillDeity(IdeoFoundation_Deity.Deity deity)
		{
			Gender supremeGender = this.ideo.SupremeGender;
			if (supremeGender != Gender.None)
			{
				deity.gender = supremeGender;
			}
			else
			{
				deity.gender = Gen.RandomEnumValue<Gender>(true);
			}
			MemeDef relatedMeme;
			MemeDef relatedMeme2;
			if ((from x in this.ideo.memes
			where !this.deities.Any((IdeoFoundation_Deity.Deity y) => y.relatedMeme == x)
			select x).TryRandomElement(out relatedMeme))
			{
				deity.relatedMeme = relatedMeme;
			}
			else if (this.ideo.memes.TryRandomElement(out relatedMeme2))
			{
				deity.relatedMeme = relatedMeme2;
			}
			if (this.ideo.StructureMeme.fixedDeityNameTypes != null)
			{
				int num = 0;
				DeityNameType deityNameType;
				do
				{
					deityNameType = this.ideo.StructureMeme.fixedDeityNameTypes.RandomElement<DeityNameType>();
					if (!this.<FillDeity>g__AllExistingDeities|19_0().Contains(deityNameType.name))
					{
						goto IL_BF;
					}
					num++;
				}
				while (num <= 20);
				Log.Error("Could not get a unique fixed deity name and type after a reasonable number of tries.");
				IL_BF:
				deity.name = deityNameType.name;
				deity.type = deityNameType.type;
			}
			else
			{
				GrammarRequest request = default(GrammarRequest);
				RulePackDef item = this.ideo.StructureMeme.deityNameMakerOverride ?? this.ideo.culture.deityNameMaker;
				request.Includes.Add(item);
				deity.name = NameGenerator.GenerateName(request, (string x) => !this.<FillDeity>g__AllExistingDeities|19_0().Contains(x), false, "r_deityName", null);
				GrammarRequest request2 = default(GrammarRequest);
				RulePackDef item2 = this.ideo.StructureMeme.deityTypeMakerOverride ?? this.ideo.culture.deityTypeMaker;
				request2.Includes.Add(item2);
				if (deity.relatedMeme != null && deity.relatedMeme.generalRules != null)
				{
					request2.IncludesBare.Add(deity.relatedMeme.generalRules);
				}
				request2.Constants.SetOrAdd("gender", deity.gender.ToString());
				deity.type = NameGenerator.GenerateName(request2, null, false, "r_deityType", null);
			}
			deity.iconPath = "UI/Deities/DeityGeneric";
		}

		// Token: 0x06008D36 RID: 36150 RVA: 0x0033780C File Offset: 0x00335A0C
		private IdeoFoundation_Deity.Deity GenerateNewDeity()
		{
			IdeoFoundation_Deity.Deity deity = new IdeoFoundation_Deity.Deity();
			this.FillDeity(deity);
			return deity;
		}

		// Token: 0x06008D37 RID: 36151 RVA: 0x00337828 File Offset: 0x00335A28
		public override void CopyTo(IdeoFoundation other)
		{
			base.CopyTo(other);
			IdeoFoundation_Deity ideoFoundation_Deity = (IdeoFoundation_Deity)other;
			ideoFoundation_Deity.deities.Clear();
			foreach (IdeoFoundation_Deity.Deity deity in this.deities)
			{
				IdeoFoundation_Deity.Deity deity2 = new IdeoFoundation_Deity.Deity();
				deity.CopyTo(deity2);
				ideoFoundation_Deity.deities.Add(deity2);
			}
		}

		// Token: 0x06008D3C RID: 36156 RVA: 0x00337909 File Offset: 0x00335B09
		[CompilerGenerated]
		private IEnumerable<string> <FillDeity>g__AllExistingDeities|19_0()
		{
			IdeoFoundation_Deity.<<FillDeity>g__AllExistingDeities|19_0>d <<FillDeity>g__AllExistingDeities|19_0>d = new IdeoFoundation_Deity.<<FillDeity>g__AllExistingDeities|19_0>d(-2);
			<<FillDeity>g__AllExistingDeities|19_0>d.<>4__this = this;
			return <<FillDeity>g__AllExistingDeities|19_0>d;
		}

		// Token: 0x0400569B RID: 22171
		private List<IdeoFoundation_Deity.Deity> deities = new List<IdeoFoundation_Deity.Deity>();

		// Token: 0x0400569C RID: 22172
		private static readonly Vector2 DeityBoxSize = IdeoUIUtility.PreceptBoxSize;

		// Token: 0x0400569D RID: 22173
		private const float IconSize = 50f;

		// Token: 0x0400569E RID: 22174
		private const float GenderIconSize = 20f;

		// Token: 0x0200342E RID: 13358
		public class Deity : IExposable
		{
			// Token: 0x170031EC RID: 12780
			// (get) Token: 0x06013329 RID: 78633 RVA: 0x00655ADF File Offset: 0x00653CDF
			public Texture2D Icon
			{
				get
				{
					if (this.icon == null)
					{
						this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
					}
					return this.icon;
				}
			}

			// Token: 0x0601332A RID: 78634 RVA: 0x00655B08 File Offset: 0x00653D08
			public void ExposeData()
			{
				Scribe_Values.Look<string>(ref this.name, "name", null, false);
				Scribe_Values.Look<string>(ref this.type, "type", null, false);
				Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
				Scribe_Values.Look<string>(ref this.iconPath, "iconPath", null, false);
				Scribe_Defs.Look<MemeDef>(ref this.relatedMeme, "relatedMeme");
			}

			// Token: 0x0601332B RID: 78635 RVA: 0x00655B6D File Offset: 0x00653D6D
			public virtual void CopyTo(IdeoFoundation_Deity.Deity other)
			{
				other.name = this.name;
				other.type = this.type;
				other.gender = this.gender;
				other.iconPath = this.iconPath;
				other.relatedMeme = this.relatedMeme;
			}

			// Token: 0x0400D13B RID: 53563
			public string name;

			// Token: 0x0400D13C RID: 53564
			public string type;

			// Token: 0x0400D13D RID: 53565
			public Gender gender;

			// Token: 0x0400D13E RID: 53566
			public string iconPath;

			// Token: 0x0400D13F RID: 53567
			public MemeDef relatedMeme;

			// Token: 0x0400D140 RID: 53568
			private Texture2D icon;
		}

		// Token: 0x0200342F RID: 13359
		private enum SymbolSource
		{
			// Token: 0x0400D142 RID: 53570
			Pack,
			// Token: 0x0400D143 RID: 53571
			Deity
		}
	}
}
