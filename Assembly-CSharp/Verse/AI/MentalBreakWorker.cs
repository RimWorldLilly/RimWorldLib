using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000856 RID: 2134
	public class MentalBreakWorker
	{
		// Token: 0x0600431B RID: 17179 RVA: 0x001A7B9C File Offset: 0x001A5D9C
		public virtual float CommonalityFor(Pawn pawn, bool moodCaused = false)
		{
			float num = this.def.baseCommonality;
			if (pawn.Faction == Faction.OfPlayer && this.def.commonalityFactorPerPopulationCurve != null)
			{
				num *= this.def.commonalityFactorPerPopulationCurve.Evaluate((float)PawnsFinder.AllMaps_FreeColonists.Count<Pawn>());
			}
			if (pawn.genes != null && this.def.mentalState != null && this.def.mentalState.IsAggro)
			{
				num *= pawn.genes.AggroMentalBreakSelectionChanceFactor;
			}
			return num;
		}

		// Token: 0x0600431C RID: 17180 RVA: 0x001A7C24 File Offset: 0x001A5E24
		public virtual bool BreakCanOccur(Pawn pawn)
		{
			if (this.def.requiredTrait != null && (pawn.story == null || !pawn.story.traits.HasTrait(this.def.requiredTrait)))
			{
				return false;
			}
			if (this.def.requiredGene != null && (pawn.genes == null || !pawn.genes.HasActiveGene(this.def.requiredGene)))
			{
				return false;
			}
			if (this.def.requiredPrecept != null && (pawn.Ideo == null || !pawn.Ideo.HasPrecept(this.def.requiredPrecept)))
			{
				return false;
			}
			if (!this.def.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && (!pawn.SpawnedOrAnyParentSpawned || !pawn.MapHeld.Tile.Valid || !this.def.layerWhitelist.Contains(pawn.MapHeld.Tile.LayerDef)))
			{
				return false;
			}
			if (this.def.mentalState != null && pawn.story != null && pawn.story.traits.allTraits.Any((Trait tr) => !tr.Suppressed && tr.CurrentData.disallowedMentalStates != null && tr.CurrentData.disallowedMentalStates.Contains(this.def.mentalState)))
			{
				return false;
			}
			if (!this.def.questLodgersCanDo && pawn.IsQuestLodger())
			{
				return false;
			}
			if (this.def.mentalState != null && !this.def.mentalState.Worker.StateCanOccur(pawn))
			{
				return false;
			}
			if (pawn.story != null)
			{
				IEnumerable<MentalBreakDef> theOnlyAllowedMentalBreaks = pawn.story.traits.TheOnlyAllowedMentalBreaks;
				if (!theOnlyAllowedMentalBreaks.Contains(this.def) && theOnlyAllowedMentalBreaks.Any((MentalBreakDef x) => x.intensity == this.def.intensity && x.Worker.BreakCanOccur(pawn)))
				{
					return false;
				}
			}
			return !TutorSystem.TutorialMode || pawn.Faction != Faction.OfPlayer;
		}

		// Token: 0x0600431D RID: 17181 RVA: 0x001A7E4C File Offset: 0x001A604C
		public virtual bool TryStart(Pawn pawn, string reason, bool causedByMood)
		{
			Pawn_MindState mindState = pawn.mindState;
			return ((mindState != null) ? mindState.mentalStateHandler : null) != null && pawn.mindState.mentalStateHandler.TryStartMentalState(this.def.mentalState, reason, false, false, causedByMood, null, false, false, false);
		}

		// Token: 0x0600431E RID: 17182 RVA: 0x001A7E94 File Offset: 0x001A6094
		protected bool TrySendLetter(Pawn pawn, string textKey, string reason)
		{
			if (!PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				return false;
			}
			TaggedString label = this.def.LabelCap + ": " + pawn.LabelShortCap;
			TaggedString taggedString = textKey.Translate(pawn.LabelShort, pawn.Named("PAWN")).CapitalizeFirst();
			if (reason != null)
			{
				taggedString += "\n\n" + reason;
			}
			taggedString = taggedString.AdjustedFor(pawn, "PAWN", true);
			Find.LetterStack.ReceiveLetter(label, taggedString, LetterDefOf.NegativeEvent, pawn, null, null, null, null, 0, true);
			return true;
		}

		// Token: 0x04002BEB RID: 11243
		public MentalBreakDef def;
	}
}
