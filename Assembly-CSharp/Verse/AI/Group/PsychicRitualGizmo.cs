using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse.AI.Group
{
	// Token: 0x0200091A RID: 2330
	public static class PsychicRitualGizmo
	{
		// Token: 0x06004709 RID: 18185 RVA: 0x001BE3D2 File Offset: 0x001BC5D2
		public static IEnumerable<Gizmo> GetGizmos(Thing target)
		{
			PsychicRitualGizmo.<GetGizmos>d__0 <GetGizmos>d__ = new PsychicRitualGizmo.<GetGizmos>d__0(-2);
			<GetGizmos>d__.<>3__target = target;
			return <GetGizmos>d__;
		}

		// Token: 0x0600470A RID: 18186 RVA: 0x001BE3E4 File Offset: 0x001BC5E4
		public static Gizmo CancelGizmo(PsychicRitual psychicRitual)
		{
			Action <>9__1;
			return new Command_Action
			{
				defaultLabel = "CommandCancelPsychicRitual".Translate(psychicRitual.def.label).CapitalizeFirst(),
				defaultDesc = "CommandCancelPsychicRitualDesc".Translate(psychicRitual.def.label).CapitalizeFirst().EndWithPeriod(),
				icon = ContentFinder<Texture2D>.Get("UI/Commands/CancelPsychicRitual_Gizmo", true),
				action = delegate()
				{
					WindowStack windowStack = Find.WindowStack;
					TaggedString text = "CommandCancelPsychicRitualConfirm".Translate(psychicRitual.def.label);
					Action confirmedAct;
					if ((confirmedAct = <>9__1) == null)
					{
						confirmedAct = (<>9__1 = delegate()
						{
							psychicRitual.CancelPsychicRitual(TaggedString.Empty);
						});
					}
					windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, false, null, WindowLayer.Dialog));
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				},
				hotKey = KeyBindingDefOf.Misc6
			};
		}

		// Token: 0x0600470B RID: 18187 RVA: 0x001BE4A0 File Offset: 0x001BC6A0
		public static Gizmo LeaveGizmo(PsychicRitual psychicRitual, Pawn pawn)
		{
			return new Command_Action
			{
				defaultLabel = "CommandLeavePsychicRitual".Translate(psychicRitual.def.label).CapitalizeFirst(),
				defaultDesc = "CommandLeavePsychicRitualDesc".Translate(psychicRitual.def.label).CapitalizeFirst().EndWithPeriod(),
				action = delegate()
				{
					psychicRitual.LeavePsychicRitual(pawn, TaggedString.Empty);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				},
				hotKey = KeyBindingDefOf.Misc5
			};
		}

		// Token: 0x0600470C RID: 18188 RVA: 0x001BE550 File Offset: 0x001BC750
		private static List<PsychicRitualDef_InvocationCircle> VisibleRituals()
		{
			PsychicRitualGizmo.tmpVisibleRituals.Clear();
			foreach (PsychicRitualDef psychicRitualDef in DefDatabase<PsychicRitualDef>.AllDefs)
			{
				if (psychicRitualDef.Visible)
				{
					PsychicRitualDef_InvocationCircle psychicRitualDef_InvocationCircle = psychicRitualDef as PsychicRitualDef_InvocationCircle;
					if (psychicRitualDef_InvocationCircle != null)
					{
						PsychicRitualGizmo.tmpVisibleRituals.Add(psychicRitualDef_InvocationCircle);
					}
				}
			}
			return PsychicRitualGizmo.tmpVisibleRituals;
		}

		// Token: 0x0600470D RID: 18189 RVA: 0x001BE5C4 File Offset: 0x001BC7C4
		private static void InitializePsychicRitual(PsychicRitualDef_InvocationCircle psychicRitualDef, Thing target)
		{
			TargetInfo target2 = new TargetInfo(target);
			PsychicRitualRoleAssignments assignments = psychicRitualDef.BuildRoleAssignments(target2);
			PsychicRitualCandidatePool candidatePool = psychicRitualDef.FindCandidatePool();
			Map currentMap = Find.CurrentMap;
			psychicRitualDef.InitializeCast(currentMap);
			Find.WindowStack.Add(new Dialog_BeginPsychicRitual(psychicRitualDef, candidatePool, assignments, currentMap));
		}

		// Token: 0x04002E31 RID: 11825
		private static readonly List<PsychicRitualDef_InvocationCircle> tmpVisibleRituals = new List<PsychicRitualDef_InvocationCircle>(16);
	}
}
