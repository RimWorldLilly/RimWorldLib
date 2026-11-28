using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E01 RID: 7681
	public static class LessonAutoActivator
	{
		// Token: 0x170024DA RID: 9434
		// (get) Token: 0x0600C79B RID: 51099 RVA: 0x0049481E File Offset: 0x00492A1E
		private static float SecondsSinceLesson
		{
			get
			{
				return LessonAutoActivator.timeSinceLastLesson;
			}
		}

		// Token: 0x170024DB RID: 9435
		// (get) Token: 0x0600C79C RID: 51100 RVA: 0x00494825 File Offset: 0x00492A25
		private static float RelaxDesire
		{
			get
			{
				return 100f - LessonAutoActivator.SecondsSinceLesson * 0.11111111f;
			}
		}

		// Token: 0x0600C79D RID: 51101 RVA: 0x00494838 File Offset: 0x00492A38
		public static void Reset()
		{
			LessonAutoActivator.alertingConcepts.Clear();
		}

		// Token: 0x0600C79E RID: 51102 RVA: 0x00494844 File Offset: 0x00492A44
		public static void TeachOpportunity(ConceptDef conc, OpportunityType opp)
		{
			LessonAutoActivator.TeachOpportunity(conc, null, opp);
		}

		// Token: 0x0600C79F RID: 51103 RVA: 0x00494850 File Offset: 0x00492A50
		public static void TeachOpportunity(ConceptDef conc, Thing subject, OpportunityType opp)
		{
			if (!TutorSystem.AdaptiveTrainingEnabled || PlayerKnowledgeDatabase.IsComplete(conc))
			{
				return;
			}
			float value = 999f;
			switch (opp)
			{
			case OpportunityType.GoodToKnow:
				value = 60f;
				break;
			case OpportunityType.Important:
				value = 80f;
				break;
			case OpportunityType.Critical:
				value = 100f;
				break;
			default:
				Log.Error("Unknown need");
				break;
			}
			LessonAutoActivator.opportunities[conc] = value;
			if (opp >= OpportunityType.Important || Find.Tutor.learningReadout.ActiveConceptsCount < 4)
			{
				LessonAutoActivator.TryInitiateLesson(conc);
			}
		}

		// Token: 0x0600C7A0 RID: 51104 RVA: 0x004948D1 File Offset: 0x00492AD1
		public static void Notify_KnowledgeDemonstrated(ConceptDef conc)
		{
			if (PlayerKnowledgeDatabase.IsComplete(conc))
			{
				LessonAutoActivator.opportunities[conc] = 0f;
			}
		}

		// Token: 0x0600C7A1 RID: 51105 RVA: 0x004948EC File Offset: 0x00492AEC
		public static void LessonAutoActivatorUpdate()
		{
			if (!TutorSystem.AdaptiveTrainingEnabled || Current.Game == null || Find.Tutor.learningReadout.ShowAllMode)
			{
				return;
			}
			LessonAutoActivator.timeSinceLastLesson += RealTime.realDeltaTime;
			if (Current.ProgramState == ProgramState.Playing && (Time.timeSinceLevelLoad < 8f || Find.WindowStack.SecondsSinceClosedGameStartDialog < 8f || Find.TickManager.NotPlaying))
			{
				return;
			}
			for (int i = LessonAutoActivator.alertingConcepts.Count - 1; i >= 0; i--)
			{
				if (PlayerKnowledgeDatabase.IsComplete(LessonAutoActivator.alertingConcepts[i]))
				{
					LessonAutoActivator.alertingConcepts.RemoveAt(i);
				}
			}
			if (Time.frameCount % 15 == 0 && Find.ActiveLesson.Current == null)
			{
				for (int j = 0; j < DefDatabase<ConceptDef>.AllDefsListForReading.Count; j++)
				{
					ConceptDef conceptDef = DefDatabase<ConceptDef>.AllDefsListForReading[j];
					if (!PlayerKnowledgeDatabase.IsComplete(conceptDef))
					{
						float num = PlayerKnowledgeDatabase.GetKnowledge(conceptDef);
						num -= 0.00015f * Time.deltaTime * 15f;
						if (num < 0f)
						{
							num = 0f;
						}
						PlayerKnowledgeDatabase.SetKnowledge(conceptDef, num);
						if (conceptDef.opportunityDecays)
						{
							float num2 = LessonAutoActivator.GetOpportunity(conceptDef);
							num2 -= 0.4f * Time.deltaTime * 15f;
							if (num2 < 0f)
							{
								num2 = 0f;
							}
							LessonAutoActivator.opportunities[conceptDef] = num2;
						}
					}
				}
				if (Find.Tutor.learningReadout.ActiveConceptsCount < 3)
				{
					ConceptDef conceptDef2 = LessonAutoActivator.MostDesiredConcept();
					if (conceptDef2 != null)
					{
						float desire = LessonAutoActivator.GetDesire(conceptDef2);
						if (desire > 0.1f && LessonAutoActivator.RelaxDesire < desire)
						{
							LessonAutoActivator.TryInitiateLesson(conceptDef2);
							return;
						}
					}
				}
				else
				{
					LessonAutoActivator.SetLastLessonTimeToNow();
				}
			}
		}

		// Token: 0x0600C7A2 RID: 51106 RVA: 0x00494A94 File Offset: 0x00492C94
		private static ConceptDef MostDesiredConcept()
		{
			float num = -9999f;
			ConceptDef result = null;
			List<ConceptDef> allDefsListForReading = DefDatabase<ConceptDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ConceptDef conceptDef = allDefsListForReading[i];
				float desire = LessonAutoActivator.GetDesire(conceptDef);
				if (desire > num && (!conceptDef.needsOpportunity || LessonAutoActivator.GetOpportunity(conceptDef) >= 0.1f) && PlayerKnowledgeDatabase.GetKnowledge(conceptDef) <= 0.15f)
				{
					num = desire;
					result = conceptDef;
				}
			}
			return result;
		}

		// Token: 0x0600C7A3 RID: 51107 RVA: 0x00494B04 File Offset: 0x00492D04
		private static float GetDesire(ConceptDef conc)
		{
			if (PlayerKnowledgeDatabase.IsComplete(conc))
			{
				return 0f;
			}
			if (Find.Tutor.learningReadout.IsActive(conc))
			{
				return 0f;
			}
			if (Current.ProgramState != conc.gameMode)
			{
				return 0f;
			}
			if (conc.needsOpportunity && LessonAutoActivator.GetOpportunity(conc) < 0.1f)
			{
				return 0f;
			}
			return (0f + conc.priority + LessonAutoActivator.GetOpportunity(conc) / 100f * 60f) * (1f - PlayerKnowledgeDatabase.GetKnowledge(conc));
		}

		// Token: 0x0600C7A4 RID: 51108 RVA: 0x00494B94 File Offset: 0x00492D94
		private static float GetOpportunity(ConceptDef conc)
		{
			float result;
			if (LessonAutoActivator.opportunities.TryGetValue(conc, out result))
			{
				return result;
			}
			LessonAutoActivator.opportunities[conc] = 0f;
			return 0f;
		}

		// Token: 0x0600C7A5 RID: 51109 RVA: 0x00494BC7 File Offset: 0x00492DC7
		private static void TryInitiateLesson(ConceptDef conc)
		{
			if (Find.Tutor.learningReadout.TryActivateConcept(conc))
			{
				LessonAutoActivator.SetLastLessonTimeToNow();
			}
		}

		// Token: 0x0600C7A6 RID: 51110 RVA: 0x00494BE0 File Offset: 0x00492DE0
		private static void SetLastLessonTimeToNow()
		{
			LessonAutoActivator.timeSinceLastLesson = 0f;
		}

		// Token: 0x0600C7A7 RID: 51111 RVA: 0x00494BEC File Offset: 0x00492DEC
		public static void Notify_TutorialEnding()
		{
			LessonAutoActivator.SetLastLessonTimeToNow();
		}

		// Token: 0x0600C7A8 RID: 51112 RVA: 0x00494BF4 File Offset: 0x00492DF4
		public static string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("RelaxDesire: " + LessonAutoActivator.RelaxDesire.ToString());
			foreach (ConceptDef conceptDef in from co in DefDatabase<ConceptDef>.AllDefs
			orderby LessonAutoActivator.GetDesire(co) descending
			select co)
			{
				if (PlayerKnowledgeDatabase.IsComplete(conceptDef))
				{
					stringBuilder.AppendLine(conceptDef.defName + " complete");
				}
				else
				{
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						conceptDef.defName,
						"\n   know ",
						PlayerKnowledgeDatabase.GetKnowledge(conceptDef).ToString("F3"),
						"\n   need ",
						LessonAutoActivator.opportunities[conceptDef].ToString("F3"),
						"\n   des ",
						LessonAutoActivator.GetDesire(conceptDef).ToString("F3")
					}));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600C7A9 RID: 51113 RVA: 0x00494D28 File Offset: 0x00492F28
		public static void DebugForceInitiateBestLessonNow()
		{
			LessonAutoActivator.TryInitiateLesson((from def in DefDatabase<ConceptDef>.AllDefs
			orderby LessonAutoActivator.GetDesire(def) descending
			select def).First<ConceptDef>());
		}

		// Token: 0x040071D0 RID: 29136
		private static Dictionary<ConceptDef, float> opportunities = new Dictionary<ConceptDef, float>();

		// Token: 0x040071D1 RID: 29137
		private static float timeSinceLastLesson = 10000f;

		// Token: 0x040071D2 RID: 29138
		private static List<ConceptDef> alertingConcepts = new List<ConceptDef>();

		// Token: 0x040071D3 RID: 29139
		private const float MapStartGracePeriod = 8f;

		// Token: 0x040071D4 RID: 29140
		private const float KnowledgeDecayRate = 0.00015f;

		// Token: 0x040071D5 RID: 29141
		private const float OpportunityDecayRate = 0.4f;

		// Token: 0x040071D6 RID: 29142
		private const float OpportunityMaxDesireAdd = 60f;

		// Token: 0x040071D7 RID: 29143
		private const int CheckInterval = 15;

		// Token: 0x040071D8 RID: 29144
		private const float MaxLessonInterval = 900f;
	}
}
