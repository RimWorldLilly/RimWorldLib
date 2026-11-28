using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B5A RID: 7002
	public class Skyfaller_FlyingPawn : Skyfaller
	{
		// Token: 0x17002086 RID: 8326
		// (get) Token: 0x0600B3D3 RID: 46035 RVA: 0x003F65E0 File Offset: 0x003F47E0
		public Pawn Pawn
		{
			get
			{
				return (Pawn)this.innerContainer[0];
			}
		}

		// Token: 0x0600B3D4 RID: 46036 RVA: 0x003F65F4 File Offset: 0x003F47F4
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			AnimationDef bestFlyAnimation = Pawn_FlightTracker.GetBestFlyAnimation(this.Pawn, new Rot4?(Rot4.FromAngleFlat(this.angle)));
			if (bestFlyAnimation != null)
			{
				this.Pawn.Drawer.renderer.SetAnimation(bestFlyAnimation);
			}
		}

		// Token: 0x0600B3D5 RID: 46037 RVA: 0x003F6640 File Offset: 0x003F4840
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			float num;
			this.GetDrawPositionAndRotation(ref drawLoc, out num);
			Pawn pawn = this.Pawn;
			if (pawn != null)
			{
				pawn.DrawNowAt(drawLoc, flip);
			}
			this.DrawDropSpotShadow();
		}

		// Token: 0x0600B3D6 RID: 46038 RVA: 0x003F6670 File Offset: 0x003F4870
		protected override void SpawnThings()
		{
			Pawn pawn = this.Pawn;
			if (pawn != null)
			{
				GenSpawn.Spawn(pawn, base.Position, base.Map, WipeMode.Vanish);
				pawn.Rotation = Rot4.East;
				pawn.Drawer.renderer.SetAnimation(null);
			}
		}

		// Token: 0x0600B3D7 RID: 46039 RVA: 0x003F66B8 File Offset: 0x003F48B8
		protected override void DrawDropSpotShadow()
		{
			Material shadowMaterial = base.ShadowMaterial;
			if (shadowMaterial != null)
			{
				Vector3 drawPos = this.DrawPos;
				drawPos.y = AltitudeLayer.Shadows.AltitudeFor();
				drawPos.z = base.Position.ToVector3Shifted().z;
				Skyfaller.DrawDropSpotShadow(drawPos, base.Rotation, shadowMaterial, this.def.skyfaller.shadowSize, this.ticksToImpact);
			}
		}
	}
}
