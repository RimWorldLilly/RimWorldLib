using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001458 RID: 5208
	public class CompProperties_AbilityStopMentalState : CompProperties_AbilityEffect
	{
		// Token: 0x170015EB RID: 5611
		// (get) Token: 0x06007FFF RID: 32767 RVA: 0x002DA6E7 File Offset: 0x002D88E7
		public override bool OverridesPsyfocusCost
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008000 RID: 32768 RVA: 0x002DA6EA File Offset: 0x002D88EA
		public CompProperties_AbilityStopMentalState()
		{
			this.compClass = typeof(CompAbilityEffect_StopMentalState);
		}

		// Token: 0x170015EC RID: 5612
		// (get) Token: 0x06008001 RID: 32769 RVA: 0x002DA724 File Offset: 0x002D8924
		public override string PsyfocusCostExplanation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder("PsyfocusCostPerMentalBreakIntensity".Translate() + ":");
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("  - " + "MentalBreakIntensityMinor".Translate().CapitalizeFirst() + ": " + this.psyfocusCostForMinor.ToStringPercent());
				stringBuilder.AppendLine("  - " + "MentalBreakIntensityMajor".Translate().CapitalizeFirst() + ": " + this.psyfocusCostForMajor.ToStringPercent());
				stringBuilder.AppendLine("  - " + "MentalBreakIntensityExtreme".Translate().CapitalizeFirst() + ": " + this.psyfocusCostForExtreme.ToStringPercent());
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170015ED RID: 5613
		// (get) Token: 0x06008002 RID: 32770 RVA: 0x002DA821 File Offset: 0x002D8A21
		public override FloatRange PsyfocusCostRange
		{
			get
			{
				return new FloatRange(this.psyfocusCostForMinor, this.psyfocusCostForExtreme);
			}
		}

		// Token: 0x06008003 RID: 32771 RVA: 0x002DA834 File Offset: 0x002D8A34
		public override IEnumerable<string> ConfigErrors(AbilityDef parentDef)
		{
			CompProperties_AbilityStopMentalState.<ConfigErrors>d__11 <ConfigErrors>d__ = new CompProperties_AbilityStopMentalState.<ConfigErrors>d__11(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04004FFF RID: 20479
		public List<MentalStateDef> exceptions;

		// Token: 0x04005000 RID: 20480
		public float psyfocusCostForMinor = -1f;

		// Token: 0x04005001 RID: 20481
		public float psyfocusCostForMajor = -1f;

		// Token: 0x04005002 RID: 20482
		public float psyfocusCostForExtreme = -1f;
	}
}
