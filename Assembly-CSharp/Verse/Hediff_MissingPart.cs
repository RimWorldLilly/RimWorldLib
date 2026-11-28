using System;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x020004BC RID: 1212
	public class Hediff_MissingPart : HediffWithComps
	{
		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x060021B5 RID: 8629 RVA: 0x000C8384 File Offset: 0x000C6584
		public override float SummaryHealthPercentImpact
		{
			get
			{
				if (!this.IsFreshNonSolidExtremity)
				{
					return 0f;
				}
				if (base.Part.def.tags.NullOrEmpty<BodyPartTagDef>() && base.Part.parts.NullOrEmpty<BodyPartRecord>() && !base.Bleeding)
				{
					return 0f;
				}
				return (float)base.Part.def.hitPoints / (75f * this.pawn.HealthScale);
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x060021B6 RID: 8630 RVA: 0x000C83F9 File Offset: 0x000C65F9
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x060021B7 RID: 8631 RVA: 0x000C83FC File Offset: 0x000C65FC
		public override string LabelBase
		{
			get
			{
				if (this.lastInjury != null && this.lastInjury.injuryProps.useRemovedLabel)
				{
					return "RemovedBodyPart".Translate();
				}
				if (this.lastInjury != null && this.lastInjury.injuryProps.alwaysUseDestroyedLabel)
				{
					return this.lastInjury.injuryProps.destroyedLabel;
				}
				if (this.lastInjury == null || base.Part.depth == BodyPartDepth.Inside)
				{
					bool solid = base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs);
					return HealthUtility.GetGeneralDestroyedPartLabel(base.Part, this.IsFreshNonSolidExtremity, solid);
				}
				if (base.Part.def.socketed && !this.lastInjury.injuryProps.destroyedOutLabel.NullOrEmpty())
				{
					return this.lastInjury.injuryProps.destroyedOutLabel;
				}
				return this.lastInjury.injuryProps.destroyedLabel;
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x060021B8 RID: 8632 RVA: 0x000C84FD File Offset: 0x000C66FD
		public override string Description
		{
			get
			{
				if (this.lastInjury != null && !string.IsNullOrEmpty(this.lastInjury.Description))
				{
					return this.lastInjury.Description;
				}
				return this.def.Description;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x060021B9 RID: 8633 RVA: 0x000C8530 File Offset: 0x000C6730
		public override string LabelInBrackets
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.LabelInBrackets);
				if (this.IsFreshNonSolidExtremity)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append("FreshMissingBodyPart".Translate());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x060021BA RID: 8634 RVA: 0x000C8588 File Offset: 0x000C6788
		public override float BleedRate
		{
			get
			{
				if (this.pawn.Dead || !this.IsFreshNonSolidExtremity || this.ParentIsMissing)
				{
					return 0f;
				}
				if (!this.pawn.health.CanBleed)
				{
					return 0f;
				}
				return base.Part.def.GetMaxHealth(this.pawn) * this.def.injuryProps.bleedRate * base.Part.def.bleedRate * this.pawn.RaceProps.bleedRateFactor;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x060021BB RID: 8635 RVA: 0x000C861C File Offset: 0x000C681C
		public override float PainOffset
		{
			get
			{
				if (this.pawn.Dead || this.causesNoPain || !this.IsFreshNonSolidExtremity || this.ParentIsMissing)
				{
					return 0f;
				}
				return base.Part.def.GetMaxHealth(this.pawn) * this.def.injuryProps.painPerSeverity / this.pawn.HealthScale;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x060021BC RID: 8636 RVA: 0x000C8688 File Offset: 0x000C6888
		private bool ParentIsMissing
		{
			get
			{
				for (int i = 0; i < this.pawn.health.hediffSet.hediffs.Count; i++)
				{
					Hediff_MissingPart hediff_MissingPart = this.pawn.health.hediffSet.hediffs[i] as Hediff_MissingPart;
					if (hediff_MissingPart != null && hediff_MissingPart.Part == base.Part.parent)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x060021BD RID: 8637 RVA: 0x000C86F4 File Offset: 0x000C68F4
		// (set) Token: 0x060021BE RID: 8638 RVA: 0x000C870C File Offset: 0x000C690C
		public bool IsFresh
		{
			get
			{
				return this.isFreshInt && !this.TicksAfterNoLongerFreshPassed;
			}
			set
			{
				if (this.isFreshInt == value)
				{
					return;
				}
				this.isFreshInt = value;
				this.pawn.Drawer.renderer.WoundOverlays.ClearCache();
				PortraitsCache.SetDirty(this.pawn);
				GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x060021BF RID: 8639 RVA: 0x000C875C File Offset: 0x000C695C
		public bool IsFreshNonSolidExtremity
		{
			get
			{
				return Current.ProgramState != ProgramState.Entry && this.IsFresh && base.Part.depth != BodyPartDepth.Inside && !base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs) && !this.pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(base.Part.parent) && !this.ParentIsMissing;
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x060021C0 RID: 8640 RVA: 0x000C87E2 File Offset: 0x000C69E2
		private bool TicksAfterNoLongerFreshPassed
		{
			get
			{
				return this.ageTicks >= 90000;
			}
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x000C87F4 File Offset: 0x000C69F4
		public override bool TendableNow(bool ignoreTimer = false)
		{
			return this.IsFreshNonSolidExtremity;
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x000C87FC File Offset: 0x000C69FC
		public override void TickInterval(int delta)
		{
			bool ticksAfterNoLongerFreshPassed = this.TicksAfterNoLongerFreshPassed;
			base.TickInterval(delta);
			bool ticksAfterNoLongerFreshPassed2 = this.TicksAfterNoLongerFreshPassed;
			if (ticksAfterNoLongerFreshPassed != ticksAfterNoLongerFreshPassed2)
			{
				this.pawn.health.Notify_HediffChanged(this);
			}
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x000C8831 File Offset: 0x000C6A31
		public override void Tended(float quality, float maxQuality, int batchPosition = 0)
		{
			base.Tended(quality, maxQuality, batchPosition);
			this.IsFresh = false;
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x060021C4 RID: 8644 RVA: 0x000C8854 File Offset: 0x000C6A54
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (Current.ProgramState != ProgramState.Playing || PawnGenerator.IsBeingGenerated(this.pawn))
			{
				this.IsFresh = false;
			}
			this.pawn.health.RestorePart(base.Part, this, false);
			for (int i = 0; i < base.Part.parts.Count; i++)
			{
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(this.def, this.pawn, null);
				hediff_MissingPart.IsFresh = false;
				hediff_MissingPart.lastInjury = this.lastInjury;
				hediff_MissingPart.Part = base.Part.parts[i];
				this.pawn.health.hediffSet.AddDirect(hediff_MissingPart, null, null);
			}
		}

		// Token: 0x060021C5 RID: 8645 RVA: 0x000C891C File Offset: 0x000C6B1C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<HediffDef>(ref this.lastInjury, "lastInjury");
			Scribe_Values.Look<bool>(ref this.isFreshInt, "isFresh", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error("Hediff_MissingPart has null part after loading.");
				this.pawn.health.hediffSet.hediffs.Remove(this);
				return;
			}
		}

		// Token: 0x04001991 RID: 6545
		public HediffDef lastInjury;

		// Token: 0x04001992 RID: 6546
		private bool isFreshInt;
	}
}
