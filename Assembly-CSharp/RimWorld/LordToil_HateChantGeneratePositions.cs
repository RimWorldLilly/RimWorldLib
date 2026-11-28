using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CDE RID: 3294
	public class LordToil_HateChantGeneratePositions : LordToil
	{
		// Token: 0x17001040 RID: 4160
		// (get) Token: 0x06005D28 RID: 23848 RVA: 0x00222B42 File Offset: 0x00220D42
		protected LordToilData_HateChantGeneratePositions Data
		{
			get
			{
				return (LordToilData_HateChantGeneratePositions)this.data;
			}
		}

		// Token: 0x17001041 RID: 4161
		// (get) Token: 0x06005D29 RID: 23849 RVA: 0x00222B4F File Offset: 0x00220D4F
		public List<PsychicRitualParticipant> Participants
		{
			get
			{
				return this.Data.chanters;
			}
		}

		// Token: 0x06005D2A RID: 23850 RVA: 0x00222B5C File Offset: 0x00220D5C
		public LordToil_HateChantGeneratePositions(IEnumerable<PsychicRitualParticipant> chanterPositionPairs, string readySignal)
		{
			PsychicRitualParticipant[] collection = (chanterPositionPairs as PsychicRitualParticipant[]) ?? chanterPositionPairs.ToArray<PsychicRitualParticipant>();
			this.data = new LordToilData_HateChantGeneratePositions();
			this.Data.chanters = new List<PsychicRitualParticipant>(collection);
			this.Data.foundPositions = new HashSet<IntVec3>();
			this.ReadySignal = readySignal;
		}

		// Token: 0x06005D2B RID: 23851 RVA: 0x00222BC4 File Offset: 0x00220DC4
		public override void UpdateAllDuties()
		{
			foreach (PsychicRitualParticipant psychicRitualParticipant in this.Data.chanters)
			{
				Pawn pawn;
				IntVec3 intVec;
				psychicRitualParticipant.Deconstruct(out pawn, out intVec);
				Pawn pawn2 = pawn;
				IntVec3 c = intVec;
				if (((pawn2 != null) ? pawn2.mindState : null) != null)
				{
					pawn2.mindState.duty = new PawnDuty(DutyDefOf.Idle, c, -1f);
				}
			}
		}

		// Token: 0x06005D2C RID: 23852 RVA: 0x00222C50 File Offset: 0x00220E50
		public override void LordToilTick()
		{
			base.LordToilTick();
			int count = this.Data.foundPositions.Count;
			if (count == this.Data.chanters.Count)
			{
				if (DebugViewSettings.drawHateChanterPositions)
				{
					Material mat = DebugSolidColorMats.MaterialOf(Color.magenta * new Color(1f, 1f, 1f, 0.4f));
					foreach (PsychicRitualParticipant psychicRitualParticipant in this.Participants)
					{
						base.Map.debugDrawer.FlashCell(psychicRitualParticipant.location, mat, null, 100000);
					}
				}
				Find.SignalManager.SendSignal(new Signal(this.ReadySignal, false));
				return;
			}
			if (count > this.Data.chanters.Count)
			{
				Log.Error("Generated more positions than chanters.");
			}
			if (base.Map.IsHashIntervalTick(this.GenerationInterval))
			{
				using (new ProfilerBlock("HateChantersBatch"))
				{
					this.ProcessBatch(count);
				}
				if (this.Data.foundPositions.Count == count)
				{
					Log.Error("No positions found in this batch, something is wrong.");
				}
			}
		}

		// Token: 0x06005D2D RID: 23853 RVA: 0x00222DAC File Offset: 0x00220FAC
		private void ProcessBatch(int foundPositionsCount)
		{
			int num = Mathf.Min(5, this.Data.chanters.Count - foundPositionsCount);
			for (int i = 0; i < num; i++)
			{
				PsychicRitualParticipant psychicRitualParticipant = this.Data.chanters[foundPositionsCount + i];
				IntVec3 position = psychicRitualParticipant.pawn.Position;
				IntVec3 intVec = this.FindChantingPosition(position, 4, 5, 2000);
				if (intVec != IntVec3.Invalid)
				{
					psychicRitualParticipant.location = intVec;
					if (!this.Data.foundPositions.Add(intVec))
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Tried adding pos to found pos twice: ");
						defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(intVec);
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
				else
				{
					IntVec3 intVec2 = CellFinder.RandomClosewalkCellNear(position, base.Map, 5, null);
					if (this.Data.foundPositions.Add(intVec2))
					{
						psychicRitualParticipant.location = intVec2;
					}
					else
					{
						psychicRitualParticipant.location = position;
						this.Data.foundPositions.Add(position);
					}
				}
			}
		}

		// Token: 0x06005D2E RID: 23854 RVA: 0x00222EB4 File Offset: 0x002210B4
		private IntVec3 FindChantingPosition(IntVec3 start, int edificeCheckRadius, int minEdificeCount, int maxIterations)
		{
			LordToil_HateChantGeneratePositions.Frontier.Clear();
			LordToil_HateChantGeneratePositions.Visited.Clear();
			int num = 0;
			LordToil_HateChantGeneratePositions.Frontier.Enqueue(start);
			LordToil_HateChantGeneratePositions.Visited.Add(start);
			while (LordToil_HateChantGeneratePositions.Frontier.Count > 0)
			{
				IntVec3 a = LordToil_HateChantGeneratePositions.Frontier.Dequeue();
				for (int i = 0; i < 4; i++)
				{
					IntVec3 intVec = a + GenAdj.CardinalDirections[i];
					if (this.ValidateChantingPosition(intVec, edificeCheckRadius, minEdificeCount, LordToil_HateChantGeneratePositions.Visited))
					{
						return intVec;
					}
					LordToil_HateChantGeneratePositions.Frontier.Enqueue(intVec);
					LordToil_HateChantGeneratePositions.Visited.Add(intVec);
				}
				num++;
				if (num > maxIterations)
				{
					return IntVec3.Invalid;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06005D2F RID: 23855 RVA: 0x00222F64 File Offset: 0x00221164
		private bool ValidateChantingPosition(IntVec3 validatedPos, int edificeCheckRadius, int minEdificeCount, HashSet<IntVec3> visited)
		{
			if (!validatedPos.InBounds(base.Map) || visited.Contains(validatedPos) || this.Data.foundPositions.Contains(validatedPos))
			{
				return false;
			}
			if ((!base.Map.TileInfo.AllowRoofedEdgeWalkIn && validatedPos.Roofed(base.Map)) || !validatedPos.Standable(base.Map) || base.Map.avoidGrid[validatedPos] != 0 || validatedPos.GetFirstPawn(base.Map) != null)
			{
				return false;
			}
			District district = validatedPos.GetDistrict(base.Map, RegionType.Set_Passable);
			if (!district.Room.TouchesMapEdge || !district.Room.PsychologicallyOutdoors)
			{
				return false;
			}
			int num = 0;
			using (CellRect.Enumerator enumerator = CellRect.CenteredOn(validatedPos, edificeCheckRadius).ClipInsideMap(base.Map).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetEdifice(base.Map) != null)
					{
						num++;
					}
					if (num == minEdificeCount)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x040034FF RID: 13567
		private static readonly Queue<IntVec3> Frontier = new Queue<IntVec3>();

		// Token: 0x04003500 RID: 13568
		private static readonly HashSet<IntVec3> Visited = new HashSet<IntVec3>();

		// Token: 0x04003501 RID: 13569
		private const int PositionsPerBatch = 5;

		// Token: 0x04003502 RID: 13570
		private readonly int GenerationInterval = 0.2f.SecondsToTicks();

		// Token: 0x04003503 RID: 13571
		private readonly string ReadySignal;
	}
}
