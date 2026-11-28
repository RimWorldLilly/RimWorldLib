using System;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004B2 RID: 1202
	public class Hediff_Injury : HediffWithComps
	{
		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06002144 RID: 8516 RVA: 0x000C60E8 File Offset: 0x000C42E8
		public override int UIGroupKey
		{
			get
			{
				int num = base.UIGroupKey;
				if (this.IsTended())
				{
					num = Gen.HashCombineInt(num, 152235495);
				}
				return num;
			}
		}

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06002145 RID: 8517 RVA: 0x000C6114 File Offset: 0x000C4314
		public override string LabelBase
		{
			get
			{
				HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent)
				{
					if (base.Part != null && base.Part.def.delicate && !hediffComp_GetsPermanent.Props.instantlyPermanentLabel.NullOrEmpty())
					{
						return hediffComp_GetsPermanent.Props.instantlyPermanentLabel;
					}
					if (!hediffComp_GetsPermanent.Props.permanentLabel.NullOrEmpty())
					{
						return hediffComp_GetsPermanent.Props.permanentLabel;
					}
				}
				return base.LabelBase;
			}
		}

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06002146 RID: 8518 RVA: 0x000C6190 File Offset: 0x000C4390
		public override string LabelInBrackets
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.LabelInBrackets);
				if (this.sourceHediffDef != null)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(this.sourceHediffDef.label);
				}
				else if (this.sourceDef != null)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					if (!this.sourceToolLabel.NullOrEmpty())
					{
						stringBuilder.Append("SourceToolLabel".Translate(this.sourceLabel, this.sourceToolLabel));
					}
					else if (this.sourceBodyPartGroup != null)
					{
						stringBuilder.Append("SourceToolLabel".Translate(this.sourceLabel, this.sourceBodyPartGroup.LabelShort));
					}
					else
					{
						stringBuilder.Append(this.sourceLabel);
					}
				}
				HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent && hediffComp_GetsPermanent.PainCategory != PainCategory.Painless)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(("PainCategory_" + hediffComp_GetsPermanent.PainCategory.ToString()).Translate());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x06002147 RID: 8519 RVA: 0x000C62E8 File Offset: 0x000C44E8
		public override Color LabelColor
		{
			get
			{
				if (this.IsPermanent())
				{
					return Hediff_Injury.PermanentInjuryColor;
				}
				return Color.white;
			}
		}

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x06002148 RID: 8520 RVA: 0x000C6300 File Offset: 0x000C4500
		public override string SeverityLabel
		{
			get
			{
				if (this.Severity == 0f)
				{
					return null;
				}
				return this.Severity.ToString("F1");
			}
		}

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x06002149 RID: 8521 RVA: 0x000C632F File Offset: 0x000C452F
		public override float SummaryHealthPercentImpact
		{
			get
			{
				if (this.IsPermanent() || !this.Visible)
				{
					return 0f;
				}
				return this.Severity / (75f * this.pawn.HealthScale);
			}
		}

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x0600214A RID: 8522 RVA: 0x000C6360 File Offset: 0x000C4560
		public override float PainOffset
		{
			get
			{
				if (this.pawn.Dead || (base.Part != null && this.pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(base.Part)) || this.causesNoPain)
				{
					return 0f;
				}
				HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
				float num;
				if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent)
				{
					num = this.Severity * this.def.injuryProps.averagePainPerSeverityPermanent * hediffComp_GetsPermanent.PainFactor;
				}
				else
				{
					num = this.Severity * this.def.injuryProps.painPerSeverity;
				}
				return num / this.pawn.HealthScale;
			}
		}

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x0600214B RID: 8523 RVA: 0x000C6408 File Offset: 0x000C4608
		public override float BleedRate
		{
			get
			{
				if (this.pawn.Dead)
				{
					return 0f;
				}
				if (this.BleedingStoppedDueToAge)
				{
					return 0f;
				}
				if (!this.pawn.health.CanBleed)
				{
					return 0f;
				}
				if ((base.Part != null && base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs)) || this.IsTended() || this.IsPermanent())
				{
					return 0f;
				}
				if (base.Part != null)
				{
					Hediff directlyAddedPartFor = this.pawn.health.hediffSet.GetDirectlyAddedPartFor(base.Part);
					if (directlyAddedPartFor != null && !directlyAddedPartFor.def.organicAddedBodypart)
					{
						return 0f;
					}
				}
				float num = this.Severity * this.def.injuryProps.bleedRate * this.pawn.RaceProps.bleedRateFactor;
				if (base.Part != null)
				{
					num *= base.Part.def.bleedRate;
				}
				return num;
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x0600214C RID: 8524 RVA: 0x000C6518 File Offset: 0x000C4718
		private int AgeTicksToStopBleeding
		{
			get
			{
				int num = 90000;
				float t = Mathf.Clamp(Mathf.InverseLerp(1f, 30f, this.Severity), 0f, 1f);
				return num + Mathf.RoundToInt(Mathf.Lerp(0f, 90000f, t));
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x0600214D RID: 8525 RVA: 0x000C6565 File Offset: 0x000C4765
		private bool BleedingStoppedDueToAge
		{
			get
			{
				return this.ageTicks >= this.AgeTicksToStopBleeding;
			}
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x000C6578 File Offset: 0x000C4778
		public override void TickInterval(int delta)
		{
			bool bleedingStoppedDueToAge = this.BleedingStoppedDueToAge;
			base.TickInterval(delta);
			bool bleedingStoppedDueToAge2 = this.BleedingStoppedDueToAge;
			if (bleedingStoppedDueToAge != bleedingStoppedDueToAge2)
			{
				this.pawn.health.Notify_HediffChanged(this);
			}
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x000C65B0 File Offset: 0x000C47B0
		public override void Heal(float amount)
		{
			this.Severity -= amount;
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostInjuryHeal(amount);
				}
			}
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x000C660C File Offset: 0x000C480C
		public override bool TryMergeWith(Hediff other)
		{
			Hediff_Injury hediff_Injury = other as Hediff_Injury;
			return hediff_Injury != null && hediff_Injury.def == this.def && hediff_Injury.Part == base.Part && !hediff_Injury.IsTended() && !hediff_Injury.IsPermanent() && !this.IsTended() && !this.IsPermanent() && this.def.injuryProps.canMerge && base.TryMergeWith(other);
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x000C667C File Offset: 0x000C487C
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (base.Part != null && base.Part.coverageAbs <= 0f && (dinfo == null || dinfo.Value.Def != DamageDefOf.SurgicalCut))
			{
				string[] array = new string[6];
				array[0] = "Added injury to ";
				int num = 1;
				BodyPartDef def = base.Part.def;
				array[num] = ((def != null) ? def.ToString() : null);
				array[2] = " but it should be impossible to hit it. pawn=";
				array[3] = this.pawn.ToStringSafe<Pawn>();
				array[4] = " dinfo=";
				array[5] = dinfo.ToStringSafe<DamageInfo?>();
				Log.Error(string.Concat(array));
			}
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x000C6725 File Offset: 0x000C4925
		public override void PostRemoved()
		{
			base.PostRemoved();
			this.pawn.Drawer.renderer.WoundOverlays.ClearCache();
			PortraitsCache.SetDirty(this.pawn);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x000C6760 File Offset: 0x000C4960
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.destroysBodyParts, "destroysBodyParts", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error("Hediff_Injury has null part after loading.");
				this.pawn.health.hediffSet.hediffs.Remove(this);
			}
		}

		// Token: 0x04001964 RID: 6500
		public bool destroysBodyParts = true;

		// Token: 0x04001965 RID: 6501
		private static readonly Color PermanentInjuryColor = new Color(0.72f, 0.72f, 0.72f);
	}
}
