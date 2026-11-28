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
	// Token: 0x0200174C RID: 5964
	public abstract class Precept_Role : Precept
	{
		// Token: 0x17001962 RID: 6498
		// (get) Token: 0x0600945C RID: 37980 RVA: 0x00361074 File Offset: 0x0035F274
		public bool Active
		{
			get
			{
				return this.active || DebugSettings.activateAllIdeoRoles;
			}
		}

		// Token: 0x17001963 RID: 6499
		// (get) Token: 0x0600945D RID: 37981 RVA: 0x00361085 File Offset: 0x0035F285
		public override string UIInfoFirstLine
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x17001964 RID: 6500
		// (get) Token: 0x0600945E RID: 37982 RVA: 0x00361097 File Offset: 0x0035F297
		public override string UIInfoSecondLine
		{
			get
			{
				return base.LabelCap;
			}
		}

		// Token: 0x17001965 RID: 6501
		// (get) Token: 0x0600945F RID: 37983 RVA: 0x0036109F File Offset: 0x0035F29F
		public override string TipLabel
		{
			get
			{
				return this.def.issue.LabelCap + ": " + base.LabelCap;
			}
		}

		// Token: 0x17001966 RID: 6502
		// (get) Token: 0x06009460 RID: 37984 RVA: 0x003610CB File Offset: 0x0035F2CB
		public override bool UsesGeneratedName
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001967 RID: 6503
		// (get) Token: 0x06009461 RID: 37985 RVA: 0x003610CE File Offset: 0x0035F2CE
		public override bool CanRegenerate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001968 RID: 6504
		// (get) Token: 0x06009462 RID: 37986 RVA: 0x003610D1 File Offset: 0x0035F2D1
		public override bool SortByImpact
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001969 RID: 6505
		// (get) Token: 0x06009463 RID: 37987 RVA: 0x003610D4 File Offset: 0x0035F2D4
		// (set) Token: 0x06009464 RID: 37988 RVA: 0x003610DC File Offset: 0x0035F2DC
		public override List<PreceptApparelRequirement> ApparelRequirements
		{
			get
			{
				return this.apparelRequirements;
			}
			set
			{
				this.tipCached = null;
				this.allApparelRequirementLabelsCached.Clear();
				this.apparelRequirements = value;
			}
		}

		// Token: 0x06009465 RID: 37989 RVA: 0x003610F8 File Offset: 0x0035F2F8
		public List<string> AllApparelRequirementLabels(Gender gender, Pawn forPawn = null)
		{
			List<string> list;
			if (!this.allApparelRequirementLabelsCached.TryGetValue(gender, out list))
			{
				list = new List<string>();
				if (this.apparelRequirements != null)
				{
					for (int i = 0; i < this.apparelRequirements.Count; i++)
					{
						ApparelRequirement requirement = this.apparelRequirements[i].requirement;
						list.Add((!requirement.groupLabel.NullOrEmpty()) ? requirement.groupLabel : requirement.AllRequiredApparel(Gender.Male).First<ThingDef>().LabelCap.Resolve());
					}
				}
				this.allApparelRequirementLabelsCached[gender] = list;
			}
			if (this.apparelRequirements != null)
			{
				bool flag = false;
				for (int j = 0; j < this.apparelRequirements.Count; j++)
				{
					ApparelRequirement requirement2 = this.apparelRequirements[j].requirement;
					string t;
					if (forPawn != null && !ApparelUtility.IsRequirementActive(requirement2, ApparelRequirementSource.Role, forPawn, out t))
					{
						if (!flag)
						{
							list = new List<string>(list);
							flag = true;
						}
						string text = list[j];
						text += " [" + "ApparelRequirementDisabledLabel".Translate() + ": " + t + "]";
						list[j] = text;
					}
				}
			}
			return list;
		}

		// Token: 0x1700196A RID: 6506
		// (get) Token: 0x06009466 RID: 37990 RVA: 0x0036123E File Offset: 0x0035F43E
		public IEnumerable<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				Precept_Role.<get_DisabledWorkTypes>d__24 <get_DisabledWorkTypes>d__ = new Precept_Role.<get_DisabledWorkTypes>d__24(-2);
				<get_DisabledWorkTypes>d__.<>4__this = this;
				return <get_DisabledWorkTypes>d__;
			}
		}

		// Token: 0x06009467 RID: 37991
		public abstract IEnumerable<Pawn> ChosenPawns();

		// Token: 0x06009468 RID: 37992
		public abstract Pawn ChosenPawnSingle();

		// Token: 0x06009469 RID: 37993
		public abstract bool IsAssigned(Pawn p);

		// Token: 0x0600946A RID: 37994
		public abstract void Unassign(Pawn p, bool generateThoughts);

		// Token: 0x0600946B RID: 37995 RVA: 0x00361250 File Offset: 0x0035F450
		public string LabelForPawn(Pawn p)
		{
			if (this.def.leaderRole)
			{
				return ((p.gender == Gender.Female && !this.ideo.leaderTitleFemale.NullOrEmpty()) ? this.ideo.leaderTitleFemale : this.ideo.leaderTitleMale).CapitalizeFirst();
			}
			return this.name;
		}

		// Token: 0x0600946C RID: 37996 RVA: 0x003612AC File Offset: 0x0035F4AC
		public static List<PreceptApparelRequirement> AllPossibleRequirements(Ideo ideo, PreceptDef def, bool desperate = false)
		{
			Precept_Role.<>c__DisplayClass30_0 CS$<>8__locals1;
			CS$<>8__locals1.apparelRequirementPool = new List<PreceptApparelRequirement>();
			foreach (MemeDef memeDef in ideo.memes)
			{
				if (!memeDef.apparelRequirements.NullOrEmpty<PreceptApparelRequirement>())
				{
					Precept_Role.<AllPossibleRequirements>g__AddDistinct|30_0(memeDef.apparelRequirements, ref CS$<>8__locals1);
				}
			}
			Precept_Role.<AllPossibleRequirements>g__AddDistinct|30_0(from pa in ideo.PreceptsListForReading.OfType<Precept_Apparel>()
			where pa.TargetGender == Gender.None
			select pa into apparelPrecept
			select new PreceptApparelRequirement
			{
				requirement = new ApparelRequirement
				{
					bodyPartGroupsMatchAny = apparelPrecept.apparelDef.apparel.bodyPartGroups,
					requiredDefs = new List<ThingDef>
					{
						apparelPrecept.apparelDef
					}
				}
			}, ref CS$<>8__locals1);
			if (def.roleApparelRequirements != null)
			{
				Precept_Role.<AllPossibleRequirements>g__AddDistinct|30_0(def.roleApparelRequirements, ref CS$<>8__locals1);
			}
			if (!desperate)
			{
				for (int i = CS$<>8__locals1.apparelRequirementPool.Count - 1; i >= 0; i--)
				{
					foreach (Precept precept in ideo.PreceptsListForReading)
					{
						Precept_Apparel precept_Apparel = precept as Precept_Apparel;
						if (precept_Apparel != null && !CS$<>8__locals1.apparelRequirementPool[i].CanWearTogetherWith(precept_Apparel.apparelDef))
						{
							CS$<>8__locals1.apparelRequirementPool.RemoveAt(i);
							break;
						}
					}
				}
			}
			return CS$<>8__locals1.apparelRequirementPool;
		}

		// Token: 0x0600946D RID: 37997 RVA: 0x0036141C File Offset: 0x0035F61C
		public override List<PreceptApparelRequirement> GenerateNewApparelRequirements(FactionDef generatingFor = null)
		{
			Precept_Role.<>c__DisplayClass31_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.generatingFor = generatingFor;
			CS$<>8__locals1.apparelRequirementCount = ((this.def.roleApparelRequirementCountCurve != null) ? Mathf.CeilToInt(this.def.roleApparelRequirementCountCurve.Evaluate(Rand.Value)) : 0);
			if (CS$<>8__locals1.apparelRequirementCount == 0)
			{
				return null;
			}
			CS$<>8__locals1.apparelRequirements = new List<PreceptApparelRequirement>();
			using (List<MemeDef>.Enumerator enumerator = this.ideo.memes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.preventApparelRequirements)
					{
						return CS$<>8__locals1.apparelRequirements;
					}
				}
			}
			this.<GenerateNewApparelRequirements>g__Choose|31_1(false, ref CS$<>8__locals1);
			if (CS$<>8__locals1.apparelRequirements.Count < CS$<>8__locals1.apparelRequirementCount)
			{
				this.<GenerateNewApparelRequirements>g__Choose|31_1(true, ref CS$<>8__locals1);
			}
			return CS$<>8__locals1.apparelRequirements;
		}

		// Token: 0x0600946E RID: 37998 RVA: 0x00361500 File Offset: 0x0035F700
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			base.Init(ideo, null);
			if (!ModLister.CheckIdeology("Ideology role"))
			{
				return;
			}
			base.RegenerateName();
			if (!ideo.classicExtraMode && !ideo.classicMode)
			{
				this.apparelRequirements = this.GenerateNewApparelRequirements(generatingFor);
			}
			this.allApparelRequirementLabelsCached.Clear();
			this.restrictToSupremeGender = (Rand.Value < this.def.restrictToSupremeGenderChance);
			this.FillOrUpdateAbilities();
		}

		// Token: 0x0600946F RID: 37999 RVA: 0x00361570 File Offset: 0x0035F770
		public override string GenerateNameRaw()
		{
			if (!this.def.leaderRole)
			{
				GrammarRequest request = default(GrammarRequest);
				request.Includes.Add(this.def.nameMaker);
				base.AddIdeoRulesTo(ref request);
				return GenText.CapitalizeAsTitle(GrammarResolver.Resolve("r_roleName", request, null, false, null, null, null, false));
			}
			this.ideo.foundation.GenerateLeaderTitle();
			foreach (Precept precept in this.ideo.PreceptsListForReading)
			{
				precept.ClearTipCache();
			}
			return this.ideo.leaderTitleMale;
		}

		// Token: 0x06009470 RID: 38000 RVA: 0x0036162C File Offset: 0x0035F82C
		public override string GetTip()
		{
			if (this.tipCached.NullOrEmpty())
			{
				Precept.tmpCompsDesc.Clear();
				if (!this.def.description.NullOrEmpty())
				{
					Precept.tmpCompsDesc.Append(this.def.description);
				}
				if (!this.def.leaderRole && this.def.activationBelieverCount != -1)
				{
					Precept.tmpCompsDesc.AppendLine();
					string value = (this.def.activationBelieverCount > 1) ? Find.ActiveLanguageWorker.Pluralize(this.ideo.memberName, -1) : this.ideo.memberName;
					string value2 = (this.def.deactivationBelieverCount > 1) ? Find.ActiveLanguageWorker.Pluralize(this.ideo.memberName, -1) : this.ideo.memberName;
					Precept.tmpCompsDesc.AppendInNewLine("RoleBelieverCountDesc".Translate(value, this.def.activationBelieverCount, value2, this.def.deactivationBelieverCount).Resolve());
				}
				if (!this.def.grantedAbilities.NullOrEmpty<AbilityDef>())
				{
					List<string> list = new List<string>();
					int i = 0;
					while (i < this.def.grantedAbilities.Count)
					{
						CompProperties_AbilityStartRitual compProperties_AbilityStartRitual = this.def.grantedAbilities[i].comps.FirstOrDefault((AbilityCompProperties comp) => comp is CompProperties_AbilityStartRitual) as CompProperties_AbilityStartRitual;
						if (compProperties_AbilityStartRitual != null)
						{
							using (List<Precept>.Enumerator enumerator = this.ideo.PreceptsListForReading.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									Precept precept = enumerator.Current;
									Precept_Ritual precept_Ritual = precept as Precept_Ritual;
									if (precept_Ritual != null && precept_Ritual.def == compProperties_AbilityStartRitual.ritualDef)
									{
										list.Add(precept_Ritual.LabelCap);
										break;
									}
								}
								goto IL_20D;
							}
							goto IL_1E6;
						}
						goto IL_1E6;
						IL_20D:
						i++;
						continue;
						IL_1E6:
						list.Add(this.def.grantedAbilities[i].LabelCap.Resolve());
						goto IL_20D;
					}
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("RoleGrantedAbilitiesLabel".Translate().Resolve() + ":"));
					Precept.tmpCompsDesc.AppendInNewLine(list.ToLineList("  - "));
				}
				List<string> list2 = new List<string>();
				foreach (Precept precept2 in this.ideo.PreceptsListForReading)
				{
					Precept_Ritual precept_Ritual2 = precept2 as Precept_Ritual;
					if (precept_Ritual2 != null && precept2.def.listedForRoles)
					{
						bool flag;
						if (precept_Ritual2 == null)
						{
							flag = (null != null);
						}
						else
						{
							RitualBehaviorWorker behavior = precept_Ritual2.behavior;
							flag = (((behavior != null) ? behavior.def.stages : null) != null);
						}
						if (flag && precept_Ritual2.behavior.def.roles != null && !list2.Contains(precept_Ritual2.LabelCap) && precept_Ritual2.behavior.def.roles.Any((RitualRole r) => r.precept == this.def))
						{
							list2.Add(precept_Ritual2.LabelCap);
						}
					}
				}
				if (list2.Count > 0)
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("RoleRitualsLabel".Translate().Resolve() + ":"));
					Precept.tmpCompsDesc.AppendInNewLine(list2.ToLineList("  - "));
				}
				if (!this.def.roleRequirements.NullOrEmpty<RoleRequirement>())
				{
					List<string> list3 = new List<string>();
					for (int j = 0; j < this.def.roleRequirements.Count; j++)
					{
						string text = this.def.roleRequirements[j].GetLabelCap(this).ResolveTags();
						if (!text.NullOrEmpty())
						{
							list3.Add(text);
						}
					}
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("RoleRequirementsLabel".Translate().Resolve() + ":"));
					Precept.tmpCompsDesc.AppendInNewLine(list3.ToLineList("  - "));
				}
				if (!this.def.roleEffects.NullOrEmpty<RoleEffect>())
				{
					List<string> list4 = new List<string>();
					foreach (RoleEffect roleEffect in from r in this.def.roleEffects
					orderby (r.IsBad > false) ? 1 : 0
					select r)
					{
						list4.Add(roleEffect.Label(null, this));
					}
					if (list4.Count > 0)
					{
						Precept.tmpCompsDesc.AppendLine();
						Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("RoleEffectsLabel".Translate().Resolve() + ":"));
						Precept.tmpCompsDesc.AppendInNewLine(list4.ToLineList("  - "));
					}
				}
				List<string> list5 = this.AllApparelRequirementLabels((this.ChosenPawnSingle() != null) ? this.ChosenPawnSingle().gender : Gender.Male, this.ChosenPawnSingle());
				if (list5.Count > 0)
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("RoleRequiredApparelLabel".Translate().Resolve() + ":"));
					Precept.tmpCompsDesc.AppendInNewLine(list5.ToLineList("  - "));
				}
				if (this.def.expectationsOffset != 0)
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("Expectations".Translate().CapitalizeFirst().Resolve() + ":"));
					Precept.tmpCompsDesc.AppendInNewLine("  - " + "RoleExpectationOffset".Translate(this.def.expectationsOffset).Resolve());
				}
				if (this.def.roleDisabledWorkTags != WorkTags.None)
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("DisabledWorkLabel".Translate().Resolve() + ":"));
					Precept.tmpCompsDesc.AppendInNewLine("  - " + (from w in this.DisabledWorkTypes
					select w.labelShort).ToCommaList(false, false).CapitalizeFirst());
				}
				if (!this.def.requiredMemes.NullOrEmpty<MemeDef>())
				{
					List<MemeDef> requiredMemes = this.def.requiredMemes;
					IEnumerable<MemeDef> source = (requiredMemes != null) ? (from m in requiredMemes
					where this.ideo.memes.Contains(m)
					select m) : null;
					if (source.Any<MemeDef>())
					{
						Precept.tmpCompsDesc.AppendLine();
						Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("UnlockedByMeme".Translate().Resolve() + ":"));
						Precept.tmpCompsDesc.AppendInNewLine((from m in source
						select m.LabelCap.Resolve()).ToLineList("  - ", false));
					}
				}
				Precept.tmpCompsDesc.AppendLine();
				Precept.tmpCompsDesc.AppendInNewLine("RoleAssignmentTip".Translate().Colorize(Color.gray).ResolveTags());
				this.tipCached = Precept.tmpCompsDesc.ToString();
			}
			return this.tipCached;
		}

		// Token: 0x06009471 RID: 38001 RVA: 0x00361E5C File Offset: 0x0036005C
		public override void Tick()
		{
			base.Tick();
			this.RecacheActivity();
		}

		// Token: 0x06009472 RID: 38002
		public abstract void RecacheActivity();

		// Token: 0x06009473 RID: 38003
		public abstract void Assign(Pawn p, bool addThoughts);

		// Token: 0x06009474 RID: 38004 RVA: 0x00361E6A File Offset: 0x0036006A
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			Precept_Role.<EditFloatMenuOptions>d__38 <EditFloatMenuOptions>d__ = new Precept_Role.<EditFloatMenuOptions>d__38(-2);
			<EditFloatMenuOptions>d__.<>4__this = this;
			return <EditFloatMenuOptions>d__;
		}

		// Token: 0x06009475 RID: 38005
		public abstract void FillOrUpdateAbilities();

		// Token: 0x06009476 RID: 38006 RVA: 0x00361E7C File Offset: 0x0036007C
		protected List<Ability> FillOrUpdateAbilityList(Pawn forPawn, List<Ability> abilities)
		{
			if (this.def.grantedAbilities == null)
			{
				return null;
			}
			if (abilities != null)
			{
				abilities.RemoveAll((Ability a) => a == null || a.def == null);
				int num = 0;
				for (int i = 0; i < abilities.Count; i++)
				{
					if (this.def.grantedAbilities.Contains(abilities[i].def))
					{
						num++;
					}
				}
				if (num != this.def.grantedAbilities.Count)
				{
					abilities = null;
				}
			}
			if (abilities == null)
			{
				abilities = new List<Ability>();
				using (List<AbilityDef>.Enumerator enumerator = this.def.grantedAbilities.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AbilityDef def = enumerator.Current;
						abilities.Add(AbilityUtility.MakeAbility(def, forPawn, this));
					}
					return abilities;
				}
			}
			foreach (Ability ability in abilities)
			{
				ability.pawn = forPawn;
				ability.verb.caster = forPawn;
				ability.sourcePrecept = this;
			}
			return abilities;
		}

		// Token: 0x06009477 RID: 38007
		public abstract List<Ability> AbilitiesFor(Pawn p);

		// Token: 0x06009478 RID: 38008 RVA: 0x00361FB8 File Offset: 0x003601B8
		public void Notify_PawnUnassigned(Pawn oldPawn)
		{
			if (oldPawn != null)
			{
				Pawn_AbilityTracker abilities = oldPawn.abilities;
				if (abilities != null)
				{
					abilities.Notify_TemporaryAbilitiesChanged();
				}
			}
			if (oldPawn != null)
			{
				oldPawn.Notify_DisabledWorkTypesChanged();
			}
			if (oldPawn != null)
			{
				Pawn_ApparelTracker apparel = oldPawn.apparel;
				if (apparel != null)
				{
					apparel.Notify_RoleChanged();
				}
			}
			if (oldPawn != null && oldPawn.IsPrisoner)
			{
				Messages.Message("MessageRoleUnassignedPrisoner".Translate(oldPawn, this.LabelForPawn(oldPawn)), oldPawn, MessageTypeDefOf.SilentInput, false);
				SoundDefOf.Quest_Succeded.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06009479 RID: 38009 RVA: 0x00362040 File Offset: 0x00360240
		public void Notify_PawnAssigned(Pawn newPawn)
		{
			if (newPawn != null)
			{
				Pawn_AbilityTracker abilities = newPawn.abilities;
				if (abilities != null)
				{
					abilities.Notify_TemporaryAbilitiesChanged();
				}
			}
			if (newPawn != null)
			{
				newPawn.Notify_DisabledWorkTypesChanged();
			}
			if (newPawn != null)
			{
				Pawn_ApparelTracker apparel = newPawn.apparel;
				if (apparel != null)
				{
					apparel.Notify_RoleChanged();
				}
			}
			if (newPawn != null)
			{
				Messages.Message("MessageRoleAssigned".Translate(newPawn, this.LabelForPawn(newPawn)), newPawn, MessageTypeDefOf.SilentInput, false);
				SoundDefOf.Quest_Succeded.PlayOneShotOnCamera(null);
			}
			this.FillOrUpdateAbilities();
		}

		// Token: 0x0600947A RID: 38010 RVA: 0x003620C4 File Offset: 0x003602C4
		public RoleRequirement GetFirstUnmetRequirement(Pawn p)
		{
			if (!this.def.roleCanBeChild && !RoleRequirement_NotChild.Requirement.Met(p, this))
			{
				return RoleRequirement_NotChild.Requirement;
			}
			foreach (RoleRequirement roleRequirement in this.def.roleRequirements)
			{
				if (!roleRequirement.Met(p, this))
				{
					return roleRequirement;
				}
			}
			return null;
		}

		// Token: 0x0600947B RID: 38011 RVA: 0x00362148 File Offset: 0x00360348
		public bool RequirementsMet(Pawn p)
		{
			if (!this.def.roleCanBeChild && !RoleRequirement_NotChild.Requirement.Met(p, this))
			{
				return false;
			}
			using (List<RoleRequirement>.Enumerator enumerator = this.def.roleRequirements.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Met(p, this))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600947C RID: 38012 RVA: 0x003621C8 File Offset: 0x003603C8
		public override void DrawIcon(Rect rect)
		{
			GUI.color = this.ideo.Color;
			GUI.DrawTexture(rect, this.Icon);
			GUI.color = Color.white;
		}

		// Token: 0x0600947D RID: 38013 RVA: 0x003621F0 File Offset: 0x003603F0
		protected override void PostDrawBox(Rect rect, out bool anyTooltipActive)
		{
			bool localAnyTooltipActive = false;
			if (!this.apparelRequirements.NullOrEmpty<PreceptApparelRequirement>())
			{
				List<ThingDef> list = this.apparelRequirements.SelectMany((PreceptApparelRequirement ar) => ar.requirement.AllRequiredApparel(Gender.None)).Distinct<ThingDef>().ToList<ThingDef>();
				int num = list.Count * 24;
				num += (list.Count - 1) * 2;
				GenUI.DrawElementStack<ThingDef>(new Rect(rect.xMax - (float)num, rect.yMin, (float)num, 24f), 24f, list, delegate(Rect r, ThingDef apparel)
				{
					if (Mouse.IsOver(r))
					{
						localAnyTooltipActive = true;
						TooltipHandler.TipRegion(r, "RoleRequiredApparelLabel".Translate() + ": " + apparel.LabelCap + "\n\n" + apparel.DescriptionDetailed);
					}
					GUI.DrawTexture(r, apparel.uiIcon, ScaleMode.ScaleToFit, true, 0f, apparel.uiIconColor, 0f, 0f);
				}, (ThingDef apparel) => 24f, 0f, 2f, true);
			}
			anyTooltipActive = localAnyTooltipActive;
		}

		// Token: 0x0600947E RID: 38014 RVA: 0x003622D0 File Offset: 0x003604D0
		public override List<Thought_Situational> SituationThoughtsToAdd(Pawn pawn, List<Thought_Situational> activeThoughts)
		{
			this.tmpSituationalThoughts.Clear();
			if (!this.Active || !pawn.IsFreeNonSlaveColonist || pawn.IsQuestLodger())
			{
				return this.tmpSituationalThoughts;
			}
			if (!this.def.leaderRole && this.def.createsRoleEmptyThought && !this.ChosenPawns().Any<Pawn>() && !activeThoughts.Any(delegate(Thought_Situational t)
			{
				Thought_IdeoRoleEmpty thought_IdeoRoleEmpty2 = t as Thought_IdeoRoleEmpty;
				return thought_IdeoRoleEmpty2 != null && thought_IdeoRoleEmpty2.Role == this;
			}))
			{
				Thought_IdeoRoleEmpty thought_IdeoRoleEmpty = (Thought_IdeoRoleEmpty)ThoughtMaker.MakeThought(ThoughtDefOf.IdeoRoleEmpty);
				if (thought_IdeoRoleEmpty != null)
				{
					thought_IdeoRoleEmpty.pawn = pawn;
					thought_IdeoRoleEmpty.sourcePrecept = this;
					this.tmpSituationalThoughts.Add(thought_IdeoRoleEmpty);
				}
			}
			if (this.IsAssigned(pawn) && this.apparelRequirements != null && this.apparelRequirements.Count > 0 && !pawn.IsQuestLodger() && !activeThoughts.Any(delegate(Thought_Situational t)
			{
				Thought_IdeoRoleApparelRequirementNotMet thought_IdeoRoleApparelRequirementNotMet2 = t as Thought_IdeoRoleApparelRequirementNotMet;
				return thought_IdeoRoleApparelRequirementNotMet2 != null && thought_IdeoRoleApparelRequirementNotMet2.Role == this;
			}))
			{
				Thought_IdeoRoleApparelRequirementNotMet thought_IdeoRoleApparelRequirementNotMet = (Thought_IdeoRoleApparelRequirementNotMet)ThoughtMaker.MakeThought(ThoughtDefOf.IdeoRoleApparelRequirementNotMet);
				if (thought_IdeoRoleApparelRequirementNotMet != null)
				{
					thought_IdeoRoleApparelRequirementNotMet.pawn = pawn;
					thought_IdeoRoleApparelRequirementNotMet.sourcePrecept = this;
					this.tmpSituationalThoughts.Add(thought_IdeoRoleApparelRequirementNotMet);
				}
			}
			return this.tmpSituationalThoughts;
		}

		// Token: 0x0600947F RID: 38015 RVA: 0x003623DD File Offset: 0x003605DD
		protected bool ValidatePawn(Pawn p)
		{
			return p.Faction != null && (!p.Faction.IsPlayer || p.IsFreeNonSlaveColonist) && !p.Destroyed && !p.Dead && this.RequirementsMet(p);
		}

		// Token: 0x06009480 RID: 38016 RVA: 0x0036241C File Offset: 0x0036061C
		public override void Notify_RemovedByReforming()
		{
			foreach (Pawn pawn in this.ChosenPawns())
			{
				if (pawn != null)
				{
					pawn.Notify_DisabledWorkTypesChanged();
				}
			}
		}

		// Token: 0x06009481 RID: 38017 RVA: 0x0036246C File Offset: 0x0036066C
		public bool CanEquip(Pawn pawn, Thing thing, out string reason)
		{
			if (this.def.roleEffects != null)
			{
				foreach (RoleEffect roleEffect in this.def.roleEffects)
				{
					if (!roleEffect.CanEquip(pawn, thing))
					{
						reason = roleEffect.Label(pawn, this);
						return false;
					}
				}
			}
			reason = null;
			return true;
		}

		// Token: 0x06009482 RID: 38018 RVA: 0x003624E8 File Offset: 0x003606E8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
			Scribe_Collections.Look<PreceptApparelRequirement>(ref this.apparelRequirements, "apparelRequirements", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.restrictToSupremeGender, "restrictToSupremeGender", false, false);
		}

		// Token: 0x06009483 RID: 38019 RVA: 0x00362538 File Offset: 0x00360738
		public override void CopyTo(Precept precept)
		{
			base.CopyTo(precept);
			Precept_Role precept_Role = (Precept_Role)precept;
			precept_Role.active = this.active;
			precept_Role.restrictToSupremeGender = this.restrictToSupremeGender;
			if (this.apparelRequirements != null)
			{
				List<PreceptApparelRequirement> list = new List<PreceptApparelRequirement>();
				for (int i = 0; i < this.apparelRequirements.Count; i++)
				{
					PreceptApparelRequirement preceptApparelRequirement = new PreceptApparelRequirement();
					this.apparelRequirements[i].CopyTo(preceptApparelRequirement);
					list.Add(preceptApparelRequirement);
				}
				precept_Role.ApparelRequirements = list;
				return;
			}
			precept_Role.ApparelRequirements = null;
		}

		// Token: 0x06009485 RID: 38021 RVA: 0x003625D0 File Offset: 0x003607D0
		[CompilerGenerated]
		internal static void <AllPossibleRequirements>g__AddDistinct|30_0(IEnumerable<PreceptApparelRequirement> requirements, ref Precept_Role.<>c__DisplayClass30_0 A_1)
		{
			using (IEnumerator<PreceptApparelRequirement> enumerator = requirements.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PreceptApparelRequirement requirement = enumerator.Current;
					bool flag = false;
					Func<ThingDef, bool> <>9__3;
					foreach (PreceptApparelRequirement preceptApparelRequirement in A_1.apparelRequirementPool)
					{
						IEnumerable<ThingDef> source = preceptApparelRequirement.requirement.AllRequiredApparel(Gender.None);
						Func<ThingDef, bool> predicate;
						if ((predicate = <>9__3) == null)
						{
							predicate = (<>9__3 = ((ThingDef a) => requirement.requirement.AllRequiredApparel(Gender.None).Contains(a)));
						}
						if (source.All(predicate))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						A_1.apparelRequirementPool.Add(requirement);
					}
				}
			}
		}

		// Token: 0x06009486 RID: 38022 RVA: 0x003626AC File Offset: 0x003608AC
		[CompilerGenerated]
		private void <GenerateNewApparelRequirements>g__ChooseApparelRequirements|31_0(List<PreceptApparelRequirement> source, ref Precept_Role.<>c__DisplayClass31_0 A_2)
		{
			if (A_2.apparelRequirements.Count >= A_2.apparelRequirementCount)
			{
				return;
			}
			List<PreceptApparelRequirement> list = new List<PreceptApparelRequirement>(source);
			list.Shuffle<PreceptApparelRequirement>();
			for (int i = 0; i < list.Count; i++)
			{
				string text;
				if (list[i].CanAddRequirement(this, A_2.apparelRequirements, out text, A_2.generatingFor) && !list[i].RequirementOverlapsOther(A_2.apparelRequirements, out text))
				{
					A_2.apparelRequirements.Add(list[i]);
					if (A_2.apparelRequirements.Count >= A_2.apparelRequirementCount)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06009487 RID: 38023 RVA: 0x00362744 File Offset: 0x00360944
		[CompilerGenerated]
		private void <GenerateNewApparelRequirements>g__Choose|31_1(bool desperate, ref Precept_Role.<>c__DisplayClass31_0 A_2)
		{
			List<PreceptApparelRequirement> list = Precept_Role.AllPossibleRequirements(this.ideo, this.def, desperate);
			List<PreceptApparelRequirement> list2 = new List<PreceptApparelRequirement>(list);
			using (List<PreceptApparelRequirement>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PreceptApparelRequirement req = enumerator.Current;
					Predicate<PreceptApparelRequirement> <>9__2;
					foreach (Precept_Role precept_Role in this.ideo.RolesListForReading)
					{
						if (precept_Role.apparelRequirements != null)
						{
							List<PreceptApparelRequirement> list3 = precept_Role.apparelRequirements;
							Predicate<PreceptApparelRequirement> predicate;
							if ((predicate = <>9__2) == null)
							{
								predicate = (<>9__2 = ((PreceptApparelRequirement o) => o.requirement.SameApparelAs(req.requirement)));
							}
							if (list3.Any(predicate))
							{
								list2.Remove(req);
							}
						}
					}
				}
			}
			this.<GenerateNewApparelRequirements>g__ChooseApparelRequirements|31_0(list2, ref A_2);
			if (A_2.apparelRequirements.Count < A_2.apparelRequirementCount)
			{
				this.<GenerateNewApparelRequirements>g__ChooseApparelRequirements|31_0(list, ref A_2);
			}
		}

		// Token: 0x04005996 RID: 22934
		protected bool active;

		// Token: 0x04005997 RID: 22935
		public bool restrictToSupremeGender;

		// Token: 0x04005998 RID: 22936
		protected const string BulletPointString = "  - ";

		// Token: 0x04005999 RID: 22937
		public List<PreceptApparelRequirement> apparelRequirements;

		// Token: 0x0400599A RID: 22938
		private Dictionary<Gender, List<string>> allApparelRequirementLabelsCached = new Dictionary<Gender, List<string>>();
	}
}
