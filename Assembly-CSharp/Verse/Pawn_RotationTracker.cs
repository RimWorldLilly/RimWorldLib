using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004E6 RID: 1254
	public class Pawn_RotationTracker : IExposable
	{
		// Token: 0x06002540 RID: 9536 RVA: 0x000E2930 File Offset: 0x000E0B30
		public Pawn_RotationTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06002541 RID: 9537 RVA: 0x000E293F File Offset: 0x000E0B3F
		public void Notify_Spawned()
		{
			this.UpdateRotation();
		}

		// Token: 0x06002542 RID: 9538 RVA: 0x000E2948 File Offset: 0x000E0B48
		public void UpdateRotation()
		{
			if (this.pawn.Destroyed)
			{
				return;
			}
			if (this.pawn.kindDef.useFixedRotation)
			{
				this.pawn.Rotation = this.pawn.kindDef.fixedRotation;
				return;
			}
			if (this.pawn.jobs.HandlingFacing)
			{
				return;
			}
			if (this.pawn.stances.stunner.Stunned && this.pawn.stances.stunner.DisableRotation)
			{
				return;
			}
			Stance_Busy stance_Busy = this.pawn.stances.curStance as Stance_Busy;
			if (stance_Busy != null && stance_Busy.focusTarg.IsValid)
			{
				if (stance_Busy.focusTarg.HasThing)
				{
					this.Face(stance_Busy.focusTarg.Thing.DrawPos);
					return;
				}
				this.FaceCell(stance_Busy.focusTarg.Cell);
				return;
			}
			else
			{
				if (!this.pawn.pather.Moving)
				{
					if (this.pawn.jobs.curJob != null)
					{
						LocalTargetInfo target = this.pawn.CurJob.GetTarget(this.pawn.jobs.curDriver.rotateToFace);
						this.FaceTarget(target);
					}
					if (this.pawn.Drafted)
					{
						this.pawn.Rotation = Rot4.South;
					}
					return;
				}
				if (this.pawn.pather.curPath == null || this.pawn.pather.curPath.NodesLeftCount < 1)
				{
					return;
				}
				this.FaceAdjacentCell(this.pawn.pather.nextCell);
				return;
			}
		}

		// Token: 0x06002543 RID: 9539 RVA: 0x000E2ADF File Offset: 0x000E0CDF
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			this.UpdateRotation();
		}

		// Token: 0x06002544 RID: 9540 RVA: 0x000E2AE8 File Offset: 0x000E0CE8
		private void FaceAdjacentCell(IntVec3 c)
		{
			if (c == this.pawn.Position)
			{
				return;
			}
			IntVec3 intVec = c - this.pawn.Position;
			if (intVec.x > 0)
			{
				this.pawn.Rotation = Rot4.East;
				return;
			}
			if (intVec.x < 0)
			{
				this.pawn.Rotation = Rot4.West;
				return;
			}
			if (intVec.z > 0)
			{
				this.pawn.Rotation = Rot4.North;
				return;
			}
			this.pawn.Rotation = Rot4.South;
		}

		// Token: 0x06002545 RID: 9541 RVA: 0x000E2B7C File Offset: 0x000E0D7C
		public void FaceCell(IntVec3 c)
		{
			if (c == this.pawn.Position)
			{
				return;
			}
			float angle = (c - this.pawn.Position).ToVector3().AngleFlat();
			this.pawn.Rotation = Pawn_RotationTracker.RotFromAngleBiased(angle);
		}

		// Token: 0x06002546 RID: 9542 RVA: 0x000E2BD0 File Offset: 0x000E0DD0
		public void Face(Vector3 p)
		{
			if (p == this.pawn.DrawPos)
			{
				return;
			}
			float angle = (p - this.pawn.DrawPos).AngleFlat();
			this.pawn.Rotation = Pawn_RotationTracker.RotFromAngleBiased(angle);
		}

		// Token: 0x06002547 RID: 9543 RVA: 0x000E2C1C File Offset: 0x000E0E1C
		public void FaceTarget(LocalTargetInfo target)
		{
			if (!target.IsValid)
			{
				return;
			}
			if (target.HasThing)
			{
				Thing thing = target.Thing.Spawned ? target.Thing : ThingOwnerUtility.GetFirstSpawnedParentThing(target.Thing);
				if (thing != null && thing.Spawned)
				{
					bool flag = false;
					IntVec3 c = default(IntVec3);
					CellRect cellRect = thing.OccupiedRect();
					for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
					{
						for (int j = cellRect.minX; j <= cellRect.maxX; j++)
						{
							if (this.pawn.Position == new IntVec3(j, 0, i))
							{
								this.Face(thing.DrawPos);
								return;
							}
						}
					}
					for (int k = cellRect.minZ; k <= cellRect.maxZ; k++)
					{
						for (int l = cellRect.minX; l <= cellRect.maxX; l++)
						{
							IntVec3 intVec = new IntVec3(l, 0, k);
							if (intVec.AdjacentToCardinal(this.pawn.Position))
							{
								this.FaceAdjacentCell(intVec);
								return;
							}
							if (intVec.AdjacentTo8Way(this.pawn.Position))
							{
								flag = true;
								c = intVec;
							}
						}
					}
					if (flag)
					{
						if (DebugViewSettings.drawPawnRotatorTarget)
						{
							this.pawn.Map.debugDrawer.FlashCell(this.pawn.Position, 0.6f, "jbthing", 50);
							GenDraw.DrawLineBetween(this.pawn.Position.ToVector3Shifted(), c.ToVector3Shifted());
						}
						this.FaceAdjacentCell(c);
						return;
					}
					this.Face(thing.DrawPos);
					return;
				}
			}
			else
			{
				if (this.pawn.Position.AdjacentTo8Way(target.Cell))
				{
					if (DebugViewSettings.drawPawnRotatorTarget)
					{
						this.pawn.Map.debugDrawer.FlashCell(this.pawn.Position, 0.2f, "jbloc", 50);
						GenDraw.DrawLineBetween(this.pawn.Position.ToVector3Shifted(), target.Cell.ToVector3Shifted());
					}
					this.FaceAdjacentCell(target.Cell);
					return;
				}
				if (target.Cell.IsValid && target.Cell != this.pawn.Position)
				{
					this.Face(target.Cell.ToVector3());
					return;
				}
			}
		}

		// Token: 0x06002548 RID: 9544 RVA: 0x000E2E88 File Offset: 0x000E1088
		public static Rot4 RotFromAngleBiased(float angle)
		{
			if (angle < 30f)
			{
				return Rot4.North;
			}
			if (angle < 150f)
			{
				return Rot4.East;
			}
			if (angle < 210f)
			{
				return Rot4.South;
			}
			if (angle < 330f)
			{
				return Rot4.West;
			}
			return Rot4.North;
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x000E2EC7 File Offset: 0x000E10C7
		public void ExposeData()
		{
		}

		// Token: 0x04001AEC RID: 6892
		private Pawn pawn;
	}
}
