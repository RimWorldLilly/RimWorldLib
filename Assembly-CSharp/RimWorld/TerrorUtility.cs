using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FD8 RID: 8152
	public static class TerrorUtility
	{
		// Token: 0x0600CEDB RID: 52955 RVA: 0x004C2E8A File Offset: 0x004C108A
		public static IEnumerable<Thought_MemoryObservationTerror> GetTerrorThoughts(Pawn pawn)
		{
			TerrorUtility.<GetTerrorThoughts>d__3 <GetTerrorThoughts>d__ = new TerrorUtility.<GetTerrorThoughts>d__3(-2);
			<GetTerrorThoughts>d__.<>3__pawn = pawn;
			return <GetTerrorThoughts>d__;
		}

		// Token: 0x0600CEDC RID: 52956 RVA: 0x004C2E9A File Offset: 0x004C109A
		public static IEnumerable<Thought_MemoryObservationTerror> TakeTopTerrorThoughts(IEnumerable<Thought_MemoryObservationTerror> thoughts)
		{
			return (from t in thoughts
			orderby t.intensity descending
			select t).Take(3);
		}

		// Token: 0x0600CEDD RID: 52957 RVA: 0x004C2EC8 File Offset: 0x004C10C8
		public static float GetTerrorLevel(this Pawn pawn)
		{
			int num = 0;
			if (!pawn.IsSlave)
			{
				return (float)num;
			}
			foreach (Thought_MemoryObservationTerror thought_MemoryObservationTerror in TerrorUtility.GetTerrorThoughts(pawn))
			{
				num += thought_MemoryObservationTerror.intensity;
			}
			return (float)Mathf.Min(num, 100) / 100f;
		}

		// Token: 0x0600CEDE RID: 52958 RVA: 0x004C2F34 File Offset: 0x004C1134
		public static void RemoveAllTerrorThoughts(Pawn pawn)
		{
			foreach (Thought_MemoryObservationTerror th in TerrorUtility.GetTerrorThoughts(pawn))
			{
				pawn.needs.mood.thoughts.memories.RemoveMemory(th);
			}
		}

		// Token: 0x0600CEDF RID: 52959 RVA: 0x004C2F98 File Offset: 0x004C1198
		public static bool TryCreateTerrorThought(Thing thing, out Thought_MemoryObservationTerror thought)
		{
			thought = null;
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			float statValue = thing.GetStatValue(StatDefOf.TerrorSource, true, -1);
			if (statValue <= 0f)
			{
				return false;
			}
			thought = (Thought_MemoryObservationTerror)ThoughtMaker.MakeThought(ThoughtDefOf.ObservedTerror);
			thought.Target = thing;
			thought.intensity = (int)statValue;
			return true;
		}

		// Token: 0x040083A1 RID: 33697
		private const int MaxTerror = 100;

		// Token: 0x040083A2 RID: 33698
		private const int TerrorThoughtsToTake = 3;

		// Token: 0x040083A3 RID: 33699
		public static SimpleCurve SuppressionFallRateOverTerror = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(25f, -15f),
				true
			},
			{
				new CurvePoint(50f, -25f),
				true
			},
			{
				new CurvePoint(100f, -45f),
				true
			}
		};
	}
}
