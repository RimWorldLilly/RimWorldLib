using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CA8 RID: 3240
	public static class PsychicRitualUtility
	{
		// Token: 0x06005BB9 RID: 23481 RVA: 0x0021B456 File Offset: 0x00219656
		public static IEnumerable<Thing> GetPsychicRitualSpotsAffectedByThing(Map map, CellRect thingRect)
		{
			PsychicRitualUtility.<GetPsychicRitualSpotsAffectedByThing>d__0 <GetPsychicRitualSpotsAffectedByThing>d__ = new PsychicRitualUtility.<GetPsychicRitualSpotsAffectedByThing>d__0(-2);
			<GetPsychicRitualSpotsAffectedByThing>d__.<>3__map = map;
			<GetPsychicRitualSpotsAffectedByThing>d__.<>3__thingRect = thingRect;
			return <GetPsychicRitualSpotsAffectedByThing>d__;
		}

		// Token: 0x06005BBA RID: 23482 RVA: 0x0021B470 File Offset: 0x00219670
		public static void DrawPsychicRitualSpotsAffectedByThingOverlay(Map map, ThingDef def, IntVec3 pos, Rot4 rotation)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			int num = 0;
			CellRect thingRect = GenAdj.OccupiedRect(pos, rotation, def.size);
			foreach (Thing t in PsychicRitualUtility.GetPsychicRitualSpotsAffectedByThing(map, thingRect))
			{
				if (num++ > 10)
				{
					break;
				}
				GenDraw.DrawLineBetween(GenThing.TrueCenter(pos, rotation, def.size, def.Altitude), t.TrueCenter(), SimpleColor.Red, 0.2f);
			}
		}

		// Token: 0x06005BBB RID: 23483 RVA: 0x0021B500 File Offset: 0x00219700
		public static SkillDef GetPhilophagySkillAndXpTransfer(Pawn invoker, Pawn target, float xpTransferPercent, out float xpTransfer)
		{
			IOrderedEnumerable<SkillRecord> orderedEnumerable = from s in target.skills.skills
			where !s.TotallyDisabled && !invoker.skills.GetSkill(s.def).TotallyDisabled
			orderby s.XpTotalEarned descending
			select s;
			if (!orderedEnumerable.Any<SkillRecord>())
			{
				xpTransfer = 0f;
				return null;
			}
			PsychicRitualUtility.tmpCandidateSkills.Clear();
			SkillRecord skillRecord = orderedEnumerable.First<SkillRecord>();
			foreach (SkillRecord skillRecord2 in orderedEnumerable)
			{
				if (skillRecord2 == skillRecord)
				{
					PsychicRitualUtility.tmpCandidateSkills.Add(skillRecord2);
				}
				else if (Mathf.Abs(skillRecord2.XpTotalEarned - skillRecord.XpTotalEarned) < 0.01f)
				{
					PsychicRitualUtility.tmpCandidateSkills.Add(skillRecord2);
				}
			}
			SkillDef def = PsychicRitualUtility.tmpCandidateSkills[0].def;
			foreach (SkillRecord skillRecord3 in PsychicRitualUtility.tmpCandidateSkills)
			{
				if (invoker.skills.GetSkill(def).XpTotalEarned < invoker.skills.GetSkill(skillRecord3.def).XpTotalEarned)
				{
					def = skillRecord3.def;
				}
			}
			float xpTotalEarned = target.skills.GetSkill(def).XpTotalEarned;
			xpTransfer = xpTotalEarned * xpTransferPercent;
			return def;
		}

		// Token: 0x06005BBC RID: 23484 RVA: 0x0021B694 File Offset: 0x00219894
		public static void RegisterAsExecutionIfPrisoner(Pawn target, Pawn invoker)
		{
			if (!target.IsPrisonerOfColony)
			{
				return;
			}
			ThoughtUtility.GiveThoughtsForPawnExecuted(target, invoker, PawnExecutionKind.GenericBrutal);
			TaleRecorder.RecordTale(TaleDefOf.ExecutedPrisoner, new object[]
			{
				invoker,
				target
			});
		}

		// Token: 0x06005BBD RID: 23485 RVA: 0x0021B6C0 File Offset: 0x002198C0
		public static void AddPsychicRitualGuiltToPawns(PsychicRitualDef ritualDef, IEnumerable<Pawn> pawns)
		{
			foreach (Pawn pawn in pawns)
			{
				Thought_PsychicRitualGuilt thought_PsychicRitualGuilt = (Thought_PsychicRitualGuilt)ThoughtMaker.MakeThought(ThoughtDefOf.PsychicRitualGuilt);
				thought_PsychicRitualGuilt.ritualDef = ritualDef;
				Pawn_NeedsTracker needs = pawn.needs;
				if (needs != null)
				{
					Need_Mood mood = needs.mood;
					if (mood != null)
					{
						ThoughtHandler thoughts = mood.thoughts;
						if (thoughts != null)
						{
							MemoryThoughtHandler memories = thoughts.memories;
							if (memories != null)
							{
								memories.TryGainMemory(thought_PsychicRitualGuilt, null);
							}
						}
					}
				}
			}
		}

		// Token: 0x0400347C RID: 13436
		private static List<SkillRecord> tmpCandidateSkills = new List<SkillRecord>();
	}
}
