using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001624 RID: 5668
	public static class IdeoDevelopmentUtility
	{
		// Token: 0x06008DC0 RID: 36288 RVA: 0x0033B964 File Offset: 0x00339B64
		public static int DevelopmentPointsForQuestSuccess(Ideo ideo, QuestScriptDef root)
		{
			int num = 0;
			if (root.successHistoryEvent == null || !ideo.Fluid)
			{
				return num;
			}
			List<Precept> preceptsListForReading = ideo.PreceptsListForReading;
			for (int i = 0; i < preceptsListForReading.Count; i++)
			{
				for (int j = 0; j < preceptsListForReading[i].def.comps.Count; j++)
				{
					PreceptComp_DevelopmentPoints preceptComp_DevelopmentPoints = preceptsListForReading[i].def.comps[j] as PreceptComp_DevelopmentPoints;
					if (preceptComp_DevelopmentPoints != null && preceptComp_DevelopmentPoints.eventDef == root.successHistoryEvent)
					{
						num += preceptComp_DevelopmentPoints.points;
					}
				}
			}
			return num;
		}

		// Token: 0x06008DC1 RID: 36289 RVA: 0x0033B9FC File Offset: 0x00339BFC
		public static SimpleCurve GetDevelopmentPointsOverOutcomeIndexCurveForRitual(Ideo ideo, Precept_Ritual ritual)
		{
			RitualOutcomeEffectWorker_FromQuality ritualOutcomeEffectWorker_FromQuality = ritual.outcomeEffect as RitualOutcomeEffectWorker_FromQuality;
			if (ritualOutcomeEffectWorker_FromQuality == null || !ritualOutcomeEffectWorker_FromQuality.GivesDevelopmentPoints)
			{
				return null;
			}
			if (ritual.isAnytime || ritual.def == PreceptDefOf.Funeral)
			{
				return IdeoDevelopmentUtility.AnytimeAndFuneralRitualDevelopmentPointsOverOutcomeIndex;
			}
			if (ritual.IsDateTriggered)
			{
				int num = Mathf.Clamp(ideo.PreceptsListForReading.Count(delegate(Precept p)
				{
					Precept_Ritual precept_Ritual = p as Precept_Ritual;
					return precept_Ritual != null && precept_Ritual.IsDateTriggered;
				}) - 1, 0, IdeoDevelopmentUtility.DateRitualDevelopmentCurvesByCount.Length - 1);
				return IdeoDevelopmentUtility.DateRitualDevelopmentCurvesByCount[num];
			}
			return null;
		}

		// Token: 0x06008DC2 RID: 36290 RVA: 0x0033BA8C File Offset: 0x00339C8C
		public static void GetAllRitualsThatGiveDevelopmentPoints(Ideo ideo, List<Precept_Ritual> rituals)
		{
			if (!ideo.Fluid)
			{
				return;
			}
			List<Precept> preceptsListForReading = ideo.PreceptsListForReading;
			for (int i = 0; i < preceptsListForReading.Count; i++)
			{
				Precept_Ritual precept_Ritual = preceptsListForReading[i] as Precept_Ritual;
				if (precept_Ritual != null && IdeoDevelopmentUtility.GetDevelopmentPointsOverOutcomeIndexCurveForRitual(ideo, precept_Ritual) != null)
				{
					rituals.Add(precept_Ritual);
				}
			}
		}

		// Token: 0x06008DC3 RID: 36291 RVA: 0x0033BADC File Offset: 0x00339CDC
		public static void GetAllQuestSuccessEventsThatGiveDevelopmentPoints(Ideo ideo, List<HistoryEventDef> successEvents)
		{
			if (!ideo.Fluid)
			{
				return;
			}
			foreach (QuestScriptDef questScriptDef in DefDatabase<QuestScriptDef>.AllDefs)
			{
				if (!successEvents.Contains(questScriptDef.successHistoryEvent) && IdeoDevelopmentUtility.DevelopmentPointsForQuestSuccess(ideo, questScriptDef) > 0)
				{
					successEvents.Add(questScriptDef.successHistoryEvent);
				}
			}
		}

		// Token: 0x06008DC4 RID: 36292 RVA: 0x0033BB50 File Offset: 0x00339D50
		public static void ConfirmChangesToIdeo(Ideo ideo, Ideo newIdeo, Action confirmCallback)
		{
			IdeoDevelopmentUtility.toRemovePrecepts.Clear();
			IdeoDevelopmentUtility.GetPreceptsToRemove(ideo, newIdeo, IdeoDevelopmentUtility.toRemovePrecepts);
			string text = "";
			for (int i = 0; i < IdeoDevelopmentUtility.toRemovePrecepts.Count; i++)
			{
				string str;
				if (IdeoDevelopmentUtility.toRemovePrecepts[i].TryGetLostByReformingWarning(out str))
				{
					text += str;
				}
			}
			if (!text.NullOrEmpty())
			{
				text += "\n\n" + "ReformIdeoContinue".Translate();
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmCallback, false, null, WindowLayer.Dialog));
			}
			else
			{
				confirmCallback();
			}
			IdeoDevelopmentUtility.toRemovePrecepts.Clear();
		}

		// Token: 0x06008DC5 RID: 36293 RVA: 0x0033BC00 File Offset: 0x00339E00
		public static void ApplyChangesToIdeo(Ideo ideo, Ideo newIdeo)
		{
			IdeoDevelopmentUtility.toRemovePrecepts.Clear();
			IdeoDevelopmentUtility.GetPreceptsToRemove(ideo, newIdeo, IdeoDevelopmentUtility.toRemovePrecepts);
			ideo.development.Notify_PreReform(newIdeo);
			newIdeo.CopyTo(ideo);
			IdeoDevelopmentUtility.UpdateAllStyleReferences(ideo);
			ideo.development.Notify_Reformed();
			for (int i = 0; i < IdeoDevelopmentUtility.toRemovePrecepts.Count; i++)
			{
				IdeoDevelopmentUtility.toRemovePrecepts[i].Notify_RemovedByReforming();
			}
			IdeoDevelopmentUtility.toRemovePrecepts.Clear();
		}

		// Token: 0x06008DC6 RID: 36294 RVA: 0x0033BC78 File Offset: 0x00339E78
		private static void UpdateAllStyleReferences(Ideo ideo)
		{
			foreach (Map map in Find.Maps)
			{
				foreach (Building thing in map.listerBuildings.allBuildingsColonist)
				{
					IdeoDevelopmentUtility.UpdateThingStyleReference(thing, ideo);
				}
			}
		}

		// Token: 0x06008DC7 RID: 36295 RVA: 0x0033BD08 File Offset: 0x00339F08
		private static void UpdateThingStyleReference(Thing thing, Ideo ideo)
		{
			if (thing.StyleSourcePrecept == null || thing.StyleSourcePrecept.ideo != ideo)
			{
				return;
			}
			List<Precept> preceptsListForReading = ideo.PreceptsListForReading;
			Precept_ThingStyle precept_ThingStyle = (Precept_ThingStyle)preceptsListForReading.FirstOrDefault((Precept p) => p.Id == thing.StyleSourcePrecept.Id);
			if (precept_ThingStyle != null)
			{
				thing.StyleSourcePrecept = precept_ThingStyle;
			}
			else
			{
				precept_ThingStyle = (Precept_ThingStyle)preceptsListForReading.FirstOrDefault(delegate(Precept p)
				{
					Precept_ThingStyle precept_ThingStyle2 = p as Precept_ThingStyle;
					return precept_ThingStyle2 != null && precept_ThingStyle2.ThingDef == thing.def;
				});
				thing.StyleSourcePrecept = precept_ThingStyle;
			}
			if (thing.Spawned)
			{
				thing.DirtyMapMesh(thing.Map);
			}
		}

		// Token: 0x06008DC8 RID: 36296 RVA: 0x0033BDBC File Offset: 0x00339FBC
		private static void GetPreceptsToRemove(Ideo ideo, Ideo newIdeo, List<Precept> preceptsOut)
		{
			List<Precept> preceptsListForReading = ideo.PreceptsListForReading;
			List<Precept> preceptsListForReading2 = newIdeo.PreceptsListForReading;
			for (int i = 0; i < preceptsListForReading.Count; i++)
			{
				bool flag = false;
				for (int j = 0; j < preceptsListForReading2.Count; j++)
				{
					if (preceptsListForReading2[j].Id == preceptsListForReading[i].Id)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					preceptsOut.Add(preceptsListForReading[i]);
				}
			}
		}

		// Token: 0x040056E0 RID: 22240
		private static readonly SimpleCurve AnytimeAndFuneralRitualDevelopmentPointsOverOutcomeIndex = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 0f),
				true
			},
			{
				new CurvePoint(2f, 1f),
				true
			},
			{
				new CurvePoint(3f, 2f),
				true
			}
		};

		// Token: 0x040056E1 RID: 22241
		private static readonly SimpleCurve[] DateRitualDevelopmentCurvesByCount = new SimpleCurve[]
		{
			new SimpleCurve
			{
				{
					new CurvePoint(0f, 2f),
					true
				},
				{
					new CurvePoint(1f, 4f),
					true
				},
				{
					new CurvePoint(2f, 6f),
					true
				},
				{
					new CurvePoint(3f, 7f),
					true
				}
			},
			new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 2f),
					true
				},
				{
					new CurvePoint(2f, 3f),
					true
				},
				{
					new CurvePoint(3f, 4f),
					true
				}
			},
			new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 1f),
					true
				},
				{
					new CurvePoint(2f, 2f),
					true
				},
				{
					new CurvePoint(3f, 3f),
					true
				}
			},
			new SimpleCurve
			{
				{
					new CurvePoint(0f, 0f),
					true
				},
				{
					new CurvePoint(1f, 1f),
					true
				},
				{
					new CurvePoint(2f, 1f),
					true
				},
				{
					new CurvePoint(3f, 2f),
					true
				}
			},
			new SimpleCurve
			{
				{
					new CurvePoint(0f, 0f),
					true
				},
				{
					new CurvePoint(1f, 1f),
					true
				},
				{
					new CurvePoint(2f, 1f),
					true
				},
				{
					new CurvePoint(3f, 1f),
					true
				}
			}
		};

		// Token: 0x040056E2 RID: 22242
		private static List<Precept> toRemovePrecepts = new List<Precept>();
	}
}
