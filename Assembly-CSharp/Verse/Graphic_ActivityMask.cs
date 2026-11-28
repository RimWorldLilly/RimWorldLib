using System;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200055B RID: 1371
	public class Graphic_ActivityMask : Graphic_WithPropertyBlock
	{
		// Token: 0x060028D2 RID: 10450 RVA: 0x000FB55C File Offset: 0x000F975C
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			CompActivity compActivity = thing.TryGetComp<CompActivity>();
			if (compActivity == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 3);
				defaultInterpolatedStringHandler.AppendFormatted(thingDef.defName);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted("Graphic_ActivityMask");
				defaultInterpolatedStringHandler.AppendLiteral(" requires ");
				defaultInterpolatedStringHandler.AppendFormatted("CompActivity");
				defaultInterpolatedStringHandler.AppendLiteral(".");
				Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), 6134621);
				return;
			}
			Color colorTwo = this.colorTwo;
			colorTwo.a = Mathf.Clamp01(compActivity.ActivityLevel);
			this.propertyBlock.SetColor(ShaderPropertyIDs.ColorTwo, colorTwo);
			base.DrawWorker(loc, rot, thingDef, thing, extraRotation);
		}
	}
}
