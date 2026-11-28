using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A3E RID: 6718
	public class CompUseEffect_InstallImplant : CompUseEffect
	{
		// Token: 0x17001D8C RID: 7564
		// (get) Token: 0x0600A875 RID: 43125 RVA: 0x003BD8B5 File Offset: 0x003BBAB5
		public CompProperties_UseEffectInstallImplant Props
		{
			get
			{
				return (CompProperties_UseEffectInstallImplant)this.props;
			}
		}

		// Token: 0x0600A876 RID: 43126 RVA: 0x003BD8C4 File Offset: 0x003BBAC4
		public override void DoEffect(Pawn user)
		{
			BodyPartRecord bodyPartRecord = user.RaceProps.body.GetPartsWithDef(this.Props.bodyPart).FirstOrFallback(null);
			if (bodyPartRecord == null)
			{
				return;
			}
			Hediff firstHediffOfDef = user.health.hediffSet.GetFirstHediffOfDef(this.Props.hediffDef, false);
			if (firstHediffOfDef == null && !this.Props.requiresExistingHediff)
			{
				user.health.AddHediff(this.Props.hediffDef, bodyPartRecord, null, null);
				return;
			}
			if (this.Props.canUpgrade)
			{
				((Hediff_Level)firstHediffOfDef).ChangeLevel(1);
			}
		}

		// Token: 0x0600A877 RID: 43127 RVA: 0x003BD960 File Offset: 0x003BBB60
		public override AcceptanceReport CanBeUsedBy(Pawn p)
		{
			if ((!p.IsFreeColonist || p.HasExtraHomeFaction(null)) && !this.Props.allowNonColonists)
			{
				return "InstallImplantNotAllowedForNonColonists".Translate();
			}
			if (p.RaceProps.body.GetPartsWithDef(this.Props.bodyPart).FirstOrFallback(null) == null)
			{
				return "InstallImplantNoBodyPart".Translate() + ": " + this.Props.bodyPart.LabelShort;
			}
			if (this.Props.requiresPsychicallySensitive && p.psychicEntropy != null && !p.psychicEntropy.IsPsychicallySensitive)
			{
				return "InstallImplantPsychicallyDeaf".Translate();
			}
			Hediff existingImplant = this.GetExistingImplant(p);
			if (this.Props.requiresExistingHediff && existingImplant == null)
			{
				return "InstallImplantHediffRequired".Translate(this.Props.hediffDef.label);
			}
			if (existingImplant != null)
			{
				if (!this.Props.canUpgrade)
				{
					return "InstallImplantAlreadyInstalled".Translate();
				}
				Hediff_Level hediff_Level = (Hediff_Level)existingImplant;
				if ((float)hediff_Level.level >= hediff_Level.def.maxSeverity)
				{
					return "InstallImplantAlreadyMaxLevel".Translate();
				}
				if (this.Props.maxSeverity <= (float)hediff_Level.level)
				{
					return "InstallImplantAlreadyMaxLevel".Translate() + " " + this.Props.maxSeverity.ToString();
				}
				if (this.Props.minSeverity > (float)hediff_Level.level)
				{
					return "InstallImplantMinLevel".Translate(this.Props.minSeverity);
				}
			}
			return true;
		}

		// Token: 0x0600A878 RID: 43128 RVA: 0x003BDB30 File Offset: 0x003BBD30
		public Hediff GetExistingImplant(Pawn p)
		{
			for (int i = 0; i < p.health.hediffSet.hediffs.Count; i++)
			{
				Hediff hediff = p.health.hediffSet.hediffs[i];
				if (hediff.def == this.Props.hediffDef && hediff.Part == p.RaceProps.body.GetPartsWithDef(this.Props.bodyPart).FirstOrFallback(null))
				{
					return hediff;
				}
			}
			return null;
		}
	}
}
