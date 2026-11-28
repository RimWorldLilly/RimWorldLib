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
	// Token: 0x02001645 RID: 5701
	public class Precept : IExposable, ILoadReferenceable
	{
		// Token: 0x170018A5 RID: 6309
		// (get) Token: 0x06008ED1 RID: 36561 RVA: 0x00347CA8 File Offset: 0x00345EA8
		public virtual string TipLabel
		{
			get
			{
				return this.def.tipLabelOverride ?? (this.def.issue.LabelCap + ": " + this.def.LabelCap);
			}
		}

		// Token: 0x170018A6 RID: 6310
		// (get) Token: 0x06008ED2 RID: 36562 RVA: 0x00347CE8 File Offset: 0x00345EE8
		public virtual string UIInfoFirstLine
		{
			get
			{
				return this.def.issue.LabelCap;
			}
		}

		// Token: 0x170018A7 RID: 6311
		// (get) Token: 0x06008ED3 RID: 36563 RVA: 0x00347CFF File Offset: 0x00345EFF
		public virtual string UIInfoSecondLine
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x170018A8 RID: 6312
		// (get) Token: 0x06008ED4 RID: 36564 RVA: 0x00347D11 File Offset: 0x00345F11
		public virtual bool CanRegenerate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170018A9 RID: 6313
		// (get) Token: 0x06008ED5 RID: 36565 RVA: 0x00347D14 File Offset: 0x00345F14
		public virtual bool SortByImpact
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170018AA RID: 6314
		// (get) Token: 0x06008ED6 RID: 36566 RVA: 0x00347D17 File Offset: 0x00345F17
		public virtual Color LabelColor
		{
			get
			{
				return new Color(0.8f, 0.8f, 0.8f);
			}
		}

		// Token: 0x170018AB RID: 6315
		// (get) Token: 0x06008ED7 RID: 36567 RVA: 0x00347D2D File Offset: 0x00345F2D
		public virtual string Label
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170018AC RID: 6316
		// (get) Token: 0x06008ED8 RID: 36568 RVA: 0x00347D38 File Offset: 0x00345F38
		public string LabelCap
		{
			get
			{
				string result;
				if ((result = this.labelCapCache) == null)
				{
					result = (this.labelCapCache = this.name.CapitalizeFirst());
				}
				return result;
			}
		}

		// Token: 0x170018AD RID: 6317
		// (get) Token: 0x06008ED9 RID: 36569 RVA: 0x00347D63 File Offset: 0x00345F63
		public string Description
		{
			get
			{
				if (!this.descOverride.NullOrEmpty())
				{
					return this.descOverride;
				}
				return this.def.description;
			}
		}

		// Token: 0x170018AE RID: 6318
		// (get) Token: 0x06008EDA RID: 36570 RVA: 0x00347D84 File Offset: 0x00345F84
		public virtual string DescriptionForTip
		{
			get
			{
				return this.Description;
			}
		}

		// Token: 0x170018AF RID: 6319
		// (get) Token: 0x06008EDB RID: 36571 RVA: 0x00347D8C File Offset: 0x00345F8C
		public virtual bool UsesGeneratedName
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170018B0 RID: 6320
		// (get) Token: 0x06008EDC RID: 36572 RVA: 0x00347D8F File Offset: 0x00345F8F
		public virtual Texture2D Icon
		{
			get
			{
				return this.def.Icon ?? this.ideo.Icon;
			}
		}

		// Token: 0x170018B1 RID: 6321
		// (get) Token: 0x06008EDD RID: 36573 RVA: 0x00347DAB File Offset: 0x00345FAB
		// (set) Token: 0x06008EDE RID: 36574 RVA: 0x00347DAE File Offset: 0x00345FAE
		public virtual List<PreceptApparelRequirement> ApparelRequirements
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x170018B2 RID: 6322
		// (get) Token: 0x06008EDF RID: 36575 RVA: 0x00347DB0 File Offset: 0x00345FB0
		public int Id
		{
			get
			{
				return this.ID;
			}
		}

		// Token: 0x06008EE0 RID: 36576 RVA: 0x00347DB8 File Offset: 0x00345FB8
		public virtual void PostMake()
		{
		}

		// Token: 0x06008EE1 RID: 36577 RVA: 0x00347DBA File Offset: 0x00345FBA
		public virtual void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			this.ideo = ideo;
			this.ID = Find.UniqueIDsManager.GetNextPreceptID();
			this.randomSeed = Rand.Int;
			this.name = this.def.issue.label;
		}

		// Token: 0x06008EE2 RID: 36578 RVA: 0x00347DF4 File Offset: 0x00345FF4
		public virtual void Regenerate(Ideo ideo, FactionDef generatingFor = null)
		{
			this.Init(ideo, generatingFor);
			this.ClearTipCache();
			ideo.anyPreceptEdited = true;
		}

		// Token: 0x06008EE3 RID: 36579 RVA: 0x00347E0B File Offset: 0x0034600B
		public virtual void ClearTipCache()
		{
			this.tipCached = null;
		}

		// Token: 0x06008EE4 RID: 36580 RVA: 0x00347E14 File Offset: 0x00346014
		protected string ColorizeDescTitle(TaggedString title)
		{
			return title.Resolve().Colorize(ColoredText.TipSectionTitleColor);
		}

		// Token: 0x06008EE5 RID: 36581 RVA: 0x00347E27 File Offset: 0x00346027
		protected string ColorizeWarning(TaggedString title)
		{
			return title.Resolve().Colorize(ColoredText.ThreatColor);
		}

		// Token: 0x06008EE6 RID: 36582 RVA: 0x00347E3C File Offset: 0x0034603C
		public virtual string GetTip()
		{
			Precept.<>c__DisplayClass52_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			Precept.tmpCompsDesc.Clear();
			if (!this.DescriptionForTip.NullOrEmpty())
			{
				Precept.tmpCompsDesc.Append(this.DescriptionForTip);
			}
			bool flag = true;
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				PreceptComp_UnwillingToDo preceptComp_UnwillingToDo = this.def.comps[i] as PreceptComp_UnwillingToDo;
				if (preceptComp_UnwillingToDo != null)
				{
					string prohibitionText = preceptComp_UnwillingToDo.GetProhibitionText();
					if (!prohibitionText.NullOrEmpty())
					{
						if (flag)
						{
							flag = false;
							Precept.tmpCompsDesc.AppendLine();
							Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("Prohibitions".Translate() + ":"));
						}
						Precept.tmpCompsDesc.AppendInNewLine("  - " + prohibitionText);
					}
				}
			}
			Precept.tmpDescs.Clear();
			bool flag2 = true;
			int j = 0;
			while (j < this.def.comps.Count)
			{
				PreceptComp_SelfTookMemoryThought preceptComp_SelfTookMemoryThought = this.def.comps[j] as PreceptComp_SelfTookMemoryThought;
				PreceptComp preceptComp;
				if (preceptComp_SelfTookMemoryThought != null)
				{
					preceptComp = preceptComp_SelfTookMemoryThought;
					goto IL_195;
				}
				PreceptComp_SituationalThought preceptComp_SituationalThought = this.def.comps[j] as PreceptComp_SituationalThought;
				if (preceptComp_SituationalThought != null && preceptComp_SituationalThought.AffectsMood)
				{
					preceptComp = preceptComp_SituationalThought;
					goto IL_195;
				}
				PreceptComp_KnowsMemoryThought preceptComp_KnowsMemoryThought = this.def.comps[j] as PreceptComp_KnowsMemoryThought;
				if (preceptComp_KnowsMemoryThought != null && preceptComp_KnowsMemoryThought.AffectsMood)
				{
					preceptComp = preceptComp_KnowsMemoryThought;
					goto IL_195;
				}
				PreceptComp_BedThought preceptComp_BedThought = this.def.comps[j] as PreceptComp_BedThought;
				if (preceptComp_BedThought != null && preceptComp_BedThought.AffectsMood)
				{
					preceptComp = preceptComp_BedThought;
					goto IL_195;
				}
				IL_21F:
				j++;
				continue;
				IL_195:
				if (flag2)
				{
					flag2 = false;
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("Mood".Translate() + ":"));
				}
				foreach (string text in preceptComp.GetDescriptions())
				{
					if (Precept.tmpDescs.Add(text))
					{
						Precept.tmpCompsDesc.AppendInNewLine("  - " + text);
					}
				}
				goto IL_21F;
			}
			Precept.tmpDescs.Clear();
			bool flag3 = true;
			int k = 0;
			while (k < this.def.comps.Count)
			{
				PreceptComp_SituationalThought preceptComp_SituationalThought2 = this.def.comps[k] as PreceptComp_SituationalThought;
				PreceptComp preceptComp2;
				if (preceptComp_SituationalThought2 != null && !preceptComp_SituationalThought2.AffectsMood)
				{
					preceptComp2 = preceptComp_SituationalThought2;
					goto IL_2AC;
				}
				PreceptComp_KnowsMemoryThought preceptComp_KnowsMemoryThought2 = this.def.comps[k] as PreceptComp_KnowsMemoryThought;
				if (preceptComp_KnowsMemoryThought2 != null && !preceptComp_KnowsMemoryThought2.AffectsMood)
				{
					preceptComp2 = preceptComp_KnowsMemoryThought2;
					goto IL_2AC;
				}
				IL_336:
				k++;
				continue;
				IL_2AC:
				if (flag3)
				{
					flag3 = false;
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("Opinions".Translate() + ":"));
				}
				foreach (string text2 in preceptComp2.GetDescriptions())
				{
					if (Precept.tmpDescs.Add(text2))
					{
						Precept.tmpCompsDesc.AppendInNewLine("  - " + text2);
					}
				}
				goto IL_336;
			}
			Precept.tmpDescs.Clear();
			bool flag4 = true;
			for (int l = 0; l < this.def.comps.Count; l++)
			{
				PreceptComp_MentalBreak preceptComp_MentalBreak = this.def.comps[l] as PreceptComp_MentalBreak;
				if (preceptComp_MentalBreak != null)
				{
					if (flag4)
					{
						flag4 = false;
						Precept.tmpCompsDesc.AppendLine();
						Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("MentalBreaks".Translate() + ":"));
					}
					foreach (string text3 in preceptComp_MentalBreak.GetDescriptions())
					{
						if (Precept.tmpDescs.Add(text3))
						{
							Precept.tmpCompsDesc.AppendInNewLine("  - " + text3);
						}
					}
				}
			}
			Precept.tmpDescs.Clear();
			bool flag5 = true;
			if (!this.def.enablesNeeds.NullOrEmpty<NeedDef>())
			{
				for (int m = 0; m < this.def.enablesNeeds.Count; m++)
				{
					if (flag5)
					{
						flag5 = false;
						Precept.tmpCompsDesc.AppendLine();
						Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("AddsNeeds".Translate() + ":"));
					}
					for (int n = 0; n < this.def.enablesNeeds.Count; n++)
					{
						NeedDef needDef = this.def.enablesNeeds[n];
						if (Precept.tmpDescs.Add(needDef.label))
						{
							Precept.tmpCompsDesc.AppendInNewLine("  - " + needDef.LabelCap);
						}
					}
				}
			}
			Precept.tmpDescs.Clear();
			flag5 = true;
			if (!this.def.disablesNeeds.NullOrEmpty<NeedDef>())
			{
				for (int num = 0; num < this.def.disablesNeeds.Count; num++)
				{
					if (flag5)
					{
						flag5 = false;
						Precept.tmpCompsDesc.AppendLine();
						Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("DisablesNeeds".Translate() + ":"));
					}
					for (int num2 = 0; num2 < this.def.disablesNeeds.Count; num2++)
					{
						NeedDef needDef2 = this.def.disablesNeeds[num2];
						if (Precept.tmpDescs.Add(needDef2.label))
						{
							Precept.tmpCompsDesc.AppendInNewLine("  - " + needDef2.LabelCap);
						}
					}
				}
			}
			CS$<>8__locals1.firstStat = true;
			if (this.def.statOffsets != null)
			{
				for (int num3 = 0; num3 < this.def.statOffsets.Count; num3++)
				{
					this.<GetTip>g__TryDrawStatLine|52_0(ref CS$<>8__locals1);
					Precept.tmpCompsDesc.AppendInNewLine("  - " + this.def.statOffsets[num3].stat.LabelCap + ": " + this.def.statOffsets[num3].ValueToStringAsOffset);
				}
			}
			if (this.def.conditionalStatAffecters != null)
			{
				for (int num4 = 0; num4 < this.def.conditionalStatAffecters.Count; num4++)
				{
					ConditionalStatAffecter conditionalStatAffecter = this.def.conditionalStatAffecters[num4];
					if (conditionalStatAffecter.statOffsets != null)
					{
						for (int num5 = 0; num5 < conditionalStatAffecter.statOffsets.Count; num5++)
						{
							this.<GetTip>g__TryDrawStatLine|52_0(ref CS$<>8__locals1);
							Precept.tmpCompsDesc.AppendInNewLine("  - " + conditionalStatAffecter.statOffsets[num5].stat.LabelCap + ": " + conditionalStatAffecter.statOffsets[num5].ValueToStringAsOffset + " (" + conditionalStatAffecter.Label + ")");
						}
					}
				}
			}
			if (this.def.statFactors != null)
			{
				for (int num6 = 0; num6 < this.def.statFactors.Count; num6++)
				{
					this.<GetTip>g__TryDrawStatLine|52_0(ref CS$<>8__locals1);
					Precept.tmpCompsDesc.AppendInNewLine("  - " + this.def.statFactors[num6].stat.LabelCap + ": " + this.def.statFactors[num6].ToStringAsFactor);
				}
			}
			if (!Mathf.Approximately(this.def.growthVatSpeedFactor, 1f))
			{
				this.<GetTip>g__TryDrawStatLine|52_0(ref CS$<>8__locals1);
				Precept.tmpCompsDesc.AppendInNewLine("  - " + StatDefOf.GrowthVatOccupantSpeed.LabelCap + ": x" + this.def.growthVatSpeedFactor.ToStringPercent());
			}
			if (!Mathf.Approximately(this.def.biosculpterPodCycleSpeedFactor, 1f))
			{
				this.<GetTip>g__TryDrawStatLine|52_0(ref CS$<>8__locals1);
				Precept.tmpCompsDesc.AppendInNewLine("  - " + StatDefOf.BiosculpterPodSpeedFactor.LabelCap + ": x" + this.def.biosculpterPodCycleSpeedFactor.ToStringPercent());
			}
			if (this.def.conditionalStatAffecters != null)
			{
				for (int num7 = 0; num7 < this.def.conditionalStatAffecters.Count; num7++)
				{
					ConditionalStatAffecter conditionalStatAffecter2 = this.def.conditionalStatAffecters[num7];
					if (conditionalStatAffecter2.statFactors != null)
					{
						for (int num8 = 0; num8 < conditionalStatAffecter2.statFactors.Count; num8++)
						{
							this.<GetTip>g__TryDrawStatLine|52_0(ref CS$<>8__locals1);
							Precept.tmpCompsDesc.AppendInNewLine("  - " + conditionalStatAffecter2.statFactors[num8].stat.LabelCap + ": " + conditionalStatAffecter2.statFactors[num8].ToStringAsFactor + " (" + conditionalStatAffecter2.Label + ")");
						}
					}
				}
			}
			if (this.def.abilityStatFactors != null)
			{
				for (int num9 = 0; num9 < this.def.abilityStatFactors.Count; num9++)
				{
					foreach (StatModifier statModifier in this.def.abilityStatFactors[num9].modifiers)
					{
						this.<GetTip>g__TryDrawStatLine|52_0(ref CS$<>8__locals1);
						Precept.tmpCompsDesc.AppendInNewLine("  - " + this.def.abilityStatFactors[num9].ability.LabelCap + ": " + statModifier.stat.LabelCap + ": " + statModifier.ToStringAsFactor);
					}
				}
			}
			if (this.def.TraitsAffecting.Count != 0)
			{
				Precept.tmpCompsDesc.AppendLine();
				Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("AffectedByTraits".Translate() + ":"));
				foreach (TraitDegreeData traitDegreeData in from x in this.def.TraitsAffecting
				select x.def.DataAtDegree(x.degree.GetValueOrDefault()))
				{
					Precept.tmpCompsDesc.AppendInNewLine("  - " + traitDegreeData.GetLabelFor(Gender.Male).CapitalizeFirst());
				}
			}
			bool flag6 = true;
			if (this.ideo.Fluid)
			{
				for (int num10 = 0; num10 < this.def.comps.Count; num10++)
				{
					if (this.def.comps[num10] is PreceptComp_DevelopmentPoints)
					{
						if (flag6)
						{
							flag6 = false;
							Precept.tmpCompsDesc.AppendLine();
							Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("DevelopmentPoints".Translate() + ":"));
						}
						foreach (string str in this.def.comps[num10].GetDescriptions())
						{
							Precept.tmpCompsDesc.AppendInNewLine("  - " + str);
						}
					}
				}
			}
			return Precept.tmpCompsDesc.ToString();
		}

		// Token: 0x06008EE7 RID: 36583 RVA: 0x00348AFC File Offset: 0x00346CFC
		public virtual IEnumerable<Alert> GetAlerts()
		{
			return Enumerable.Empty<Alert>();
		}

		// Token: 0x06008EE8 RID: 36584 RVA: 0x00348B03 File Offset: 0x00346D03
		public virtual void Tick()
		{
		}

		// Token: 0x06008EE9 RID: 36585 RVA: 0x00348B05 File Offset: 0x00346D05
		public virtual string GenerateNameRaw()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06008EEA RID: 36586 RVA: 0x00348B0C File Offset: 0x00346D0C
		public virtual List<PreceptApparelRequirement> GenerateNewApparelRequirements(FactionDef generatingFor = null)
		{
			return null;
		}

		// Token: 0x06008EEB RID: 36587 RVA: 0x00348B10 File Offset: 0x00346D10
		public string GenerateNewName()
		{
			Precept.tmpUsedWords.Clear();
			foreach (Precept precept in this.ideo.PreceptsListForReading)
			{
				if (precept != this && precept.GetType() == base.GetType() && precept.name != null)
				{
					Precept.tmpUsedWords.Add(precept.name);
					foreach (string item in precept.name.Split(' ', StringSplitOptions.None))
					{
						Precept.tmpUsedWords.Add(item);
					}
				}
			}
			float num = 0f;
			if (this.def.takeNameFrom != null)
			{
				return this.ideo.PreceptsListForReading.First((Precept p) => p.def == this.def.takeNameFrom).name;
			}
			this.labelCapCache = null;
			string result;
			if (this.def.ignoreNameUniqueness || this.ideo.classicMode)
			{
				result = this.GenerateNameRaw();
			}
			else
			{
				int num2 = 0;
				string text;
				for (;;)
				{
					text = this.GenerateNameRaw();
					float num3 = Precept.<GenerateNewName>g__NameUniqueness|58_0(text);
					if (num3 >= 1f)
					{
						break;
					}
					if (num3 > num)
					{
						num = num3;
					}
					if (num2++ > 50)
					{
						goto Block_6;
					}
				}
				return text;
				Block_6:
				result = text;
			}
			return result;
		}

		// Token: 0x06008EEC RID: 36588 RVA: 0x00348C74 File Offset: 0x00346E74
		public void RegenerateName()
		{
			this.SetName(this.GenerateNewName());
		}

		// Token: 0x06008EED RID: 36589 RVA: 0x00348C84 File Offset: 0x00346E84
		public void SetName(string newName)
		{
			if (newName == this.name)
			{
				return;
			}
			this.labelCapCache = null;
			this.name = newName;
			if (this.ideo != null)
			{
				foreach (Precept precept in this.ideo.PreceptsListForReading)
				{
					if (precept.def.takeNameFrom == this.def)
					{
						precept.RegenerateName();
					}
				}
			}
		}

		// Token: 0x06008EEE RID: 36590 RVA: 0x00348D14 File Offset: 0x00346F14
		public virtual IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			return null;
		}

		// Token: 0x06008EEF RID: 36591 RVA: 0x00348D17 File Offset: 0x00346F17
		public virtual List<Thought_Situational> SituationThoughtsToAdd(Pawn pawn, List<Thought_Situational> activeThoughts)
		{
			this.tmpSituationalThoughts.Clear();
			return this.tmpSituationalThoughts;
		}

		// Token: 0x06008EF0 RID: 36592 RVA: 0x00348D2C File Offset: 0x00346F2C
		protected void AddIdeoRulesTo(ref GrammarRequest request)
		{
			string ideoName = null;
			IdeoSymbolPack ideoSymbolPack;
			if (this.TryGuessChosenSymbolPack(out ideoSymbolPack))
			{
				if (!ideoSymbolPack.ideoName.NullOrEmpty())
				{
					request.Rules.Add(new Rule_String("chosenIdeoName", ideoSymbolPack.ideoName));
					ideoName = ideoSymbolPack.ideoName;
				}
				if (!ideoSymbolPack.theme.NullOrEmpty())
				{
					request.Rules.Add(new Rule_String("chosenTheme", this.MatchIdeoNameCapitalization(ideoSymbolPack.theme, ideoName)));
				}
			}
			foreach (MemeDef memeDef in this.ideo.memes)
			{
				if (memeDef.generalRules != null)
				{
					request.IncludesBare.Add(memeDef.generalRules);
				}
				if (memeDef.symbolPacks != null)
				{
					foreach (IdeoSymbolPack ideoSymbolPack2 in memeDef.symbolPacks)
					{
						if (!ideoSymbolPack2.theme.NullOrEmpty())
						{
							request.Rules.Add(new Rule_String("memePackTheme", this.MatchIdeoNameCapitalization(ideoSymbolPack2.theme, ideoName)));
						}
						if (!ideoSymbolPack2.adjective.NullOrEmpty())
						{
							request.Rules.Add(new Rule_String("memePackAdjective", this.MatchIdeoNameCapitalization(ideoSymbolPack2.adjective, ideoName)));
						}
					}
				}
			}
			if (!this.ideo.adjective.NullOrEmpty())
			{
				request.Rules.Add(new Rule_String("chosenAdjective", this.ideo.adjective));
			}
			if (this.ideo.KeyDeityName != null)
			{
				request.Rules.Add(new Rule_String("keyDeity", this.ideo.KeyDeityName));
			}
			if (!this.ideo.leaderTitleMale.NullOrEmpty())
			{
				request.Rules.Add(new Rule_String("leaderTitle", this.ideo.leaderTitleMale));
			}
		}

		// Token: 0x06008EF1 RID: 36593 RVA: 0x00348F44 File Offset: 0x00347144
		private bool TryGuessChosenSymbolPack(out IdeoSymbolPack result)
		{
			foreach (MemeDef memeDef in this.ideo.memes)
			{
				if (memeDef.symbolPacks != null)
				{
					foreach (IdeoSymbolPack ideoSymbolPack in memeDef.symbolPacks)
					{
						if (ideoSymbolPack.adjective == this.ideo.adjective || ideoSymbolPack.member == this.ideo.memberName || (ideoSymbolPack.ideoName != null && this.ideo.name.Contains(ideoSymbolPack.ideoName)))
						{
							result = ideoSymbolPack;
							return true;
						}
					}
				}
			}
			result = new IdeoSymbolPack();
			return false;
		}

		// Token: 0x06008EF2 RID: 36594 RVA: 0x00349044 File Offset: 0x00347244
		private string MatchIdeoNameCapitalization(string value, string ideoName)
		{
			if (value.Equals(ideoName, StringComparison.InvariantCultureIgnoreCase))
			{
				return ideoName;
			}
			return value;
		}

		// Token: 0x06008EF3 RID: 36595 RVA: 0x00349054 File Offset: 0x00347254
		public virtual void DrawPreceptBox(Rect preceptBox, IdeoEditMode editMode, bool forceHighlight = false)
		{
			GUI.color = Color.white;
			Color backgroundColor = IdeoUIUtility.GetBackgroundColor(this.def.impact);
			Widgets.DrawRectFast(preceptBox, backgroundColor, null);
			GUI.color = new Color(backgroundColor.r + 0.05f, backgroundColor.g + 0.05f, backgroundColor.b + 0.05f);
			Widgets.DrawBox(preceptBox, 1, null);
			GUI.color = Color.white;
			if (Mouse.IsOver(preceptBox) || forceHighlight)
			{
				Widgets.DrawHighlight(preceptBox);
			}
			if (editMode != IdeoEditMode.None && Widgets.ButtonInvisible(preceptBox, true) && IdeoUIUtility.TutorAllowsInteraction(editMode))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				MemeDef memeThatRequiresPrecept = this.ideo.GetMemeThatRequiresPrecept(this.def);
				bool flag = false;
				IEnumerable<FloatMenuOption> enumerable = this.EditFloatMenuOptions();
				if (enumerable != null)
				{
					foreach (FloatMenuOption floatMenuOption in enumerable)
					{
						flag = true;
						floatMenuOption.orderInPriority = 2000;
						list.Add(floatMenuOption);
					}
				}
				FloatMenuOption floatMenuOption2 = null;
				if (this.def.canRemoveInUI && !this.def.issue.HasDefaultPrecept)
				{
					floatMenuOption2 = new FloatMenuOption("Remove".Translate().CapitalizeFirst(), delegate()
					{
						this.ideo.RemovePrecept(this, false);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 1900);
					MemeDef memeThatRequiresPrecept2 = this.ideo.GetMemeThatRequiresPrecept(this.def);
					if (editMode != IdeoEditMode.Dev && memeThatRequiresPrecept2 != null)
					{
						floatMenuOption2.action = null;
						floatMenuOption2.Label = "CannotRemove".Translate() + ": " + "RequiredByMeme".Translate(memeThatRequiresPrecept2.label);
					}
					else
					{
						flag = true;
					}
				}
				if (this.CanRegenerate)
				{
					FactionDef faction = (Find.World == null) ? Find.Scenario.playerFaction.factionDef : null;
					list.Add(new FloatMenuOption("Regenerate".Translate().CapitalizeFirst(), delegate()
					{
						this.Regenerate(this.ideo, faction);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 1800));
				}
				else if (base.GetType() == typeof(Precept))
				{
					using (IEnumerator<PreceptDef> enumerator2 = (from x in DefDatabase<PreceptDef>.AllDefs
					where x != this.def && x.issue == this.def.issue
					select x).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							PreceptDef p = enumerator2.Current;
							AcceptanceReport report = IdeoUIUtility.CanListPrecept(this.ideo, p, editMode);
							if (report || !string.IsNullOrWhiteSpace(report.Reason))
							{
								TaggedString taggedString = p.LabelCap;
								Action action = delegate()
								{
									this.ideo.AddPrecept(PreceptMaker.MakePrecept(p), true, null, null);
									this.ideo.RemovePrecept(this, true);
								};
								if (!report)
								{
									action = null;
									taggedString += " (" + report.Reason + ")";
								}
								FloatMenuOption floatMenuOption3 = new FloatMenuOption(taggedString, action, p.issue.Icon, IdeoUIUtility.GetIconAndLabelColor(p.impact), MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false);
								floatMenuOption3.orderInPriority = p.displayOrderInIssue;
								flag = true;
								list.Add(floatMenuOption3);
							}
						}
					}
				}
				if (flag)
				{
					if (floatMenuOption2 != null)
					{
						list.Add(floatMenuOption2);
					}
				}
				else if (memeThatRequiresPrecept == null)
				{
					list.Add(new FloatMenuOption("CannotEdit".Translate() + ": " + "Required".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				else
				{
					list.Add(new FloatMenuOption("CannotEdit".Translate() + ": " + "RequiredByMeme".Translate(memeThatRequiresPrecept.LabelCap), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.EditingPrecepts, KnowledgeAmount.Total);
				Find.WindowStack.Add(new FloatMenu(list));
			}
			GUI.color = IdeoUIUtility.GetIconAndLabelColor(this.def.impact);
			Rect rect = new Rect(preceptBox.x + (preceptBox.height - 50f) / 2f, preceptBox.y + (preceptBox.height - 50f) / 2f, 50f, 50f);
			this.DrawIcon(rect);
			Rect rect2 = new Rect(rect.xMax + 10f, preceptBox.y + 3f, preceptBox.xMax - rect.xMax - 10f, preceptBox.height / 2f);
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = this.LabelColor;
			Widgets.Label(rect2, this.UIInfoFirstLine);
			GUI.color = Color.white;
			GenUI.ResetLabelAlign();
			GUI.color = IdeoUIUtility.GetIconAndLabelColor(this.def.impact);
			Rect rect3 = new Rect(rect.xMax + 10f, preceptBox.y + preceptBox.height / 2f - 3f, preceptBox.xMax - rect.xMax - 10f, preceptBox.height / 2f);
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.LabelFit(rect3, this.UIInfoSecondLine);
			GenUI.ResetLabelAlign();
			bool flag2;
			this.PostDrawBox(preceptBox, out flag2);
			if (!flag2 && Mouse.IsOver(preceptBox) && Find.WindowStack.WindowOfType<FloatMenu>() == null)
			{
				TooltipHandler.TipRegion(preceptBox, this.TipLabel.Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + this.GetTip() + ((editMode != IdeoEditMode.None) ? ("\n\n" + IdeoUIUtility.ClickToEdit) : string.Empty));
			}
		}

		// Token: 0x06008EF4 RID: 36596 RVA: 0x0034968C File Offset: 0x0034788C
		public virtual void DrawIcon(Rect rect)
		{
			GUI.DrawTexture(rect, this.def.issue.Icon);
		}

		// Token: 0x06008EF5 RID: 36597 RVA: 0x003496A4 File Offset: 0x003478A4
		public virtual bool CompatibleWith(Precept other)
		{
			return true;
		}

		// Token: 0x06008EF6 RID: 36598 RVA: 0x003496A8 File Offset: 0x003478A8
		public virtual bool GetPlayerWarning(out string shortText, out string description)
		{
			string text;
			description = (text = null);
			shortText = text;
			return false;
		}

		// Token: 0x06008EF7 RID: 36599 RVA: 0x003496BE File Offset: 0x003478BE
		protected virtual void PostDrawBox(Rect rect, out bool anyIconTooltipActive)
		{
			anyIconTooltipActive = false;
		}

		// Token: 0x06008EF8 RID: 36600 RVA: 0x003496C4 File Offset: 0x003478C4
		public FloatMenuOption EditFloatMenuOption()
		{
			return new FloatMenuOption("Edit".Translate() + "...", delegate()
			{
				Find.WindowStack.Add(new Dialog_EditPrecept(this));
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
		}

		// Token: 0x06008EF9 RID: 36601 RVA: 0x00349707 File Offset: 0x00347907
		public virtual void Notify_MemberChangedFaction(Pawn p, Faction oldFaction, Faction newFaction)
		{
		}

		// Token: 0x06008EFA RID: 36602 RVA: 0x00349709 File Offset: 0x00347909
		public virtual void Notify_IdeoNotPrimaryAnymore(Ideo newIdeo)
		{
		}

		// Token: 0x06008EFB RID: 36603 RVA: 0x0034970B File Offset: 0x0034790B
		public virtual void Notify_MemberLost(Pawn pawn)
		{
		}

		// Token: 0x06008EFC RID: 36604 RVA: 0x0034970D File Offset: 0x0034790D
		public virtual void Notify_MemberGained(Pawn pawn)
		{
		}

		// Token: 0x06008EFD RID: 36605 RVA: 0x0034970F File Offset: 0x0034790F
		public virtual void Notify_MemberGuestStatusChanged(Pawn pawn)
		{
		}

		// Token: 0x06008EFE RID: 36606 RVA: 0x00349711 File Offset: 0x00347911
		public virtual void Notify_GameStarted()
		{
		}

		// Token: 0x06008EFF RID: 36607 RVA: 0x00349713 File Offset: 0x00347913
		public virtual void Notify_IdeoReformed()
		{
		}

		// Token: 0x06008F00 RID: 36608 RVA: 0x00349718 File Offset: 0x00347918
		public virtual void Notify_MemberGenerated(Pawn pawn, bool newborn, bool ignoreApparel = false)
		{
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				this.def.comps[i].Notify_MemberGenerated(pawn, this, newborn, ignoreApparel);
			}
		}

		// Token: 0x06008F01 RID: 36609 RVA: 0x0034975C File Offset: 0x0034795C
		public virtual void Notify_HistoryEvent(HistoryEvent ev)
		{
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				this.def.comps[i].Notify_HistoryEvent(ev, this);
			}
		}

		// Token: 0x06008F02 RID: 36610 RVA: 0x0034979C File Offset: 0x0034799C
		public virtual void Notify_RecachedPrecepts()
		{
		}

		// Token: 0x06008F03 RID: 36611 RVA: 0x0034979E File Offset: 0x0034799E
		public virtual bool TryGetLostByReformingWarning(out string warning)
		{
			warning = null;
			return false;
		}

		// Token: 0x06008F04 RID: 36612 RVA: 0x003497A4 File Offset: 0x003479A4
		public virtual void Notify_RemovedByReforming()
		{
		}

		// Token: 0x06008F05 RID: 36613 RVA: 0x003497A8 File Offset: 0x003479A8
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Defs.Look<PreceptDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Values.Look<int>(ref this.randomSeed, "randomSeed", 0, false);
			Scribe_Values.Look<bool>(ref this.usesDefiniteArticle, "usesDefiniteArticle", false, false);
			Scribe_Values.Look<string>(ref this.descOverride, "descOverride", null, false);
			Scribe_Values.Look<bool>(ref this.nameLocked, "nameLocked", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && (this.ID == -1 || GameDataSaveLoader.IsSavingOrLoadingExternalIdeo))
			{
				this.ID = Find.UniqueIDsManager.GetNextPreceptID();
			}
		}

		// Token: 0x06008F06 RID: 36614 RVA: 0x00349859 File Offset: 0x00347A59
		public virtual void Notify_MemberDied(Pawn p)
		{
		}

		// Token: 0x06008F07 RID: 36615 RVA: 0x0034985B File Offset: 0x00347A5B
		public virtual void Notify_MemberCorpseDestroyed(Pawn p)
		{
		}

		// Token: 0x06008F08 RID: 36616 RVA: 0x0034985D File Offset: 0x00347A5D
		public string GetUniqueLoadID()
		{
			return "Precept_" + this.ID.ToString();
		}

		// Token: 0x06008F09 RID: 36617 RVA: 0x00349874 File Offset: 0x00347A74
		public virtual void CopyTo(Precept other)
		{
			other.ideo = this.ideo;
			other.def = this.def;
			other.name = this.name;
			other.ID = this.ID;
			other.usesDefiniteArticle = this.usesDefiniteArticle;
			other.descOverride = this.descOverride;
			other.randomSeed = this.randomSeed;
			other.nameLocked = this.nameLocked;
		}

		// Token: 0x06008F0C RID: 36620 RVA: 0x00349924 File Offset: 0x00347B24
		[CompilerGenerated]
		private void <GetTip>g__TryDrawStatLine|52_0(ref Precept.<>c__DisplayClass52_0 A_1)
		{
			if (A_1.firstStat)
			{
				Precept.tmpCompsDesc.AppendLine();
				Precept.tmpCompsDesc.AppendInNewLine(this.ColorizeDescTitle("PreceptStats".Translate() + ":"));
				A_1.firstStat = false;
			}
		}

		// Token: 0x06008F0D RID: 36621 RVA: 0x00349970 File Offset: 0x00347B70
		[CompilerGenerated]
		internal static float <GenerateNewName>g__NameUniqueness|58_0(string newName)
		{
			string[] array = newName.Split(' ', StringSplitOptions.None);
			int num = 0;
			int num2 = 0;
			foreach (string text in array)
			{
				if (text.Length >= 4)
				{
					num2++;
					if (Precept.tmpUsedWords.Contains(text))
					{
						num++;
					}
				}
			}
			if (num2 != 0)
			{
				return 1f - (float)num / (float)num2;
			}
			return 1f;
		}

		// Token: 0x04005779 RID: 22393
		public Ideo ideo;

		// Token: 0x0400577A RID: 22394
		public PreceptDef def;

		// Token: 0x0400577B RID: 22395
		protected string name;

		// Token: 0x0400577C RID: 22396
		private int ID = -1;

		// Token: 0x0400577D RID: 22397
		public bool usesDefiniteArticle = true;

		// Token: 0x0400577E RID: 22398
		public string descOverride;

		// Token: 0x0400577F RID: 22399
		public int randomSeed;

		// Token: 0x04005780 RID: 22400
		public bool nameLocked;

		// Token: 0x04005781 RID: 22401
		protected readonly List<Thought_Situational> tmpSituationalThoughts = new List<Thought_Situational>();

		// Token: 0x04005782 RID: 22402
		protected string labelCapCache;

		// Token: 0x04005783 RID: 22403
		protected string tipCached;

		// Token: 0x04005784 RID: 22404
		public const int FloatMenuEditOrder = 2000;

		// Token: 0x04005785 RID: 22405
		public const int FloatMenuRemoveOrder = 1900;

		// Token: 0x04005786 RID: 22406
		public const int FloatMenuRegenerateOrder = 1800;

		// Token: 0x04005787 RID: 22407
		public const float OutlineBrightnessOffset = 0.05f;

		// Token: 0x04005788 RID: 22408
		private static readonly HashSet<string> tmpDescs = new HashSet<string>();

		// Token: 0x04005789 RID: 22409
		protected static readonly StringBuilder tmpCompsDesc = new StringBuilder();

		// Token: 0x0400578A RID: 22410
		private static readonly HashSet<string> tmpUsedWords = new HashSet<string>();
	}
}
