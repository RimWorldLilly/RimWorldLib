using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B54 RID: 6996
	public class PassengerShuttleIncoming : ShuttleIncoming
	{
		// Token: 0x17002079 RID: 8313
		// (get) Token: 0x0600B393 RID: 45971 RVA: 0x003F4B22 File Offset: 0x003F2D22
		public Building_PassengerShuttle Shuttle
		{
			get
			{
				return (Building_PassengerShuttle)this.innerContainer.FirstOrDefault<Thing>();
			}
		}

		// Token: 0x1700207A RID: 8314
		// (get) Token: 0x0600B394 RID: 45972 RVA: 0x003F4B34 File Offset: 0x003F2D34
		public override Color DrawColor
		{
			get
			{
				return this.Shuttle.DrawColor;
			}
		}

		// Token: 0x0600B395 RID: 45973 RVA: 0x003F4B41 File Offset: 0x003F2D41
		protected override void Impact()
		{
			CompLaunchable compLaunchable = this.Shuttle.TryGetComp<CompLaunchable>();
			if (compLaunchable != null)
			{
				compLaunchable.Notify_Arrived();
			}
			base.Impact();
		}

		// Token: 0x0600B396 RID: 45974 RVA: 0x003F4B5F File Offset: 0x003F2D5F
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (respawningAfterLoad || base.BeingTransportedOnGravship)
			{
				return;
			}
			this.angle = PassengerShuttleIncoming.GetAngle(0f, base.Rotation);
		}

		// Token: 0x0600B397 RID: 45975 RVA: 0x003F4B8B File Offset: 0x003F2D8B
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (!this.hasImpacted)
			{
				Log.Error("Destroying passenger shuttle skyfaller without ever having impacted");
			}
			base.Destroy(mode);
		}

		// Token: 0x0600B398 RID: 45976 RVA: 0x003F4BA8 File Offset: 0x003F2DA8
		protected override void GetDrawPositionAndRotation(ref Vector3 drawLoc, out float extraRotation)
		{
			extraRotation = 0f;
			this.angle = PassengerShuttleIncoming.GetAngle(base.TimeInAnimation, base.Rotation);
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

		// Token: 0x0600B399 RID: 45977 RVA: 0x003F4C64 File Offset: 0x003F2E64
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

		// Token: 0x0600B39A RID: 45978 RVA: 0x003F4CE0 File Offset: 0x003F2EE0
		private static float GetAngle(float timeInAnimation, Rot4 rotation)
		{
			int asInt = rotation.AsInt;
			if (asInt == 1)
			{
				return rotation.Opposite.AsAngle + PassengerShuttleIncoming.AngleCurve.Evaluate(timeInAnimation);
			}
			if (asInt != 3)
			{
				return rotation.Opposite.AsAngle;
			}
			return rotation.Opposite.AsAngle - PassengerShuttleIncoming.AngleCurve.Evaluate(timeInAnimation);
		}

		// Token: 0x04006795 RID: 26517
		private static readonly SimpleCurve AngleCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 30f),
				true
			},
			{
				new CurvePoint(1f, 0f),
				true
			}
		};
	}
}
