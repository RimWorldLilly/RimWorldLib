using System;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200048E RID: 1166
	[StaticConstructorOnStartup]
	public class HediffComp_Immunizable : HediffComp_SeverityModifierBase
	{
		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x0600205B RID: 8283 RVA: 0x000C26C8 File Offset: 0x000C08C8
		public HediffCompProperties_Immunizable Props
		{
			get
			{
				return (HediffCompProperties_Immunizable)this.props;
			}
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x0600205C RID: 8284 RVA: 0x000C26D5 File Offset: 0x000C08D5
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (!this.Hidden && this.FullyImmune)
				{
					return "DevelopedImmunityLower".Translate();
				}
				return null;
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x0600205D RID: 8285 RVA: 0x000C26F8 File Offset: 0x000C08F8
		public override string CompTipStringExtra
		{
			get
			{
				if (!this.Hidden && base.Def.PossibleToDevelopImmunityNaturally() && !this.FullyImmune)
				{
					return "Immunity".Translate() + ": " + this.NaturalImmunity.ToStringPercent("0.#");
				}
				return null;
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x0600205E RID: 8286 RVA: 0x000C2752 File Offset: 0x000C0952
		public float NaturalImmunity
		{
			get
			{
				return base.Pawn.health.immunity.GetImmunity(base.Def, true);
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x0600205F RID: 8287 RVA: 0x000C2770 File Offset: 0x000C0970
		public float Immunity
		{
			get
			{
				return base.Pawn.health.immunity.GetImmunity(base.Def, false);
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x06002060 RID: 8288 RVA: 0x000C278E File Offset: 0x000C098E
		public bool FullyImmune
		{
			get
			{
				return this.Immunity >= 1f;
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x06002061 RID: 8289 RVA: 0x000C27A0 File Offset: 0x000C09A0
		public override TextureAndColor CompStateIcon
		{
			get
			{
				if (this.FullyImmune)
				{
					return HediffComp_Immunizable.IconImmune;
				}
				return TextureAndColor.None;
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06002062 RID: 8290 RVA: 0x000C27BA File Offset: 0x000C09BA
		private bool Hidden
		{
			get
			{
				return (!Prefs.DevMode || !DebugSettings.godMode) && this.Props.hidden;
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06002063 RID: 8291 RVA: 0x000C27D8 File Offset: 0x000C09D8
		private float SeverityFactorFromHediffs
		{
			get
			{
				float num = 1f;
				if (!this.Props.severityFactorsFromHediffs.NullOrEmpty<HediffDefFactor>())
				{
					for (int i = 0; i < this.Props.severityFactorsFromHediffs.Count; i++)
					{
						if (base.Pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.severityFactorsFromHediffs[i].HediffDef, false) != null)
						{
							num *= this.Props.severityFactorsFromHediffs[i].Factor;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x06002064 RID: 8292 RVA: 0x000C2860 File Offset: 0x000C0A60
		public override void CopyFrom(HediffComp other)
		{
			HediffComp_Immunizable hediffComp_Immunizable = other as HediffComp_Immunizable;
			if (hediffComp_Immunizable != null)
			{
				Pawn pawn = this.parent.pawn;
				pawn.health.immunity.TryAddImmunityRecord(this.parent.def, this.parent.def);
				ImmunityRecord immunityRecord = pawn.health.immunity.GetImmunityRecord(this.parent.def);
				if (immunityRecord != null)
				{
					immunityRecord.immunity = hediffComp_Immunizable.Immunity;
				}
			}
		}

		// Token: 0x06002065 RID: 8293 RVA: 0x000C28D2 File Offset: 0x000C0AD2
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.severityPerDayNotImmuneRandomFactor = this.Props.severityPerDayNotImmuneRandomFactor.RandomInRange;
		}

		// Token: 0x06002066 RID: 8294 RVA: 0x000C28F1 File Offset: 0x000C0AF1
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<float>(ref this.severityPerDayNotImmuneRandomFactor, "severityPerDayNotImmuneRandomFactor", 1f, false);
		}

		// Token: 0x06002067 RID: 8295 RVA: 0x000C290F File Offset: 0x000C0B0F
		public override float SeverityChangePerDay()
		{
			return (this.FullyImmune ? this.Props.severityPerDayImmune : (this.Props.severityPerDayNotImmune * this.severityPerDayNotImmuneRandomFactor)) * this.SeverityFactorFromHediffs;
		}

		// Token: 0x06002068 RID: 8296 RVA: 0x000C2940 File Offset: 0x000C0B40
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.CompDebugString());
			if (this.severityPerDayNotImmuneRandomFactor != 1f)
			{
				stringBuilder.AppendLine("severityPerDayNotImmuneRandomFactor: " + this.severityPerDayNotImmuneRandomFactor.ToString("0.##"));
			}
			if (!base.Pawn.Dead)
			{
				ImmunityRecord immunityRecord = base.Pawn.health.immunity.GetImmunityRecord(base.Def);
				if (immunityRecord != null)
				{
					stringBuilder.AppendLine("immunity change per day: " + (immunityRecord.ImmunityChangePerTick(base.Pawn, true, this.parent) * 60000f).ToString("F3"));
					stringBuilder.AppendLine("pawn immunity gain speed: " + StatDefOf.ImmunityGainSpeed.ValueToString(base.Pawn.GetStatValue(StatDefOf.ImmunityGainSpeed, true, -1), ToStringNumberSense.Absolute, true));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001919 RID: 6425
		private float severityPerDayNotImmuneRandomFactor = 1f;

		// Token: 0x0400191A RID: 6426
		private static readonly Texture2D IconImmune = ContentFinder<Texture2D>.Get("UI/Icons/Medical/IconImmune", true);
	}
}
