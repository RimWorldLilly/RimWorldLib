using System;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200084D RID: 2125
	public static class JobUtility
	{
		// Token: 0x060042CE RID: 17102 RVA: 0x001A5184 File Offset: 0x001A3384
		public static void TryStartErrorRecoverJob(Pawn pawn, string message, Exception exception = null, JobDriver concreteDriver = null)
		{
			string text = message;
			JobUtility.AppendVarsInfoToDebugMessage(pawn, ref text, concreteDriver);
			if (exception != null)
			{
				text = text + "\n" + ((exception != null) ? exception.ToString() : null);
			}
			Log.Error(text);
			if (pawn.jobs != null)
			{
				if (pawn.jobs.curJob != null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.Errored, false, true);
				}
				if (JobUtility.startingErrorRecoverJob)
				{
					Log.Error("An error occurred while starting an error recover job. We have to stop now to avoid infinite loops. This means that the pawn is now jobless which can cause further bugs. pawn=" + pawn.ToStringSafe<Pawn>());
					return;
				}
				JobUtility.startingErrorRecoverJob = true;
				try
				{
					pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Wait, 150, false), JobCondition.None, null, false, true, null, null, false, false, null, false, true, false);
				}
				finally
				{
					JobUtility.startingErrorRecoverJob = false;
				}
			}
		}

		// Token: 0x060042CF RID: 17103 RVA: 0x001A5258 File Offset: 0x001A3458
		public static string GetResolvedJobReport(string baseText, LocalTargetInfo a)
		{
			return JobUtility.GetResolvedJobReport(baseText, a, LocalTargetInfo.Invalid, LocalTargetInfo.Invalid);
		}

		// Token: 0x060042D0 RID: 17104 RVA: 0x001A526B File Offset: 0x001A346B
		public static string GetResolvedJobReport(string baseText, LocalTargetInfo a, LocalTargetInfo b)
		{
			return JobUtility.GetResolvedJobReport(baseText, a, b, LocalTargetInfo.Invalid);
		}

		// Token: 0x060042D1 RID: 17105 RVA: 0x001A527C File Offset: 0x001A347C
		public static string GetResolvedJobReport(string baseText, LocalTargetInfo a, LocalTargetInfo b, LocalTargetInfo c)
		{
			string aText;
			object aObj;
			JobUtility.<GetResolvedJobReport>g__GetText|4_0(a, out aText, out aObj);
			string bText;
			object bObj;
			JobUtility.<GetResolvedJobReport>g__GetText|4_0(b, out bText, out bObj);
			string cText;
			object cObj;
			JobUtility.<GetResolvedJobReport>g__GetText|4_0(c, out cText, out cObj);
			return JobUtility.GetResolvedJobReportRaw(baseText, aText, aObj, bText, bObj, cText, cObj);
		}

		// Token: 0x060042D2 RID: 17106 RVA: 0x001A52B8 File Offset: 0x001A34B8
		public static string GetResolvedJobReportRaw(string baseText, string aText, object aObj, string bText, object bObj, string cText, object cObj)
		{
			baseText = baseText.Formatted(aObj.Named("TargetA"), bObj.Named("TargetB"), cObj.Named("TargetC"));
			baseText = baseText.Replace("TargetA", aText);
			baseText = baseText.Replace("TargetB", bText);
			baseText = baseText.Replace("TargetC", cText);
			return baseText;
		}

		// Token: 0x060042D3 RID: 17107 RVA: 0x001A5324 File Offset: 0x001A3524
		private static void AppendVarsInfoToDebugMessage(Pawn pawn, ref string msg, JobDriver concreteDriver)
		{
			if (concreteDriver != null)
			{
				msg = string.Concat(new string[]
				{
					msg,
					" driver=",
					concreteDriver.GetType().Name,
					" (toilIndex=",
					concreteDriver.CurToilIndex.ToString(),
					")"
				});
				if (concreteDriver.job != null)
				{
					msg = msg + " driver.job=(" + concreteDriver.job.ToStringSafe<Job>() + ")";
					return;
				}
			}
			else if (pawn.jobs != null)
			{
				if (pawn.jobs.curDriver != null)
				{
					msg = string.Concat(new string[]
					{
						msg,
						" curDriver=",
						pawn.jobs.curDriver.GetType().Name,
						" (toilIndex=",
						pawn.jobs.curDriver.CurToilIndex.ToString(),
						")"
					});
				}
				if (pawn.jobs.curJob != null)
				{
					msg = msg + " curJob=(" + pawn.jobs.curJob.ToStringSafe<Job>() + ")";
				}
			}
		}

		// Token: 0x060042D4 RID: 17108 RVA: 0x001A544C File Offset: 0x001A364C
		[CompilerGenerated]
		internal static void <GetResolvedJobReport>g__GetText|4_0(LocalTargetInfo x, out string backCompatibleText, out object obj)
		{
			if (!x.IsValid)
			{
				backCompatibleText = "UnknownLower".Translate();
				obj = backCompatibleText;
				return;
			}
			if (x.HasThing && !x.ThingDiscarded)
			{
				backCompatibleText = x.Thing.LabelShort;
				obj = x.Thing;
				return;
			}
			backCompatibleText = "AreaLower".Translate();
			obj = backCompatibleText;
		}

		// Token: 0x04002B72 RID: 11122
		private static bool startingErrorRecoverJob;
	}
}
