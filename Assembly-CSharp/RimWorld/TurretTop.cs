using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A75 RID: 6773
	public class TurretTop
	{
		// Token: 0x17001E0E RID: 7694
		// (get) Token: 0x0600AA68 RID: 43624 RVA: 0x003C577D File Offset: 0x003C397D
		// (set) Token: 0x0600AA69 RID: 43625 RVA: 0x003C5788 File Offset: 0x003C3988
		public float CurRotation
		{
			get
			{
				return this.curRotationInt;
			}
			set
			{
				this.curRotationInt = value;
				if (this.curRotationInt > 360f)
				{
					this.curRotationInt -= 360f;
				}
				if (this.curRotationInt < 0f)
				{
					this.curRotationInt += 360f;
				}
			}
		}

		// Token: 0x0600AA6A RID: 43626 RVA: 0x003C57DC File Offset: 0x003C39DC
		public void SetRotationFromOrientation()
		{
			this.CurRotation = this.parentTurret.Rotation.AsAngle;
		}

		// Token: 0x0600AA6B RID: 43627 RVA: 0x003C5802 File Offset: 0x003C3A02
		public TurretTop(Building_Turret ParentTurret)
		{
			this.parentTurret = ParentTurret;
		}

		// Token: 0x0600AA6C RID: 43628 RVA: 0x003C5814 File Offset: 0x003C3A14
		public void ForceFaceTarget(LocalTargetInfo targ)
		{
			if (targ.IsValid)
			{
				float curRotation = (targ.Cell.ToVector3Shifted() - this.parentTurret.DrawPos).AngleFlat();
				this.CurRotation = curRotation;
			}
		}

		// Token: 0x0600AA6D RID: 43629 RVA: 0x003C5858 File Offset: 0x003C3A58
		public void TurretTopTick()
		{
			LocalTargetInfo currentTarget = this.parentTurret.CurrentTarget;
			if (currentTarget.IsValid)
			{
				float curRotation = (currentTarget.Cell.ToVector3Shifted() - this.parentTurret.DrawPos).AngleFlat();
				this.CurRotation = curRotation;
				this.ticksUntilIdleTurn = Rand.RangeInclusive(150, 350);
				return;
			}
			if (this.ticksUntilIdleTurn > 0)
			{
				this.ticksUntilIdleTurn--;
				if (this.ticksUntilIdleTurn == 0)
				{
					if (Rand.Value < 0.5f)
					{
						this.idleTurnClockwise = true;
					}
					else
					{
						this.idleTurnClockwise = false;
					}
					this.idleTurnTicksLeft = 140;
					return;
				}
			}
			else
			{
				if (this.idleTurnClockwise)
				{
					this.CurRotation += 0.26f;
				}
				else
				{
					this.CurRotation -= 0.26f;
				}
				this.idleTurnTicksLeft--;
				if (this.idleTurnTicksLeft <= 0)
				{
					this.ticksUntilIdleTurn = Rand.RangeInclusive(150, 350);
				}
			}
		}

		// Token: 0x0600AA6E RID: 43630 RVA: 0x003C5960 File Offset: 0x003C3B60
		public void DrawTurret(Vector3 drawLoc, Vector3 recoilDrawOffset, float recoilAngleOffset)
		{
			Vector3 vector = new Vector3(this.parentTurret.def.building.turretTopOffset.x, 0f, this.parentTurret.def.building.turretTopOffset.y);
			float turretTopDrawSize = this.parentTurret.def.building.turretTopDrawSize;
			vector = vector.RotatedBy(recoilAngleOffset);
			vector += recoilDrawOffset;
			Verb currentEffectiveVerb = this.parentTurret.CurrentEffectiveVerb;
			float num = ((currentEffectiveVerb != null) ? currentEffectiveVerb.AimAngleOverride : null) ?? this.CurRotation;
			Vector3 pos = drawLoc + Altitudes.AltIncVect + vector;
			Quaternion q = ((float)TurretTop.ArtworkRotation + num).ToQuat();
			Vector3 s = new Vector3(turretTopDrawSize, 1f, turretTopDrawSize);
			Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(pos, q, s), this.parentTurret.TurretTopMaterial, 0);
		}

		// Token: 0x04006391 RID: 25489
		private Building_Turret parentTurret;

		// Token: 0x04006392 RID: 25490
		private float curRotationInt;

		// Token: 0x04006393 RID: 25491
		private int ticksUntilIdleTurn;

		// Token: 0x04006394 RID: 25492
		private int idleTurnTicksLeft;

		// Token: 0x04006395 RID: 25493
		private bool idleTurnClockwise;

		// Token: 0x04006396 RID: 25494
		private const float IdleTurnDegreesPerTick = 0.26f;

		// Token: 0x04006397 RID: 25495
		private const int IdleTurnDuration = 140;

		// Token: 0x04006398 RID: 25496
		private const int IdleTurnIntervalMin = 150;

		// Token: 0x04006399 RID: 25497
		private const int IdleTurnIntervalMax = 350;

		// Token: 0x0400639A RID: 25498
		public static readonly int ArtworkRotation = -90;
	}
}
