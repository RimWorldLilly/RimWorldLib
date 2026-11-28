using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B53 RID: 6995
	public class PassengerShuttleLeaving : FlyShipLeaving
	{
		// Token: 0x17002078 RID: 8312
		// (get) Token: 0x0600B38C RID: 45964 RVA: 0x003F491C File Offset: 0x003F2B1C
		public override Color DrawColor
		{
			get
			{
				return base.Contents.GetShuttle().DrawColor;
			}
		}

		// Token: 0x0600B38D RID: 45965 RVA: 0x003F492E File Offset: 0x003F2B2E
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (respawningAfterLoad || base.BeingTransportedOnGravship)
			{
				return;
			}
			this.angle = PassengerShuttleLeaving.GetAngle(0f, base.Rotation);
		}

		// Token: 0x0600B38E RID: 45966 RVA: 0x003F495C File Offset: 0x003F2B5C
		protected override void GetDrawPositionAndRotation(ref Vector3 drawLoc, out float extraRotation)
		{
			extraRotation = 0f;
			this.angle = PassengerShuttleLeaving.GetAngle(base.TimeInAnimation, base.Rotation);
			switch (base.Rotation.AsInt)
			{
			case 1:
				extraRotation += this.def.skyfaller.rotationCurve.Evaluate(base.TimeInAnimation);
				break;
			case 3:
				extraRotation -= this.def.skyfaller.rotationCurve.Evaluate(base.TimeInAnimation);
				break;
			}
			drawLoc.z += this.def.skyfaller.zPositionCurve.Evaluate(base.TimeInAnimation);
		}

		// Token: 0x0600B38F RID: 45967 RVA: 0x003F4A18 File Offset: 0x003F2C18
		public override float DrawAngle()
		{
			float num = 0f;
			switch (base.Rotation.AsInt)
			{
			case 1:
				num += this.def.skyfaller.rotationCurve.Evaluate(base.TimeInAnimation);
				break;
			case 3:
				num -= this.def.skyfaller.rotationCurve.Evaluate(base.TimeInAnimation);
				break;
			}
			return num;
		}

		// Token: 0x0600B390 RID: 45968 RVA: 0x003F4A94 File Offset: 0x003F2C94
		private static float GetAngle(float timeInAnimation, Rot4 rotation)
		{
			int asInt = rotation.AsInt;
			if (asInt == 1)
			{
				return rotation.AsAngle - PassengerShuttleLeaving.AngleCurve.Evaluate(timeInAnimation);
			}
			if (asInt != 3)
			{
				return rotation.AsAngle;
			}
			return rotation.AsAngle + PassengerShuttleLeaving.AngleCurve.Evaluate(timeInAnimation);
		}

		// Token: 0x04006794 RID: 26516
		private static readonly SimpleCurve AngleCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 20f),
				true
			}
		};
	}
}
