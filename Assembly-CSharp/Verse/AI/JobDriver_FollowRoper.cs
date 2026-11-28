using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x020007FF RID: 2047
	public class JobDriver_FollowRoper : JobDriver
	{
		// Token: 0x06004093 RID: 16531 RVA: 0x0019B6CE File Offset: 0x001998CE
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06004094 RID: 16532 RVA: 0x0019B6D1 File Offset: 0x001998D1
		protected override IEnumerable<Toil> MakeNewToils()
		{
			JobDriver_FollowRoper.<MakeNewToils>d__2 <MakeNewToils>d__ = new JobDriver_FollowRoper.<MakeNewToils>d__2(-2);
			<MakeNewToils>d__.<>4__this = this;
			return <MakeNewToils>d__;
		}

		// Token: 0x06004095 RID: 16533 RVA: 0x0019B6E4 File Offset: 0x001998E4
		private void FollowRoper(Pawn roper)
		{
			LocalTargetInfo localTargetInfo = new LocalTargetInfo(roper);
			PathEndMode peMode = PathEndMode.Touch;
			if (roper.Position.InHorDistOf(this.pawn.Position, 7.2f) && roper.pather.Moving && roper.pather.curPath != null)
			{
				int marchingPos = Mathf.Abs(roper.roping.Ropees.IndexOf(this.pawn));
				IntVec3 c = this.MarchingOrder(roper, marchingPos);
				if (c.IsValid)
				{
					localTargetInfo = c;
					peMode = PathEndMode.OnCell;
				}
			}
			if (!this.pawn.pather.Moving || this.pawn.pather.Destination != localTargetInfo)
			{
				this.pawn.pather.StartPath(localTargetInfo, peMode);
			}
		}

		// Token: 0x06004096 RID: 16534 RVA: 0x0019B7B0 File Offset: 0x001999B0
		private IntVec3 MarchingOrder(Pawn roper, int marchingPos)
		{
			PawnPath curPath = roper.pather.curPath;
			if (curPath.NodesLeftCount <= 0)
			{
				return IntVec3.Invalid;
			}
			Map map = this.pawn.Map;
			int num = -2 - marchingPos % 4;
			num = Mathf.Clamp(num, -curPath.NodesConsumedCount, curPath.NodesLeftCount);
			IntVec3 intVec = curPath.Peek(num);
			int num2 = Mathf.Abs(this.pawn.HashOffset()) % 3;
			if (num + 1 < curPath.NodesLeftCount && num2 != 0)
			{
				IntVec3 orig = curPath.Peek(num + 1) - intVec;
				IntVec3 intVec2 = (num2 == 1) ? (intVec + orig.RotatedBy(Rot4.East)) : (intVec + orig.RotatedBy(Rot4.West));
				if (intVec2.InBounds(map) && intVec2.Standable(map) && intVec2.GetDistrict(map, RegionType.Set_Passable) == intVec.GetDistrict(map, RegionType.Set_Passable))
				{
					intVec = intVec2;
				}
			}
			return intVec;
		}

		// Token: 0x06004097 RID: 16535 RVA: 0x0019B898 File Offset: 0x00199A98
		private void GotoStandCell(IntVec3 standCell)
		{
			if (!this.pawn.pather.Moving || this.pawn.pather.Destination != standCell)
			{
				this.pawn.pather.StartPath(standCell, PathEndMode.OnCell);
			}
		}

		// Token: 0x06004098 RID: 16536 RVA: 0x0019B8EB File Offset: 0x00199AEB
		public override bool IsContinuation(Job j)
		{
			return this.job.GetTarget(TargetIndex.A) == j.GetTarget(TargetIndex.A);
		}

		// Token: 0x04002AE6 RID: 10982
		private const TargetIndex RoperInd = TargetIndex.A;
	}
}
