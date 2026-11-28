using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001191 RID: 4497
	public class IncidentWorker_RansomDemand : IncidentWorker
	{
		// Token: 0x0600709C RID: 28828 RVA: 0x002768E4 File Offset: 0x00274AE4
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return CommsConsoleUtility.PlayerHasPoweredCommsConsole((Map)parms.target) && this.RandomKidnappedColonist() != null && base.CanFireNowSub(parms);
		}

		// Token: 0x0600709D RID: 28829 RVA: 0x0027690C File Offset: 0x00274B0C
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			Pawn pawn = this.RandomKidnappedColonist();
			if (pawn == null)
			{
				return false;
			}
			Faction faction = this.FactionWhichKidnapped(pawn);
			int num = this.RandomFee(pawn);
			ChoiceLetter_RansomDemand choiceLetter_RansomDemand = (ChoiceLetter_RansomDemand)LetterMaker.MakeLetter(this.def.letterLabel, "RansomDemand".Translate(pawn.LabelShort, faction.NameColored, num, pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true), this.def.letterDef, null, null);
			choiceLetter_RansomDemand.title = "RansomDemandTitle".Translate(map.Parent.Label);
			choiceLetter_RansomDemand.radioMode = true;
			choiceLetter_RansomDemand.kidnapped = pawn;
			choiceLetter_RansomDemand.faction = faction;
			choiceLetter_RansomDemand.map = map;
			choiceLetter_RansomDemand.fee = num;
			choiceLetter_RansomDemand.relatedFaction = faction;
			choiceLetter_RansomDemand.quest = parms.quest;
			choiceLetter_RansomDemand.StartTimeout(60000);
			Find.LetterStack.ReceiveLetter(choiceLetter_RansomDemand, null, 0, true);
			return true;
		}

		// Token: 0x0600709E RID: 28830 RVA: 0x00276A2C File Offset: 0x00274C2C
		private Pawn RandomKidnappedColonist()
		{
			IncidentWorker_RansomDemand.candidates.Clear();
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				List<Pawn> kidnappedPawnsListForReading = allFactionsListForReading[i].kidnapped.KidnappedPawnsListForReading;
				for (int j = 0; j < kidnappedPawnsListForReading.Count; j++)
				{
					if (kidnappedPawnsListForReading[j].Faction == Faction.OfPlayer && kidnappedPawnsListForReading[j].RaceProps.Humanlike)
					{
						IncidentWorker_RansomDemand.candidates.Add(kidnappedPawnsListForReading[j]);
					}
				}
			}
			List<Letter> lettersListForReading = Find.LetterStack.LettersListForReading;
			for (int k = 0; k < lettersListForReading.Count; k++)
			{
				ChoiceLetter_RansomDemand choiceLetter_RansomDemand = lettersListForReading[k] as ChoiceLetter_RansomDemand;
				if (choiceLetter_RansomDemand != null)
				{
					IncidentWorker_RansomDemand.candidates.Remove(choiceLetter_RansomDemand.kidnapped);
				}
			}
			Pawn result;
			if (!IncidentWorker_RansomDemand.candidates.TryRandomElement(out result))
			{
				return null;
			}
			IncidentWorker_RansomDemand.candidates.Clear();
			return result;
		}

		// Token: 0x0600709F RID: 28831 RVA: 0x00276B28 File Offset: 0x00274D28
		private Faction FactionWhichKidnapped(Pawn pawn)
		{
			return Find.FactionManager.AllFactionsListForReading.Find((Faction x) => x.kidnapped.KidnappedPawnsListForReading.Contains(pawn));
		}

		// Token: 0x060070A0 RID: 28832 RVA: 0x00276B60 File Offset: 0x00274D60
		private int RandomFee(Pawn pawn)
		{
			return (int)(pawn.MarketValue * DiplomacyTuning.RansomFeeMarketValueFactorRange.RandomInRange);
		}

		// Token: 0x040047B5 RID: 18357
		private const int TimeoutTicks = 60000;

		// Token: 0x040047B6 RID: 18358
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
