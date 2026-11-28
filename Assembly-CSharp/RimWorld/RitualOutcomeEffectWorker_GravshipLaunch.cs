using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016EE RID: 5870
	public class RitualOutcomeEffectWorker_GravshipLaunch : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x060092CC RID: 37580 RVA: 0x0035A414 File Offset: 0x00358614
		public RitualOutcomeEffectWorker_GravshipLaunch()
		{
		}

		// Token: 0x060092CD RID: 37581 RVA: 0x0035A41C File Offset: 0x0035861C
		public RitualOutcomeEffectWorker_GravshipLaunch(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x060092CE RID: 37582 RVA: 0x0035A428 File Offset: 0x00358628
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			if (progress < 1f)
			{
				Messages.Message("GravshipLaunchInterrupted".Translate(), MessageTypeDefOf.NegativeEvent, true);
				return;
			}
			try
			{
				Thing thing = jobRitual.selectedTarget.Thing;
				CompPilotConsole consoleComp = (thing != null) ? thing.TryGetComp<CompPilotConsole>() : null;
				float quality = base.GetQuality(jobRitual, progress);
				consoleComp.engine.launchInfo = new LaunchInfo
				{
					pilot = jobRitual.PawnWithRole("pilot"),
					copilot = jobRitual.PawnWithRole("copilot"),
					quality = quality,
					doNegativeOutcome = Rand.Chance(GravshipUtility.NegativeLandingOutcomeFromQuality(quality))
				};
				if (jobRitual.Map.listerThings.AnyThingWithDef(ThingDefOf.GravAnchor))
				{
					consoleComp.StartChoosingDestination_NewTemp(true);
				}
				else
				{
					GravshipUtility.PreLaunchConfirmation(consoleComp.engine, delegate
					{
						consoleComp.StartChoosingDestination_NewTemp(true);
					});
				}
			}
			catch (Exception ex)
			{
				string str = "Error launching gravship: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}
	}
}
