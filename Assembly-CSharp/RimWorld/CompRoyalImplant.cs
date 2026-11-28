using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200195F RID: 6495
	public class CompRoyalImplant : ThingComp
	{
		// Token: 0x17001C3D RID: 7229
		// (get) Token: 0x0600A30B RID: 41739 RVA: 0x003A3CB2 File Offset: 0x003A1EB2
		public CompProperties_RoyalImplant Props
		{
			get
			{
				return (CompProperties_RoyalImplant)this.props;
			}
		}

		// Token: 0x0600A30C RID: 41740 RVA: 0x003A3CBF File Offset: 0x003A1EBF
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompRoyalImplant.<SpecialDisplayStats>d__2 <SpecialDisplayStats>d__ = new CompRoyalImplant.<SpecialDisplayStats>d__2(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600A30D RID: 41741 RVA: 0x003A3CD0 File Offset: 0x003A1ED0
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Pair<Faction, RoyalTitleDef> minTitleForImplantAllFactions = Faction.GetMinTitleForImplantAllFactions(this.Props.implantHediff);
			if (minTitleForImplantAllFactions.First != null)
			{
				stringBuilder.AppendLine("MinimumRoyalTitleInspectString".Translate(minTitleForImplantAllFactions.First.Named("FACTION"), minTitleForImplantAllFactions.Second.GetLabelCapForBothGenders().Named("TITLE")).Resolve());
			}
			if (stringBuilder.Length <= 0)
			{
				return null;
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600A30E RID: 41742 RVA: 0x003A3D54 File Offset: 0x003A1F54
		public static TaggedString CheckForViolations(Pawn pawn, HediffDef hediff, int levelOffset)
		{
			if (levelOffset < 0)
			{
				return "";
			}
			if (pawn.Faction != Faction.OfPlayer || !hediff.HasComp(typeof(HediffComp_RoyalImplant)))
			{
				return "";
			}
			Hediff_Level hediff_Level = (Hediff_Level)pawn.health.hediffSet.hediffs.FirstOrDefault((Hediff h) => h.def == hediff);
			int num = (levelOffset != 0 && hediff_Level != null) ? (hediff_Level.level + levelOffset) : 0;
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				if (pawn.Faction != null && !faction.Hidden && !faction.HostileTo(Faction.OfPlayer) && ThingRequiringRoyalPermissionUtility.IsViolatingRulesOf(hediff, pawn, faction, num))
				{
					RoyalTitleDef minTitleForImplant = faction.GetMinTitleForImplant(hediff, num);
					HediffCompProperties_RoyalImplant hediffCompProperties_RoyalImplant = hediff.CompProps<HediffCompProperties_RoyalImplant>();
					string arg = hediff.label + ((num == 0) ? "" : (" (" + num.ToString() + "x)"));
					TaggedString taggedString = hediffCompProperties_RoyalImplant.violationTriggerDescriptionKey.Translate(pawn.Named("PAWN"));
					TaggedString taggedString2 = "RoyalImplantIllegalUseWarning".Translate(pawn.Named("PAWN"), arg.Named("IMPLANT"), faction.Named("FACTION"), minTitleForImplant.GetLabelCapFor(pawn).Named("TITLE"), taggedString.Named("VIOLATIONTRIGGER"));
					if (levelOffset != 0)
					{
						taggedString2 += "\n\n" + "RoyalImplantUpgradeConfirmation".Translate();
					}
					else
					{
						taggedString2 += "\n\n" + "RoyalImplantInstallConfirmation".Translate();
					}
					return taggedString2;
				}
			}
			return "";
		}
	}
}
