using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E20 RID: 7712
	public static class TutorSystem
	{
		// Token: 0x17002502 RID: 9474
		// (get) Token: 0x0600C85D RID: 51293 RVA: 0x0049722D File Offset: 0x0049542D
		public static bool TutorialMode
		{
			get
			{
				return Find.Storyteller != null && Find.Storyteller.def != null && Find.Storyteller.def.tutorialMode;
			}
		}

		// Token: 0x17002503 RID: 9475
		// (get) Token: 0x0600C85E RID: 51294 RVA: 0x00497253 File Offset: 0x00495453
		public static bool AdaptiveTrainingEnabled
		{
			get
			{
				return Prefs.AdaptiveTrainingEnabled && (Find.Storyteller == null || Find.Storyteller.def == null || !Find.Storyteller.def.disableAdaptiveTraining);
			}
		}

		// Token: 0x0600C85F RID: 51295 RVA: 0x00497285 File Offset: 0x00495485
		public static void Notify_Event(string eventTag, IntVec3 cell)
		{
			TutorSystem.Notify_Event(new EventPack(eventTag, cell));
		}

		// Token: 0x0600C860 RID: 51296 RVA: 0x00497294 File Offset: 0x00495494
		public static void Notify_Event(EventPack ep)
		{
			if (!TutorSystem.TutorialMode)
			{
				return;
			}
			if (DebugViewSettings.logTutor)
			{
				string str = "Notify_Event: ";
				EventPack eventPack = ep;
				Log.Message(str + eventPack.ToString());
			}
			if (Current.Game == null)
			{
				return;
			}
			Lesson lesson = Find.ActiveLesson.Current;
			if (Find.ActiveLesson.Current != null)
			{
				Find.ActiveLesson.Current.Notify_Event(ep);
			}
			foreach (InstructionDef instructionDef in DefDatabase<InstructionDef>.AllDefs)
			{
				if (instructionDef.eventTagInitiate == ep.Tag && (instructionDef.eventTagInitiateSource == null || (lesson != null && instructionDef.eventTagInitiateSource == lesson.Instruction)) && (TutorSystem.TutorialMode || !instructionDef.tutorialModeOnly))
				{
					Find.ActiveLesson.Activate(instructionDef);
					break;
				}
			}
		}

		// Token: 0x0600C861 RID: 51297 RVA: 0x00497380 File Offset: 0x00495580
		public static bool AllowAction(EventPack ep)
		{
			if (!TutorSystem.TutorialMode)
			{
				return true;
			}
			if (DebugViewSettings.logTutor)
			{
				string str = "AllowAction: ";
				EventPack eventPack = ep;
				Log.Message(str + eventPack.ToString());
			}
			if (ep.Cells != null && ep.Cells.Count<IntVec3>() == 1)
			{
				return TutorSystem.AllowAction(new EventPack(ep.Tag, ep.Cells.First<IntVec3>()));
			}
			if (Find.ActiveLesson.Current != null)
			{
				AcceptanceReport acceptanceReport = Find.ActiveLesson.Current.AllowAction(ep);
				if (!acceptanceReport.Accepted)
				{
					Messages.Message((!acceptanceReport.Reason.NullOrEmpty()) ? acceptanceReport.Reason : Find.ActiveLesson.Current.DefaultRejectInputMessage, MessageTypeDefOf.RejectInput, false);
					return false;
				}
			}
			return true;
		}
	}
}
