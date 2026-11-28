using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B31 RID: 6961
	[StaticConstructorOnStartup]
	public class MoteProgressBar : MoteDualAttached
	{
		// Token: 0x17002035 RID: 8245
		// (get) Token: 0x0600B295 RID: 45717 RVA: 0x003EE0BC File Offset: 0x003EC2BC
		protected virtual bool OnlyShowForClosestZoom
		{
			get
			{
				return !this.alwaysShow;
			}
		}

		// Token: 0x0600B296 RID: 45718 RVA: 0x003EE0C8 File Offset: 0x003EC2C8
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.UpdatePositionAndRotation();
			if ((!this.OnlyShowForClosestZoom || Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest) && !Find.ScreenshotModeHandler.Active)
			{
				GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
				r.center = this.exactPosition;
				r.center.z = r.center.z + this.offsetZ;
				r.size = new Vector2(this.linearScale.x, this.linearScale.z);
				r.fillPercent = this.progress;
				r.filledMat = MoteProgressBar.FilledMat;
				r.unfilledMat = MoteProgressBar.UnfilledMat;
				r.margin = 0.12f;
				if (this.offsetZ >= -0.8f && this.offsetZ <= -0.3f && this.AnyThingWithQualityHere())
				{
					r.center.z = r.center.z + 0.25f;
				}
				GenDraw.DrawFillableBar(r);
			}
		}

		// Token: 0x0600B297 RID: 45719 RVA: 0x003EE1BC File Offset: 0x003EC3BC
		private bool AnyThingWithQualityHere()
		{
			IntVec3 c = this.exactPosition.ToIntVec3();
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].TryGetComp<CompQuality>() != null && (thingList[i].DrawPos - this.exactPosition).MagnitudeHorizontalSquared() < 0.0001f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04006707 RID: 26375
		public float progress;

		// Token: 0x04006708 RID: 26376
		public float offsetZ;

		// Token: 0x04006709 RID: 26377
		public bool alwaysShow;

		// Token: 0x0400670A RID: 26378
		private static readonly Material UnfilledMat = SolidColorMaterials.NewSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f, 0.65f), ShaderDatabase.MetaOverlay);

		// Token: 0x0400670B RID: 26379
		private static readonly Material FilledMat = SolidColorMaterials.NewSolidColorMaterial(new Color(0.9f, 0.85f, 0.2f, 0.65f), ShaderDatabase.MetaOverlay);
	}
}
