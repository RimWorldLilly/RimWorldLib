using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001653 RID: 5715
	public class Precept_GravshipLaunch : Precept_Ritual
	{
		// Token: 0x06008F91 RID: 36753 RVA: 0x0034BA4C File Offset: 0x00349C4C
		public override Window GetRitualBeginWindow(TargetInfo targetInfo, RitualObligation obligation = null, Action onConfirm = null, Pawn organizer = null, Dictionary<string, Pawn> forcedForRole = null, Pawn selectedPawn = null)
		{
			string text = this.behavior.CanStartRitualNow(targetInfo, this, selectedPawn, forcedForRole);
			if (!string.IsNullOrEmpty(text))
			{
				Messages.Message(text, targetInfo, MessageTypeDefOf.RejectInput, false);
			}
			string ritualLabel = this.Label.CapitalizeFirst();
			TargetInfo targetInfo2 = targetInfo;
			Map map = targetInfo.Map;
			Dialog_BeginRitual.ActionCallback action = delegate(RitualRoleAssignments assignments)
			{
				this.behavior.TryExecuteOn(targetInfo, organizer, this, obligation, assignments, true);
				Action onConfirm2 = onConfirm;
				if (onConfirm2 != null)
				{
					onConfirm2();
				}
				return true;
			};
			Pawn organizer2 = organizer;
			RitualObligation obligation2 = obligation;
			Dialog_BeginRitual.PawnFilter filter = (Pawn pawn, bool voluntary, bool allowOtherIdeos) => pawn.GetLord() == null && (!pawn.RaceProps.Animal || this.behavior.def.roles.Any(delegate(RitualRole r)
			{
				string text2;
				return r.AppliesToPawn(pawn, out text2, targetInfo, null, null, null, true);
			})) && !pawn.IsSubhuman && (!this.ritualOnlyForIdeoMembers || this.def.allowSpectatorsFromOtherIdeos || pawn.Ideo == this.ideo || !voluntary || allowOtherIdeos || pawn.IsPrisonerOfColony || pawn.RaceProps.Animal || (!forcedForRole.NullOrEmpty<string, Pawn>() && forcedForRole.ContainsValue(pawn)));
			string okButtonText = "Begin".Translate();
			object requiredPawns;
			if (organizer == null)
			{
				requiredPawns = null;
			}
			else
			{
				(requiredPawns = new List<Pawn>()).Add(organizer);
			}
			return new Dialog_BeginGravshipLaunch(ritualLabel, this, targetInfo2, map, action, organizer2, obligation2, filter, okButtonText, requiredPawns, forcedForRole, null, null, selectedPawn);
		}

		// Token: 0x06008F92 RID: 36754 RVA: 0x0034BB40 File Offset: 0x00349D40
		public override void ExposeData()
		{
			bool flag = false;
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.sourcePattern == null)
			{
				this.sourcePattern = DefDatabase<RitualPatternDef>.GetNamed("GravshipLaunch", true);
				flag = true;
			}
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.LoadingVars && flag)
			{
				this.obligationTriggers = new List<RitualObligationTrigger>();
				DefDatabase<RitualPatternDef>.GetNamed("GravshipLaunch", true).Fill(this);
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.ignoreExtremeTemperatures = true;
			}
		}
	}
}
