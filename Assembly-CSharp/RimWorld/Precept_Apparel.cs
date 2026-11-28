using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001648 RID: 5704
	public class Precept_Apparel : Precept
	{
		// Token: 0x170018B6 RID: 6326
		// (get) Token: 0x06008F18 RID: 36632 RVA: 0x00349B05 File Offset: 0x00347D05
		public override bool SortByImpact
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170018B7 RID: 6327
		// (get) Token: 0x06008F19 RID: 36633 RVA: 0x00349B08 File Offset: 0x00347D08
		public override bool CanRegenerate
		{
			get
			{
				return this.def.Worker.ThingDefs.Count<PreceptThingChance>() > 1;
			}
		}

		// Token: 0x170018B8 RID: 6328
		// (get) Token: 0x06008F1A RID: 36634 RVA: 0x00349B22 File Offset: 0x00347D22
		public override string TipLabel
		{
			get
			{
				return this.UIInfoFirstLine + ": " + this.def.LabelCap;
			}
		}

		// Token: 0x170018B9 RID: 6329
		// (get) Token: 0x06008F1B RID: 36635 RVA: 0x00349B49 File Offset: 0x00347D49
		public override string UIInfoFirstLine
		{
			get
			{
				if (this.apparelDef != null)
				{
					return this.apparelDef.LabelCap;
				}
				return base.UIInfoFirstLine;
			}
		}

		// Token: 0x170018BA RID: 6330
		// (get) Token: 0x06008F1C RID: 36636 RVA: 0x00349B6C File Offset: 0x00347D6C
		public override string UIInfoSecondLine
		{
			get
			{
				if (this.TargetGender == Gender.None)
				{
					return "Everyone".Translate() + ": " + base.UIInfoSecondLine;
				}
				return this.TargetGender.GetLabel(false).CapitalizeFirst() + ": " + base.UIInfoSecondLine;
			}
		}

		// Token: 0x170018BB RID: 6331
		// (get) Token: 0x06008F1D RID: 36637 RVA: 0x00349BC8 File Offset: 0x00347DC8
		public Gender TargetGender
		{
			get
			{
				Gender? gender = this.overrideGender;
				if (gender == null)
				{
					return this.targetGender;
				}
				return gender.GetValueOrDefault();
			}
		}

		// Token: 0x06008F1E RID: 36638 RVA: 0x00349BF4 File Offset: 0x00347DF4
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			base.Init(ideo, generatingFor);
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				PreceptComp_Apparel preceptComp_Apparel = this.def.comps[i] as PreceptComp_Apparel;
				if (preceptComp_Apparel != null)
				{
					this.targetGender = preceptComp_Apparel.AffectedGender(ideo);
					break;
				}
			}
			this.InitDescription();
		}

		// Token: 0x06008F1F RID: 36639 RVA: 0x00349C54 File Offset: 0x00347E54
		public override void Regenerate(Ideo ideo, FactionDef generatingFor = null)
		{
			IEnumerable<PreceptThingChance> source;
			if (!this.def.canUseAlreadyUsedThingDef)
			{
				Precept_Apparel.usedThingDefsTmp.Clear();
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					Precept_Apparel precept_Apparel = precept as Precept_Apparel;
					if (precept_Apparel != null)
					{
						Precept_Apparel.usedThingDefsTmp.Add(precept_Apparel.apparelDef);
					}
				}
				source = from bd in this.def.Worker.ThingDefsForIdeo(ideo, generatingFor)
				where !Precept_Apparel.usedThingDefsTmp.Contains(bd.def)
				select bd;
			}
			else
			{
				source = this.def.Worker.ThingDefsForIdeo(ideo, generatingFor);
			}
			if (source.Any<PreceptThingChance>())
			{
				PreceptThingChance preceptThingChance;
				if (source.TryRandomElementByWeight((PreceptThingChance d) => d.chance, out preceptThingChance))
				{
					this.apparelDef = preceptThingChance.def;
					goto IL_144;
				}
			}
			this.apparelDef = this.def.Worker.ThingDefsForIdeo(ideo, generatingFor).RandomElementByWeight((PreceptThingChance d) => d.chance).def;
			Log.Warning("Failed to generate a unique apparel for " + ideo.name);
			IL_144:
			if (ideo.SupremeGender == Gender.None && Rand.Value < 0.5f)
			{
				this.overrideGender = new Gender?((Rand.Value < 0.5f) ? Gender.Male : Gender.Female);
			}
			else
			{
				this.overrideGender = null;
			}
			base.Regenerate(ideo, null);
		}

		// Token: 0x06008F20 RID: 36640 RVA: 0x00349DFC File Offset: 0x00347FFC
		public override void DrawIcon(Rect rect)
		{
			Widgets.ThingIcon(rect, this.apparelDef, null, this.ideo.GetStyleFor(this.apparelDef), 1f, new Color?(this.ideo.ApparelColor), null, 1f);
		}

		// Token: 0x06008F21 RID: 36641 RVA: 0x00349E4A File Offset: 0x0034804A
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			Precept_Apparel.<EditFloatMenuOptions>d__19 <EditFloatMenuOptions>d__ = new Precept_Apparel.<EditFloatMenuOptions>d__19(-2);
			<EditFloatMenuOptions>d__.<>4__this = this;
			return <EditFloatMenuOptions>d__;
		}

		// Token: 0x06008F22 RID: 36642 RVA: 0x00349E5C File Offset: 0x0034805C
		private void InitDescription()
		{
			string arg = (this.TargetGender != Gender.None) ? this.TargetGender.GetLabel(false) : "All".Translate();
			this.descOverride = this.def.description.Formatted(arg.Named("GENDER"), this.apparelDef.Named("APPAREL")).CapitalizeFirst();
		}

		// Token: 0x06008F23 RID: 36643 RVA: 0x00349ED0 File Offset: 0x003480D0
		public override bool CompatibleWith(Precept other)
		{
			if (other.def.prefersNudity && (this.TargetGender == Gender.None || this.TargetGender == other.def.genderPrefersNudity))
			{
				return false;
			}
			Precept_Apparel precept_Apparel = other as Precept_Apparel;
			return (precept_Apparel == null || this.def != other.def || (this.TargetGender != precept_Apparel.TargetGender && this.TargetGender != Gender.None && precept_Apparel.TargetGender != Gender.None) || ApparelUtility.CanWearTogether(this.apparelDef, precept_Apparel.apparelDef, BodyDefOf.Human)) && base.CompatibleWith(other);
		}

		// Token: 0x06008F24 RID: 36644 RVA: 0x00349F60 File Offset: 0x00348160
		public override bool GetPlayerWarning(out string shortText, out string description)
		{
			foreach (Precept_Role precept_Role in this.ideo.RolesListForReading)
			{
				if ((this.TargetGender == Gender.None || !precept_Role.restrictToSupremeGender || this.ideo.SupremeGender == Gender.None || this.ideo.SupremeGender == this.TargetGender) && precept_Role.ApparelRequirements != null)
				{
					foreach (PreceptApparelRequirement preceptApparelRequirement in precept_Role.ApparelRequirements)
					{
						IEnumerable<ThingDef> source = preceptApparelRequirement.requirement.AllRequiredApparel(this.ideo.SupremeGender);
						if (source.Any((ThingDef ap) => ap != this.apparelDef && !ApparelUtility.CanWearTogether(this.apparelDef, ap, BodyDefOf.Human)))
						{
							shortText = "MessageIdeoWarnRoleApparelOverlapsDesiredApparel".Translate(this.apparelDef.label.Named("DESIREDAPPAREL"), precept_Role.def.label.Named("ROLE"));
							description = "DescriptionIdeoWarnRoleApparelOverlapsDesiredApparel".Translate(this.apparelDef.label.Named("DESIREDAPPAREL"), precept_Role.def.label.Named("ROLE"), string.Join(", ", from ap in source
							select ap.label).CapitalizeFirst().Named("ROLEAPPAREL")).CapitalizeFirst();
							return true;
						}
					}
				}
			}
			shortText = null;
			description = null;
			return false;
		}

		// Token: 0x06008F25 RID: 36645 RVA: 0x0034A148 File Offset: 0x00348348
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.apparelDef, "apparelDef");
			Scribe_Values.Look<Gender>(ref this.targetGender, "targetGender", Gender.None, false);
			Scribe_Values.Look<Gender?>(ref this.overrideGender, "overrideGender", null, false);
		}

		// Token: 0x06008F26 RID: 36646 RVA: 0x0034A197 File Offset: 0x00348397
		public override void CopyTo(Precept other)
		{
			base.CopyTo(other);
			Precept_Apparel precept_Apparel = (Precept_Apparel)other;
			precept_Apparel.apparelDef = this.apparelDef;
			precept_Apparel.targetGender = this.targetGender;
			precept_Apparel.overrideGender = this.overrideGender;
		}

		// Token: 0x0400578B RID: 22411
		public ThingDef apparelDef;

		// Token: 0x0400578C RID: 22412
		private Gender targetGender;

		// Token: 0x0400578D RID: 22413
		private Gender? overrideGender;

		// Token: 0x0400578E RID: 22414
		private static List<ThingDef> usedThingDefsTmp = new List<ThingDef>();
	}
}
