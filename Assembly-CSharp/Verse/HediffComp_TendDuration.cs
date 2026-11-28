using System;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000483 RID: 1155
	[StaticConstructorOnStartup]
	public class HediffComp_TendDuration : HediffComp_SeverityModifierBase
	{
		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06002026 RID: 8230 RVA: 0x000C19CF File Offset: 0x000BFBCF
		public HediffCompProperties_TendDuration TProps
		{
			get
			{
				return (HediffCompProperties_TendDuration)this.props;
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06002027 RID: 8231 RVA: 0x000C19DC File Offset: 0x000BFBDC
		public override bool CompShouldRemove
		{
			get
			{
				return base.CompShouldRemove || (this.TProps.disappearsAtTotalTendQuality >= 0 && this.totalTendQuality >= (float)this.TProps.disappearsAtTotalTendQuality);
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06002028 RID: 8232 RVA: 0x000C1A0F File Offset: 0x000BFC0F
		public bool IsTended
		{
			get
			{
				return Current.ProgramState == ProgramState.Playing && this.tendTicksLeft > 0;
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06002029 RID: 8233 RVA: 0x000C1A24 File Offset: 0x000BFC24
		public bool AllowTend
		{
			get
			{
				if (this.TProps.TendIsPermanent)
				{
					return !this.IsTended;
				}
				return this.TProps.TendTicksOverlap > this.tendTicksLeft;
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x0600202A RID: 8234 RVA: 0x000C1A50 File Offset: 0x000BFC50
		public override string CompTipStringExtra
		{
			get
			{
				if (this.parent.IsPermanent())
				{
					return null;
				}
				StringBuilder stringBuilder = new StringBuilder();
				if (!this.IsTended)
				{
					if (!base.Pawn.Dead && this.parent.TendableNow(false))
					{
						stringBuilder.AppendLine("NeedsTendingNow".Translate());
					}
				}
				else
				{
					if (this.TProps.showTendQuality)
					{
						string text;
						if (this.parent.Part != null && this.parent.Part.def.IsSolid(this.parent.Part, base.Pawn.health.hediffSet.hediffs))
						{
							text = this.TProps.labelSolidTendedWell;
						}
						else if (this.parent.Part != null && this.parent.Part.depth == BodyPartDepth.Inside)
						{
							text = this.TProps.labelTendedWellInner;
						}
						else
						{
							text = this.TProps.labelTendedWell;
						}
						if (text != null)
						{
							stringBuilder.AppendLine(text.CapitalizeFirst() + " (" + "quality".Translate() + " " + this.tendQuality.ToStringPercent("F0") + ")");
						}
						else
						{
							stringBuilder.AppendLine(string.Format("{0}: {1}", "TendQuality".Translate(), this.tendQuality.ToStringPercent()));
						}
						if (this.TProps.disappearsAtTotalTendQuality >= 0)
						{
							stringBuilder.AppendLine("DisappearsAtTotalTendQuality".Translate() + ": " + this.totalTendQuality.ToStringPercent() + " / " + ((float)this.TProps.disappearsAtTotalTendQuality).ToStringPercent());
						}
					}
					if (!base.Pawn.Dead && !this.TProps.TendIsPermanent && this.parent.TendableNow(true))
					{
						int num = this.tendTicksLeft - this.TProps.TendTicksOverlap;
						if (num < 0)
						{
							stringBuilder.AppendLine("CanTendNow".Translate());
						}
						else if ("NextTendIn".CanTranslate())
						{
							stringBuilder.AppendLine("NextTendIn".Translate(num.ToStringTicksToPeriod(true, false, true, true, false)));
						}
						else
						{
							stringBuilder.AppendLine("NextTreatmentIn".Translate(num.ToStringTicksToPeriod(true, false, true, true, false)));
						}
						stringBuilder.AppendLine("TreatmentExpiresIn".Translate(this.tendTicksLeft.ToStringTicksToPeriod(true, false, true, true, false)));
					}
				}
				return stringBuilder.ToString().TrimEndNewlines();
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x0600202B RID: 8235 RVA: 0x000C1D28 File Offset: 0x000BFF28
		public override TextureAndColor CompStateIcon
		{
			get
			{
				if (this.parent is Hediff_Injury)
				{
					if (this.IsTended && !this.parent.IsPermanent())
					{
						Color color = Color.Lerp(HediffComp_TendDuration.UntendedColor, Color.white, Mathf.Clamp01(this.tendQuality));
						return new TextureAndColor(HediffComp_TendDuration.TendedIcon_Well_Injury, color);
					}
				}
				else if (!(this.parent is Hediff_MissingPart) && !this.parent.FullyImmune())
				{
					if (this.IsTended)
					{
						Color color2 = Color.Lerp(HediffComp_TendDuration.UntendedColor, Color.white, Mathf.Clamp01(this.tendQuality));
						return new TextureAndColor(HediffComp_TendDuration.TendedIcon_Well_General, color2);
					}
					return HediffComp_TendDuration.TendedIcon_Need_General;
				}
				return TextureAndColor.None;
			}
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x000C1DDA File Offset: 0x000BFFDA
		public override void CompExposeData()
		{
			Scribe_Values.Look<int>(ref this.tendTicksLeft, "tendTicksLeft", -1, false);
			Scribe_Values.Look<float>(ref this.tendQuality, "tendQuality", 0f, false);
			Scribe_Values.Look<float>(ref this.totalTendQuality, "totalTendQuality", 0f, false);
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x000C1E1A File Offset: 0x000C001A
		public override float SeverityChangePerDay()
		{
			if (this.IsTended)
			{
				return this.TProps.severityPerDayTended * this.tendQuality;
			}
			return 0f;
		}

		// Token: 0x0600202E RID: 8238 RVA: 0x000C1E3C File Offset: 0x000C003C
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			if (this.tendTicksLeft > 0 && !this.TProps.TendIsPermanent)
			{
				this.tendTicksLeft--;
			}
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x000C1E6C File Offset: 0x000C006C
		public override void CompTended(float quality, float maxQuality, int batchPosition = 0)
		{
			this.tendQuality = Mathf.Clamp(quality + Rand.Range(-0.25f, 0.25f), 0f, maxQuality);
			this.totalTendQuality += this.tendQuality;
			if (this.TProps.TendIsPermanent)
			{
				this.tendTicksLeft = 1;
			}
			else
			{
				this.tendTicksLeft = Mathf.Max(0, this.tendTicksLeft) + this.TProps.TendTicksFull;
			}
			if (batchPosition == 0 && base.Pawn.Spawned)
			{
				string text = "TextMote_Tended".Translate(this.parent.Label).CapitalizeFirst() + "\n" + "Quality".Translate() + " " + this.tendQuality.ToStringPercent();
				MoteMaker.ThrowText(base.Pawn.DrawPos, base.Pawn.Map, text, Color.white, 3.65f);
			}
			base.Pawn.health.Notify_HediffChanged(this.parent);
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x000C1F90 File Offset: 0x000C0190
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.IsTended)
			{
				stringBuilder.AppendLine("tendQuality: " + this.tendQuality.ToStringPercent());
				if (!this.TProps.TendIsPermanent)
				{
					stringBuilder.AppendLine("tendTicksLeft: " + this.tendTicksLeft.ToString());
				}
			}
			else
			{
				stringBuilder.AppendLine("untended");
			}
			stringBuilder.AppendLine("severity/day: " + this.SeverityChangePerDay().ToString());
			if (this.TProps.disappearsAtTotalTendQuality >= 0)
			{
				stringBuilder.AppendLine("totalTendQuality: " + this.totalTendQuality.ToString("F2") + " / " + this.TProps.disappearsAtTotalTendQuality.ToString());
			}
			return stringBuilder.ToString().Trim();
		}

		// Token: 0x040018FE RID: 6398
		public int tendTicksLeft = -1;

		// Token: 0x040018FF RID: 6399
		public float tendQuality;

		// Token: 0x04001900 RID: 6400
		private float totalTendQuality;

		// Token: 0x04001901 RID: 6401
		public const float TendQualityRandomVariance = 0.25f;

		// Token: 0x04001902 RID: 6402
		private static readonly Color UntendedColor = new ColorInt(116, 101, 72).ToColor;

		// Token: 0x04001903 RID: 6403
		private static readonly Texture2D TendedIcon_Need_General = ContentFinder<Texture2D>.Get("UI/Icons/Medical/TendedNeed", true);

		// Token: 0x04001904 RID: 6404
		private static readonly Texture2D TendedIcon_Well_General = ContentFinder<Texture2D>.Get("UI/Icons/Medical/TendedWell", true);

		// Token: 0x04001905 RID: 6405
		private static readonly Texture2D TendedIcon_Well_Injury = ContentFinder<Texture2D>.Get("UI/Icons/Medical/BandageWell", true);
	}
}
